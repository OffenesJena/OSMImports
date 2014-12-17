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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.OpenDataAPI.OverpassAPI;

using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;

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
                ContinueWith(task => Console.WriteLine(FilenamePrefix + ".* files are ready!")).
                Wait();

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
            var ThüringenId = new OverpassQuery("Thüringen").AreaId;
            //var SecretKey   = OpenGPG.ReadSecretKey(File.OpenRead("jod-test-secring.gpg"));
            //var Passphrase  = File.ReadAllText("jod-test-passphrase.txt");
            var SecretKey   = OpenGPG.ReadSecretKey(File.OpenRead("jod-official-secring.gpg"));
            var Passphrase  = File.ReadAllText("jod-official-passphrase.txt");

            Directory.CreateDirectory("Gemeinschaftsanlage");
            Directory.CreateDirectory("Freizeit");
            Directory.CreateDirectory("Gebäude");
            Directory.CreateDirectory("Bundestagswahlkreise");
            Directory.CreateDirectory("Gebietsgrenzen");
            Directory.CreateDirectory("Gebietsgrenzen/Ortsteile_Jena");
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

            // cat amenity.json|grep amenity|sed -e 's/,//g'|sort|uniq
            //new OverpassQuery(JenaId).
            //    WithAny      ("amenity").
            //    ToFile       ("Gemeinschaftsanlage/amenity.json").
            //    RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "Cocktailbar").
                RunAll       ("Gemeinschaftsanlage/amenity.cocktailbar",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "animal_shelter").
                RunAll       ("Gemeinschaftsanlage/amenity.animal_shelter",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "arts_centre").
                RunAll       ("Gemeinschaftsanlage/amenity.arts_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "atm").
                RunAll       ("Gemeinschaftsanlage/amenity.atm",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "bank").
                RunAll       ("Gemeinschaftsanlage/amenity.bank",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "bar").
                RunAll       ("Gemeinschaftsanlage/amenity.bar",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "bbq").
                RunAll       ("Gemeinschaftsanlage/amenity.bbq",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "bench").
                RunAll       ("Gemeinschaftsanlage/amenity.bench",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "bicycle_parking").
                RunAll       ("Gemeinschaftsanlage/amenity.bicycle_parking",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "biergarten").
                RunAll       ("Gemeinschaftsanlage/amenity.biergarten",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "brothel").
                RunAll       ("Gemeinschaftsanlage/amenity.brothel",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "cafe").
                RunAll       ("Gemeinschaftsanlage/amenity.cafe",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "canteen").
                RunAll       ("Gemeinschaftsanlage/amenity.canteen",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "car_rental").
                RunAll       ("Gemeinschaftsanlage/amenity.car_rental",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "car_sharing").
                RunAll       ("Gemeinschaftsanlage/amenity.car_sharing",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "car_wash").
                RunAll       ("Gemeinschaftsanlage/amenity.car_wash",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "cinema").
                RunAll       ("Gemeinschaftsanlage/amenity.cinema",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "clinic").
                RunAll       ("Gemeinschaftsanlage/amenity.clinic",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "clock").
                RunAll       ("Gemeinschaftsanlage/amenity.clock",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "college").
                RunAll       ("Gemeinschaftsanlage/amenity.college",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "community_centre").
                RunAll       ("Gemeinschaftsanlage/amenity.community_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "courthouse").
                RunAll       ("Gemeinschaftsanlage/amenity.courthouse",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "coworking_space").
                RunAll       ("Gemeinschaftsanlage/amenity.coworking_space",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "crematorium").
                RunAll       ("Gemeinschaftsanlage/amenity.crematorium",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "dentist").
                RunAll       ("Gemeinschaftsanlage/amenity.dentist",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "doctors").
                RunAll       ("Gemeinschaftsanlage/amenity.doctors",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "drinking_water").
                RunAll       ("Gemeinschaftsanlage/amenity.drinking_water",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "driving_school").
                RunAll       ("Gemeinschaftsanlage/amenity.driving_school",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "fast_food").
                RunAll       ("Gemeinschaftsanlage/amenity.fast_food",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "fire_station").
                RunAll       ("Gemeinschaftsanlage/amenity.fire_station",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "fountain").
                RunAll       ("Gemeinschaftsanlage/amenity.fountain",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "fuel").
                RunAll       ("Gemeinschaftsanlage/amenity.fuel",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "grave_yard").
                RunAll       ("Gemeinschaftsanlage/amenity.grave_yard",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "grit_bin").
                RunAll       ("Gemeinschaftsanlage/amenity.grit_bin",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "health_centre").
                RunAll       ("Gemeinschaftsanlage/amenity.health_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "hospital").
                RunAll       ("Gemeinschaftsanlage/amenity.hospital",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "hunting_stand").
                RunAll       ("Gemeinschaftsanlage/amenity.hunting_stand",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "kindergarten").
                RunAll       ("Gemeinschaftsanlage/amenity.kindergarten",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "library").
                RunAll       ("Gemeinschaftsanlage/amenity.library",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "marketplace").
                RunAll       ("Gemeinschaftsanlage/amenity.marketplace",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "motorcycle_parking").
                RunAll       ("Gemeinschaftsanlage/amenity.motorcycle_parking",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "nightclub").
                RunAll       ("Gemeinschaftsanlage/amenity.nightclub",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "nursing_home").
                RunAll       ("Gemeinschaftsanlage/amenity.nursing_home",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "observatory").
                RunAll       ("Gemeinschaftsanlage/amenity.observatory",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "orphanage").
                RunAll       ("Gemeinschaftsanlage/amenity.orphanage",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "parking").
                RunAll       ("Gemeinschaftsanlage/amenity.parking",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "parking_entrance").
                RunAll       ("Gemeinschaftsanlage/amenity.parking_entrance",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "pharmacy").
                RunAll       ("Gemeinschaftsanlage/amenity.pharmacy",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "place_of_worship").
                RunAll       ("Gemeinschaftsanlage/amenity.place_of_worship",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "planetarium").
                RunAll       ("Gemeinschaftsanlage/amenity.planetarium",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "police").
                RunAll       ("Gemeinschaftsanlage/amenity.police",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "post_box").
                RunAll       ("Gemeinschaftsanlage/amenity.post_box",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "post_office").
                RunAll       ("Gemeinschaftsanlage/amenity.post_office",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "pub").
                RunAll       ("Gemeinschaftsanlage/amenity.pub",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "public_building").
                RunAll       ("Gemeinschaftsanlage/amenity.public_building",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "recycling").
                RunAll       ("Gemeinschaftsanlage/amenity.recycling",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "register_office").
                RunAll       ("Gemeinschaftsanlage/amenity.register_office",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "restaurant").
                RunAll       ("Gemeinschaftsanlage/amenity.restaurant",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "sauna").
                RunAll       ("Gemeinschaftsanlage/amenity.sauna",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "school").
                RunAll       ("Gemeinschaftsanlage/amenity.school",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "shelter").
                RunAll       ("Gemeinschaftsanlage/amenity.shelter",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "social_facility").
                RunAll       ("Gemeinschaftsanlage/amenity.social_facility",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "spa").
                RunAll       ("Gemeinschaftsanlage/amenity.spa",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "stripclub").
                RunAll       ("Gemeinschaftsanlage/amenity.stripclub",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "swimming_pool").
                RunAll       ("Gemeinschaftsanlage/amenity.swimming_pool",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "taxi").
                RunAll       ("Gemeinschaftsanlage/amenity.taxi",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "telephone").
                RunAll       ("Gemeinschaftsanlage/amenity.telephone",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "theatre").
                RunAll       ("Gemeinschaftsanlage/amenity.theatre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "toilets").
                RunAll       ("Gemeinschaftsanlage/amenity.toilets",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "townhall").
                RunAll       ("Gemeinschaftsanlage/amenity.townhall",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "university").
                RunAll       ("Gemeinschaftsanlage/amenity.university",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "vending_machine").
                RunAll       ("Gemeinschaftsanlage/amenity.vending_machine",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "veterinary").
                RunAll       ("Gemeinschaftsanlage/amenity.veterinary",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("amenity", "waste_basket").
                RunAll       ("Gemeinschaftsanlage/amenity.waste_basket",
                              SecretKey, Passphrase);

            #endregion

            #region Freizeit

            //// cat leisure.json|grep leisure|sed -e 's/,//g'|sort|uniq
            //new OverpassQuery(JenaId).
            //    WithAny      ("leisure").
            //    ToFile       ("Freizeit/leisure.json").
            //    RunNow();

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "common").
                RunAll       ("Freizeit/leisure.common",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "dance").
                RunAll       ("Freizeit/leisure.dance",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "fitness_centre").
                RunAll       ("Freizeit/leisure.fitness_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "gambling").
                RunAll       ("Freizeit/leisure.gambling",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "garden").
                RunAll       ("Freizeit/leisure.garden",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "golf_course").
                RunAll       ("Freizeit/leisure.golf_course",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "hackerspace").
                RunAll       ("Freizeit/leisure.hackerspace",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "horse_riding").
                RunAll       ("Freizeit/leisure.horse_riding",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "nature_reserve").
                RunAll       ("Freizeit/leisure.nature_reserve",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "park").
                RunAll       ("Freizeit/leisure.park",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "pitch").
                RunAll       ("Freizeit/leisure.pitch",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "playground").
                RunAll       ("Freizeit/leisure.playground",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "slipway").
                RunAll       ("Freizeit/leisure.slipway",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "sports_centre").
                RunAll       ("Freizeit/leisure.sports_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "stadium").
                RunAll       ("Freizeit/leisure.stadium",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "swimming_pool").
                RunAll       ("Freizeit/leisure.swimming_pool",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "tanning_salon").
                RunAll       ("Freizeit/leisure.tanning_salon",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "track").
                RunAll       ("Freizeit/leisure.track",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "water_park").
                RunAll       ("Freizeit/leisure.water_park",
                              SecretKey, Passphrase);

            #endregion

            #region Gebäude

            new OverpassQuery(JenaId).
                WithAny      ("building").
                RunAll       ("Gebäude/building",
                              SecretKey, Passphrase);

            #endregion

            #region Bundestagswahlkreise

            new OverpassQuery().
                WithRelations("boundary", "political").
                And          ("name",     "Gera - Jena - Saale-Holzland-Kreis").
                RunAll       ("Bundestagswahlkreise/Bundestagswahlkreise.json",
                              SecretKey, Passphrase);

            #endregion

            #region Gebietsgrenzen

            // Thüringen
            // http://wiki.openstreetmap.org/wiki/DE:Gemeindegrenze
            // http://wiki.openstreetmap.org/wiki/DE:Grenze#Kommunale_Ebene_-_Ortsgrenzen_admin_level.3D7-9
            // http://kahla.de/cms/index.php?page=Gewerbesteuer-Allgemeine-Informationen
            // http://www.statistik.thueringen.de/datenbank/TabAnzeige.asp?tabelle=GE001613%7C%7CHebes%E4tze+der+Gemeinden&startpage=99&csv=&richtung=&sortiere=&vorspalte=0&tit2=&TIS=&SZDT=&anzahlH=-1&fontgr=12&mkro=&AnzeigeAuswahl=&XLS=&auswahlNr=&felder=0&felder=1&felder=2&zeit=2013%7C%7C99

            new OverpassQuery(ThüringenId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "4").
                RunAll       ("Gebietsgrenzen/Thüringen_Gesamt",
                              SecretKey, Passphrase);

            new OverpassQuery(ThüringenId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "6").
                RunAll       ("Gebietsgrenzen/Thüringen_Kreisgrenzen",
                              SecretKey, Passphrase);

            new OverpassQuery(ThüringenId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "7").
                RunAll       ("Gebietsgrenzen/Thüringen_Verwaltungsgemeinschaften",
                              SecretKey, Passphrase);

            new OverpassQuery(ThüringenId).
                // Bei admin_level = 8 fehlen die großen Städte!
                WithRelations("boundary",    "administrative").And("admin_level", "8").
                WithRelations("boundary",    "administrative").And("name",        "Eisenach").
                WithRelations("boundary",    "administrative").And("name",        "Erfurt").
                WithRelations("boundary",    "administrative").And("name",        "Weimar").
                WithRelations("boundary",    "administrative").And("name",        "Jena").
                WithRelations("boundary",    "administrative").And("name",        "Gera").
                WithRelations("boundary",    "administrative").And("name",        "Suhl").
                RunAll       ("Gebietsgrenzen/Thüringen_Gemeinden.json",
                              SecretKey, Passphrase);


            // Jena

            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                RunAll       ("Gebietsgrenzen/Ortsteile_Jena/Jena_Ortsteile",
                              SecretKey, Passphrase);

            // ToDo: Missing OpenGPG signing for splitted GeoJSON has to be implemented!
            new OverpassQuery(JenaId).
                WithRelations("boundary",    "administrative").
                And          ("admin_level", "9").
                ToGeoJSON    ().
                SplitFeatures().
                ToGeoJSONFile(JSON => "Gebietsgrenzen/Ortsteile_Jena/" + JSON["features"][0]["properties"]["name"].ToString().Replace("/", "_").Replace(" ", "") + ".geojson").
                RunNow();

            #endregion

            #region Öffentlicher Nahverkehr

            // http://wiki.openstreetmap.org/wiki/DE:Relation:route
            // trolleybus, share_taxi

            new OverpassQuery(JenaId).
                WithRelations("route",   "bus").
                WithNodes    ("highway", "bus_stop").
                WithNodes    ("amenity", "bus_station").
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
                RunAll       ("Tempolimits/Blitzer",
                              SecretKey, Passphrase);

            // ToDo: In this case it's not that simple! Split and merge GeoJSON has to be implemented!
            //new OverpassQuery(JenaId).
            //    WithWays     ("highway").
            //    And          ("maxspeed").
            //    ToGeoJSON    ().
            //    SplitFeatures().
            //    ToGeoJSONFile(JSON => "Tempolimits/" + JSON["features"][0]["properties"]["maxspeed"].ToString() + ".geojson").
            //    RunNow();

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "5").
                RunAll       ("Tempolimits/Tempo5",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "6").
                RunAll       ("Tempolimits/Tempo6",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "10").
                RunAll       ("Tempolimits/Tempo10",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "20").
                RunAll       ("Tempolimits/Tempo20",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "30").
                RunAll       ("Tempolimits/Tempo30",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "40").
                RunAll       ("Tempolimits/Tempo40",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "50").
                RunAll       ("Tempolimits/Tempo50",
                              SecretKey, Passphrase);


            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "60").
                RunAll       ("Tempolimits/Tempo60",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "70").
                RunAll       ("Tempolimits/Tempo70",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "80").
                RunAll       ("Tempolimits/Tempo80",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "90").
                RunAll       ("Tempolimits/Tempo90",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "100").
                RunAll       ("Tempolimits/Tempo100",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "110").
                RunAll       ("Tempolimits/Tempo110",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "120").
                RunAll       ("Tempolimits/Tempo120",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithWays     ("highway").
                And          ("maxspeed", "130").
                RunAll       ("Tempolimits/Tempo130",
                              SecretKey, Passphrase);

            #endregion

            #region Energie

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

                RunAll       ("Energie/Strom",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithNodes    ("power", "pole").
                WithNodes    ("power", "tower").
                WithNodes    ("power", "cable_distribution_cabinet").
                WithWays     ("power", "minor_line").
                WithWays     ("power", "line").
                WithWays     ("power", "lines").
                RunAll       ("Energie/Stromleitungen",
                              SecretKey, Passphrase);

            #endregion

            #region Flächennutzung / Grünfläche

            // landuse  = farm, farmyard, farmland, forest, garden, grass, green, greenfield, meadow, scrub, village_green, cemetery, allotments, orchard, vineyard
            // natural  = grass, grassland, heath, marsh, meadow, tree, tree_row, scrub, wood, beach, cave_entrance, cliff, peak, sand, scree, sinkhole, spring, water, wetland
            // wood     = wood
            // leisure  = garden, golf_course, nature_reserve, park, pitch, village_green, recreation_ground, playground, wildlife_hide

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farm").
                RunAll       ("Flächennutzung/Grünfläche/landuse.farm",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmyard").
                RunAll       ("Flächennutzung/Grünfläche/landuse.farmyard",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmland").
                RunAll       ("Flächennutzung/Grünfläche/landuse.farmland",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "farmland").
                RunAll       ("Flächennutzung/Grünfläche/landuse.farmland",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "forest").
                RunAll       ("Flächennutzung/Grünfläche/landuse.forest",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "garden").
                RunAll       ("Flächennutzung/Grünfläche/landuse.garden",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "grass").
                RunAll       ("Flächennutzung/Grünfläche/landuse.grass",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "green").
                RunAll       ("Flächennutzung/Grünfläche/landuse.green",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "greenfield").
                RunAll       ("Flächennutzung/Grünfläche/landuse.greenfield",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "meadow").
                RunAll       ("Flächennutzung/Grünfläche/landuse.meadow",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "scrub").
                RunAll       ("Flächennutzung/Grünfläche/landuse.scrub",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "village_green").
                RunAll       ("Flächennutzung/Grünfläche/landuse.village_green",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "cemetery").
                RunAll       ("Flächennutzung/Grünfläche/landuse.cemetery",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "allotments").
                RunAll       ("Flächennutzung/Grünfläche/landuse.allotments",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "orchard").
                RunAll       ("Flächennutzung/Grünfläche/landuse.orchard",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "vineyard").
                RunAll       ("Flächennutzung/Grünfläche/landuse.vineyard",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("natural", "grass").
                RunAll       ("Flächennutzung/Grünfläche/natural.grass",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "grassland").
                RunAll       ("Flächennutzung/Grünfläche/natural.grassland",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "heath").
                RunAll       ("Flächennutzung/Grünfläche/natural.heath",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "marsh").
                RunAll       ("Flächennutzung/Grünfläche/natural.marsh",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "meadow").
                RunAll       ("Flächennutzung/Grünfläche/natural.meadow",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "tree").
                RunAll       ("Flächennutzung/Grünfläche/natural.tree",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "tree_row").
                RunAll       ("Flächennutzung/Grünfläche/natural.tree_row",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "scrub").
                RunAll       ("Flächennutzung/Grünfläche/natural.scrub",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "wood").
                RunAll       ("Flächennutzung/Grünfläche/natural.wood",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "beach").
                RunAll       ("Flächennutzung/Grünfläche/natural.beach",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "cave_entrance").
                RunAll       ("Flächennutzung/Grünfläche/natural.cave_entrance",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "cliff").
                RunAll       ("Flächennutzung/Grünfläche/natural.cliff",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "peak").
                RunAll       ("Flächennutzung/Grünfläche/natural.peak",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "sand").
                RunAll       ("Flächennutzung/Grünfläche/natural.sand",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "scree").
                RunAll       ("Flächennutzung/Grünfläche/natural.scree",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "sinkhole").
                RunAll       ("Flächennutzung/Grünfläche/natural.sinkhole",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "spring").
                RunAll       ("Flächennutzung/Grünfläche/natural.spring",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "water").
                RunAll       ("Flächennutzung/Grünfläche/natural.water",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("natural", "wetland").
                RunAll       ("Flächennutzung/Grünfläche/natural.wetland",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("wood", "wood").
                RunAll       ("Flächennutzung/Grünfläche/wood.wood",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------
            // leisure  = , , , , , , , , , water_park, 

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "garden").
                RunAll       ("Flächennutzung/Grünfläche/leisure.garden",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "golf_course").
                RunAll       ("Flächennutzung/Grünfläche/leisure.golf_course",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "nature_reserve").
                RunAll       ("Flächennutzung/Grünfläche/leisure.nature_reserve",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "park").
                RunAll       ("Flächennutzung/Grünfläche/leisure.park",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "pitch").
                RunAll       ("Flächennutzung/Grünfläche/leisure.pitch",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "village_green").
                RunAll       ("Flächennutzung/Grünfläche/leisure.village_green",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "recreation_ground").
                RunAll       ("Flächennutzung/Grünfläche/leisure.recreation_ground",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "wildlife_hide").
                RunAll       ("Flächennutzung/Grünfläche/leisure.wildlife_hide",
                              SecretKey, Passphrase);

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
                RunAll       ("Flächennutzung/Wasser/waterway.river",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "riverbank").
                RunAll       ("Flächennutzung/Wasser/waterway.riverbank",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "stream").
                RunAll       ("Flächennutzung/Wasser/waterway.stream",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "wadi").
                RunAll       ("Flächennutzung/Wasser/waterway.wadi",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "canal").
                RunAll       ("Flächennutzung/Wasser/waterway.canal",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "drain").
                RunAll       ("Flächennutzung/Wasser/waterway.drain",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "ditch").
                RunAll       ("Flächennutzung/Wasser/waterway.ditch",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "dam").
                RunAll       ("Flächennutzung/Wasser/waterway.dam",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "weir").
                RunAll       ("Flächennutzung/Wasser/waterway.weir",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "lock_gate").
                RunAll       ("Flächennutzung/Wasser/waterway.lock_gate",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "fish_pass").
                RunAll       ("Flächennutzung/Wasser/waterway.fish_pass",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "mooring").
                RunAll       ("Flächennutzung/Wasser/waterway.mooring",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "waterfall").
                RunAll       ("Flächennutzung/Wasser/waterway.waterfall",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("natural", "water").
                RunAll       ("Flächennutzung/Wasser/natural.water",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "water").
                RunAll       ("Flächennutzung/Wasser/landuse.water",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "basin").
                RunAll       ("Flächennutzung/Wasser/landuse.basin",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "reservoir").
                RunAll       ("Flächennutzung/Wasser/landuse.reservoir",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "swimming_pool").
                RunAll       ("Flächennutzung/Wasser/leisure.swimming_pool",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "water_park").
                RunAll       ("Flächennutzung/Wasser/leisure.water_park",
                              SecretKey, Passphrase);

            #endregion

            #region Flächennutzung / Gewerbe

            // landuse   = industrial, commercial, retail, brownfield, quarry, railway
            // leisure   = dance, fitness_centre, gambling, horse_riding, pitch, sports_centre, stadium, miniature_golf, slipway, tanning_salon, track
            // waterway  = dock, boatyard

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "industrial").
                RunAll       ("Flächennutzung/Gewerbe/landuse.industrial",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "commercial").
                RunAll       ("Flächennutzung/Gewerbe/landuse.commercial",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "retail").
                RunAll       ("Flächennutzung/Gewerbe/landuse.retail",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "brownfield").
                RunAll       ("Flächennutzung/Gewerbe/landuse.brownfield",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "quarry").
                RunAll       ("Flächennutzung/Gewerbe/landuse.quarry",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "railway").
                RunAll       ("Flächennutzung/Gewerbe/landuse.railway",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "dance").
                RunAll       ("Flächennutzung/Gewerbe/leisure.dance",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "fitness_centre").
                RunAll       ("Flächennutzung/Gewerbe/leisure.fitness_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "dance").
                RunAll       ("Flächennutzung/Gewerbe/leisure.dance",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "gambling").
                RunAll       ("Flächennutzung/Gewerbe/leisure.gambling",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "horse_riding").
                RunAll       ("Flächennutzung/Gewerbe/leisure.horse_riding",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "pitch").
                RunAll       ("Flächennutzung/Gewerbe/leisure.pitch",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "sports_centre").
                RunAll       ("Flächennutzung/Gewerbe/leisure.sports_centre",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "stadium").
                RunAll       ("Flächennutzung/Gewerbe/leisure.stadium",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "miniature_golf").
                RunAll       ("Flächennutzung/Gewerbe/leisure.miniature_golf",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "slipway").
                RunAll       ("Flächennutzung/Gewerbe/leisure.slipway",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "tanning_salon").
                RunAll       ("Flächennutzung/Gewerbe/leisure.tanning_salon",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("leisure", "track").
                RunAll       ("Flächennutzung/Gewerbe/leisure.track",
                              SecretKey, Passphrase);

            // ----------------------------------------------------------------------------

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "dock").
                RunAll       ("Flächennutzung/Gewerbe/waterway.dock",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("waterway", "boatyard").
                RunAll       ("Flächennutzung/Gewerbe/waterway.boatyard",
                              SecretKey, Passphrase);

            #endregion

            #region Flächennutzung / Wohngebiete

            // landuse      = residential, garages
            // residential  = *

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "residential").
                RunAll       ("Flächennutzung/Wohngebiete/landuse.residential",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "garages").
                RunAll       ("Flächennutzung/Wohngebiete/landuse.garages",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("residential", "*").
                RunAll       ("Flächennutzung/Wohngebiete/residential",
                              SecretKey, Passphrase);

            #endregion

            #region Flächennutzung / Militär

            // landuse   = military
            // military  = *

            new OverpassQuery(JenaId).
                WithAny      ("landuse", "military").
                RunAll       ("Flächennutzung/Militär/landuse.military",
                              SecretKey, Passphrase);

            new OverpassQuery(JenaId).
                WithAny      ("military").
                RunAll       ("Flächennutzung/Militär/military",
                              SecretKey, Passphrase);

            #endregion

            // -----------------------------------------------------------------

            #region Hebesätze der Gemeinden

            Directory.EnumerateFiles("Hebesätze der Gemeinden").ForEach(InFile => {

                OpenGPG.CreateSignature(File.OpenRead (InFile),
                                        File.OpenWrite(InFile.Replace(".csv", ".sig")),
                                        SecretKey,
                                        Passphrase,
                                        HashAlgorithms.Sha512,
                                        ArmoredOutput: true);

                OpenGPG.CreateSignature(File.OpenRead (InFile),
                                        File.OpenWrite(InFile.Replace(".csv", ".bsig")),
                                        SecretKey,
                                        Passphrase,
                                        HashAlgorithms.Sha512,
                                        ArmoredOutput: false);

            });

            #endregion

            Console.WriteLine("ready...");
            Console.ReadLine();

        }

    }

}
