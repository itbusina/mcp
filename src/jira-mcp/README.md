# JIRA MCP server

The `jira-mcp` server is a .NET application that integrates with JIRA to provide Model Context Protocol (MCP) services for JIRA projects.

## Features
- Connects to JIRA and retrieves ticket information

## Prerequisites
- Docker installed on your machine
- Access to the target JIRA instance (PAT token and URL)

## How to Get a JIRA Personal Access Token (PAT)
1. Log in to your JIRA instance in your web browser.
2. Click on your profile icon (top right) and select **Account settings** or **Profile**.
3. In the left sidebar, find and click **Personal Access Tokens**.
4. Click **Create token**.
5. Enter a label for your token, set expiry date and click **Create**.
6. Copy the generated token and save it securely. You will use this as your `JIRA_PAT`.

For more details, see the Atlassian documentation: https://confluence.atlassian.com/enterprise/using-personal-access-tokens-1026032365.html

## Local Setup
1. **Clone the repository:**

```sh
git clone <repo-url>
cd <repo-url>
```

2. **Build Docker image:**

```sh
docker build -t itbusina/jira-mcp:latest -f src/jira-mcp/Dockerfile .
```

## MCP config

Open MCP config file and setup MCP for JIRA. Set your JIRA host and PAT in the `env` section.

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
          "JIRA_HOST",
          "-e",
          "JIRA_PAT",
          "itbusina/jira-mcp:latest"
      ],
      "env": {
          "JIRA_HOST": "https://server.com/",
          "JIRA_PAT": "32asd.."
      }
    }
  }
}
```

You can now interact with jira mcp server from GitHub Copilot chat in `Agent` mode.
