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

ENTRYPOINT ["dotnet", "FlexberryGisTestStand.ODataBackend.dll"]
