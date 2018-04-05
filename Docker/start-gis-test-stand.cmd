docker pull flexberry/gis-test-stand:latest
docker pull flexberry/gis-test-stand-postgres-db:latest
docker stack ls >NUL  2>NUL
IF %ERRORLEVEL% NEQ 0 (
	docker swarm init
)
docker stack ls | findstr GisTestStand
IF %ERRORLEVEL% EQU 0 (
	echo Gis test stand is already started.
	echo To stop the Gis test stand, you need to run the command 'stop-gis-test-stand.cmd'
) ELSE (
	docker stack  deploy -c  gis-test-stand-swarm-configuration.yml GisTestStand
)
