# TESTLEMON MCP server

The `testlemon-mcp` server is a .NET application that integrates with Testlemon to provide Model Context Protocol (MCP) services for Testlemon projects.

## Features
- Connects to testlemon and execute different actions.

## Prerequisites
- Docker installed on your machine
- Access to the target Testlemon instance

## Local Setup
1. **Clone the repository:**

```sh
git clone <repo-url>
cd <repo-url>
```

2. **Build Docker image:**

```sh
docker build -t itbusina/testlemon-mcp:latest -f src/testlemon-mcp/Dockerfile .
```

## MCP config

Open MCP config file and setup MCP server. Set your Sonar host and token in the `env` section.

```json
{
  "servers": {
    "testlemon-mcp": {
      "type": "stdio",
      "command": "docker",
      "args": [
          "run",
          "-i",
          "--rm",
          "itbusina/testlemon-mcp:latest"
      ],
      "env": {
      }
    }
  }
}
```

You can now interact with mcp server from GitHub Copilot chat in `Agent` mode.
