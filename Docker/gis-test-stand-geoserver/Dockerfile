FROM tomcat

COPY ./host /tmp/

RUN \
  wget http://downloads.sourceforge.net/project/geoserver/GeoServer/2.15.1/geoserver-2.15.1-war.zip -O /tmp/geoserver.zip && \
  unzip /tmp/geoserver.zip -d /tmp && \
  cd /usr/local/tomcat/webapps && rm -rf * && mkdir geoserver && \
  unzip /tmp/geoserver.war -d ./geoserver/ && \
  cp -r /tmp/fgis-test-stand ./geoserver/data/workspaces/ && \
  cd geoserver/WEB-INF && sed -i -e 's/<\/web-app>//g' web.xml && cat /tmp/cors >> web.xml && echo \<\/web-app\> >> web.xml && \
  sed -i -e 's/ADMIN,GROUP_ADMIN/*/g' /usr/local/tomcat/webapps/geoserver/data/security/layers.properties && \
  rm -rf /tmp/*
