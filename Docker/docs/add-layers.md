## Настройки для создания wfs слоя

* Main settings
  * Layer type = wfs

*  Coordinate system
  * Layer coordinate reference system (CRS) = EPSG:4326

*  Layer settings
  * **Format** = GeoJSON
  * **Url** = http://localhost:1819/geoserver/fgis-test-stand/wfs
  * **WMS version** = 1.1.0
  * **Type namespace** = fgis-test-stand
  * **Type name** = water_polygon (или water_line)
  * **Geometry field** = shape
  * **Show existing** = true
  * **Create multi geometries** = true (так как [у нас в бд поле shape типа MULTILINESTRING](../gis-test-stand-postgres-db/host/SQL/PostgreSql/create_geo.sql#17))
