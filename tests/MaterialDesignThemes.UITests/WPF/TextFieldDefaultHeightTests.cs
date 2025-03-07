

namespace MaterialDesignThemes.UITests.WPF;

public class TextFieldDefaultHeightTests(ITestOutputHelper output) : TestBase(output)
{
    private const int Precision = 3;

    [Test]
    public async Task SameHeightWithDefaultStyle()
    {
        await using var recorder = new TestRecorder(App);

        var stackPanel = await LoadXaml<StackPanel>("""
            <StackPanel>
                <TextBox />
                <PasswordBox />
                <ComboBox IsEditable="True" />
                <DatePicker />
                <materialDesign:TimePicker />
            </StackPanel>
            """);

        double height = await GetHeight(stackPanel, "TextBox");
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "ComboBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "DatePicker"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "TimePicker"), Precision);

        recorder.Success();
    }

 [Test]
    public async Task SameHeightWithFloatingHintStyle()
    {
        await using var recorder = new TestRecorder(App);

        var stackPanel = await LoadXaml<StackPanel>("""
            <StackPanel>
                <TextBox Style="{StaticResource MaterialDesignFloatingHintTextBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox Style="{StaticResource MaterialDesignFloatingHintPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox x:Name="RevealPasswordBox" Style="{StaticResource MaterialDesignFloatingHintRevealPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <ComboBox IsEditable="True" Style="{StaticResource MaterialDesignFloatingHintComboBox}" materialDesign:HintAssist.Hint="Hint" />
                <DatePicker Style="{StaticResource MaterialDesignFloatingHintDatePicker}" materialDesign:HintAssist.Hint="Hint" />
                <materialDesign:TimePicker Style="{StaticResource MaterialDesignFloatingHintTimePicker}" materialDesign:HintAssist.Hint="Hint" />
            </StackPanel>
            """);

        double height = await GetHeight(stackPanel, "TextBox");
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox", "RevealPasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "ComboBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "DatePicker"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "TimePicker"), Precision);

        recorder.Success();
    }

    [Test]
    public async Task SameHeightWithFilledStyle()
    {
        await using var recorder = new TestRecorder(App);

        var stackPanel = await LoadXaml<StackPanel>("""
            <StackPanel>
                <TextBox Style="{StaticResource MaterialDesignFilledTextBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox Style="{StaticResource MaterialDesignFilledPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox x:Name="RevealPasswordBox" Style="{StaticResource MaterialDesignFilledRevealPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <ComboBox IsEditable="True" Style="{StaticResource MaterialDesignFilledComboBox}" materialDesign:HintAssist.Hint="Hint" />
                <DatePicker Style="{StaticResource MaterialDesignFilledDatePicker}" materialDesign:HintAssist.Hint="Hint" />
                <materialDesign:TimePicker Style="{StaticResource MaterialDesignFilledTimePicker}" materialDesign:HintAssist.Hint="Hint" />
            </StackPanel>
            """);

        double height = await GetHeight(stackPanel, "TextBox");
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox", "RevealPasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "ComboBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "DatePicker"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "TimePicker"), Precision);

        recorder.Success();
    }

    [Test]
    public async Task SameHeightWithOutlinedStyle()
    {
        await using var recorder = new TestRecorder(App);

        var stackPanel = await LoadXaml<StackPanel>("""
            <StackPanel>
                <TextBox Style="{StaticResource MaterialDesignOutlinedTextBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox Style="{StaticResource MaterialDesignOutlinedPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <PasswordBox x:Name="RevealPasswordBox" Style="{StaticResource MaterialDesignOutlinedRevealPasswordBox}" materialDesign:HintAssist.Hint="Hint" />
                <ComboBox IsEditable="True" Style="{StaticResource MaterialDesignOutlinedComboBox}" />
                <DatePicker Style="{StaticResource MaterialDesignOutlinedDatePicker}" materialDesign:HintAssist.Hint="Hint" />
                <materialDesign:TimePicker Style="{StaticResource MaterialDesignOutlinedTimePicker}" materialDesign:HintAssist.Hint="Hint" />
            </StackPanel>
            """);

        double height = await GetHeight(stackPanel, "TextBox");
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "PasswordBox", "RevealPasswordBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "ComboBox"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "DatePicker"), Precision);
        await Assert.Equal(height, await GetHeight(stackPanel, "TimePicker"), Precision);

        recorder.Success();
    }

    private static async Task<double> GetHeight(IVisualElement container, string type, string? optionalName = null)
    {
        var element = await container.GetElement<FrameworkElement>(optionalName ?? "/" + type);
        double height = await element.GetActualHeight();
        await Assert.True(height > 0);
        return height;
    }
}
