create schema geo;

set search_path=geo,public;

CREATE TABLE water_polygon (

 primaryKey serial NOT NULL,

 osm_id VARCHAR(255) NULL,

 code INT NULL,

 fclass VARCHAR(255) NULL,

 name VARCHAR(255) NULL,

 shape GEOMETRY('MULTIPOLYGON', 4326) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE water_line (

 primaryKey serial NOT NULL,

 osm_id VARCHAR(255) NULL,

 code INT NULL,

 fclass VARCHAR(255) NULL,

 name VARCHAR(255) NULL,

 width INT NULL,

 shape GEOMETRY('MULTILINESTRING', 4326) NULL,

 PRIMARY KEY (primaryKey));

CREATE INDEX geo_water_polygon_shape
  ON geo.water_polygon
  USING gist
  (shape);

CREATE INDEX geo_water_line_shape
  ON geo.water_line
  USING gist
  (shape);