using System.Windows.Controls;
using FootballBooking.WPF.ViewModels;

namespace FootballBooking.WPF.Views;

public partial class FieldsView : UserControl
{
    public FieldsView(FieldsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
