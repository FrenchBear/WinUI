// 01_FalloutDecrypter
// My first app in WinUI3
// Fallout 3 Decrypt Helper (WinUI3 version)
//
// 2023-01-25   PV
// 2023-08-25   PV      Restart playing with WinUI 3

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;

#pragma warning disable IDE0051 // Remove unused private members

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3First;

public sealed partial class MainWindow: Window
{
    public MainWindow()
    {
        InitializeComponent();
        Title = "Fallout 3 Decrypter (WinUI3)";        // For some reason, can't set it in XAML...

        // Resize window
        // https://stackoverflow.com/questions/67169712/winui-3-0-reunion-0-5-window-size
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 340, Height = 300 });

        AnalysisTextBlock.Text = "Enter first word";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(WordTextBox.Text) && int.TryParse(PlacedTextBox.Text, out int _))
        {
            if (string.IsNullOrEmpty(Word1TextBox.Text))
            {
                Word1TextBox.Text = WordTextBox.Text;
                Placed1TextBox.Text = PlacedTextBox.Text;
            }
            else if (string.IsNullOrEmpty(Word2TextBox.Text))
            {
                Word2TextBox.Text = WordTextBox.Text;
                Placed2TextBox.Text = PlacedTextBox.Text;
            }
            else if (string.IsNullOrEmpty(Word3TextBox.Text))
            {
                Word3TextBox.Text = WordTextBox.Text;
                Placed3TextBox.Text = PlacedTextBox.Text;
            }
            else if (string.IsNullOrEmpty(Word3TextBox.Text))
            {
                Word3TextBox.Text = WordTextBox.Text;
                Placed3TextBox.Text = PlacedTextBox.Text;
            }
            else
                Debug.WriteLine("Full error!");

            WordTextBox.Text = "";
            PlacedTextBox.Text = "";
            WordTextBox.Focus(FocusState.Programmatic);
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        WordTextBox.Text = "";
        Word1TextBox.Text = "";
        Word2TextBox.Text = "";
        Word3TextBox.Text = "";
        Word4TextBox.Text = "";
        PlacedTextBox.Text = "";
        Placed1TextBox.Text = "";
        Placed2TextBox.Text = "";
        Placed3TextBox.Text = "";
        Placed4TextBox.Text = "";
        ClearStatuses();
        WordTextBox.Focus(FocusState.Programmatic);
    }

    private void ClearStatuses()
    {
        Status1TextBlock.Text = "";
        Status2TextBlock.Text = "";
        Status3TextBlock.Text = "";
        Status4TextBlock.Text = "";
    }

    private void WordTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(WordTextBox.Text))
        {
            AnalysisTextBlock.Text = string.Empty;
            ClearStatuses();
        }
        else if (Placed1TextBox.Text == string.Empty)
        {
            AnalysisTextBlock.Text = "Enter first word";
            ClearStatuses();
        }
        else if (WordTextBox.Text.Length != Word1TextBox.Text.Length)
        {
            AnalysisTextBlock.Text = "Enter next word";
            CompareWords();
        }
        else
        {
            CompareWords();
            if (Status1TextBlock.Text == "OK" && (Status2TextBlock.Text == "OK" || Status2TextBlock.Text == "") && (Status3TextBlock.Text == "OK" || Status3TextBlock.Text == "") && (Status4TextBlock.Text == "OK" || Status4TextBlock.Text == ""))
                AnalysisTextBlock.Text = "Possible";
            else
                AnalysisTextBlock.Text = "Nope";
            PlacedTextBox.Focus(FocusState.Programmatic);
            WordTextBox.SelectAll();
        }
    }

    private void CompareWords()
    {
        CompareWord(WordTextBox.Text, Word1TextBox.Text, int.Parse(Placed1TextBox.Text), Status1TextBlock);
        if (!string.IsNullOrWhiteSpace(Word2TextBox.Text))
            CompareWord(WordTextBox.Text, Word2TextBox.Text, int.Parse(Placed2TextBox.Text), Status2TextBlock);
        if (!string.IsNullOrWhiteSpace(Word3TextBox.Text))
            CompareWord(WordTextBox.Text, Word3TextBox.Text, int.Parse(Placed3TextBox.Text), Status3TextBlock);
        if (!string.IsNullOrWhiteSpace(Word4TextBox.Text))
            CompareWord(WordTextBox.Text, Word4TextBox.Text, int.Parse(Placed4TextBox.Text), Status4TextBlock);
    }

    private static void CompareWord(string word, string guessn, int placedn, TextBlock statusnTextBlock)
    {
        if (string.IsNullOrWhiteSpace(guessn))
            return;

        bool partialCheck = word.Length != guessn.Length;
        int match = 0;
        for (int i = 0; i < word.Length; i++)
            if (word[i] == guessn[i])
                match++;
        if (partialCheck)
        {
            if (match > placedn)
                statusnTextBlock.Text = "NO";
            else
                statusnTextBlock.Text = "";
        }
        else
        {
            if (match == placedn)
                statusnTextBlock.Text = "OK";
            else
                statusnTextBlock.Text = "NO";
        }
    }

    private void PlacedTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key==Windows.System.VirtualKey.Enter)
            AddButton_Click(sender, e);
    }
}
