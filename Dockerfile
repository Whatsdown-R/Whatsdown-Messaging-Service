
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
ENV PATH_WITH_SPACE="MessageService"
copy . ./
# COPY *.sln .

COPY "${PATH_WITH_SPACE}/*.csproj" "./${PATH_WITH_SPACE}/"
RUN dotnet restore
# copy everything else and build app
COPY "${PATH_WITH_SPACE}/." "./${PATH_WITH_SPACE}/"
WORKDIR "/source/${PATH_WITH_SPACE}"
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Whatsdown-Message-Service.dll"]