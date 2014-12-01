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
using System.Text;

using org.GraphDefined.OpenDataAPI.OverpassAPI;

#endregion

namespace org.GraphDefined.OpenDataAPI.OSMImporter
{

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

            var JenaId = new OverpassQuery("Jena").AreaId;

            //new OverpassQuery(JenaId).
            //    WithNodes("leisure", "hackerspace").
            //    ToFile("hackerspaces.json");

            #region Bundestagswahlkreise

            new OverpassQuery().
                WithRelations("boundary", "political").
                And          ("name",     "Gera - Jena - Saale-Holzland-Kreis").
                ToFile       ("Bundestagswahlkreise/Bundestagswahlkreise.json").
                ToGeoJSONFile("Bundestagswahlkreise/Bundestagswahlkreise.geojson").
                RunNow();

            #endregion

            #region Ortsteile

            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                WithRelations("boundary",    "administrative").
                And          ("name",        "Ilmnitz").        // Should be fixed within the OSM soon!
                ToFile       ("Ortsteile/Ortsteile.json").
                ToGeoJSONFile("Ortsteile/Ortsteile.geojson").
                RunNow();

            #endregion

            #region Öffentlicher Nahverkehr

            // http://wiki.openstreetmap.org/wiki/DE:Relation:route
            // trolleybus, share_taxi

            new OverpassQuery(JenaId).
                WithRelations("route",   "bus").
                WithNodes    ("highway", "bus_stop").
                ToFile       ("ÖffentlicherNahverkehr/Buslinien.json").
                ToGeoJSONFile("ÖffentlicherNahverkehr/Buslinien.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithRelations("route",   "tram").
                WithNodes    ("railway", "tram_stop").
                ToFile       ("ÖffentlicherNahverkehr/Strassenbahnen.json").
                ToGeoJSONFile("ÖffentlicherNahverkehr/Strassenbahnen.geojson").
                RunNow();

            #endregion

            #region Strassen

            // highway (nodes/relations)

            new OverpassQuery(JenaId).
                WithNodes    ("highway").
                ToFile       ("Strassen/highway-nodes.json").
                ToGeoJSONFile("Strassen/highway-nodes.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                ToFile       ("Strassen/highway-ways.json").
                ToGeoJSONFile("Strassen/highway-ways.geojson").
                RunNow();

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
                WithAny("landuse", "green").
                ToFile("Flächennutzung/Grünfläche/landuse.green.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "greenfield").
                ToFile("Flächennutzung/Grünfläche/landuse.greenfield.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "meadow").
                ToFile       ("Flächennutzung/Grünfläche/landuse.meadow.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.meadow.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "scrub").
                ToFile("Flächennutzung/Grünfläche/landuse.scrub.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "village_green").
                ToFile("Flächennutzung/Grünfläche/landuse.village_green.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "cemetery").
                ToFile("Flächennutzung/Grünfläche/landuse.cemetery.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "allotments").
                ToFile       ("Flächennutzung/Grünfläche/landuse.allotments.json").
                ToGeoJSONFile("Flächennutzung/Grünfläche/landuse.allotments.geojson").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "orchard").
                ToFile("Flächennutzung/Grünfläche/landuse.orchard.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "vineyard").
                ToFile("Flächennutzung/Grünfläche/landuse.vineyard.json").
                ToGeoJSON().
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("natural", "grass").
                ToFile("Flächennutzung/Grünfläche/natural.grass.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "grassland").
                ToFile("Flächennutzung/Grünfläche/natural.grassland.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "heath").
                ToFile("Flächennutzung/Grünfläche/natural.heath.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "marsh").
                ToFile("Flächennutzung/Grünfläche/natural.marsh.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "meadow").
                ToFile("Flächennutzung/Grünfläche/natural.meadow.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "tree").
                ToFile("Flächennutzung/Grünfläche/natural.tree.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "tree_row").
                ToFile("Flächennutzung/Grünfläche/natural.tree_row.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "scrub").
                ToFile("Flächennutzung/Grünfläche/natural.scrub.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "wood").
                ToFile("Flächennutzung/Grünfläche/natural.wood.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "beach").
                ToFile("Flächennutzung/Grünfläche/natural.beach.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "cave_entrance").
                ToFile("Flächennutzung/Grünfläche/natural.cave_entrance.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "cliff").
                ToFile("Flächennutzung/Grünfläche/natural.cliff.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "peak").
                ToFile("Flächennutzung/Grünfläche/natural.peak.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "sand").
                ToFile("Flächennutzung/Grünfläche/natural.sand.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "scree").
                ToFile("Flächennutzung/Grünfläche/natural.scree.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "sinkhole").
                ToFile("Flächennutzung/Grünfläche/natural.sinkhole.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "spring").
                ToFile("Flächennutzung/Grünfläche/natural.spring.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "water").
                ToFile("Flächennutzung/Grünfläche/natural.water.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("natural", "wetland").
                ToFile("Flächennutzung/Grünfläche/natural.wetland.json").
                ToGeoJSON().
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("wood", "wood").
                ToFile("Flächennutzung/Grünfläche/wood.wood.json").
                ToGeoJSON().
                RunNow();

            // ----------------------------------------------------------------------------
            // leisure  = , , , , , , , , , water_park, 

            new OverpassQuery(JenaId).
                WithAny("leisure", "garden").
                ToFile("Flächennutzung/Grünfläche/leisure.garden.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "golf_course").
                ToFile("Flächennutzung/Grünfläche/leisure.golf_course.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "nature_reserve").
                ToFile("Flächennutzung/Grünfläche/leisure.nature_reserve.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "park").
                ToFile("Flächennutzung/Grünfläche/leisure.park.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "pitch").
                ToFile("Flächennutzung/Grünfläche/leisure.pitch.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "village_green").
                ToFile("Flächennutzung/Grünfläche/leisure.village_green.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "recreation_ground").
                ToFile("Flächennutzung/Grünfläche/leisure.recreation_ground.json").
                ToGeoJSON().
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "wildlife_hide").
                ToFile("Flächennutzung/Grünfläche/leisure.wildlife_hide.json").
                ToGeoJSON().
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
                WithAny("waterway", "river").
                ToFile("Flächennutzung/Wasser/waterway.river.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "riverbank").
                ToFile("Flächennutzung/Wasser/waterway.riverbank.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "stream").
                ToFile("Flächennutzung/Wasser/waterway.stream.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "wadi").
                ToFile("Flächennutzung/Wasser/waterway.wadi.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "canal").
                ToFile("Flächennutzung/Wasser/waterway.canal.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "drain").
                ToFile("Flächennutzung/Wasser/waterway.drain.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "ditch").
                ToFile("Flächennutzung/Wasser/waterway.ditch.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "dam").
                ToFile("Flächennutzung/Wasser/waterway.dam.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "weir").
                ToFile("Flächennutzung/Wasser/waterway.weir.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "lock_gate").
                ToFile("Flächennutzung/Wasser/waterway.lock_gate.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "fish_pass").
                ToFile("Flächennutzung/Wasser/waterway.fish_pass.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "mooring").
                ToFile("Flächennutzung/Wasser/waterway.mooring.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "waterfall").
                ToFile("Flächennutzung/Wasser/waterway.waterfall.json").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("natural", "water").
                ToFile("Flächennutzung/Wasser/natural.water.json").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("landuse", "water").
                ToFile("Flächennutzung/Wasser/landuse.water.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "basin").
                ToFile("Flächennutzung/Wasser/landuse.basin.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "reservoir").
                ToFile("Flächennutzung/Wasser/landuse.reservoir.json").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("leisure", "swimming_pool").
                ToFile("Flächennutzung/Wasser/leisure.swimming_pool.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "water_park").
                ToFile("Flächennutzung/Wasser/leisure.water_park.json").
                RunNow();

            #endregion

            #region Flächennutzung / Gewerbe

            // landuse   = industrial, commercial, retail, brownfield, quarry, railway
            // leisure   = dance, fitness_centre, gambling, horse_riding, pitch, sports_centre, stadium, miniature_golf, slipway, tanning_salon, track
            // waterway  = dock, boatyard

            new OverpassQuery(JenaId).
                WithAny("landuse", "industrial").
                ToFile("Flächennutzung/Gewerbe/landuse.industrial.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "commercial").
                ToFile("Flächennutzung/Gewerbe/landuse.commercial.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "retail").
                ToFile("Flächennutzung/Gewerbe/landuse.retail.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "brownfield").
                ToFile("Flächennutzung/Gewerbe/landuse.brownfield.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "quarry").
                ToFile("Flächennutzung/Gewerbe/landuse.quarry.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "railway").
                ToFile("Flächennutzung/Gewerbe/landuse.railway.json").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("leisure", "dance").
                ToFile("Flächennutzung/Gewerbe/leisure.dance.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "fitness_centre").
                ToFile("Flächennutzung/Gewerbe/leisure.fitness_centre.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "dance").
                ToFile("Flächennutzung/Gewerbe/leisure.dance.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "gambling").
                ToFile("Flächennutzung/Gewerbe/leisure.gambling.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "horse_riding").
                ToFile("Flächennutzung/Gewerbe/leisure.horse_riding.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "pitch").
                ToFile("Flächennutzung/Gewerbe/leisure.pitch.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "sports_centre").
                ToFile("Flächennutzung/Gewerbe/leisure.sports_centre.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "stadium").
                ToFile("Flächennutzung/Gewerbe/leisure.stadium.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "miniature_golf").
                ToFile("Flächennutzung/Gewerbe/leisure.miniature_golf.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "slipway").
                ToFile("Flächennutzung/Gewerbe/leisure.slipway.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "tanning_salon").
                ToFile("Flächennutzung/Gewerbe/leisure.tanning_salon.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("leisure", "track").
                ToFile("Flächennutzung/Gewerbe/leisure.track.json").
                RunNow();

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny("waterway", "dock").
                ToFile("Flächennutzung/Gewerbe/waterway.dock.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("waterway", "boatyard").
                ToFile("Flächennutzung/Gewerbe/waterway.boatyard.json").
                RunNow();

            #endregion

            #region Flächennutzung / Wohngebiete

            // landuse      = residential, garages
            // residential  = *

            new OverpassQuery(JenaId).
                WithAny("landuse", "residential").
                ToFile("Flächennutzung/Wohngebiete/landuse.residential.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("landuse", "garages").
                ToFile("Flächennutzung/Wohngebiete/landuse.garages.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("residential", "*").
                ToFile("Flächennutzung/Wohngebiete/residential.json").
                RunNow();

            #endregion

            #region Flächennutzung / Militär

            // landuse   = military
            // military  = *

            new OverpassQuery(JenaId).
                WithAny("landuse", "military").
                ToFile("Flächennutzung/Militär/landuse.military.json").
                RunNow();

            new OverpassQuery(JenaId).
                WithAny("military").
                ToFile("Flächennutzung/Militär/military.json").
                RunNow();

            #endregion


            Console.WriteLine("ready...");
            Console.ReadLine();

        }

    }

}
