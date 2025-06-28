# jira-mcp

A .NET tool for integrating Jira with the Model Context Protocol (MCP).

## Features
- Connects to Jira and retrieves issues

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Access to a Jira instance

### Build

```
dotnet build
```

## MCP Setup

To add the jira mcp use the following configuration:

```jsonc
{
  "servers": {
    "jira-mcp": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/your/jira/src",
        "--no-build"
      ],
      "env": {
        "JIRA_HOST": "https://your-domain.atlassian.net",
        "JIRA_AUTH_TYPE": "basic", // or "bearer" (default)
        "JIRA_USER": "your-email@domain.com", // required for basic auth
        "JIRA_TOKEN": "YOUR_JIRA_API_TOKEN" // or PAT for bearer
      }
    }
  }
}
```

Replace the values in `env` with your actual Jira host, authentication type, user email (for basic auth), and Jira API token. If using bearer authentication, set only `JIRA_HOST` and `JIRA_TOKEN` (or PAT). Adjust the `args` path to match your project location.


## Logging

The application writes logs to a file located at:

```
jira/src/bin/Debug/net8.0/logs/app.log
```

This log file contains information about server startup, environment variable usage, requests, and errors. You can use it to troubleshoot issues or monitor the application's activity.

Place your `mcp.json` file in the project root or specify its path as required by your implementation.