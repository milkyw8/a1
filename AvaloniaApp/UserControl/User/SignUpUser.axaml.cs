using System;
using System.Linq;
using Avalonia.Interactivity;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.UserControl.User.UserProfile;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.UserControl.User;

public partial class SignUpUser : Avalonia.Controls.UserControl
{
    public SignUpUser()
    {
        InitializeComponent();
    }

    private void BtnSignUp_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Login = TxbLogin.Text;
            var Password = TxbPassword.Text;

            if (Login != null && Password != null)
            {
                var Data = ConnectDb.connect.Users.FirstOrDefault(x => x.Login == Login && x.Password == Password);
                if (Data != null)
                    FrameNavigation.navigate.Content = new UserProfileMain();
                else
                    MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Такой пользователь не найден").Show();
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Поле логин или пароль не могут быть пустыми",
                    ButtonEnum.Ok, Icon.Error).Show();
            }
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных",
                ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }
}