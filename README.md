# FlexberryGisTestStand.ODataBackend

OData backed for flexberry-gis-test-stand-ember-frontend

[master: ![Build Status](https://travis-ci.org/Flexberry/FlexberryGisTestStand.ODataBackend.svg?branch=master)](https://travis-ci.org/Flexberry/FlexberryGisTestStand.ODataBackend)
[develop: ![Build Status](https://travis-ci.org/Flexberry/FlexberryGisTestStand.ODataBackend.svg?branch=develop)](https://travis-ci.org/Flexberry/FlexberryGisTestStand.ODataBackend)

## Test site

<https://flexberry-gis-test-stand.azurewebsites.net/odata>

## Docker images

* PostgreSQL Database: [flexberry/gis-test-stand-postgres-db](https://hub.docker.com/r/flexberry/gis-test-stand-postgres-db/)
* Web Application: [flexberry/gis-test-stand-odata](https://hub.docker.com/r/flexberry/gis-test-stand-data/)
* Geoserver Application [https://hub.docker.com/r/flexberry/gis-test-stand-geoserver](https://hub.docker.com/r/https://hub.docker.com/r/flexberry/gis-test-stand-geoserver)

## Локальный запуск

* Локальная сборка docker образов - `create-image.cmd`. Собираются все 3 образа, если нужен определенный образ, то можно в консоли выполнить команду нужного образа.
* Локальный запуск docker образов - `start-local.cmd`.
