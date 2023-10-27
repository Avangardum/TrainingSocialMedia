FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY TrainingSocialMedia.sln .
COPY TrainingSocialMedia/TrainingSocialMedia.csproj TrainingSocialMedia/
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -p:UseAppHost=false --warnaserror --no-restore
RUN dotnet test

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /src/TrainingSocialMedia/bin/Release/net7.0/publish .
ENTRYPOINT ["dotnet", "TrainingSocialMedia.dll"]