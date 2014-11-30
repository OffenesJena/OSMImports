Example
=======

In order to query all tram lines and stations in Jena, Germany

First the area "Jena" will be searched using the 

    new OverpassQuery("Jena").
	
    WithRelations("route",   "tram").
    WithNodes    ("railway", "tram_stop").
    ToFile       ("ÖffentlicherNahverkehr/route.tram.json").
    ToGeoJSONFile("ÖffentlicherNahverkehr/route.tram.geojson").
    RunNow();


