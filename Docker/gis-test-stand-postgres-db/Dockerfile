FROM flexberry/alt.p8-postgresql-postgis

COPY /host /docker/

ARG TAG
RUN cd /docker/host/ && \
  /bin/sh ./createdb.sh && \
  rm -rf /docker

MAINTAINER mail@flexberry.net


