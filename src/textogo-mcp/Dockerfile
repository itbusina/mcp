# Use the official .NET Core SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy project files
COPY ./src/textogo-mcp ./src/textogo-mcp

# Publish app to out folder
WORKDIR /src/textogo-mcp
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /src/textogo-mcp
COPY --from=build /src/textogo-mcp/out .

# Entry point when the container starts
ENTRYPOINT ["dotnet", "textogo-mcp.dll"]