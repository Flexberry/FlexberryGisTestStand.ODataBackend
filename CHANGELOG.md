# Flexberry GIS Test Stand ODataBackend Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [1.1.0] 23-05-2019
### Added
- /flexberry/gis-test-stand-geoserver:1.1.0 image
- postges initial data
- autobuilding all images

### Fixed

### Changed
- Git-tagging of images changes from 1.1.0-postgresql-db, to flexberry/gis-test-stand-postgres-db_1.1.0
- Image flexberry/gis-test-stand rename to flexberry/gis-test-stand-odata;
- Optimize Files structure (move /SQL to Docker/gis-test-stand-postgres-db/host, source of odata to Docker/gis-test-stand-odata);
- Optimize Dockerfiles;
- Change version of images in docker-compose.yml from  latest to 1.1


