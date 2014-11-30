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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using System.Threading;

#endregion

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    /// <summary>
    /// A query against an Overpass API.
    /// </summary>
    public class OverpassQuery
    {

        private        Object   LockObject  = new Object();
        private static DateTime LastCall    = DateTime.Now;

        #region Documentation

        // A Overpass query example...
        // 
        // [out:json]
        // [timeout:100];
        // area($areaId)->.searchArea;
        // (
        //   node     ["leisure"]            (area.searchArea);
        //   way      ["waterway" = "river"] (area.searchArea);
        //   relation ["leisure"]            (area.searchArea);
        // );
        // out body;
        // >;
        // out skel qt;

        #endregion

        #region Data

        /// <summary>
        /// The URI of the OverpassAPI.
        /// </summary>
        public static readonly Uri          OverpassAPI_URI  = new Uri("http://overpass-api.de/api/interpreter");

        /// <summary>
        /// The URIO of the NominatimAPI.
        /// </summary>
        public static readonly Uri          NominatimAPI_URI = new Uri("http://nominatim.openstreetmap.org/search");


        private Dictionary<String, String>  Nodes;
        private Dictionary<String, String>  Ways;
        private Dictionary<String, String>  Relations;

        #endregion

        #region Properties

        #region AreaId

        private UInt64 _AreaId;

        /// <summary>
        /// The area identification used for this query.
        /// </summary>
        public UInt64 AreaId
        {
            get
            {
                return _AreaId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region OverpassQuery()

        /// <summary>
        /// Create a new OverpassQuery.
        /// </summary>
        public OverpassQuery()
        {

            this._AreaId     = 0;

            this.Nodes      = new Dictionary<String, String>();
            this.Ways       = new Dictionary<String, String>();
            this.Relations  = new Dictionary<String, String>();

        }

        #endregion

        #region OverpassQuery(AreaId)

        /// <summary>
        /// Create a new OverpassQuery for the given area identification.
        /// </summary>
        /// <param name="AreaId">An area reference using an OpenStreetMap area identification.</param>
        public OverpassQuery(UInt64 AreaId)
            : this()
        {
            InArea(AreaId);
        }

        #endregion

        #region OverpassQuery(AreaName)

        /// <summary>
        /// Create a new OverpassQuery for the given area name.
        /// </summary>
        /// <param name="AreaName">an area reference using an OpenStreetMap area name. This will search for the given name via the Nominatim API and use the first matching result (normally this is the result having the highest importance).</param>
        public OverpassQuery(String AreaName)
            : this()
        {
            InArea(AreaName);
        }

        #endregion

        #endregion


        #region InArea(AreaId)

        /// <summary>
        /// Add an area reference using an OpenStreetMap area identification.
        /// </summary>
        /// <param name="AreaId">A OpenStreetMap area identification</param>
        public OverpassQuery InArea(UInt64 AreaId)
        {
            this._AreaId = AreaId;
            return this;
        }

        #endregion

        #region InArea(AreaName)

        /// <summary>
        /// Add an area reference using an OpenStreetMap area name.
        /// This will search for the given name via the Nominatim API and use the first matching result (normally this is the result having the highest importance).
        /// </summary>
        /// <param name="AreaName">A OpenStreetMap area name</param>
        public OverpassQuery InArea(String AreaName)
        {

            using (var HTTPClient = new HttpClient())
            {

                try
                {

                    // Note: This query currently does not support to narrow down the results to be of "osm_type = relation".
                    //       Therefore we query up to 100 results and hope that at least one will be of this relation type.
                    using (var ResponseMessage = HTTPClient.GetAsync(NominatimAPI_URI + "/" + AreaName + "?format=json&addressdetails=1&limit=100"))
                    {

                        if (ResponseMessage.Result.StatusCode == HttpStatusCode.OK)
                        {

                            using (var ResponseContent = ResponseMessage.Result.Content)
                            {

                                var result = ResponseContent.ReadAsStringAsync();

                                // [
                                //    {
                                //        "place_id:        "158729066",
                                //        "licence:         "Data © OpenStreetMap contributors, ODbL 1.0. http://www.openstreetmap.org/copyright",
                                //        "osm_type:        "relation",
                                //        "osm_id: "        62693",
                                //        "boundingbox: [
                                //              "50.856077",
                                //              "50.988898",
                                //              "11.4989589",
                                //              "11.6728014"
                                //        "],
                                //        "lat:             "50.9221871",
                                //        "lon:             "11.5888846280636",
                                //        "display_name:    "Jena, Thüringen, Deutschland",
                                //        "class:           "boundary",
                                //        "type:            "administrative",
                                //        "importance:      0.72701320621596,
                                //        "icon:            "http://nominatim.openstreetmap.org/images/mapicons/poi_boundary_administrative.p.20.png",
                                //        "address: {
                                //              "county:          "Jena",
                                //              "state:           "Thüringen",
                                //              "country:         "Deutschland",
                                //              "country_code:    "de"
                                //        }
                                //    }
                                // ]
                                var JSON = JArray.Parse(result.Result).
                                               Children<JObject>().
                                               Where(JSONObject => JSONObject["osm_type"].ToString() == "relation").
                                               FirstOrDefault();

                                // result ==      62693 (Jena)
                                // wanted == 3600062693
                                //           3600000000

                                if (JSON != null)
                                    this._AreaId = UInt64.Parse(JSON["osm_id"].ToString()) + 3600000000;

                                return this;

                            }

                        }

                    }

                }

                catch (OperationCanceledException)
                { }

            }

            throw new Exception();

        }

        #endregion


        #region WithAny(Type, Value = "")

        public OverpassQuery WithAny(String Type, String Value = "")
        {

            this.Nodes.    Add(Type, Value);
            this.Ways.     Add(Type, Value);
            this.Relations.Add(Type, Value);

            return this;

        }

        #endregion

        #region WithNodes(NodeType, Value = "")

        public OverpassQuery WithNodes(String NodeType, String Value = "")
        {
            this.Nodes.Add(NodeType, Value);
            return this;
        }

        #endregion

        #region WithWays(NodeType, Value = "")

        public OverpassQuery WithWays(String WayType, String Value = "")
        {
            this.Ways.Add(WayType, Value);
            return this;
        }

        #endregion

        #region WithRelations(RelationType, Value = "")

        public OverpassQuery WithRelations(String RelationType, String Value = "")
        {
            this.Relations.Add(RelationType, Value);
            return this;
        }

        #endregion


        #region RunQuery()

        /// <summary>
        /// Execute this Overpass query.
        /// </summary>
        /// <returns>A Overpass query result.</returns>
        public async Task<OverpassResult> RunQuery()
        {

            var wait = false;

            lock (LockObject)
            {

                do
                {

                    var CurrentTime = DateTime.Now;

                    if (CurrentTime - LastCall < TimeSpan.FromSeconds(2))
                        Thread.Sleep(500);

                    else
                        wait = false;

                } while (wait);

            }

            using (var HTTPClient = new HttpClient())
            {

                try
                {

                    using (var ResponseMessage = await HTTPClient.PostAsync(OverpassAPI_URI, new StringContent(this.ToString())))
                    {

                        if (ResponseMessage.StatusCode == HttpStatusCode.OK)
                        {

                            using (var ResponseContent = ResponseMessage.Content)
                            {

                                // {
                                //   "version": 0.6,
                                //   "generator": "Overpass API",
                                //   "osm3s": {
                                //     "timestamp_osm_base":    "2014-11-29T20:02:02Z",
                                //     "timestamp_areas_base":  "2014-11-29T08:42:02Z",
                                //     "copyright":             "The data included in this document is from www.openstreetmap.org. The data is made available under ODbL."
                                //   },
                                //   "elements": [
                                //                   {
                                //                     "type": "node",
                                //                     "id": 1875593753,
                                //                     "lat": 50.9292604,
                                //                     "lon": 11.5824008,
                                //                     "tags": {
                                //                       "addr:city": "Jena",
                                //                       "addr:housenumber": "26",
                                //                       "addr:postcode": "07743",
                                //                       "addr:street": "Krautgasse",
                                //                       "amenity": "community_centre",
                                //                       "building:level": "1",
                                //                       "club": "it",
                                //                       "contact:phone": "0162/6318746",
                                //                       "contact:website": "https://www.krautspace.de",
                                //                       "drink:club-mate": "yes",
                                //                       "leisure": "hackerspace",
                                //                       "name": "Krautspace",
                                //                       "office": "club",
                                //                       "operator": "Hackspace Jena e.V."
                                //                     }
                                //                   }
                                //               ]
                                // }

                                return await ResponseContent.
                                                 ReadAsStringAsync().
                                                 ContinueWith(QueryTask => new OverpassResult(this,
                                                                                              JObject.Parse(QueryTask.Result)));

                            }

                        }

                        else if (ResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                            throw new Exception("Bad request!");

                        else if (((Int32) ResponseMessage.StatusCode) == 429)
                            throw new Exception("Too Many Requests!");

                        else
                        {
                        }

                    }

                }

                catch (OperationCanceledException)
                { }

                catch (Exception e)
                {
                    throw new Exception("The OverpassQuery led to an error!", e);
                }

            }

            throw new Exception("General HTTP client error!");

        }

        #endregion


        #region ClearAll()

        /// <summary>
        /// Clear all internal node, way and relation queries and the area identification.
        /// </summary>
        public OverpassQuery ClearAll()
        {

            _AreaId = 0;

            Nodes.Clear();
            Ways.Clear();
            Relations.Clear();

            return this;

        }

        #endregion

        #region ClearAll_ExceptAreaId()

        /// <summary>
        /// Clear all internal node, way and relation queries without the area identification.
        /// </summary>
        public OverpassQuery ClearAll_ExceptAreaId()
        {

            Nodes.Clear();
            Ways.Clear();
            Relations.Clear();

            return this;

        }

        #endregion


        #region ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            var QueryString = new StringBuilder();

            QueryString.AppendLine("[out:json]");
            QueryString.AppendLine("[timeout:100];");

            if (_AreaId > 0)
                QueryString.AppendLine("area(" + _AreaId.ToString() + ")->.searchArea;");

            QueryString.AppendLine("(");

            foreach (var Node in Nodes)
                QueryString.AppendLine(String.Concat(@"node[""", Node.Key, @"""", (Node.Value != "" ? @"=""" + Node.Value + @"""" : ""), "]", (_AreaId > 0 ? " (area.searchArea);" : ";")));

            foreach (var Way in Ways)
                QueryString.AppendLine(String.Concat(@"way[""", Way.Key, @"""", (Way.Value != "" ? @"=""" + Way.Value + @"""" : ""), "]", (_AreaId > 0 ? " (area.searchArea);" : ";")));

            foreach (var Relation in Relations)
                QueryString.AppendLine(String.Concat(@"relation[""", Relation.Key, @"""", (Relation.Value != "" ? @"=""" + Relation.Value + @"""" : ""), "]", (_AreaId > 0 ? " (area.searchArea);" : ";")));

            QueryString.AppendLine(");");

            QueryString.AppendLine("out body;");
            QueryString.AppendLine(">;");
            QueryString.AppendLine("out skel qt;");

            return QueryString.ToString();

        }

        #endregion

    }

}
