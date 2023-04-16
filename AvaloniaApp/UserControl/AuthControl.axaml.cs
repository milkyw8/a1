using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.UserControl.Admin;
using AvaloniaApp.UserControl.Employee;
using AvaloniaApp.UserControl.EmployeeGeneral;
using AvaloniaApp.UserControl.SecurityGuard;
using AvaloniaApp.UserControl.SecurityOfficer;
using AvaloniaApp.UserControl.User;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.UserControl;

/// <summary>
///     Старница входа пользователя
/// </summary>
public partial class AuthControl : Avalonia.Controls.UserControl
{
    /// <summary>
    ///     Логика старницы входа пользователя
    /// </summary>

    public AuthControl()
    {
        InitializeComponent();
        CmbItmRole.Items = ConnectDb.connect.Roles.Select(x => x.Name).ToList();
        CmbItmRole.SelectedIndex = 0;
        BtnSignUpAdmin.IsEnabled = true;

    }

    /// <summary>
    ///     Показ конкретных элементов в зависимости от роли пользователя
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CmbItmRole_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        switch (CmbItmRole.SelectedItem)
        {
            case "Пользователь":
                SpUser.IsVisible = true;
                SpAdmin.IsVisible = false;
                SpDefault.IsVisible = false;
                break;

            case "Администратор доступа":
                SpUser.IsVisible = false;
                SpAdmin.IsVisible = true;
                SpDefault.IsVisible = false;
                break;

            case "Специалист по ИБ":
                SpUser.IsVisible = false;
                SpAdmin.IsVisible = true;
                SpDefault.IsVisible = false;
                break;

            default:
                SpUser.IsVisible = false;
                SpAdmin.IsVisible = false;
                SpDefault.IsVisible = true;
                break;
        }
    }

    /// <summary>
    ///     Обработка данных при входе сотрудников
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSignUp_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Auth = ConnectDb.connect.Users.FirstOrDefault(x =>
                x.Role.Name == CmbItmRole.SelectedItem &&
                x.SecretWord == TxbSecretWord.Text);


            if (Auth == null)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Ошибка входа",
                    "Секретное слово введено неверно, повторите попытку",
                        ButtonEnum.Ok, Icon.Warning).Show();
                
            }
            else
            {
                switch (Auth.RoleId)
                {
                    // Сотрудник общего отдела
                    case 3:
                        FrameNavigation.navigate.Content = new EmployeeGeneralMain();
                        MessageBoxManager
                            .GetMessageBoxStandardWindow("Успех",
                                $"Вы в системе, ваша роль: {CmbItmRole.SelectedItem}",
                                ButtonEnum.Ok,
                                Icon.Success)
                            .Show();
                        break;
                    
                    // Сотрудник подразделения 
                    case 4:
                        FrameNavigation.navigate.Content = new EmployeeMain();
                        MessageBoxManager
                            .GetMessageBoxStandardWindow("Успех",
                                $"Вы в системе, ваша роль: {CmbItmRole.SelectedItem}",
                                ButtonEnum.Ok,
                                Icon.Success)
                            .Show();
                        break;
                    
                    // Охранник
                    case 5:
                        FrameNavigation.navigate.Content = new SecurityGuardMain();
                        MessageBoxManager
                            .GetMessageBoxStandardWindow("Успех",
                                $"Вы в системе, ваша роль: {CmbItmRole.SelectedItem}",
                                ButtonEnum.Ok,
                                Icon.Success)
                            .Show();
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                    "Ошибка в обработке данных. Проверьте корректность введенных данных.",
                    ButtonEnum.Ok,
                    Icon.Error)
                .Show();
            Console.WriteLine(exception);
        }
    }

    private void BtnSignUpAdmin_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Data = ConnectDb.connect.UserGroups
                .Include(x => x.Person)
                .Include(x => x.User)
                .Include(x => x.Employee)
                .Where(x =>
                    x.User.Login == TxbLogin.Text &&
                    x.User.Password == TxbPassword.Text &&
                    x.User.SecretWord == TxbSecretWordAdmin.Text);

            var Auth = Data.FirstOrDefault(x =>
                x.User.Login == TxbLogin.Text &&
                x.User.Password == TxbPassword.Text &&
                x.User.SecretWord == TxbSecretWordAdmin.Text);

            HeapUserGroup.UserGroupHeap = Auth;
            if (Auth == null)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        "Ошибка в обработке данных. Проверьте корректность введенных данных.",
                        ButtonEnum.Ok,
                        Icon.Error)
                    .Show();
               
            }
            else
            {
                switch (Auth.User.RoleId)
                {
                    // Администратор доступа
                    case 2:
                        FrameNavigation.navigate.Content = new AdminControl();
                        break;
                    // Специалист по ИБ
                    case 6:
                        FrameNavigation.navigate.Content = new SecurityOfficerControl();
                        break;
                }
              
            }

        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Ошибка в обработке данных. Проверьте данные на корректность", ButtonEnum.Ok, Icon.Error).Show();
        }
    }

    private void BtnUserSendRequest_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new ChooseVisitType();
    }
}