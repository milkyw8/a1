using Avalonia.Interactivity;
using AvaloniaApp.Helper.Navigation;

namespace AvaloniaApp.UserControl.User;

public partial class ChooseVisitType : Avalonia.Controls.UserControl
{
    public ChooseVisitType()
    {
        InitializeComponent();
    }


    private void BtnSignUpUser_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new SignUpUser();
    }

    private void BtnRegistrationUser_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new RegistrationUser();
    }

    private void BtnChoseOtherRole_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }

    private void BtnPersonalVisit_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new RequestUserPersonal();
    }

    private void BtnGroupVisit_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new RequestUserGroup();
    }
}