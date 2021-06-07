# needs sdk since jobs need sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ./*.sln ./
COPY Src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p Src/${file%.*}/ && mv $file Src/${file%.*}/; done

COPY Test/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p Test/${file%.*}/ && mv $file Test/${file%.*}/; done

RUN dotnet restore
COPY . .
WORKDIR "/src/Src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Isitar.DependencyUpdater.Api.dll"]
