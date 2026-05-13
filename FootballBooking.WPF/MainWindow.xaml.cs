using System.Windows;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}