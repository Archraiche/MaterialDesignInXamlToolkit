﻿using System.ComponentModel;
using MaterialDesignThemes.UITests.Samples.AutoSuggestBoxes;
using MaterialDesignThemes.UITests.Samples.AutoSuggestTextBoxes;
namespace MaterialDesignThemes.UITests.WPF.AutoSuggestBoxes;

public class AutoSuggestBoxTests : TestBase
{
    [Test]
    public async Task CanFilterItems_WithSuggestionsAndDisplayMember_FiltersSuggestions()
    {
        await using var recorder = new TestRecorder(App);

        //Arrange
        IVisualElement<AutoSuggestBox> suggestBox = (await LoadUserControl<AutoSuggestTextBoxWithTemplate>()).As<AutoSuggestBox>();
        IVisualElement<Popup> popup = await suggestBox.GetElement<Popup>();
        IVisualElement<ListBox> suggestionListBox = await popup.GetElement<ListBox>();

        //Act
        await suggestBox.MoveKeyboardFocus();
        await suggestBox.SendInput(new KeyboardInput("B"));


        //Assert
        await Assert.That(await suggestBox.GetIsSuggestionOpen()).IsTrue();
        await Assert.That(await popup.GetIsOpen()).IsTrue();

        //Validates these elements are found
        await AssertExists(suggestionListBox, "Bananas");
        await AssertExists(suggestionListBox, "Beans");

        //Validate other items are hidden
        await AssertExists(suggestionListBox, "Apples", false);
        await AssertExists(suggestionListBox, "Mtn Dew", false);
        await AssertExists(suggestionListBox, "Orange", false);

        recorder.Success();
    }

    [Test]
    public async Task CanChoiceItem_FromTheSuggestions_AssertTheTextUpdated()
    {
        await using var recorder = new TestRecorder(App);

        //Arrange
        IVisualElement<AutoSuggestBox> suggestBox = (await LoadUserControl<AutoSuggestTextBoxWithTemplate>()).As<AutoSuggestBox>();
        IVisualElement<Popup> popup = await suggestBox.GetElement<Popup>();
        IVisualElement<ListBox> suggestionListBox = await popup.GetElement<ListBox>();

        //Act
        await suggestBox.MoveKeyboardFocus();
        await suggestBox.SendInput(new KeyboardInput("B"));

        //Assert
        await Assert.That(await suggestBox.GetIsSuggestionOpen()).IsTrue();
        await Assert.That(await popup.GetIsOpen()).IsTrue();

        double? lastHeight = null;
        await Wait.For(async () =>
        {
            double currentHeight = await suggestionListBox.GetActualHeight();

            bool rv = currentHeight == lastHeight && currentHeight > 50;
            lastHeight = currentHeight;
            if (!rv)
            {
                await Task.Delay(100);
            }
            return rv;
        });

        //Choose Item from the list
        var bananas = await suggestionListBox.GetElement<ListBoxItem>("/ListBoxItem[0]");
        await bananas.MoveCursorTo();
        await bananas.LeftClick();

        // Wait for the text to be updated
        await Task.Delay(50);

        var suggestBoxText = await suggestBox.GetText();
        //Validate that the current text is the same as the selected item
        await Assert.That(suggestBoxText).IsEqualTo("Bananas");

        recorder.Success();
    }

    [Test]
    public async Task CanFilterItems_WithCollectionView_FiltersSuggestions()
    {
        await using var recorder = new TestRecorder(App);

        //Arrange
        IVisualElement userControl = await LoadUserControl<AutoSuggestTextBoxWithCollectionView>();
        IVisualElement<AutoSuggestBox> suggestBox = await userControl.GetElement<AutoSuggestBox>();
        IVisualElement<Popup> popup = await suggestBox.GetElement<Popup>();
        IVisualElement<ListBox> suggestionListBox = await popup.GetElement<ListBox>();

        //Act
        await suggestBox.MoveKeyboardFocus();
        await suggestBox.SendInput(new KeyboardInput("B"));


        //Assert
        await Assert.That(await suggestBox.GetIsSuggestionOpen()).IsTrue();
        await Assert.That(await popup.GetIsOpen()).IsTrue();

        //Validates these elements are found
        await AssertExists(suggestionListBox, "Bananas");
        await AssertExists(suggestionListBox, "Beans");

        //Validate other items are hidden
        await AssertExists(suggestionListBox, "Apples", false);
        await AssertExists(suggestionListBox, "Mtn Dew", false);
        await AssertExists(suggestionListBox, "Orange", false);

        recorder.Success();
    }

    [Test]
    [Description("Issue 3761")]
    public async Task AutoSuggestBox_MovesFocusToNextElement_WhenPopupIsClosed()
    {
        await using var recorder = new TestRecorder(App);

        // Arrange
        string xaml = """
            <StackPanel>
                <local:AutoSuggestTextBoxWithCollectionView x:Name="AutoSuggestBoxSample" />
                <TextBox x:Name="NextTextBox" />
            </StackPanel>
        """;

        IVisualElement<StackPanel> stackPanel = await LoadXaml<StackPanel>(xaml, ("local", typeof(AutoSuggestTextBoxWithCollectionView)));
        var suggestBoxSample = await stackPanel.GetElement<AutoSuggestTextBoxWithCollectionView>("AutoSuggestBoxSample");
        IVisualElement<AutoSuggestBox> suggestBox = await suggestBoxSample.GetElement<AutoSuggestBox>();
        IVisualElement<TextBox> nextTextBox = await stackPanel.GetElement<TextBox>("NextTextBox");

        // Act
        await suggestBox.MoveKeyboardFocus();
        await Task.Delay(50);
        await suggestBox.SendInput(new KeyboardInput("B")); // Open the popup
        await Task.Delay(50);
        await suggestBox.SendInput(new KeyboardInput(Key.Escape)); // Close the popup
        await Task.Delay(50);
        await suggestBox.SendInput(new KeyboardInput(Key.Tab)); // Press TAB to focus the next element
        await Task.Delay(50);

        // Assert
        await Assert.That(await suggestBox.GetIsFocused()).IsFalse();
        await Assert.That(await nextTextBox.GetIsFocused()).IsTrue();

        recorder.Success();
    }

    private static async Task AssertExists(IVisualElement<ListBox> suggestionListBox, string text, bool existsOrNotCheck = true)
    {
        _ = await suggestionListBox.GetElement(ElementQuery.PropertyExpression<TextBlock>(x => x.Text, text));
        await Assert.That(existsOrNotCheck).IsTrue();
    }
}
