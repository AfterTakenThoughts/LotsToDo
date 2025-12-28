using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace LotsToDo.Views.InitialScreen;

public partial class RecommendedTasks : UserControl
{
    public RecommendedTasks()
    {
        InitializeComponent();
    }
    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Click!");
    }
}