FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TrainingSocialMedia/Client/TrainingSocialMedia.Client.csproj", "TrainingSocialMedia/Client/"]
COPY ["TrainingSocialMedia/Shared/TrainingSocialMedia.Shared.csproj", "TrainingSocialMedia/Shared/"]
RUN dotnet restore "TrainingSocialMedia/Client/TrainingSocialMedia.Client.csproj"
COPY . .
WORKDIR "/src/TrainingSocialMedia/Client"
RUN dotnet build "TrainingSocialMedia.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrainingSocialMedia.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainingSocialMedia.Client.dll"]
