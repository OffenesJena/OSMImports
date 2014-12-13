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
using System.IO;

using org.GraphDefined.OpenDataAPI.OverpassAPI;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg.OpenPgp;

#endregion

namespace org.GraphDefined.OpenDataAPI.OSMImporter
{

    public static class Extentions
    {

        #region RunAll(this OverpassQuery, Filename)

        /// <summary>
        /// Standard workflow...
        /// </summary>
        /// <param name="OverpassQuery">An Overpass query.</param>
        /// <param name="FilenamePrefix">A file name prefix.</param>
        public static void RunAll(this OverpassQuery  OverpassQuery,
                                  String              FilenamePrefix,
                                  PgpSecretKey        SecretKey,
                                  String              Passphrase)
        {

            OverpassQuery.
                ToFile       (FilenamePrefix + ".json").
                ToGeoJSONFile(FilenamePrefix + ".geojson").
                SignGeoJSON  (FilenamePrefix + ".geojson.sig",  SecretKey, Passphrase).
                SignGeoJSON  (FilenamePrefix + ".geojson.bsig", SecretKey, Passphrase, ArmoredOutput: false).
                RunNow();

        }

        #endregion

    }


    /// <summary>
    /// A little demo... can be tested via http://overpass-turbo.eu
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main...
        /// </summary>
        /// <param name="Arguments">CLI arguments...</param>
        public static void Main(String[] Arguments)
        {

            var JenaId      = new OverpassQuery("Jena").AreaId;
            var SecretKey   = OpenGPG.ReadSecretKey(File.OpenRead("jod-secring.gpg"));
            var Passphrase  = File.ReadAllText("jod-passphrase.txt");

            Directory.CreateDirectory("Gemeinschaftsanlage");
            Directory.CreateDirectory("Freizeit");
            Directory.CreateDirectory("Gebäude");
            Directory.CreateDirectory("Bundestagswahlkreise");
            Directory.CreateDirectory("Ortsteile");
            Directory.CreateDirectory("ÖffentlicherNahverkehr");
            Directory.CreateDirectory("Strassen");
            Directory.CreateDirectory("Tempolimits");
            Directory.CreateDirectory("Energie");
            Directory.CreateDirectory("Flächennutzung");
            Directory.CreateDirectory("Flächennutzung/Grünfläche");
            Directory.CreateDirectory("Flächennutzung/Wasser");
            Directory.CreateDirectory("Flächennutzung/Gewerbe");
            Directory.CreateDirectory("Flächennutzung/Wohngebiete");
            Directory.CreateDirectory("Flächennutzung/Militär");


            #region Gemeinschaftsanlage

            //new OverpassQuery(JenaId).
            //    WithAny      ("amenity").
            //    ToFile       ("Gemeinschaftsanlage/amenity.json").
            //  //  ToGeoJSONFile("Gemeinschaftsanlage/amenity.geojson").
            //    RunNow();


            new OverpassQuery(JenaId).
                WithAny      ("amenity", "school").
                RunAll       ("Gemeinschaftsanlage/amenity.school",
                              SecretKey, Passphrase);



            #endregion

            #region Freizeit

            //new OverpassQuery(JenaId).
            //    WithNodes("leisure", "hackerspace").
            //    ToFile("hackerspaces.json");

            #endregion

            #region Gebäude

            new OverpassQuery(JenaId).
                WithAny      ("building").
                ToFile       ("Gebäude/building.json").
                RunNow();

            #endregion

            #region Bundestagswahlkreise

            new OverpassQuery().
                WithRelations("boundary", "political").
                And          ("name",     "Gera - Jena - Saale-Holzland-Kreis").
                RunAll       ("Bundestagswahlkreise/Bundestagswahlkreise.json",
                              SecretKey, Passphrase);

            #endregion

            #region Ortsteile

            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                RunAll       ("Ortsteile/Ortsteile.json",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                ToGeoJSON    ().
                SplitFeatures().
                ToGeoJSONFile(JSON => "Ortsteile/" + JSON["features"][0]["properties"]["name"].ToString().Replace("/", "_").Replace(" ", "") + ".geojson").
                RunNow();

            #endregion

            #region Öffentlicher Nahverkehr

            // http://wiki.openstreetmap.org/wiki/DE:Relation:route
            // trolleybus, share_taxi

            new OverpassQuery(JenaId).
                WithRelations("route",   "bus").
                WithNodes    ("highway", "bus_stop").
                RunAll       ("ÖffentlicherNahverkehr/Buslinien",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithRelations("route",   "tram").
                WithNodes    ("railway", "tram_stop").
                RunAll       ("ÖffentlicherNahverkehr/Strassenbahnen",
                              SecretKey, Passphrase);

            #endregion

            #region Strassen

            // highway (nodes/relations)

            new OverpassQuery(JenaId).
                WithNodes    ("highway").
                RunAll       ("Strassen/highway-nodes",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                RunAll       ("Strassen/highway-ways",
                              SecretKey, Passphrase);

            #endregion

            #region Tempolimits

            new OverpassQuery(JenaId).
                WithNodes    ("highway", "speed_camera").
                ToFile       ("Tempolimits/Blitzer.json").
                ToGeoJSONFile("Tempolimits/Blitzer.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "5").
                ToFile       ("Tempolimits/Tempo5.json").
                ToGeoJSONFile("Tempolimits/Tempo5.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "6").
                ToFile       ("Tempolimits/Tempo6.json").
                ToGeoJSONFile("Tempolimits/Tempo6.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "10").
                ToFile       ("Tempolimits/Tempo10.json").
                ToGeoJSONFile("Tempolimits/Tempo10.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "20").
                ToFile       ("Tempolimits/Tempo20.json").
                ToGeoJSONFile("Tempolimits/Tempo20.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "30").
                ToFile       ("Tempolimits/Tempo30.json").
                ToGeoJSONFile("Tempolimits/Tempo30.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "40").
                ToFile       ("Tempolimits/Tempo40.json").
                ToGeoJSONFile("Tempolimits/Tempo40.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "50").
                ToFile       ("Tempolimits/Tempo50.json").
                ToGeoJSONFile("Tempolimits/Tempo50.geojson").
                RunNow();


            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "60").
                ToFile       ("Tempolimits/Tempo60.json").
                ToGeoJSONFile("Tempolimits/Tempo60.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "70").
                ToFile       ("Tempolimits/Tempo70.json").
                ToGeoJSONFile("Tempolimits/Tempo70.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "80").
                ToFile       ("Tempolimits/Tempo80.json").
                ToGeoJSONFile("Tempolimits/Tempo80.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "90").
                ToFile       ("Tempolimits/Tempo90.json").
                ToGeoJSONFile("Tempolimits/Tempo90.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "100").
                ToFile       ("Tempolimits/Tempo100.json").
                ToGeoJSONFile("Tempolimits/Tempo100.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "110").
                ToFile       ("Tempolimits/Tempo110.json").
                ToGeoJSONFile("Tempolimits/Tempo110.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "120").
                ToFile       ("Tempolimits/Tempo120.json").
                ToGeoJSONFile("Tempolimits/Tempo120.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "130").
                ToFile       ("Tempolimits/Tempo130.json").
                ToGeoJSONFile("Tempolimits/Tempo130.geojson").
                RunNow();

            #endregion

            #region Energie

            // highway (nodes/relations)

            new OverpassQuery(JenaId).
                WithNodes    ("power", "generator").
                WithWays     ("power", "generator").

                WithNodes    ("power", "plant").
                WithWays     ("power", "plant").

                WithNodes    ("power", "station").
                WithNodes    ("power", "sub_station").
                WithNodes    ("power", "sub_station").
                WithWays     ("power", "station").
                WithWays     ("power", "substation").
                WithWays     ("power", "sub_station").

                WithNodes    ("power", "switch").
                WithNodes    ("power", "switchgear").
                WithWays     ("power", "switch").
                WithWays     ("power", "switchgear").

                WithNodes    ("power", "transformer").
                WithWays     ("power", "transformer").

                ToFile       ("Energie/Strom.json").
                ToGeoJSONFile("Energie/Strom.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithNodes    ("power", "pole").
                WithNodes    ("power", "tower").
                WithNodes    ("power", "cable_distribution_cabinet").
                WithWays     ("power", "minor_line").
                WithWays     ("power", "line").
                WithWays     ("power", "lines").
                ToFile       ("Energie/Stromleitungen.json").
                ToGeoJSONFile("Energie/Stromleitungen.geojson").
                RunNow();

            #endregion


            #region Flächennutzung / Grünfläche

            // landuse  = farm, farmyard, farmland, forest, garden, grass, green, greenfield, meadow, scrub, village_green, cemetery, allotments, orchard, vineyard
            // natural  = grass, grassland, heath, marsh, meadow, tree, tree_row, scrub, wood, beach, cave_entrance, cliff, peak, sand, scree, sinkhole, spring, water, wetland
            // wood     = wood
            // leisure  = garden, golf_course, nature_reserve, park, pitch, village_green, recreation_ground, playground, wildlife_hide

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farm").
                ToFile       ("Flächennutzung/Grünfläche/landuse.farm.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.farm.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmyard").
                ToFile       ("Flächennutzung/Grünfläche/landuse.farmyard.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.farmyard.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmland").
                ToFile       ("Flächennutzung/Grünfläche/landuse.farmland.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.farmland.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmland").
                ToFile       ("Flächennutzung/Grünfläche/landuse.farmland.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.farmland.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "forest").
                ToFile       ("Flächennutzung/Grünfläche/landuse.forest.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.forest.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "garden").
                ToFile       ("Flächennutzung/Grünfläche/landuse.garden.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.garden.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "grass").
                ToFile       ("Flächennutzung/Grünfläche/landuse.grass.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.grass.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "green").
                ToFile       ("Flächennutzung/Grünfläche/landuse.green.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.green.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "greenfield").
                ToFile       ("Flächennutzung/Grünfläche/landuse.greenfield.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.greenfield.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "meadow").
                ToFile       ("Flächennutzung/Grünfläche/landuse.meadow.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.meadow.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "scrub").
                ToFile       ("Flächennutzung/Grünfläche/landuse.scrub.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.scrub.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "village_green").
                ToFile       ("Flächennutzung/Grünfläche/landuse.village_green.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.village_green.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "cemetery").
                ToFile       ("Flächennutzung/Grünfläche/landuse.cemetery.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.cemetery.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "allotments").
                ToFile       ("Flächennutzung/Grünfläche/landuse.allotments.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.allotments.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "orchard").
                ToFile       ("Flächennutzung/Grünfläche/landuse.orchard.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.orchard.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "vineyard").
                ToFile       ("Flächennutzung/Grünfläche/landuse.vineyard.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.vineyard.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("natural", "grass").
                ToFile       ("Flächennutzung/Grünfläche/natural.grass.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.grass.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "grassland").
                ToFile       ("Flächennutzung/Grünfläche/natural.grassland.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.grassland.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "heath").
                ToFile       ("Flächennutzung/Grünfläche/natural.heath.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.heath.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "marsh").
                ToFile       ("Flächennutzung/Grünfläche/natural.marsh.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.marsh.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "meadow").
                ToFile       ("Flächennutzung/Grünfläche/natural.meadow.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.meadow.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "tree").
                ToFile       ("Flächennutzung/Grünfläche/natural.tree.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.tree.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "tree_row").
                ToFile       ("Flächennutzung/Grünfläche/natural.tree_row.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.tree_row.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "scrub").
                ToFile       ("Flächennutzung/Grünfläche/natural.scrub.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.scrub.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "wood").
                ToFile       ("Flächennutzung/Grünfläche/natural.wood.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.wood.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "beach").
                ToFile       ("Flächennutzung/Grünfläche/natural.beach.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.beach.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "cave_entrance").
                ToFile       ("Flächennutzung/Grünfläche/natural.cave_entrance.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.cave_entrance.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "cliff").
                ToFile       ("Flächennutzung/Grünfläche/natural.cliff.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.cliff.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "peak").
                ToFile       ("Flächennutzung/Grünfläche/natural.peak.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.peak.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "sand").
                ToFile       ("Flächennutzung/Grünfläche/natural.sand.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.sand.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "scree").
                ToFile       ("Flächennutzung/Grünfläche/natural.scree.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.scree.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "sinkhole").
                ToFile       ("Flächennutzung/Grünfläche/natural.sinkhole.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.sinkhole.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "spring").
                ToFile       ("Flächennutzung/Grünfläche/natural.spring.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.spring.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "water").
                ToFile       ("Flächennutzung/Grünfläche/natural.water.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.water.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("natural", "wetland").
                ToFile       ("Flächennutzung/Grünfläche/natural.wetland.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/natural.wetland.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("wood", "wood").
                ToFile       ("Flächennutzung/Grünfläche/wood.wood.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/wood.wood.geojson").
                RunNow();

            // ----------------------------------------------------------------------------
            // leisure  = , , , , , , , , , water_park, 

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "garden").
                ToFile       ("Flächennutzung/Grünfläche/leisure.garden.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.garden.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "golf_course").
                ToFile       ("Flächennutzung/Grünfläche/leisure.golf_course.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.golf_course.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "nature_reserve").
                ToFile       ("Flächennutzung/Grünfläche/leisure.nature_reserve.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.nature_reserve.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "park").
                ToFile       ("Flächennutzung/Grünfläche/leisure.park.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.park.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "pitch").
                ToFile       ("Flächennutzung/Grünfläche/leisure.pitch.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.pitch.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "village_green").
                ToFile       ("Flächennutzung/Grünfläche/leisure.village_green.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.village_green.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "recreation_ground").
                ToFile       ("Flächennutzung/Grünfläche/leisure.recreation_ground.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.recreation_ground.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "wildlife_hide").
                ToFile       ("Flächennutzung/Grünfläche/leisure.wildlife_hide.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/leisure.wildlife_hide.geojson").
                RunNow();

            #endregion

            #region Flächennutzung / Wasser

            // WithAny("waterway").
            // cat waterway.json|grep "waterway"|sort|uniq

            // waterway  = river, riverbank, stream, wadi, canal, drain, ditch, dam, weir, lock_gate, fish_pass, mooring, waterfall
            // natural   = water
            // landuse   = water, reservoir, basin
            // leisure   = swimming_pool, water_park

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "river").
                ToFile       ("Flächennutzung/Wasser/waterway.river.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.river.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "riverbank").
                ToFile       ("Flächennutzung/Wasser/waterway.riverbank.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.riverbank.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "stream").
                ToFile       ("Flächennutzung/Wasser/waterway.stream.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.stream.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "wadi").
                ToFile       ("Flächennutzung/Wasser/waterway.wadi.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.wadi.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "canal").
                ToFile       ("Flächennutzung/Wasser/waterway.canal.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.canal.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "drain").
                ToFile       ("Flächennutzung/Wasser/waterway.drain.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.drain.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "ditch").
                ToFile       ("Flächennutzung/Wasser/waterway.ditch.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.ditch.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "dam").
                ToFile       ("Flächennutzung/Wasser/waterway.dam.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.dam.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "weir").
                ToFile       ("Flächennutzung/Wasser/waterway.weir.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.weir.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "lock_gate").
                ToFile       ("Flächennutzung/Wasser/waterway.lock_gate.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.lock_gate.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "fish_pass").
                ToFile       ("Flächennutzung/Wasser/waterway.fish_pass.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.fish_pass.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "mooring").
                ToFile       ("Flächennutzung/Wasser/waterway.mooring.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.mooring.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "waterfall").
                ToFile       ("Flächennutzung/Wasser/waterway.waterfall.json").
                ToGeoJSONFile("Flächennutzung/Wasser/waterway.waterfall.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("natural", "water").
                ToFile       ("Flächennutzung/Wasser/natural.water.json").
                ToGeoJSONFile("Flächennutzung/Wasser/natural.water.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "water").
                ToFile       ("Flächennutzung/Wasser/landuse.water.json").
                ToGeoJSONFile("Flächennutzung/Wasser/landuse.water.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "basin").
                ToFile       ("Flächennutzung/Wasser/landuse.basin.json").
                ToGeoJSONFile("Flächennutzung/Wasser/landuse.basin.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "reservoir").
                ToFile       ("Flächennutzung/Wasser/landuse.reservoir.json").
                ToGeoJSONFile("Flächennutzung/Wasser/landuse.reservoir.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "swimming_pool").
                ToFile       ("Flächennutzung/Wasser/leisure.swimming_pool.json").
                ToGeoJSONFile("Flächennutzung/Wasser/leisure.swimming_pool.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "water_park").
                ToFile       ("Flächennutzung/Wasser/leisure.water_park.json").
                ToGeoJSONFile("Flächennutzung/Wasser/leisure.water_park.geojson").
                RunNow();

            #endregion

            #region Flächennutzung / Gewerbe

            // landuse   = industrial, commercial, retail, brownfield, quarry, railway
            // leisure   = dance, fitness_centre, gambling, horse_riding, pitch, sports_centre, stadium, miniature_golf, slipway, tanning_salon, track
            // waterway  = dock, boatyard

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "industrial").
                ToFile       ("Flächennutzung/Gewerbe/landuse.industrial.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.industrial.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "commercial").
                ToFile       ("Flächennutzung/Gewerbe/landuse.commercial.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.commercial.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "retail").
                ToFile       ("Flächennutzung/Gewerbe/landuse.retail.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.retail.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "brownfield").
                ToFile       ("Flächennutzung/Gewerbe/landuse.brownfield.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.brownfield.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "quarry").
                ToFile       ("Flächennutzung/Gewerbe/landuse.quarry.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.quarry.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "railway").
                ToFile       ("Flächennutzung/Gewerbe/landuse.railway.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/landuse.railway.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "dance").
                ToFile       ("Flächennutzung/Gewerbe/leisure.dance.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.dance.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "fitness_centre").
                ToFile       ("Flächennutzung/Gewerbe/leisure.fitness_centre.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.fitness_centre.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "dance").
                ToFile       ("Flächennutzung/Gewerbe/leisure.dance.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.dance.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "gambling").
                ToFile       ("Flächennutzung/Gewerbe/leisure.gambling.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.gambling.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "horse_riding").
                ToFile       ("Flächennutzung/Gewerbe/leisure.horse_riding.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.horse_riding.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "pitch").
                ToFile       ("Flächennutzung/Gewerbe/leisure.pitch.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.pitch.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "sports_centre").
                ToFile       ("Flächennutzung/Gewerbe/leisure.sports_centre.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.sports_centre.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "stadium").
                ToFile       ("Flächennutzung/Gewerbe/leisure.stadium.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.stadium.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "miniature_golf").
                ToFile       ("Flächennutzung/Gewerbe/leisure.miniature_golf.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.miniature_golf.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "slipway").
                ToFile       ("Flächennutzung/Gewerbe/leisure.slipway.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.slipway.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "tanning_salon").
                ToFile       ("Flächennutzung/Gewerbe/leisure.tanning_salon.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.tanning_salon.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "track").
                ToFile       ("Flächennutzung/Gewerbe/leisure.track.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/leisure.track.geojson").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "dock").
                ToFile       ("Flächennutzung/Gewerbe/waterway.dock.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/waterway.dock.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "boatyard").
                ToFile       ("Flächennutzung/Gewerbe/waterway.boatyard.json").
                ToGeoJSONFile("Flächennutzung/Gewerbe/waterway.boatyard.geojson").
                RunNow();

            #endregion

            #region Flächennutzung / Wohngebiete

            // landuse      = residential, garages
            // residential  = *

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "residential").
                ToFile       ("Flächennutzung/Wohngebiete/landuse.residential.json").
                ToGeoJSONFile("Flächennutzung/Wohngebiete/landuse.residential.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "garages").
                ToFile       ("Flächennutzung/Wohngebiete/landuse.garages.json").
                ToGeoJSONFile("Flächennutzung/Wohngebiete/landuse.garages.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("residential", "*").
                ToFile       ("Flächennutzung/Wohngebiete/residential.json").
                ToGeoJSONFile("Flächennutzung/Wohngebiete/residential.geojson").
                RunNow();

            #endregion

            #region Flächennutzung / Militär

            // landuse   = military
            // military  = *

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "military").
                ToFile       ("Flächennutzung/Militär/landuse.military.json").
                ToGeoJSONFile("Flächennutzung/Militär/landuse.military.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("military").
                ToFile       ("Flächennutzung/Militär/military.json").
                ToGeoJSONFile("Flächennutzung/Militär/military.geojson").
                RunNow();

            #endregion


            Console.WriteLine("ready...");
            Console.ReadLine();

        }

    }

}
