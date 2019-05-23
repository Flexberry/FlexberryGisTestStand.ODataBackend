FROM flexberry/alt.p8-postgresql-postgis

COPY . /docker/

ARG TAG
RUN cd /docker/host/ && \ 
  wget https://raw.githubusercontent.com/Flexberry/FlexberryGisTestStand.ODataBackend/$TAG/SQL/PostgreSql/create_tables.sql && \
  wget https://raw.githubusercontent.com/Flexberry/FlexberryGisTestStand.ODataBackend/$TAG/SQL/PostgreSql/create_geo.sql && \
  wget https://raw.githubusercontent.com/Flexberry/FlexberryGisTestStand.ODataBackend/$TAG/SQL/PostgreSql/import_water_line.sql && \
  wget https://raw.githubusercontent.com/Flexberry/FlexberryGisTestStand.ODataBackend/$TAG/SQL/PostgreSql/import_water_poly.sql && \
  /bin/sh /docker/host/createdb.sh && \
  rm -rf /docker

MAINTAINER mail@flexberry.net


