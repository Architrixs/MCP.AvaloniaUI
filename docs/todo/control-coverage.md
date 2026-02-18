# TODO: Control Coverage

Goal: extend `src/AvaloniaUI.MCP/Data/controls.json` to cover controls present in latest Avalonia docs but missing from current MCP reference.

## Missing controls to add

- ButtonSpinner
- ColorPicker
- GridSplitter
- ItemsControl
- ItemsRepeater
- MaskedTextBox
- Panel
- PathIcon
- RelativePanel
- RepeatButton
- SelectableTextBlock
- SplitButton
- ToggleButton
- ToggleSplitButton
- UniformGrid

## Required updates

- Add description, key properties, usage, and valid XAML example for each control.
- Add tests that validate lookup for each newly added control via `GetControlInfo`.
- Ensure docs and examples do not claim unsupported status for newly added controls.
