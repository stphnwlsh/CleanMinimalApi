# Docker image ARGS
ARG BASE_IMAGE_REPO=mcr.microsoft.com
ARG BASE_IMAGE_BUILD=dotnet/sdk
ARG BASE_IMAGE_BUILD_TAG=10.0-alpine
ARG BASE_IMAGE_RUNTIME=dotnet/aspnet
ARG BASE_IMAGE_RUNTIME_TAG=10.0-alpine

# Setup Build Image
FROM ${BASE_IMAGE_REPO}/${BASE_IMAGE_BUILD}:${BASE_IMAGE_BUILD_TAG} AS build

# Build, Test and Publish ARGS
ARG VERSION_PREFIX=1.0.0.0
ARG VERSION_SUFFIX
ARG ENVIRONMENT=docker

# Build, Test and Publish ENVS
ENV DOTNET_ENVIRONMENT=${ENVIRONMENT}

WORKDIR /sln

# Dotnet Restore
COPY ./*.sln ./NuGet.config ./Directory.Build.props ./Directory.Packages.props ./
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done

RUN dotnet restore -v minimal

# Copy Remaining Files
COPY . .

# Dotnet Build
RUN dotnet build --no-restore -c Release -v minimal -p:VersionPrefix=${VERSION_PREFIX} -p:VersionSuffix=${VERSION_SUFFIX}

# Dotnet Test
FROM build AS test
RUN dotnet test --no-restore --no-build -c Release -v minimal -p:CollectCoverage=true  -p:CoverletOutput=../results/ -p:MergeWith="../results/coverage.json" -p:CoverletOutputFormat=opencover%2cjson -m:1 -p:ExcludeByFile="**/Program.cs%2c**/Migrations/*.cs"

FROM scratch AS coverage
COPY --from=test /sln/tests/results/*.xml .

# Dotnet Publish
FROM build AS publish
ARG VERSION_PREFIX
ARG VERSION_SUFFIX
RUN dotnet publish ./src/**/Presentation.csproj --no-restore -c Release -v quiet -o app -p:VersionPrefix=${VERSION_PREFIX} -p:VersionSuffix=${VERSION_SUFFIX}

# Runtime Image
FROM ${BASE_IMAGE_REPO}/${BASE_IMAGE_RUNTIME}:${BASE_IMAGE_RUNTIME_TAG} AS run
WORKDIR /app
EXPOSE 8080
COPY --from=publish /sln/app .

# The database lives in /app/data, the only folder the non-root app user can write to.
RUN mkdir -p /app/data && chown $APP_UID /app/data
USER $APP_UID

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget -qO- http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "Presentation.dll"]
