ARG DOTNET_VERSION=9.0
ARG PORT=8080

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}
WORKDIR /app

EXPOSE ${PORT}
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ColorApi.dll"]
