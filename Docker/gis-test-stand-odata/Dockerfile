FROM mcr.microsoft.com/dotnet/sdk:7.0 as backend

COPY /sources /sources

WORKDIR /sources
	
RUN \
  dotnet restore FlexberryGisTestStand.sln;\
  dotnet build FlexberryGisTestStand.sln -c Release;\
  dotnet publish ODataBackend/FlexberryGisTestStand.ODataBackend.csproj -c Release -o out -f net7.0

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=backend /sources/out ./

# Check webapi responses
HEALTHCHECK --interval=60s --timeout=15s --start-period=20s \
  CMD curl -f "http://0.0.0.0/health" || exit

ENTRYPOINT ["dotnet", "FlexberryGisTestStand.ODataBackend.dll"]
