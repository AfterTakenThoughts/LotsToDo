using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LotsToDo.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Click!");
    }
}