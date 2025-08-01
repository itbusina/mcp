# TEXTOGO MCP server

The `textogo-mcp` server is a .NET application that integrates with Textogo to provide Model Context Protocol (MCP) services for TTS.

## Features
- Connects to textogo and execute different actions.

## Prerequisites
- Docker installed on your machine
- Access to the target textogo account

## Local Setup
1. **Clone the repository:**

```sh
git clone <repo-url>
cd <repo-url>
```

2. **Build Docker image:**

```sh
docker build -t itbusina/textogo-mcp:latest -f src/textogo-mcp/Dockerfile .
```

## MCP config

Open MCP config file and setup MCP server. Set your Sonar host and token in the `env` section.

```json
{
  "servers": {
    "textogo": {
      "type": "stdio",
      "command": "docker",
      "args": [
          "run",
          "-i",
          "--rm",
          "-e",
          "BILLING_ID",
          "itbusina/textogo-mcp:latest"
      ],
      "env": {
        "BILLING_ID": "dsa131..."
      }
    }
  }
}
```

You can now interact with mcp server from GitHub Copilot chat in `Agent` mode.
