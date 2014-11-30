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

-------------------------------------------------------------------------------------------------------	
	
As a first step the area "Jena" will be looked up via the Nominatim API. This will hopefully return a numeric area identification for later internal use.

    new OverpassQuery("Jena").

In the next step we tell the query which node, ways or relations to include into the result set.	
	
    WithRelations("route",   "tram").
    WithNodes    ("railway", "tram_stop").

The plain JSON result will be stored and converted into the more common GeoJSON format.
	
    ToFile       ("ÖffentlicherNahverkehr/route.tram.json").
    ToGeoJSONFile("ÖffentlicherNahverkehr/route.tram.geojson").
	
Normally everything is async, but be aware of HTTP status code 429 ;)
	
    RunNow();
	