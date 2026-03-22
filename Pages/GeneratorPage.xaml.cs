using QRCodeApp.ViewModels;

namespace QRCodeApp;

public partial class GeneratorPage : ContentPage
{
    public GeneratorPage(GeneratorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}