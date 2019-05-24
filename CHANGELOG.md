# Flexberry GIS Test Stand ODataBackend Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [1.1.0] 23-05-2019
### Added
- /flexberry/gis-test-stand-geoserver:1.1.0 image
- postgres initial data
- autobuilding all images

### Fixed

### Changed
- Git-tagging of images changes from 1.1.0-postgresql-db, to gis-test-stand-postgres-db_1.1.0
- Image flexberry/gis-test-stand rename to flexberry/gis-test-stand-odata;
- Optimize Files structure (move /SQL to Docker/gis-test-stand-postgres-db/host, source of odata to Docker/gis-test-stand-odata/source);
- Optimize Dockerfiles, remove git clone ..., wget ... from native repository;
- Remove hooks/build, hooks/post_push, add hooks/pre_push, hooks/post_push for all images
- Add Docker/hookFunctions.sh
- Change version of images in yml-files from  latest to 1.1



