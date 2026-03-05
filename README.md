# AvaloniaUI.MCP

[![Build Status](https://github.com/Architrixs/MCP.AvaloniaUI/workflows/CI/badge.svg)](https://github.com/Architrixs/MCP.AvaloniaUI/actions)
[![Test Coverage](https://img.shields.io/badge/coverage-90%25-brightgreen)](https://github.com/Architrixs/MCP.AvaloniaUI)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Documentation](https://img.shields.io/badge/docs-GitHub%20Pages-blue)](https://Architrixs.github.io/MCP.AvaloniaUI)

## Model Context Protocol Server for AvaloniaUI Development

A comprehensive MCP server providing tools, resources, and guidance for building cross-platform AvaloniaUI applications. It connects AI assistants (like Claude) directly to AvaloniaUI knowledge and code generation capabilities.

## 🤖 What Is MCP and How Does It Work?

**Model Context Protocol (MCP)** is an open standard created by Anthropic that lets AI assistants (such as Claude) communicate with external tools and data sources using a well-defined JSON-RPC 2.0 protocol over STDIO or HTTP.

### The Protocol at a Glance

```text
AI Client (e.g. Claude Desktop)
      │
      │  JSON-RPC 2.0 over STDIO / HTTP
      ▼
MCP Server (this project)
      │
      ├── Tools    – callable functions (e.g. generate a project, validate XAML)
      ├── Resources – read-only data sources (e.g. controls reference, patterns)
      └── Prompts  – reusable prompt templates for common workflows
```

1. **Client sends a request** – e.g. `tools/call` with `{ "name": "CreateAvaloniaProject", "arguments": {...} }`.
2. **Server executes the tool** and returns a structured result.
3. **AI incorporates the result** into its response to the user.

This means when you ask Claude _"Create an MVVM project called MyApp"_, Claude calls this server's `CreateAvaloniaProject` tool behind the scenes and returns the generated code to you.

### MCP Capabilities Used by This Server

| Capability | Description |
|------------|-------------|
| **Tools** | Executable actions – project generation, XAML validation, code conversion |
| **Resources** | Queryable knowledge – controls catalog, XAML patterns, migration guide, community resources |
| **Prompts** | Pre-built prompt templates for common AvaloniaUI tasks |

For the full specification see the [official MCP docs](https://modelcontextprotocol.io/introduction).

## 🏗️ How This Server Works

### Architecture Overview

```text
┌──────────────────────────────────────────────────────────────┐
│                      MCP Client                               │
│          (Claude Desktop, VS Code, Codex CLI, …)             │
└──────────────────┬───────────────────────────────────────────┘
                   │ JSON-RPC 2.0 over STDIO
                   ▼
┌──────────────────────────────────────────────────────────────┐
│                   AvaloniaUI.MCP Server                       │
│                                                               │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────────┐  │
│  │    Tools    │  │  Resources   │  │      Prompts       │  │
│  │ (18 tools)  │  │ (knowledge)  │  │ (prompt templates) │  │
│  └──────┬──────┘  └──────┬───────┘  └────────────────────┘  │
│         │                │                                    │
│         └────────────────▼────────────────────────────────── │
│                   ┌──────────────┐                            │
│                   │ Knowledge DB │                            │
│                   │ (JSON files) │                            │
│                   │ controls     │                            │
│                   │ xaml-patterns│                            │
│                   │ migration    │                            │
│                   │ community    │                            │
│                   └──────────────┘                            │
└──────────────────────────────────────────────────────────────┘
```

### Server Components

| Component | Location | Purpose |
|-----------|----------|---------|
| **Entry point** | `src/AvaloniaUI.MCP/Program.cs` | Starts the STDIO transport, wires up DI |
| **Tools** | `src/AvaloniaUI.MCP/Tools/` | 18 `[McpServerTool]` methods callable by AI |
| **Resources** | `src/AvaloniaUI.MCP/Resources/` | Read-only knowledge providers |
| **Prompts** | `src/AvaloniaUI.MCP/Prompts/` | Reusable prompt template generators |
| **Knowledge base** | `src/AvaloniaUI.MCP/Data/` | JSON files: `controls.json` (49 controls), `xaml-patterns.json` (20 patterns), `migration-guide.json`, `community-resources.json` |
| **Services** | `src/AvaloniaUI.MCP/Services/` | Caching (`ResourceCacheService`) and telemetry |

### Request Lifecycle

1. Client sends `tools/call` (or `resources/read`, `prompts/get`) via STDIO.
2. The official **Microsoft MCP SDK** deserialises the request and dispatches it to the matching method.
3. The method reads from the in-memory cache (populated at startup from JSON files) and returns a result.
4. Telemetry is recorded via the `ITelemetryService` / Sentry integration.
5. The response is serialised back to the client over STDIO.

## 🚀 Features

### 🛠️ 18 Development Tools

| Category | Tools |
|----------|-------|
| **Project Generation** | `ProjectGeneratorTool`, `ArchitectureTemplateTool` |
| **Validation & Quality** | `XamlValidationTool`, `SecurityPatternTool`, `AccessibilityTool` |
| **UI Development** | `ThemingTool`, `CustomControlGenerator`, `AnimationTool`, `UIUXDesignTool` |
| **Migration & Integration** | `APIIntegrationTool`, `LocalizationTool`, `DataAccessPatternTool`, `ServiceLayerTool` |
| **Debugging & Diagnostics** | `DiagnosticTool`, `DebuggingAssistantTool`, `TestingIntegrationTool`, `PerformanceAnalysisTool` |
| **Utilities** | `EchoTool` |

### 📚 Built-in Knowledge Base

- **Controls catalog** (`controls.json`) – 49 controls with descriptions, key properties, and XAML examples covering layout, input, display, selection, container, navigation, date/time, and advanced controls
- **XAML patterns** (`xaml-patterns.json`) – 20 reusable patterns including layout, binding, animation, drag-drop, file picker, clipboard, localization, splash screen, native OS menus, and custom controls; sourced from [AvaloniaUI.QuickGuides](https://github.com/AvaloniaUI/AvaloniaUI.QuickGuides)
- **WPF migration guide** (`migration-guide.json`) – control mappings, namespace changes, binding differences, and step-by-step migration process
- **Community resources** (`community-resources.json`) – curated MVVM frameworks, UI themes, chart libraries, control extensions, and notable open-source apps; sourced from [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia)

### ⚡ Infrastructure

- **< 100ms response times** – JSON knowledge base preloaded into memory at startup
- **Async/non-blocking** – all I/O uses `async`/`await`
- **Telemetry** – Sentry integration for error tracking and performance monitoring
- **Structured logging** – configurable log levels via `AVALONIA_MCP_LOG_LEVEL`

## 📖 Quick Start

### Prerequisites

- **.NET 10.0 SDK** or later
- **MCP-compatible client** (Claude Desktop, Codex Desktop/CLI, VS Code with MCP extension)

### Installation

```bash
# Clone the repository
git clone https://github.com/Architrixs/MCP.AvaloniaUI.git
cd MCP.AvaloniaUI

# Build the project
dotnet build

# Run tests (optional)
dotnet test

# Start the server
dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj
```

### Configuration

Add to your MCP client configuration:

**Claude Desktop** (`~/Library/Application Support/Claude/claude_desktop_config.json` on macOS):

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

**Codex Desktop/CLI**:

```bash
# Linux/macOS
codex mcp add avalonia -- sh -lc 'cd /path/to/MCP.AvaloniaUI && dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj'

# Windows (PowerShell)
codex mcp add avalonia -- powershell -NoProfile -Command "Set-Location 'C:\\path\\to\\MCP.AvaloniaUI'; dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"
```

**VS Code** (`.vscode/settings.json`):

```json
{
  "mcp.servers": [
    {
      "name": "AvaloniaUI",
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/MCP.AvaloniaUI/src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj"],
      "cwd": "/path/to/MCP.AvaloniaUI"
    }
  ]
}
```

> **Note:** If your default SDK is .NET 8, install .NET 10 and run from the repository root so `global.json` (pinned to `10.0.100`) is applied.

### First Commands

Try these with your MCP client:

```text
"Create a new AvaloniaUI MVVM project called MyApp"
"Validate this XAML code for best practices"
"Generate JWT authentication pattern with high security"
"Show me how to migrate this WPF control to AvaloniaUI"
"Perform a health check on the server"
```

## 📚 Documentation

| Resource | Description |
|----------|-------------|
| [**📖 Documentation Site**](https://Architrixs.github.io/MCP.AvaloniaUI) | Complete documentation with examples |
| [**🚀 Quick Start Guide**](docs/quick-start.md) | Get running in 5 minutes |
| [**🛠️ Tools Reference**](docs/tools/) | Detailed tool documentation |
| [**💡 Examples & Tutorials**](docs/examples/) | Real-world usage examples |
| [**🐛 Troubleshooting**](docs/troubleshooting.md) | Common issues and solutions |

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific categories
dotnet test --filter Category=Unit
dotnet test --filter Category=Integration
```

## 📖 Expanding the Knowledge Base

The knowledge base lives in three JSON files under `src/AvaloniaUI.MCP/Data/`. These files drive both the **Resources** (what the AI can query) and the **Tools** (what gets generated). Adding or improving entries here is one of the easiest and most impactful ways to contribute.

### `controls.json`

Each entry describes one AvaloniaUI control:

```json
{
  "name": "Button",
  "description": "A clickable button control",
  "namespace": "Avalonia.Controls",
  "properties": ["Content", "Command", "IsEnabled", "..."],
  "example": "<Button Content=\"Click me\" Command=\"{Binding MyCommand}\" />"
}
```

To add a control: copy an existing entry, fill in the fields, and add a test in `tests/AvaloniaUI.MCP.Tests/` that calls `GetControlInfo` with the new control name.

### `xaml-patterns.json`

Each entry is a named, reusable XAML snippet:

```json
{
  "name": "mvvm-binding",
  "description": "Basic MVVM data binding pattern",
  "category": "binding",
  "xaml": "<TextBox Text=\"{Binding UserName, Mode=TwoWay}\" />"
}
```

### `migration-guide.json`

Contains WPF-to-AvaloniaUI control mappings, namespace changes, and migration steps.

### Contribution Tips

- Keep XAML examples minimal but valid and idiomatic.
- Test your additions: run `dotnet test` after any JSON change.
- See [`docs/todo/control-coverage.md`](docs/todo/control-coverage.md) for the list of controls that are still missing.

## 🗺️ Roadmap – Planned Features

The following are features we intend to add. Contributions in any of these areas are very welcome.

### Short-term (next release)

- [x] **Missing controls in knowledge base** – ButtonSpinner, ColorPicker, GridSplitter, ItemsControl, ItemsRepeater, MaskedTextBox, Panel, PathIcon, RelativePanel, RepeatButton, SelectableTextBlock, SplitButton, ToggleButton, ToggleSplitButton, UniformGrid – **✅ Added** (see [`docs/todo/control-coverage.md`](docs/todo/control-coverage.md))
- [x] **QuickGuides patterns** – bindings/converters, drag-drop, file picker, clipboard, localization, splash screen, native menu, custom control, ItemsRepeater list – **✅ Added** (from [AvaloniaUI.QuickGuides](https://github.com/AvaloniaUI/AvaloniaUI.QuickGuides))
- [x] **Community resources knowledge base** – MVVM frameworks, UI themes, chart libraries, control extensions, notable apps – **✅ Added** (from [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia))
- [ ] **XAML validator accuracy fixes** – remove false "unsupported" warnings for `Viewbox` (see [`docs/todo/xaml-validator-fixes.md`](docs/todo/xaml-validator-fixes.md))
- [ ] **DockPanel generation fix** – emit idiomatic `DockPanel.Dock` child layout instead of `Content=` usage (see [`docs/todo/control-generation-fixes.md`](docs/todo/control-generation-fixes.md))
- [ ] **ScrollViewer property correction** – verify and fix `ZoomMode` property in controls data

### Medium-term

- [ ] **HTTP/SSE transport** – allow clients to connect over HTTP in addition to STDIO
- [ ] **AvaloniaUI 12.x parity** – keep the knowledge base in sync with the latest Avalonia release from [docs.avaloniaui.net](https://docs.avaloniaui.net/)
- [ ] **Interactive XAML preview suggestions** – detect common layout mistakes and propose fixes
- [ ] **Custom control template scaffolding** – generate boilerplate for `TemplatedControl` subclasses
- [ ] **More comprehensive migration automation** – deeper WPF property mapping and code-behind conversion hints
- [ ] **ReactiveUI patterns** – deeper reactive programming examples (from ReactiveUI docs and community samples)
- [ ] **Native AOT publishing guide** – based on [AvaloniaUI.QuickGuides/NativeAot](https://github.com/AvaloniaUI/AvaloniaUI.QuickGuides/tree/main/NativeAot)

### Long-term

- [ ] **AvaloniaUI designer integration** – surface MCP tools directly inside the visual designer
- [ ] **Project analysis tool** – analyse an existing project and suggest improvements
- [ ] **Automated upgrade tool** – migrate an AvaloniaUI 0.x project to 11/12
- [ ] **Mobile/Android/iOS deployment guide** – cross-platform deployment patterns
- [ ] **WASM/Browser target guide** – AvaloniaUI WebAssembly knowledge

## ⚠️ Known Issues

These are active issues. Pull requests fixing them are especially appreciated.

| # | Area | Description | Tracking |
|---|------|-------------|----------|
| 1 | XAML validator | `Viewbox` incorrectly flagged as unsupported | [`docs/todo/xaml-validator-fixes.md`](docs/todo/xaml-validator-fixes.md) |
| 2 | Code generation | `DockPanel` example uses `Content=` syntax instead of child-docking layout | [`docs/todo/control-generation-fixes.md`](docs/todo/control-generation-fixes.md) |
| 3 | Knowledge base | `ScrollViewer.ZoomMode` property may be incorrect for Avalonia | [`docs/todo/xaml-validator-fixes.md`](docs/todo/xaml-validator-fixes.md) |

## 🌍 Contributing

We welcome all kinds of contributions — new tools, knowledge-base entries, bug fixes, tests, and documentation. Please read [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidance.

### Quick Steps

1. **Fork** the repository on GitHub
2. **Clone** your fork: `git clone https://github.com/<your-username>/MCP.AvaloniaUI.git`
3. **Install dependencies**: `dotnet restore`
4. **Create a branch**: `git checkout -b feature/my-improvement`
5. **Make your changes** (add tests for new functionality)
6. **Verify**: `dotnet build && dotnet test`
7. **Push** and open a Pull Request against `main`

### Adding a New Tool

```csharp
// src/AvaloniaUI.MCP/Tools/MyNewTool.cs
[McpServerToolType]
public static class MyNewTool
{
    [McpServerTool, Description("What this tool does")]
    public static string DoSomething(
        [Description("Input description")] string input)
    {
        return $"Result: {input}";
    }
}
```

The SDK auto-discovers tools via `WithToolsFromAssembly()` — no registration needed.

### Adding a New Resource

```csharp
// src/AvaloniaUI.MCP/Resources/MyResource.cs
[McpServerResourceType]
public static class MyResource
{
    [McpServerResource(UriTemplate = "avalonia://my-resource")]
    public static async Task<string> GetMyResourceAsync()
    {
        return await ResourceCacheService.GetOrLoadAsync("my-key", LoadDataAsync);
    }
}
```

### Areas for Contribution

| Priority | Area |
|----------|------|
| 🔴 High | Missing controls in knowledge base (see Known Issues #1) |
| 🔴 High | XAML validator false positives (see Known Issues #2) |
| 🟡 Medium | DockPanel generation fix (see Known Issues #3) |
| 🟡 Medium | Additional XAML patterns and examples |
| 🟢 Low | Documentation improvements |
| 🟢 Low | Additional test coverage |
| 🟢 Low | HTTP/SSE transport support |

## 🔒 Security

- **Input Validation** – all tool parameters are validated before use
- **Error Handling** – no sensitive information is exposed in error responses
- **No external network calls** – the server is fully offline; all knowledge is bundled as JSON

## 📄 License

MIT License — see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **[decriptor/AvaloniaUI.MCP](https://github.com/decriptor/AvaloniaUI.MCP)** – the original project this work is based on; the core knowledge base (controls, XAML patterns, migration guide) and server architecture originated there
- **AvaloniaUI Team** – for the excellent cross-platform UI framework and the [AvaloniaUI.QuickGuides](https://github.com/AvaloniaUI/AvaloniaUI.QuickGuides) examples that informed many of the patterns in this server
- **AvaloniaCommunity** – for the [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia) curated list, which drives the community resources knowledge base
- **Anthropic / Microsoft** – for the Model Context Protocol specification and SDK
- **Contributors** – everyone who helps improve this project

### 📖 Knowledge Base Sources

The MCP knowledge base is compiled from several authoritative sources:

| Source | Content |
|--------|---------|
| [decriptor/AvaloniaUI.MCP](https://github.com/decriptor/AvaloniaUI.MCP) | Original controls catalog, XAML patterns, WPF migration guide |
| [docs.avaloniaui.net](https://docs.avaloniaui.net/) | Official API reference, control documentation, tutorials |
| [AvaloniaUI.QuickGuides](https://github.com/AvaloniaUI/AvaloniaUI.QuickGuides) | Practical patterns: bindings, drag-drop, file I/O, localization, native menus, custom controls |
| [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia) | Ecosystem: MVVM libraries, UI themes, chart controls, community apps |

### 📋 What Knowledge This Server Has

The current knowledge base covers:

**Controls** (in `controls.json`):
- Layout: Grid, StackPanel, DockPanel, Canvas, WrapPanel, Panel, UniformGrid, RelativePanel
- Input: Button, TextBox, CheckBox, RadioButton, Slider, RepeatButton, ToggleButton, SplitButton, ToggleSplitButton, MaskedTextBox
- Display: TextBlock, Image, ProgressBar, SelectableTextBlock, PathIcon
- Selection: ComboBox, ListBox, DataGrid, ItemsControl, ItemsRepeater
- Container: TabControl, Expander, ScrollViewer, Border, Popup, ButtonSpinner, GridSplitter
- Navigation: TreeView, Menu, ContextMenu, MenuItem
- Date/Time: Calendar, DatePicker, CalendarDatePicker, TimePicker
- Advanced: AutoCompleteBox, NumericUpDown, ToolTip, Flyout, SplitView, ColorPicker

**XAML Patterns** (in `xaml-patterns.json`):
- Basic window, MVVM window, data binding, styles/resources
- Grid layout, user control, container queries, compiled bindings
- Advanced styling, spacing, popup/overlay
- Bindings and value converters, drag and drop, file picker dialogs
- Clipboard operations, localization/i18n, splash screen
- Native OS menus, custom control authoring, items-repeater lists

**Migration Guide** (in `migration-guide.json`):
- WPF to Avalonia control mappings, namespace changes, binding differences
- Step-by-step migration process

**Community Resources** (in `community-resources.json`):
- MVVM frameworks: ReactiveUI, CommunityToolkit.Mvvm, Prism.Avalonia
- UI themes: Semi.Avalonia, Material.Avalonia, FluentAvalonia
- Chart/data-viz libraries: LiveCharts2, OxyPlot, ScottPlot
- Control libraries: AvaloniaEdit, DataGrid extras, docking panels
- Notable open-source apps built with Avalonia
- Official tutorials and learning resources

### 🗺️ What Could Be Added Next

- **Avalonia 12 / .NET 10 specific APIs** – keep in sync with the latest release notes from [docs.avaloniaui.net](https://docs.avaloniaui.net/)
- **Reactive programming patterns** – deeper ReactiveUI examples from the community
- **Testing patterns** – Avalonia.Headless unit testing examples
- **Native AOT guidance** – publishing guidance from the QuickGuides NativeAot sample
- **Mobile/Android/iOS deployment** – cross-platform deployment patterns
- **WASM/Browser target** – AvaloniaUI WebAssembly deployment knowledge

## 📞 Support

- **📖 Documentation**: [GitHub Pages](https://Architrixs.github.io/MCP.AvaloniaUI)
- **🐛 Bug Reports**: [GitHub Issues](https://github.com/Architrixs/MCP.AvaloniaUI/issues)
- **💬 Discussions**: [GitHub Discussions](https://github.com/Architrixs/MCP.AvaloniaUI/discussions)

---

<div align="center">
  <strong>Built with ❤️ for the AvaloniaUI community</strong>
  <br>
  <sub>
    🌟 Star us on GitHub • 🐛 Report issues • 🤝 Contribute • 📖 Read the docs
  </sub>
</div>
