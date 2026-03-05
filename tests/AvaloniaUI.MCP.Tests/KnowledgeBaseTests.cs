using AvaloniaUI.MCP.Resources;

namespace AvaloniaUI.MCP.Tests;

/// <summary>
/// Tests that verify the new controls added from docs/todo/control-coverage.md are present
/// and discoverable via GetControlInfo.
/// </summary>
[TestClass]
public class NewControlsTests
{
    // ── Layout controls ──────────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_Panel_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("Panel");
        Assert.IsFalse(result.StartsWith("Control 'Panel' not found", StringComparison.Ordinal),
            "Panel should be present in controls.json");
        StringAssert.Contains(result, "Panel", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_UniformGrid_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("UniformGrid");
        Assert.IsFalse(result.StartsWith("Control 'UniformGrid' not found", StringComparison.Ordinal),
            "UniformGrid should be present in controls.json");
        StringAssert.Contains(result, "UniformGrid", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_RelativePanel_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("RelativePanel");
        Assert.IsFalse(result.StartsWith("Control 'RelativePanel' not found", StringComparison.Ordinal),
            "RelativePanel should be present in controls.json");
        StringAssert.Contains(result, "RelativePanel", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_GridSplitter_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("GridSplitter");
        Assert.IsFalse(result.StartsWith("Control 'GridSplitter' not found", StringComparison.Ordinal),
            "GridSplitter should be present in controls.json");
        StringAssert.Contains(result, "GridSplitter", "Result should mention the control name");
    }

    // ── Input controls ───────────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_RepeatButton_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("RepeatButton");
        Assert.IsFalse(result.StartsWith("Control 'RepeatButton' not found", StringComparison.Ordinal),
            "RepeatButton should be present in controls.json");
        StringAssert.Contains(result, "RepeatButton", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_ToggleButton_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ToggleButton");
        Assert.IsFalse(result.StartsWith("Control 'ToggleButton' not found", StringComparison.Ordinal),
            "ToggleButton should be present in controls.json");
        StringAssert.Contains(result, "ToggleButton", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_SplitButton_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("SplitButton");
        Assert.IsFalse(result.StartsWith("Control 'SplitButton' not found", StringComparison.Ordinal),
            "SplitButton should be present in controls.json");
        StringAssert.Contains(result, "SplitButton", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_ToggleSplitButton_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ToggleSplitButton");
        Assert.IsFalse(result.StartsWith("Control 'ToggleSplitButton' not found", StringComparison.Ordinal),
            "ToggleSplitButton should be present in controls.json");
        StringAssert.Contains(result, "ToggleSplitButton", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_MaskedTextBox_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("MaskedTextBox");
        Assert.IsFalse(result.StartsWith("Control 'MaskedTextBox' not found", StringComparison.Ordinal),
            "MaskedTextBox should be present in controls.json");
        StringAssert.Contains(result, "MaskedTextBox", "Result should mention the control name");
    }

    // ── Display controls ─────────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_SelectableTextBlock_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("SelectableTextBlock");
        Assert.IsFalse(result.StartsWith("Control 'SelectableTextBlock' not found", StringComparison.Ordinal),
            "SelectableTextBlock should be present in controls.json");
        StringAssert.Contains(result, "SelectableTextBlock", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_PathIcon_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("PathIcon");
        Assert.IsFalse(result.StartsWith("Control 'PathIcon' not found", StringComparison.Ordinal),
            "PathIcon should be present in controls.json");
        StringAssert.Contains(result, "PathIcon", "Result should mention the control name");
    }

    // ── Selection controls ───────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_ItemsControl_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ItemsControl");
        Assert.IsFalse(result.StartsWith("Control 'ItemsControl' not found", StringComparison.Ordinal),
            "ItemsControl should be present in controls.json");
        StringAssert.Contains(result, "ItemsControl", "Result should mention the control name");
    }

    [TestMethod]
    public async Task GetControlInfo_ItemsRepeater_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ItemsRepeater");
        Assert.IsFalse(result.StartsWith("Control 'ItemsRepeater' not found", StringComparison.Ordinal),
            "ItemsRepeater should be present in controls.json");
        StringAssert.Contains(result, "ItemsRepeater", "Result should mention the control name");
    }

    // ── Container controls ───────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_ButtonSpinner_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ButtonSpinner");
        Assert.IsFalse(result.StartsWith("Control 'ButtonSpinner' not found", StringComparison.Ordinal),
            "ButtonSpinner should be present in controls.json");
        StringAssert.Contains(result, "ButtonSpinner", "Result should mention the control name");
    }

    // ── Advanced controls ────────────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_ColorPicker_ReturnsInfo()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("ColorPicker");
        Assert.IsFalse(result.StartsWith("Control 'ColorPicker' not found", StringComparison.Ordinal),
            "ColorPicker should be present in controls.json");
        StringAssert.Contains(result, "ColorPicker", "Result should mention the control name");
    }

    // ── Case-insensitive lookup ───────────────────────────────────────────────

    [TestMethod]
    public async Task GetControlInfo_IsCaseInsensitive()
    {
        string result = await AvaloniaControlsResource.GetControlInfo("colORpicker");
        Assert.IsFalse(result.StartsWith("Control 'colORpicker' not found", StringComparison.Ordinal),
            "Control lookup should be case-insensitive");
    }
}

/// <summary>
/// Tests that verify the new XAML patterns from AvaloniaUI.QuickGuides are present.
/// </summary>
[TestClass]
public class NewXamlPatternsTests
{
    [TestMethod]
    public async Task GetXamlPattern_ValueConverter_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("value_converter");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "value_converter pattern should exist");
        StringAssert.Contains(result, "Converter", "Result should mention converters");
    }

    [TestMethod]
    public async Task GetXamlPattern_DragAndDrop_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("drag_and_drop");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "drag_and_drop pattern should exist");
    }

    [TestMethod]
    public async Task GetXamlPattern_FilePicker_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("file_picker");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "file_picker pattern should exist");
        StringAssert.Contains(result, "StorageProvider", "Result should mention StorageProvider");
    }

    [TestMethod]
    public async Task GetXamlPattern_Clipboard_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("clipboard");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "clipboard pattern should exist");
    }

    [TestMethod]
    public async Task GetXamlPattern_Localization_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("localization");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "localization pattern should exist");
    }

    [TestMethod]
    public async Task GetXamlPattern_SplashScreen_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("splash_screen");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "splash_screen pattern should exist");
    }

    [TestMethod]
    public async Task GetXamlPattern_NativeMenu_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("native_menu");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "native_menu pattern should exist");
    }

    [TestMethod]
    public async Task GetXamlPattern_CustomControl_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("custom_control");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "custom_control pattern should exist");
        StringAssert.Contains(result, "TemplatedControl", "Result should mention TemplatedControl");
    }

    [TestMethod]
    public async Task GetXamlPattern_ItemsRepeaterList_ReturnsInfo()
    {
        string result = await XamlPatternsResource.GetXamlPattern("items_repeater_list");
        Assert.IsFalse(result.Contains("not found", StringComparison.OrdinalIgnoreCase),
            "items_repeater_list pattern should exist");
        StringAssert.Contains(result, "ItemsRepeater", "Result should mention ItemsRepeater");
    }
}

/// <summary>
/// Tests that verify the new community resources are properly loaded and returned.
/// </summary>
[TestClass]
public class CommunityResourcesTests
{
    [TestMethod]
    public async Task GetCommunityResources_ReturnsContent()
    {
        string result = await CommunityResourcesResource.GetCommunityResources();
        Assert.IsFalse(string.IsNullOrWhiteSpace(result), "GetCommunityResources should return non-empty content");
        StringAssert.Contains(result, "ReactiveUI", "Result should mention ReactiveUI");
        StringAssert.Contains(result, "awesome-avalonia", "Result should credit awesome-avalonia");
    }

    [TestMethod]
    public async Task GetMvvmFrameworks_ReturnsFrameworks()
    {
        string result = await CommunityResourcesResource.GetMvvmFrameworks();
        Assert.IsFalse(string.IsNullOrWhiteSpace(result), "GetMvvmFrameworks should return content");
        StringAssert.Contains(result, "ReactiveUI", "Result should list ReactiveUI");
        StringAssert.Contains(result, "CommunityToolkit.Mvvm", "Result should list CommunityToolkit.Mvvm");
        StringAssert.Contains(result, "Prism.Avalonia", "Result should list Prism.Avalonia");
    }

    [TestMethod]
    public async Task GetUiThemes_ReturnsThemes()
    {
        string result = await CommunityResourcesResource.GetUiThemes();
        Assert.IsFalse(string.IsNullOrWhiteSpace(result), "GetUiThemes should return content");
        StringAssert.Contains(result, "Semi.Avalonia", "Result should mention Semi.Avalonia");
        StringAssert.Contains(result, "FluentAvalonia", "Result should mention FluentAvalonia");
    }

    [TestMethod]
    public async Task GetOfficialResources_ReturnsOfficialLinks()
    {
        string result = await CommunityResourcesResource.GetOfficialResources();
        Assert.IsFalse(string.IsNullOrWhiteSpace(result), "GetOfficialResources should return content");
        StringAssert.Contains(result, "docs.avaloniaui.net", "Result should reference official docs URL");
        StringAssert.Contains(result, "QuickGuides", "Result should reference QuickGuides");
    }
}
