# TODO: XAML Validator Fixes

Goal: make validation messages match real Avalonia support and properties.

## Items

- Remove incorrect unsupported-control warnings for Avalonia controls:
  - UniformGrid
  - Viewbox
- Keep DockPanel fix in place and covered by regression tests.
- Review and correct property-level assumptions in control data:
  - `ScrollViewer.ZoomMode` appears incorrect for Avalonia and should be verified/replaced.

## Required tests

- Validation tests proving no unsupported warning for `UniformGrid` and `Viewbox`.
- Regression tests for controls flagged as unsupported in the past.
