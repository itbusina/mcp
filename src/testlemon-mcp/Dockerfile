# Use the official .NET Core SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy project files
COPY ./src/testlemon-mcp ./src/testlemon-mcp

# Publish app to out folder
WORKDIR /src/testlemon-mcp
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /src/testlemon-mcp
COPY --from=build /src/testlemon-mcp/out .

# Entry point when the container starts
ENTRYPOINT ["dotnet", "testlemon-mcp.dll"]