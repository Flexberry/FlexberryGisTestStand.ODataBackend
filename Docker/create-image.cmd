docker build --no-cache -f gis-test-stand-postgres-db\Dockerfile -t flexberry/gis-test-stand-postgres-db ./gis-test-stand-postgres-db

docker build --no-cache -f gis-test-stand-odata\Dockerfile -t flexberry/gis-test-stand-odata ./gis-test-stand-odata

docker build --no-cache -f gis-test-stand-geoserver\Dockerfile -t flexberry/gis-test-stand-geoserver ./gis-test-stand-geoserver