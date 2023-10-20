FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -p:UseAppHost=false --warnaserror
RUN dotnet test

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /src/Server/bin/Release/net7.0/publish .
ENTRYPOINT ["dotnet", "TrainingSocialMedia.Server.dll"]