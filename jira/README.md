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
        "JIRA_PAT": "YOUR_JIRA_PAT_TOKEN"
      }
    }
  }
}
```

Replace the values in `env` with your actual Jira host, and Jira PAT token. Adjust the `args` path to match your project location.

Place your `mcp.json` file in the project root or specify its path as required by your implementation.