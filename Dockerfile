# Setup Build Image
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:cc02b814237a76c4735fb6eeee87181597b5e56f2ce458a435e527674d91f4d7 AS build

# Build, Test and Publish ARGS
ARG VERSION_PREFIX=1.0.0.0
ARG VERSION_SUFFIX
ARG ENVIRONMENT=docker

# Build, Test and Publish ENVS
ENV DOTNET_ENVIRONMENT=${ENVIRONMENT}

WORKDIR /sln

# Dotnet Restore
COPY ./*.sln ./NuGet.config ./Directory.Build.props ./Directory.Packages.props ./
COPY --parents src/*/*.csproj src/*/packages.lock.json tests/*/*.csproj tests/*/packages.lock.json ./

RUN dotnet restore --locked-mode -v minimal

# Copy Remaining Files
COPY . .

# Dotnet Build
RUN dotnet build --no-restore -c Release -v minimal -p:VersionPrefix=${VERSION_PREFIX} -p:VersionSuffix=${VERSION_SUFFIX}

# Dotnet Test
FROM build AS test
RUN dotnet test --no-restore --no-build -c Release -v minimal -p:CollectCoverage=true  -p:CoverletOutput=../results/ -p:MergeWith="../results/coverage.json" -p:CoverletOutputFormat=opencover%2cjson -m:1 -p:ExcludeByFile="**/Program.cs%2c**/Migrations/*.cs%2c**/obj/**/*.cs" -p:ExcludeByAttribute="GeneratedCodeAttribute"

FROM scratch AS coverage
COPY --from=test /sln/tests/results/*.xml .

# Dotnet Publish
FROM build AS publish
ARG VERSION_PREFIX
ARG VERSION_SUFFIX
RUN dotnet publish ./src/**/Presentation.csproj --no-restore -c Release -v quiet -o app -p:VersionPrefix=${VERSION_PREFIX} -p:VersionSuffix=${VERSION_SUFFIX}

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:254fe017495996d3abcc487ffacdcce2b73affb380d3af87af12c8dff41fe0b0 AS run
WORKDIR /app
EXPOSE 8080
COPY --from=publish /sln/app .

# The database lives in /app/data, the only folder the non-root app user can write to.
RUN mkdir -p /app/data && chown $APP_UID /app/data
USER $APP_UID

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget -qO- http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "Presentation.dll"]
