Example 1
=========

In order to query all tram lines and stations in Jena, Germany

As a first step the area "Jena" will be looked up via the Nominatim API. This will hopefully return a numeric area identification for later internal use.

    new OverpassQuery("Jena").

In the next step we tell the query which node, ways or relations to include into the result set.	
	
    WithRelations("route",   "tram").
    WithNodes    ("railway", "tram_stop").

The plain JSON result will be stored and converted into the more common GeoJSON format.
	
    ToFile       ("route.tram.json").
    ToGeoJSONFile("route.tram.geojson").
	
Normally everything is async, but be aware of HTTP status code 429 ;)
	
    RunNow();
	
