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

            Directory.CreateDirectory("Flächennutzung");
            Directory.CreateDirectory("Flächennutzung/Grünfläche");
            Directory.CreateDirectory("Flächennutzung/Wasser");
            Directory.CreateDirectory("Flächennutzung/Gewerbe");
            Directory.CreateDirectory("Flächennutzung/Wohngebiete");
            Directory.CreateDirectory("Flächennutzung/Militär");
            Directory.CreateDirectory("Strassen");
            Directory.CreateDirectory("ÖffentlicherNahverkehr");

            var JenaId = new OverpassQuery("Jena").AreaId;

            //new OverpassQuery(JenaId).
            //    WithNodes("leisure", "hackerspace").
            //    ToFile("hackerspaces.json");

            #region Buslinien

            //new OverpassQuery(JenaId).
            //    WithAny("highway", "bus_stop").
            //    ToFile("ÖffentlicherNahverkehr/highway.bus_stop.json").
            //    ToGeoJSONFile("ÖffentlicherNahverkehr/highway.bus_stop.geojson").
            //    RunNow();

            #endregion

            #region Strassenbahnen

            //new OverpassQuery(JenaId).
            //    WithAny("railway", "tram_stop").
            //    ToFile       ("ÖffentlicherNahverkehr/railway.tram_stop.json").
            //    ToGeoJSONFile("ÖffentlicherNahverkehr/railway.tram_stop.geojson").
            //    RunNow();

            // http://wiki.openstreetmap.org/wiki/DE:Relation:route

            new OverpassQuery(JenaId).
                WithRelations("route",   "tram").
                WithNodes    ("railway", "tram_stop").
                ToFile       ("ÖffentlicherNahverkehr/route.tram.json").
                ToGeoJSONFile("ÖffentlicherNahverkehr/route.tram.geojson").
                RunNow();

            #endregion

            //#region Flächennutzung / Grünfläche

            //// landuse  = farm, farmyard, farmland, forest, garden, grass, green, greenfield, meadow, scrub, village_green, cemetery, allotments, orchard, vineyard
            //// natural  = grass, grassland, heath, marsh, meadow, tree, tree_row, scrub, wood, beach, cave_entrance, cliff, peak, sand, scree, sinkhole, spring, water, wetland
            //// wood     = wood
            //// leisure  = garden, golf_course, nature_reserve, park, pitch, village_green, recreation_ground, playground, wildlife_hide

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "farm").
            //    ToFile("Flächennutzung/Grünfläche/landuse.farm.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "farmyard").
            //    ToFile("Flächennutzung/Grünfläche/landuse.farmyard.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "farmland").
            //    ToFile("Flächennutzung/Grünfläche/landuse.farmland.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "forest").
            //    ToFile("Flächennutzung/Grünfläche/landuse.forest.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "garden").
            //    ToFile("Flächennutzung/Grünfläche/landuse.garden.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "grass").
            //    ToFile("Flächennutzung/Grünfläche/landuse.grass.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "green").
            //    ToFile("Flächennutzung/Grünfläche/landuse.green.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "greenfield").
            //    ToFile("Flächennutzung/Grünfläche/landuse.greenfield.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "meadow").
            //    ToFile("Flächennutzung/Grünfläche/landuse.meadow.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "scrub").
            //    ToFile("Flächennutzung/Grünfläche/landuse.scrub.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "village_green").
            //    ToFile("Flächennutzung/Grünfläche/landuse.village_green.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "cemetery").
            //    ToFile("Flächennutzung/Grünfläche/landuse.cemetery.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "allotments").
            //    ToFile("Flächennutzung/Grünfläche/landuse.allotments.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "orchard").
            //    ToFile("Flächennutzung/Grünfläche/landuse.orchard.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "vineyard").
            //    ToFile("Flächennutzung/Grünfläche/landuse.vineyard.json").
            //    ToGeoJSON().
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "grass").
            //    ToFile("Flächennutzung/Grünfläche/natural.grass.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "grassland").
            //    ToFile("Flächennutzung/Grünfläche/natural.grassland.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "heath").
            //    ToFile("Flächennutzung/Grünfläche/natural.heath.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "marsh").
            //    ToFile("Flächennutzung/Grünfläche/natural.marsh.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "meadow").
            //    ToFile("Flächennutzung/Grünfläche/natural.meadow.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "tree").
            //    ToFile("Flächennutzung/Grünfläche/natural.tree.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "tree_row").
            //    ToFile("Flächennutzung/Grünfläche/natural.tree_row.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "scrub").
            //    ToFile("Flächennutzung/Grünfläche/natural.scrub.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "wood").
            //    ToFile("Flächennutzung/Grünfläche/natural.wood.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "beach").
            //    ToFile("Flächennutzung/Grünfläche/natural.beach.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "cave_entrance").
            //    ToFile("Flächennutzung/Grünfläche/natural.cave_entrance.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "cliff").
            //    ToFile("Flächennutzung/Grünfläche/natural.cliff.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "peak").
            //    ToFile("Flächennutzung/Grünfläche/natural.peak.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "sand").
            //    ToFile("Flächennutzung/Grünfläche/natural.sand.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "scree").
            //    ToFile("Flächennutzung/Grünfläche/natural.scree.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "sinkhole").
            //    ToFile("Flächennutzung/Grünfläche/natural.sinkhole.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "spring").
            //    ToFile("Flächennutzung/Grünfläche/natural.spring.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "water").
            //    ToFile("Flächennutzung/Grünfläche/natural.water.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "wetland").
            //    ToFile("Flächennutzung/Grünfläche/natural.wetland.json").
            //    ToGeoJSON().
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("wood", "wood").
            //    ToFile("Flächennutzung/Grünfläche/wood.wood.json").
            //    ToGeoJSON().
            //    RunNow();

            //// ----------------------------------------------------------------------------
            //// leisure  = , , , , , , , , , water_park, 

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "garden").
            //    ToFile("Flächennutzung/Grünfläche/leisure.garden.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "golf_course").
            //    ToFile("Flächennutzung/Grünfläche/leisure.golf_course.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "nature_reserve").
            //    ToFile("Flächennutzung/Grünfläche/leisure.nature_reserve.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "park").
            //    ToFile("Flächennutzung/Grünfläche/leisure.park.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "pitch").
            //    ToFile("Flächennutzung/Grünfläche/leisure.pitch.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "village_green").
            //    ToFile("Flächennutzung/Grünfläche/leisure.village_green.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "recreation_ground").
            //    ToFile("Flächennutzung/Grünfläche/leisure.recreation_ground.json").
            //    ToGeoJSON().
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "wildlife_hide").
            //    ToFile("Flächennutzung/Grünfläche/leisure.wildlife_hide.json").
            //    ToGeoJSON().
            //    RunNow();

            //#endregion

            //#region Flächennutzung / Wasser

            //// WithAny("waterway").
            //// cat waterway.json|grep "waterway"|sort|uniq

            //// waterway  = river, riverbank, stream, wadi, canal, drain, ditch, dam, weir, lock_gate, fish_pass, mooring, waterfall
            //// natural   = water
            //// landuse   = water, reservoir, basin
            //// leisure   = swimming_pool, water_park

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "river").
            //    ToFile("Flächennutzung/Wasser/waterway.river.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "riverbank").
            //    ToFile("Flächennutzung/Wasser/waterway.riverbank.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "stream").
            //    ToFile("Flächennutzung/Wasser/waterway.stream.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "wadi").
            //    ToFile("Flächennutzung/Wasser/waterway.wadi.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "canal").
            //    ToFile("Flächennutzung/Wasser/waterway.canal.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "drain").
            //    ToFile("Flächennutzung/Wasser/waterway.drain.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "ditch").
            //    ToFile("Flächennutzung/Wasser/waterway.ditch.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "dam").
            //    ToFile("Flächennutzung/Wasser/waterway.dam.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "weir").
            //    ToFile("Flächennutzung/Wasser/waterway.weir.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "lock_gate").
            //    ToFile("Flächennutzung/Wasser/waterway.lock_gate.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "fish_pass").
            //    ToFile("Flächennutzung/Wasser/waterway.fish_pass.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "mooring").
            //    ToFile("Flächennutzung/Wasser/waterway.mooring.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "waterfall").
            //    ToFile("Flächennutzung/Wasser/waterway.waterfall.json").
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("natural", "water").
            //    ToFile("Flächennutzung/Wasser/natural.water.json").
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "water").
            //    ToFile("Flächennutzung/Wasser/landuse.water.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "basin").
            //    ToFile("Flächennutzung/Wasser/landuse.basin.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "reservoir").
            //    ToFile("Flächennutzung/Wasser/landuse.reservoir.json").
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "swimming_pool").
            //    ToFile("Flächennutzung/Wasser/leisure.swimming_pool.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "water_park").
            //    ToFile("Flächennutzung/Wasser/leisure.water_park.json").
            //    RunNow();

            //#endregion

            //#region Flächennutzung / Gewerbe

            //// landuse   = industrial, commercial, retail, brownfield, quarry, railway
            //// leisure   = dance, fitness_centre, gambling, horse_riding, pitch, sports_centre, stadium, miniature_golf, slipway, tanning_salon, track
            //// waterway  = dock, boatyard

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "industrial").
            //    ToFile("Flächennutzung/Gewerbe/landuse.industrial.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "commercial").
            //    ToFile("Flächennutzung/Gewerbe/landuse.commercial.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "retail").
            //    ToFile("Flächennutzung/Gewerbe/landuse.retail.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "brownfield").
            //    ToFile("Flächennutzung/Gewerbe/landuse.brownfield.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "quarry").
            //    ToFile("Flächennutzung/Gewerbe/landuse.quarry.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "railway").
            //    ToFile("Flächennutzung/Gewerbe/landuse.railway.json").
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "dance").
            //    ToFile("Flächennutzung/Gewerbe/leisure.dance.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "fitness_centre").
            //    ToFile("Flächennutzung/Gewerbe/leisure.fitness_centre.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "dance").
            //    ToFile("Flächennutzung/Gewerbe/leisure.dance.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "gambling").
            //    ToFile("Flächennutzung/Gewerbe/leisure.gambling.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "horse_riding").
            //    ToFile("Flächennutzung/Gewerbe/leisure.horse_riding.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "pitch").
            //    ToFile("Flächennutzung/Gewerbe/leisure.pitch.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "sports_centre").
            //    ToFile("Flächennutzung/Gewerbe/leisure.sports_centre.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "stadium").
            //    ToFile("Flächennutzung/Gewerbe/leisure.stadium.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "miniature_golf").
            //    ToFile("Flächennutzung/Gewerbe/leisure.miniature_golf.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "slipway").
            //    ToFile("Flächennutzung/Gewerbe/leisure.slipway.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "tanning_salon").
            //    ToFile("Flächennutzung/Gewerbe/leisure.tanning_salon.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("leisure", "track").
            //    ToFile("Flächennutzung/Gewerbe/leisure.track.json").
            //    RunNow();

            //// ----------------------------------------------------------------------------

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "dock").
            //    ToFile("Flächennutzung/Gewerbe/waterway.dock.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("waterway", "boatyard").
            //    ToFile("Flächennutzung/Gewerbe/waterway.boatyard.json").
            //    RunNow();

            //#endregion

            //#region Flächennutzung / Wohngebiete

            //// landuse      = residential, garages
            //// residential  = *

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "residential").
            //    ToFile("Flächennutzung/Wohngebiete/landuse.residential.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "garages").
            //    ToFile("Flächennutzung/Wohngebiete/landuse.garages.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("residential", "*").
            //    ToFile("Flächennutzung/Wohngebiete/residential.json").
            //    RunNow();

            //#endregion

            //#region Flächennutzung / Militär

            //// landuse   = military
            //// military  = *

            //new OverpassQuery(JenaId).
            //    WithAny("landuse", "military").
            //    ToFile("Flächennutzung/Militär/landuse.military.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithAny("military").
            //    ToFile("Flächennutzung/Militär/military.json").
            //    RunNow();

            //#endregion

            //#region Strassen

            //// highway (nodes/relations)

            //new OverpassQuery(JenaId).
            //    WithNodes("highway").
            //    ToFile("Strassen/highway-nodes.json").
            //    RunNow();

            //new OverpassQuery(JenaId).
            //    WithWays("highway").
            //    ToFile("Strassen/highway-relations.json").
            //    RunNow();

            //#endregion

            Console.WriteLine("ready...");
            Console.ReadLine();

        }

    }

}
