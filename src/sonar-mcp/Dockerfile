# Use the official .NET Core SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy project files
COPY ./src/sonar-mcp ./src/sonar-mcp

# Publish app to out folder
WORKDIR /src/sonar-mcp
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /src/sonar-mcp
COPY --from=build /src/sonar-mcp/out .

# Entry point when the container starts
ENTRYPOINT ["dotnet", "sonar-mcp.dll"]