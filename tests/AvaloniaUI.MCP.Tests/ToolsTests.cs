using AvaloniaUI.MCP.Tools;
using AvaloniaUI.MCP.Settings;

using System.Xml.Linq;

namespace AvaloniaUI.MCP.Tests;

[TestClass]
public class ToolsTests
{
    [TestMethod]
    public void EchoTool_ReturnsCorrectMessage()
    {
        // Arrange
        string message = "Hello World";

        // Act
        string result = EchoTool.Echo(message);

        // Assert
        StringAssert.Contains(result, "Hello from AvaloniaUI MCP Server", "Result should contain server greeting");
        StringAssert.Contains(result, message, "Result should contain the original message");
    }

    [TestMethod]
    public void EchoTool_GetServerInfo_ReturnsServerInformation()
    {
        // Act
        string result = EchoTool.GetServerInfo();

        // Assert
        StringAssert.Contains(result, "AvaloniaUI MCP Server", "Result should contain server name");
        StringAssert.Contains(result, "project generation", "Result should mention project generation capabilities");
        StringAssert.Contains(result, "XAML validation", "Result should mention XAML validation capabilities");
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_ValidatesProjectName()
    {
        // Arrange
        string emptyProjectName = "";

        // Act
        string result = ProjectGeneratorTool.CreateAvaloniaProject(emptyProjectName);

        // Assert
        StringAssert.Contains(result, "# ❌ Error", "Result should contain error header");
        StringAssert.Contains(result, "Project name cannot be empty", "Result should contain specific validation error message");
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_ValidatesTemplate()
    {
        // Arrange
        string projectName = "TestProject";
        string invalidTemplate = "invalid";

        // Act
        string result = ProjectGeneratorTool.CreateAvaloniaProject(projectName, invalidTemplate);

        // Assert
        StringAssert.Contains(result, "# ❌ Error", "Result should contain error header");
        StringAssert.Contains(result, "Invalid template type", "Result should contain template validation error");
        StringAssert.Contains(result, "invalid", "Result should contain the invalid template name");
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_GeneratesValidMvvmCsproj()
    {
        // Arrange
        string outputDirectory = Path.Combine(Path.GetTempPath(), $"avalonia-mcp-{Guid.NewGuid():N}");
        string projectName = "MyApp";

        try
        {
            // Act
            string result = ProjectGeneratorTool.CreateAvaloniaProject(projectName, "mvvm", "desktop", outputDirectory);

            // Assert
            StringAssert.Contains(result, "Successfully created MVVM AvaloniaUI project", "Project should be generated successfully");

            string projectFilePath = Path.Combine(outputDirectory, projectName, $"{projectName}.csproj");
            Assert.IsTrue(File.Exists(projectFilePath), "Generated csproj file should exist");

            string projectFileContent = File.ReadAllText(projectFilePath);

            Assert.IsFalse(projectFileContent.Contains("\\n", StringComparison.Ordinal), "Generated csproj should use real newlines, not literal \\n text");
            Assert.IsFalse(projectFileContent.Contains("<AvaloniaResource Include=\"**/*.axaml\" />", StringComparison.Ordinal), "Generated csproj should not explicitly include axaml files to avoid duplicate x:Class compilation");
            StringAssert.Contains(projectFileContent, "<PackageReference Include=\"Avalonia.Themes.Fluent\"", "Generated csproj should include Fluent theme package because App.axaml uses <FluentTheme />.");
            StringAssert.Contains(projectFileContent, "<AvaloniaXaml Remove=\"**/*.axaml\"", "Generated csproj should remove existing AvaloniaXaml items before including to avoid duplicate x:Class compilation.");
            StringAssert.Contains(projectFileContent, "<AvaloniaXaml Include=\"**/*.axaml\"", "Generated csproj should explicitly include axaml files as AvaloniaXaml.");
            StringAssert.Contains(projectFileContent, "<EnableDefaultAvaloniaItems>false</EnableDefaultAvaloniaItems>", "Generated csproj should disable default Avalonia item globbing to avoid duplicate item resolution.");

            XDocument parsedProject = XDocument.Parse(projectFileContent);
            Assert.IsNotNull(parsedProject.Root, "Generated csproj should be valid XML");
        }
        finally
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, recursive: true);
            }
        }
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_UsesPublishedAvaloniaVersion()
    {
        // Arrange
        string outputDirectory = Path.Combine(Path.GetTempPath(), $"avalonia-mcp-{Guid.NewGuid():N}");
        string projectName = "MyApp";

        try
        {
            // Act
            ProjectGeneratorTool.CreateAvaloniaProject(projectName, "mvvm", "desktop", outputDirectory);

            // Assert
            string projectFilePath = Path.Combine(outputDirectory, projectName, $"{projectName}.csproj");
            string projectFileContent = File.ReadAllText(projectFilePath);

            string expectedPackageReference = $"<PackageReference Include=\"Avalonia.ReactiveUI\" Version=\"{McpSettings.AvaloniaVersion}\" />";
            StringAssert.Contains(projectFileContent, expectedPackageReference, "Generated project should use configured Avalonia package version");
            Assert.AreEqual("11.3.9", McpSettings.AvaloniaVersion, "MCP should target a published Avalonia version");
        }
        finally
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, recursive: true);
            }
        }
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_RejectsUnsupportedMobileGeneration()
    {
        // Arrange
        string projectName = "MobileApp";

        // Act
        string result = ProjectGeneratorTool.CreateAvaloniaProject(projectName, "mvvm", "mobile");

        // Assert
        StringAssert.Contains(result, "# ❌ Error", "Result should contain error header");
        StringAssert.Contains(result, "temporarily unavailable", "Result should explain unsupported platform generation");
    }

    [TestMethod]
    public void ProjectGeneratorTool_CreateAvaloniaProject_CrossPlatformDesktopProgramTargetsSharedAppNamespace()
    {
        // Arrange
        string outputDirectory = Path.Combine(Path.GetTempPath(), $"avalonia-mcp-{Guid.NewGuid():N}");
        string projectName = "MyApp";

        try
        {
            // Act
            ProjectGeneratorTool.CreateAvaloniaProject(projectName, "crossplatform", "desktop", outputDirectory);

            // Assert
            string desktopProgramPath = Path.Combine(outputDirectory, projectName, "Program.Desktop.cs");
            Assert.IsTrue(File.Exists(desktopProgramPath), "Cross-platform desktop program file should exist");

            string desktopProgramContent = File.ReadAllText(desktopProgramPath);
            StringAssert.Contains(desktopProgramContent, $"namespace {projectName}.Desktop;", "Desktop program should use desktop namespace");
            StringAssert.Contains(desktopProgramContent, $"AppBuilder.Configure<global::{projectName}.App>()", "Desktop program should reference the shared App namespace");
        }
        finally
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, recursive: true);
            }
        }
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_RejectsEmptyContent()
    {
        // Arrange
        string emptyXaml = "";

        // Act
        string result = XamlValidationTool.ValidateXaml(emptyXaml);

        // Assert
        StringAssert.Contains(result, "# ❌ Error", "Result should contain error header");
        StringAssert.Contains(result, "XAML content cannot be empty", "Result should contain empty content validation error");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_AcceptsValidXaml()
    {
        // Arrange
        string validXaml = @"<Window xmlns=""https://github.com/avaloniaui""
                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                             <TextBlock Text=""Hello World"" />
                          </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(validXaml);

        // Assert
        StringAssert.Contains(result, "✅ XAML Validation Passed", "Result should contain validation success message");
        StringAssert.Contains(result, "✓ XML syntax is valid", "Result should confirm XML syntax validity");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_DoesNotFlagDockPanelAsUnsupported()
    {
        // Arrange
        string validXaml = @"<Window xmlns=""https://github.com/avaloniaui""
                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                             <DockPanel>
                                <TextBlock Text=""Header"" DockPanel.Dock=""Top"" />
                                <TextBlock Text=""Body"" />
                             </DockPanel>
                          </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(validXaml);

        // Assert
        Assert.IsFalse(result.Contains("DockPanel is not available in AvaloniaUI", StringComparison.Ordinal),
            "DockPanel should not be reported as unsupported.");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_DoesNotFlagViewboxAsUnsupported()
    {
        // Arrange
        string validXaml = @"<Window xmlns=""https://github.com/avaloniaui""
                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                             <Viewbox>
                                <TextBlock Text=""Scaled"" />
                             </Viewbox>
                          </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(validXaml);

        // Assert
        Assert.IsFalse(result.Contains("Viewbox is not available in AvaloniaUI", StringComparison.Ordinal),
            "Viewbox should not be reported as unsupported.");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_DoesNotFlagUniformGridAsUnsupported()
    {
        // Arrange
        string validXaml = @"<Window xmlns=""https://github.com/avaloniaui""
                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                             <UniformGrid Rows=""1"" Columns=""2"">
                                <TextBlock Text=""A"" />
                                <TextBlock Text=""B"" />
                             </UniformGrid>
                          </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(validXaml);

        // Assert
        Assert.IsFalse(result.Contains("UniformGrid is not available in AvaloniaUI", StringComparison.Ordinal),
            "UniformGrid should not be reported as unsupported.");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_FlagsStatusBarAsUnsupported()
    {
        // Arrange
        string xaml = @"<Window xmlns=""https://github.com/avaloniaui""
                            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                        <StatusBar>
                            <TextBlock Text=""Ready"" />
                        </StatusBar>
                        </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xaml);

        // Assert
        StringAssert.Contains(result, "StatusBar is not available in AvaloniaUI", "StatusBar should be reported as unsupported.");
    }

    [TestMethod]
    public void XamlValidationTool_ValidateXaml_DoesNotWarnForFindAncestorBinding()
    {
        // Arrange
        string xaml = @"<Window xmlns=""https://github.com/avaloniaui""
                            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                        <TextBlock Text=""{Binding DataContext.Title, RelativeSource={RelativeSource FindAncestor, AncestorType=Window}}"" />
                        </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xaml);

        // Assert
        Assert.IsFalse(result.Contains("may not work as expected", StringComparison.Ordinal),
            "FindAncestor binding should no longer produce an inaccurate warning.");
    }

    [TestMethod]
    public void AnimationTool_GenerateStoryboard_UsesAvaloniaAnimationNotStoryboard()
    {
        // Act
        string result = AnimationTool.GenerateStoryboard("fade in panel", 800, "PanelIntroAnimation");

        // Assert
        StringAssert.Contains(result, "<Animation x:Key=\"PanelIntroAnimation\"", "Generated animation should define an Avalonia Animation resource.");
        Assert.IsFalse(result.Contains("<Storyboard>", StringComparison.Ordinal),
            "Generated animation output should not contain WPF Storyboard markup.");
    }

    [TestMethod]
    public void CustomControlGenerator_GenerateControlTemplate_DoesNotEmitStoryboardMarkup()
    {
        // Act
        string result = CustomControlGenerator.GenerateControlTemplate("Button", "PrimaryButtonTemplate", includeAnimations: "true");

        // Assert
        Assert.IsFalse(result.Contains("<Storyboard>", StringComparison.Ordinal),
            "Generated control template guidance should avoid WPF Storyboard markup.");
        StringAssert.Contains(result, "<VisualState.Setters>", "Generated control template should use visual state setters.");
    }

    [TestMethod]
    public void XamlValidationTool_ConvertWpfXamlToAvalonia_RejectsEmptyContent()
    {
        // Arrange
        string emptyXaml = "";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(emptyXaml);

        // Assert
        StringAssert.Contains(result, "Error: WPF XAML content cannot be empty", "Result should contain empty content error message");
    }

    [TestMethod]
    public void XamlValidationTool_ConvertWpfXamlToAvalonia_ConvertsNamespaces()
    {
        // Arrange
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <TextBlock Text=""Hello World"" />
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        StringAssert.Contains(result, "🔄 WPF to AvaloniaUI XAML Conversion Complete", "Result should contain conversion completion message");
        StringAssert.Contains(result, "https://github.com/avaloniaui", "Result should contain AvaloniaUI namespace");
        StringAssert.Contains(result, "✓ Replaced WPF presentation namespace", "Result should confirm namespace replacement");
    }
}
