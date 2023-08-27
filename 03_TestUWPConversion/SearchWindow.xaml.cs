using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static WinUI03_TestUWPConversion.CurrentSearchOptions;
using System.Text;

namespace WinUI03_TestUWPConversion;

public sealed partial class SearchWindow: Window
{
    private readonly SearchViewModel VM;

    public SearchWindow()
    {
        InitializeComponent();

        VM = new SearchViewModel();
        MainGrid.DataContext = VM;

        MainGrid.Loaded += (s, e) => {
            FilterTextBox.Focus(FocusState.Programmatic);
            VM.InitializeBindings();
        };
    }

    // Main function called from outside to show modal search form, and return null (nothing selected or cancelled) or
    // a string representing character(s) selected
    internal string? GetChar()
    {
        // Multiple selection is allowed
        var sb = new StringBuilder();
        foreach (UnicodeSequence us in MatchesListView.SelectedItems.Cast<UnicodeSequence>())
        {
            if (VM.OutputName ?? false)
                sb.Append("{" + us.Name + "}");
            else if (VM.OutputCharacters ?? false)
                sb.Append(us.SequenceAsString);
            else
                foreach (int cp in us.Sequence)
                    sb.Append($"U+{cp:X4}");
        }

        return sb.ToString();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        // No commanding here (didn't want to add RelayCommand) so Ok may be rejected if nothing is selected
        if (VM.SelectedSequence != null)
            Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

    private void MatchesListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (VM.SelectedSequence != null)        // Just in case we double-click in a part of the list that is not a sequence
            Close();
    }
}
