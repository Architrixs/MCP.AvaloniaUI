# TODO: Control Generation Fixes

Goal: improve generated output so control examples are valid and idiomatic for Avalonia.

## DockPanel (first target)

Issue: generic template generation currently emits usage that can be invalid for panel semantics (e.g., `Content`-style usage instead of child docking layout).

### Fix

- Add DockPanel-specific generation path for usage examples.
- Output should demonstrate:
  - Multiple child controls
  - `DockPanel.Dock` assignments
  - `LastChildFill` behavior

### Tests

- Add generator test asserting DockPanel output contains child layout example and `DockPanel.Dock` attributes.
- Ensure no `Content="..."` usage is emitted for DockPanel examples.
