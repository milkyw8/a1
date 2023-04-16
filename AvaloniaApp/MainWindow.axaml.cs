using Avalonia.Controls;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.UserControl;

namespace AvaloniaApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        FrameNavigation.navigate = FrmMain;
        FrmMain.Content = new AuthControl();
    }


}