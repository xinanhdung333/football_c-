using System.Windows.Controls;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class ServicesView : UserControl
{
    public ServicesView(ServicesViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
