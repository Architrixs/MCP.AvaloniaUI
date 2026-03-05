# TODO: Control Coverage

Goal: extend `src/AvaloniaUI.MCP/Data/controls.json` to cover controls present in latest Avalonia docs but missing from current MCP reference.

## Controls added ✅

All previously missing controls have been added in the knowledge base update:

- ✅ ButtonSpinner (`container_controls`)
- ✅ ColorPicker (`advanced_controls`)
- ✅ GridSplitter (`layout_controls`)
- ✅ ItemsControl (`selection_controls`)
- ✅ ItemsRepeater (`selection_controls`)
- ✅ MaskedTextBox (`input_controls`)
- ✅ Panel (`layout_controls`)
- ✅ PathIcon (`display_controls`)
- ✅ RelativePanel (`layout_controls`)
- ✅ RepeatButton (`input_controls`)
- ✅ SelectableTextBlock (`display_controls`)
- ✅ SplitButton (`input_controls`)
- ✅ ToggleButton (`input_controls`)
- ✅ ToggleSplitButton (`input_controls`)
- ✅ UniformGrid (`layout_controls`)

## Required updates (completed)

- ✅ Added description, key properties, usage, and valid XAML example for each control.
- ✅ Added tests in `tests/AvaloniaUI.MCP.Tests/KnowledgeBaseTests.cs` that validate lookup for each newly added control via `GetControlInfo`.
- ✅ XAML validator no longer claims unsupported status for these controls.

## Remaining work

- Keep in sync with new controls introduced in future Avalonia releases (check [docs.avaloniaui.net](https://docs.avaloniaui.net/)).
- Consider adding `Viewbox` (display/layout helper) and `NativeMenuItem` (native menu integration) entries.

