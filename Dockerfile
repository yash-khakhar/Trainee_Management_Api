# Build stage using the corporate devcontainer image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet AS build
WORKDIR /src

# 1. Copy
COPY . .

# 2. Actual dotnet restore command targeting the API csproj
RUN --mount=type=secret,id=aws_token \
    export CODEARTIFACT_TOKEN=$(cat /run/secrets/aws_token) && \
    dotnet restore TraineeManagement.api/TraineeManagement.api.csproj --configfile NuGet.config

# 3. Publish
RUN dotnet publish TraineeManagement.api/TraineeManagement.api.csproj \
    -c Release \
    -o /App/out \
    --no-restore

# Build runtime image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet
WORKDIR /App
COPY --from=build /App/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TraineeManagement.api.dll"]