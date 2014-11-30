OSM Importer
============

A tiny tool to query the Open Street map database via the Overpass API and to convert the result to GeoJSON.

In order to query all tram lines and stations in Jena, Germany

    new OverpassQuery("Jena").
    WithRelations("route",   "tram").
    WithNodes    ("railway", "tram_stop").
    ToFile       ("ÖffentlicherNahverkehr/route.tram.json").
    ToGeoJSONFile("ÖffentlicherNahverkehr/route.tram.geojson").
    RunNow();
