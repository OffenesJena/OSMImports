OSM Importer
============

A tiny tool to query the Open Street map database via the Overpass API and to convert the result to GeoJSON. The examples show in which ways the OK Lab Jena (Offenes Jena / Jena Open Data) makes use of its features for their OpenData projects.

In order to query all tram lines and stations in Jena, Germany

    new OverpassQuery("Jena").
    WithRelations("route",   "tram").
    WithNodes    ("railway", "tram_stop").
    ToFile       ("trams.json").
    ToGeoJSONFile("trams.geojson").
    RunNow();
