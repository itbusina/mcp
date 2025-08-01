# SONAR MCP server

The `sonar-mcp` server is a .NET application that integrates with SonarCloud to provide Model Context Protocol (MCP) services for Sonar projects.

## Features
- Connects to sonar and retrieves projects information

## Prerequisites
- Docker installed on your machine
- Access to the target Sonar instance

## Local Setup
1. **Clone the repository:**

```sh
git clone <repo-url>
cd <repo-url>
```

2. **Build Docker image:**

```sh
docker build -t itbusina/sonar-mcp:latest -f src/sonar-mcp/Dockerfile .
```

## MCP config

Open MCP config file and setup MCP server. Set your Sonar host and token in the `env` section.

```json
{
  "servers": {
    "jira-mcp": {
      "type": "stdio",
      "command": "docker",
      "args": [
          "run",
          "-i",
          "--rm",
          "-e",
          "SONAR_HOST",
          "-e",
          "SONAR_TOKEN",
          "itbusina/sonar-mcp:latest"
      ],
      "env": {
          "JIRA_HOST": "https://server.com/",
          "JIRA_PAT": "dsads.."
      }
  }
  }
}
```

You can now interact with mcp server from GitHub Copilot chat in `Agent` mode.
