#!/bin/sh
set -x

/docker-cmd.sh 2>/dev/null &


until psql -U postgres -f ./create.sql; do echo "Wait...";sleep 2; done
until psql -U postgres -d stand -f ./postgis_enable.sql; do echo "Wait...";sleep 5; done
until psql -U standuser -d stand -f ./SQL/PostgreSql/create_geo.sql; do echo "Wait...";sleep 5; done
until psql -U standuser -d stand -f ./SQL/PostgreSql/create_tables.sql; do echo "Wait...";sleep 5; done
until psql -U standuser -d stand -f ./SQL/PostgreSql/import_water_line.sql; do echo "Wait...";sleep 5; done
until psql -U standuser -d stand -f ./SQL/PostgreSql/import_water_poly.sql; do echo "Wait...";sleep 5; done

#/etc/init.d/postgresql stop;  while psql -U postgres </dev/null >/dev/null 2>&1; do sleep 1; done; echo "postgresql stopped"
su -l postgres -s /bin/sh -c "pg_ctl -D /var/lib/pgsql/data/ stop"; \
while echo | psql -U postgres ; do sleep 5; done; \


