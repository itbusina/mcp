# qTest MCP server

The `qtest-mcp` server is a .NET application that integrates with qtest to provide Model Context Protocol (MCP) services for qtest projects.

## Features
- Connects to qTest and retrieves test case and test steps information

## Prerequisites
- Docker installed on your machine
- Access to the target qTest instance (Token and URL)
For more details, see the qTest documentation: https://qtest.dev.tricentis.com/#/

## Local Setup
1. **Clone the repository:**

```sh
git clone <repo-url>
cd <repo-url>
```

2. **Build Docker image:**

```sh
docker build -t itbusina/qtest-mcp:latest -f src/qtest-mcp/Dockerfile .
```

3. **Configure qTest credentials and server:**

Open MCP config file and setup MCP for qTest. Set your qTest host and PAT in the `env` section.

```json
{
  "servers": {
    "qtest-mcp": {
      "type": "stdio",
      "command": "docker",
      "args": [
          "run",
          "-i",
          "--rm",
          "-e",
          "QTEST_HOST",
          "-e",
          "QTEST_TOKEN",
          "itbusina/qtest-mcp:latest"
      ],
      "env": {
          "QTEST_HOST": "https://server.com/",
          "QTEST_TOKEN": "3213-.."
      }
  }
  }
}
```

You can now interact with qTest mcp server from GitHub Copilot chat in `Agent` mode.
