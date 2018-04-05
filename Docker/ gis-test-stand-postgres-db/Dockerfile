FROM flexberry/alt.p8-postgresql


COPY . /docker/

ARG TAG
RUN cd /docker/host/ && wget https://raw.githubusercontent.com/Flexberry/FlexberryGisTestStand.ODataBackend/$TAG/SQL/PostgreSql.create.sql && \
  /bin/sh /docker/host/createdb.sh

MAINTAINER mail@flexberry.net


