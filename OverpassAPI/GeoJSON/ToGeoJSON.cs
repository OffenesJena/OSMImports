/*
 * Copyright (c) 2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of OpenDataAPI <http://www.github.com/GraphDefined/OpenDataAPI>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    public struct GeoCoord : IEquatable<GeoCoord>
    {

        public Double Longitude;
        public Double Latitude;

        public GeoCoord(Double _Longitude, Double _Latitude)
        {
            Longitude = _Longitude;
            Latitude = _Latitude;
        }

        public override Boolean Equals(object obj)
        {

            var other = (GeoCoord) obj;

            if (Longitude != other.Longitude)
                return false;

            if (Latitude != other.Latitude)
                return false;

            return true;

        }

        public override Int32 GetHashCode()
        {
            return Longitude.GetHashCode() ^ Latitude.GetHashCode();
        }

        public override String ToString()
        {
            return Longitude.ToString() + ", " + Latitude.ToString();
        }

        public Boolean Equals(GeoCoord other)
        {

            if (Latitude != other.Latitude)
                return false;

            if (Longitude != other.Longitude)
                return false;

            return true;

        }

    }


    /// <summary>
    /// Convert the OSM JSON result of an Overpass query to GeoJSON.
    /// </summary>
    public static partial class GeoJSONExtentions
    {

        #region ToGeoJSON(this OverpassQuery)

        /// <summary>
        /// Run the given Overpass query and convert the result to GeoJSON.
        /// </summary>
        /// <param name="OverpassQuery">An Overpass query.</param>
        public static Task<JObject> ToGeoJSON(this OverpassQuery OverpassQuery)
        {

            return OverpassQuery.
                       RunQuery().
                       ToGeoJSON();

        }

        #endregion

        #region ToGeoJSON(this ResultTask)

        /// <summary>
        /// Convert the given Overpass query result to GeoJSON.
        /// </summary>
        /// <param name="ResultTask">A Overpass query result task.</param>
        public static Task<JObject> ToGeoJSON(this Task<OverpassResult> ResultTask)
        {

            return ResultTask.ContinueWith(task => {

                // The order of the nodes, ways and relations seem not to be sorted by default!

                #region 1st: Add all nodes

                Node Node;
                var Nodes = new Dictionary<UInt64, Node>();

                foreach (var element in ResultTask.Result.Elements)
                {

                    // {
                    //   "type": "node",
                    //   "id":    35304749,
                    //   "lat":   50.8926376,
                    //   "lon":   11.6023278,
                    //   "tags": {
                    //       "highway":     "bus_stop",
                    //       "name":        "Lobeda",
                    //       "operator":    "JES",
                    //       "wheelchair":  "yes"
                    //   }
                    // }

                    if (element["type"].ToString() == "node")
                    {

                        Node = Node.Parse(element);

                        if (Nodes.ContainsKey(Node.Id))
                            Console.WriteLine("Duplicate node id detected!");
                        else
                            Nodes.Add(Node.Id, Node);

                    }

                }

                #endregion

                #region 2nd: Add all ways

                Way Way;
                var Ways = new Dictionary<UInt64, Way>();

                foreach (var element in ResultTask.Result.Elements)
                {

                    // {
                    //   "type": "way",
                    //   "id":   154676600,
                    //   "nodes": [
                    //     747761494,
                    //     582476538,
                    //     582476541,
                    //     582476543,
                    //     1671750275,
                    //     407850195,
                    //     407850192,
                    //     407850188,
                    //     1671750245,
                    //     1671750330,
                    //     1671750408,
                    //     1671750415,
                    //     1671750433,
                    //     1671750438,
                    //     1671750441,
                    //     747761494
                    //   ],
                    //   "tags": {
                    //     "landuse": "farm"
                    //   }
                    // }

                    if (element["type"].ToString() == "way")
                    {

                        Way = Way.Parse(element,
                                        NodeResolver: nodeId => Nodes[nodeId]);

                        if (Ways.ContainsKey(Way.Id))
                            Console.WriteLine("Duplicate way id detected!");
                        else
                            Ways.Add(Way.Id, Way);

                    }

                }

                #endregion

                #region 3rd: Add all relations

                Relation Relation;
                var Relations = new Dictionary<UInt64, Relation>();

                foreach (var element in ResultTask.Result.Elements)
                {

                    // {
                    //   "type": "relation",
                    //   "id":   3806843,
                    //   "members": [
                    //        {
                    //          "type": "way",
                    //          "ref":  71002045,
                    //          "role": "outer"
                    //        },
                    //        {
                    //          "type": "way",
                    //          "ref":  286959663,
                    //          "role": "outer"
                    //        },
                    //        {
                    //          "type": "way",
                    //          "ref":  286959664,
                    //          "role": "outer"
                    //        },
                    //        {
                    //          "type": "way",
                    //          "ref":  286959641,
                    //          "role": "outer"
                    //        }
                    //   ],
                    //   "tags": {
                    //       "landuse": "farm",
                    //       "type":    "multipolygon"
                    //   }
                    // }

                    if (element["type"].ToString() == "relation")
                    {

                        Relation = Relation.Parse(element,
                                                  NodeResolver: nodeId => Nodes[nodeId],
                                                  WayResolver:  wayId  => Ways [wayId]);

                        if (Relations.ContainsKey(Relation.Id))
                            Console.WriteLine("Duplicate relation id detected!");
                        else
                            Relations.Add(Relation.Id, Relation);

                    }

                }

                #endregion

                // {
                //    "type":      "FeatureCollection",
                //    "generator": "overpass-turbo",
                //    "copyright": "The data included in this document is from www.openstreetmap.org. The data is made available under ODbL.",
                //    "timestamp": "2014-11-29T23:08:02Z",
                //    "features": [ ]
                // }

                return new JObject(

                    new JProperty("type",       "FeatureCollection"),
                    new JProperty("generator",  "GraphDefined OSM Importer"),
                    new JProperty("copyright",  ResultTask.Result.Copyright),
                    new JProperty("timestamp",  DateTime.Now.ToIso8601()),

                    new JProperty("features",   new JArray(Nodes.Values.
                                                               // Do not include nodes which only have a geo coordinate, but not tags!
                                                               // (They will be most likely only be useful within ways or relations)
                                                               Where (n => n.Tags.Count > 0).
                                                               Select(n => n.ToGeoJSON())).

                                                Concat(
                                                new JArray(Ways.Values.
                                                               // Do not include ways which do not have any tags!
                                                               // (They will be most likely only be useful within relations)
                                                               Where (w => w.Tags.Count > 0).
                                                               Select(w => w.ToGeoJSON())) ).

                                                Concat(
                                                new JArray(Relations.Values.
                                                               // Do not include nodes which only have a geo coordinate, but not tags!
                                                               // (They will be most likely only be useful within ways or relations)
                                                               //Where (r => r.Tags.Count > 0).
                                                               Select(r => r.ToGeoJSON())) )

                                                )

                );

            });

        }

        #endregion


        #region ToGeoJSONFile(this ResultTask)

        /// <summary>
        /// Convert the given Overpass query result to GeoJSON.
        /// </summary>
        /// <param name="ResultTask">A Overpass query result task.</param>
        /// <param name="Filename">A file name.</param>
        public static Task<JObject> ToGeoJSONFile(this Task<OverpassResult>  ResultTask,
                                                  String                     Filename)
        {
            return ResultTask.ToGeoJSON().ToFile(Filename);
        }

        #endregion


        #region ToGeoJSON(this Node)

        /// <summary>
        /// Convert the given OSM node to a GeoJSON point feature.
        /// </summary>
        /// <param name="Node">An OSM node.</param>
        public static JObject ToGeoJSON(this Node Node)
        {

            // {
            //     "type":  "Feature",
            //     "id":    "node/35304749",
            //     "properties": {
            //         "@id":         "node/35304749",
            //         "highway":     "bus_stop",
            //         "name":        "Lobeda",
            //         "operator":    "JES",
            //         "wheelchair":  "yes"
            //     },
            //     "geometry": {
            //         "type":        "Point",
            //         "coordinates": [ 11.6023278, 50.8926376 ]
            //     }
            // }

            return new JObject(

                new JProperty("type",        "Feature"),
                new JProperty("id",          "node/" + Node.Id),

                new JProperty("properties",  new JObject(
                        new List<JProperty>() { new JProperty("@id", "node/" + Node.Id) }.
                        AddAndReturnList(Node.Tags.Select(kvp => new JProperty(kvp.Key, kvp.Value))).
                        ToArray()
                    )),

                new JProperty("geometry",    new JObject(
                    new JProperty("type",         "Point"),
                    new JProperty("coordinates",  new JArray(Node.Longitude, Node.Latitude))
                ))

            );

        }

        #endregion

        #region ToGeoJSON(this Way)

        /// <summary>
        /// Convert the given OSM way to a GeoJSON line feature.
        /// </summary>
        /// <param name="Way">An OSM way.</param>
        public static JObject ToGeoJSON(this Way Way)
        {

            // {
            //     "type":  "Feature",
            //     "id":    "way/305352912",
            //     "properties": {
            //         "@id":         "way/305352912",
            //     },
            //     "geometry": {
            //         "type":        "LineString",
            //         "coordinates": [ [ 11.6023278, 50.8926376 ], [ 11.5054540, 50.7980146 ], [ 11.6023278, 50.8926376 ] ]
            //     }
            // }

            // https://wiki.openstreetmap.org/wiki/Overpass_turbo/Polygon_Features

            var FirstNode = Way.Nodes.First();
            var LastNode  = Way.Nodes.Last();
            var isClosed  = FirstNode.Latitude == LastNode.Latitude && FirstNode.Longitude == LastNode.Longitude;

            return new JObject(

                new JProperty("type", "Feature"),
                new JProperty("id",   "way/" + Way.Id),

                new JProperty("properties", new JObject(
                        new List<JProperty>() { new JProperty("@id", "way/" + Way.Id) }.
                        AddAndReturnList(Way.Tags.Select(kvp => new JProperty(kvp.Key, kvp.Value))).
                        ToArray()
                    )),

                new JProperty("geometry", new JObject(
                    new JProperty("type",         isClosed ? "Polygon" : "LineString"),
                    new JProperty("coordinates",  isClosed ? new JArray() { new JArray(Way.Nodes.Select(n => new JArray(n.Longitude, n.Latitude))) }
                                                           : new JArray(Way.Nodes.Select(n => new JArray(n.Longitude, n.Latitude))))
                ))

            );

        }

        #endregion

        #region ToGeoJSON(this Relation)

        /// <summary>
        /// Convert the given OSM relation to a GeoJSON line feature.
        /// </summary>
        /// <param name="Relation">An OSM relation.</param>
        public static JObject ToGeoJSON(this Relation Relation)
        {

            // {
            //     "type":  "Feature",
            //     "id":    "way/305352912",
            //     "properties": {
            //         "@id":         "way/305352912",
            //     },
            //     "geometry": {
            //         "type":        "LineString",
            //         "coordinates": [ [ 11.6023278, 50.8926376 ], [ 11.5054540, 50.7980146 ], [ 11.6023278, 50.8926376 ] ]
            //     }
            // }

            // https://wiki.openstreetmap.org/wiki/Overpass_turbo/Polygon_Features

            // 1) Combine all ways into a single big list of geo coordinate (it's a puzzle! Some ways must be reversed in order to find matches!)
            // 2) Check if first geo coordinate is the same as the last
            // 3) If yes => polygon (exceptions see wiki link)


            // Relation.Ways.Select(Way => new JArray(Way.Nodes.Select(Node => new JArray(Node.Longitude, Node.Latitude))))

            var AllElements = Relation.Ways.Select(Way => Way.Nodes.Select(Node => new GeoCoord(Node.Longitude, Node.Latitude)).ToArray()).ToArray();

            var FirstElement = new List<GeoCoord>(AllElements.First());
            var Rest         = AllElements.Skip(1).ToList();
            Boolean Found;

            do
            {

                Found = false;
                var Element = FirstElement.Last();

                foreach (var r1 in Rest)
                {
                    if (r1.First().Equals(Element))
                    {
                        Rest.Remove(r1);
                        FirstElement.AddRange(r1);
                        Found = true;
                        break;
                    }
                    else if (r1.Last().Equals(Element))
                    {
                        Rest.Remove(r1);
                        FirstElement.AddRange(r1.Reverse());
                        Found = true;
                        break;
                    }
                }

                if (!Found)
                {
                    Console.WriteLine("Broken OSM relation to GeoJSON export?");
                    break;
                }

            } while (Rest.Count > 0);


            var FirstNode = FirstElement.First();
            var LastNode  = FirstElement.Last();
            var isClosed  = FirstNode.Latitude == LastNode.Latitude && FirstNode.Longitude == LastNode.Longitude;


                return new JObject(

                    new JProperty("type", "Feature"),
                    new JProperty("id", "relation/" + Relation.Id),

                    new JProperty("properties", new JObject(
                            new List<JProperty>() { new JProperty("@id", "relation/" + Relation.Id) }.
                            AddAndReturnList(Relation.Tags.Select(kvp => new JProperty(kvp.Key, kvp.Value))).
                            ToArray()
                        )),

                        (!Found | !isClosed)?
                    new JProperty("geometry", new JObject(
                        new JProperty("type",        "MultiLineString"),
                        new JProperty("coordinates", new JArray(Relation.Ways.Select(Way => new JArray(Way.Nodes.Select(Node => new JArray(Node.Longitude, Node.Latitude))))))
                    ))

                    :
                    new JProperty("geometry", new JObject(
                        new JProperty("type",        "Polygon"),
                        new JProperty("coordinates", new JArray() { new JArray(FirstElement.Select(e2 => new JArray(e2.Longitude, e2.Latitude))) })
                    ))

                );

        }

        #endregion

    }

}
