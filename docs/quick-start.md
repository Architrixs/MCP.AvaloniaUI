# Quick Start Guide

Get up and running with AvaloniaUI.MCP in minutes.

## Prerequisites

Before you begin, ensure you have:

- **.NET 10.0 SDK** or later
- **MCP-compatible client** such as:
  - Claude Desktop
  - Codex Desktop
  - Codex CLI
  - VS Code with MCP extension
  - Custom MCP client

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/Architrixs/MCP.AvaloniaUI.git
cd MCP.AvaloniaUI
```

### 2. Build the Project

```bash
dotnet build
```

### 3. Run Tests (Optional)

```bash
dotnet test
```

## Configuration

### Claude Desktop

Add the following to your Claude Desktop MCP configuration:

```json
{
  "mcpServers": {
    "avalonia": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/MCP.AvaloniaUI/src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"
      ],
      "cwd": "/path/to/MCP.AvaloniaUI"
    }
  }
}
```

### Codex CLI

Add AvaloniaUI.MCP to Codex:

```bash
# Linux/macOS
codex mcp add avalonia -- sh -lc 'cd /path/to/MCP.AvaloniaUI && dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj'

# Windows (PowerShell)
codex mcp add avalonia -- powershell -NoProfile -Command "Set-Location 'C:\\path\\to\\MCP.AvaloniaUI'; dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"
```

Verify it is configured:

```bash
codex mcp list
codex mcp get avalonia
```

### Codex Desktop

Codex Desktop uses the same MCP configuration as Codex CLI.

Option 1 (recommended): add it with CLI:

```bash
# Linux/macOS
codex mcp add avalonia -- sh -lc 'cd /path/to/MCP.AvaloniaUI && dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj'

# Windows (PowerShell)
codex mcp add avalonia -- powershell -NoProfile -Command "Set-Location 'C:\\path\\to\\MCP.AvaloniaUI'; dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"
```

Option 2: manually edit `~/.codex/config.toml`:

```toml
[mcp_servers.avalonia]
command = "dotnet"
args = ["run", "--project", "src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"]
cwd = "/path/to/MCP.AvaloniaUI"
enabled = true
```

On Windows, use a Windows path for `cwd`, for example:

```toml
cwd = "C:\\path\\to\\MCP.AvaloniaUI"
```

Restart Codex Desktop after updating the config.

### VS Code

Install the MCP extension and add to your settings:

```json
{
  "mcp.servers": [
    {
      "name": "AvaloniaUI",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/MCP.AvaloniaUI/src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"
      ],
      "cwd": "/path/to/MCP.AvaloniaUI"
    }
  ]
}
```

## First Steps

### 1. Create Your First Project

```
Ask your MCP client: "Create a new AvaloniaUI MVVM project called MyApp"
```

This will generate a complete AvaloniaUI project with:
- MVVM architecture
- ReactiveUI integration
- Cross-platform support
- Best practices structure

### 2. Validate XAML

```
Ask: "Validate this XAML code: <Window>...</Window>"
```

The server will check for:
- Syntax errors
- Property validation
- Best practices compliance

### 3. Get Help with Migration

```
Ask: "How do I migrate this WPF control to AvaloniaUI?"
```

Get detailed migration guidance including:
- Control mappings
- Namespace changes
- Property updates

## Environment Configuration

### Log Levels

Set the log level using an environment variable:

```bash
export AVALONIA_MCP_LOG_LEVEL=Debug
dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj
```

Available levels: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`

### Performance Tuning

For production use, consider these environment settings:

```bash
# Optimize for production
export ENVIRONMENT=production
export AVALONIA_MCP_LOG_LEVEL=Information

# Enable garbage collection optimization
export DOTNET_gcServer=1
export DOTNET_gcConcurrent=1
```

## Troubleshooting

### Common Issues

#### Default SDK Is .NET 8.0

This repository requires SDK 10 and includes `global.json` pinned to `10.0.100`.

```bash
# Check installed SDKs
dotnet --list-sdks

# Run from repository root so global.json is applied
cd /path/to/MCP.AvaloniaUI
dotnet --version  # should print 10.0.x
dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj
```

If `dotnet --version` still shows `8.0.x`, install .NET 10 SDK, then reopen your terminal.

#### Server Not Starting
```bash
# Check .NET version
dotnet --version

# Ensure correct project path
dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj --verbosity normal
```

#### Connection Issues
```bash
# Test server manually
echo '{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test","version":"1.0.0"}}}' | dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj
```

#### Performance Issues
```bash
# Check health status
# Use the diagnostic tool in your MCP client
```

### Logging and Diagnostics

Enable detailed logging for troubleshooting:

```bash
export AVALONIA_MCP_LOG_LEVEL=Trace
dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj 2>&1 | tee avalonia-mcp.log
```

### Getting Help

- Check the [troubleshooting guide](./troubleshooting.md)
- Review [common examples](./examples/)
- Open an issue on [GitHub](https://github.com/Architrixs/MCP.AvaloniaUI/issues)

## Next Steps

- Explore the [Tools Reference](./tools/) for complete functionality
- Check out [Examples](./examples/) for common use cases
- Read the [Tools Reference](./tools/) for advanced usage
- Join the community [discussions](https://github.com/Architrixs/MCP.AvaloniaUI/discussions)
