# Build stage using the corporate devcontainer image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet AS build
WORKDIR /src

# 1. Copy the entire repository first (so both API and Shared projects are present)
COPY . .

# 2. FIXED: Added the actual dotnet restore command targeting the API csproj
RUN --mount=type=secret,id=aws_token \
    export CODEARTIFACT_TOKEN=$(cat /run/secrets/aws_token) && \
    dotnet restore TraineeManagement.api/TraineeManagement.api.csproj --configfile NuGet.config

# 3. Publish the API project explicitly (it will automatically compile the referenced Shared project)
RUN dotnet publish TraineeManagement.api/TraineeManagement.api.csproj \
    -c Release \
    -o /App/out \
    --no-restore

# Build runtime image
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TraineeManagement.api.dll"]