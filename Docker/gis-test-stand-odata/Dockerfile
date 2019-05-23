FROM mono:latest as backend

COPY /sources /sources

RUN	 \
	mono --version ; \
	msbuild /version ; \
	cd /sources ; \
	nuget restore FlexberryGisTestStand.sln ; \
	msbuild /p:Configuration=Release FlexberryGisTestStand.sln

FROM flexberry/alt.p8-apache2-mono:latest

MAINTAINER mail@flexberry.net

COPY --from=backend /sources/ODataBackend /var/www/vhosts/gis-test-stand
COPY vhosts.conf /conf

RUN 	rm -f /var/www/vhosts/gis-test-stand/bin/Mono.Security.dll ; \
	rm -f /var/www/vhosts/gis-test-stand/Global.asax.cs ; \
	rm -f /var/www/vhosts/gis-test-stand/FlexberryGisTestStand.ODataBackend.csproj ; \
	rm -f /var/www/vhosts/gis-test-stand/Web.Debug.config ; \
	rm -f /var/www/vhosts/gis-test-stand/Web.Release.config ; \
	rm -f /var/www/vhosts/gis-test-stand/packages.config ; \
	rm -fr /var/www/vhosts/gis-test-stand/App_Start ; \
	rm -fr /var/www/vhosts/gis-test-stand/Properties ; \
	rm -fr /var/www/vhosts/gis-test-stand/obj ; \
	rm /var/www/vhosts/gis-test-stand/Web.config ; \
	mv /var/www/vhosts/gis-test-stand/Web.Docker.config /var/www/vhosts/gis-test-stand/Web.config
