using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.UserControl.Admin;

/// <summary>
///     Логика для страницы Админа
/// </summary>
public partial class AdminControl : Avalonia.Controls.UserControl
{
    public List<string> _Gender = new() { "М", "Ж" };
    private static int _ErrorUser { get; set; }

    public AdminControl()
    {
        InitializeComponent();
        CheckBlockWindow();
        
        TxtbFio.Text =
            $"{HeapUserGroup.UserGroupHeap.Person.Patronymic} {HeapUserGroup.UserGroupHeap.Person.Firstname[0]}.{HeapUserGroup.UserGroupHeap.Person.Lastname[0]}.";
        CmbGender.Items = _Gender;
        CmbPosition.Items = ConnectDb.connect.Positions.Select(x => x.Name).ToList();
        
    }

    private void BtnExit_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }

    private async void BtnSave_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (TxbFirstname.Text == null && TxbLastname.Text == null && TxbPatronymic.Text == null &&
                CmbPosition.SelectedItem == null)
            {
                if (_ErrorUser >= 2)
                {
                    await MessageBoxManager.GetMessageBoxStandardWindow("Блокировка",
                        "Функция создания новых пользоватлей для вас временно недоступна.\nПопробуйте через 5 минут.",
                        ButtonEnum.Ok, Icon.Warning).Show();
                    BtnSave.IsEnabled = false;
    
                    var TimeBlock = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60;
                    Console.WriteLine(TimeBlock);
                    var UserData =
                        ConnectDb.connect.Users.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.UserId);
                    Console.WriteLine(UserData);
    
                    UserData.DateTimeWindowBlock = TimeBlock;
                    ConnectDb.connect.SaveChanges();

                    var CurrentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var StatusWindowBlock = ConnectDb.connect.Users
                        .Select(x => new { x.Id, x.DateTimeWindowBlock })
                        .Any(x => x.DateTimeWindowBlock < CurrentDateTime);

                    BtnSave.IsEnabled = StatusWindowBlock;
                    _ErrorUser = 0;


                }
                else
                {
                    await MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        "Вы отправляете пустые данные при двухкратном повторении вы будете заблокированы на 300 секунд",
                        ButtonEnum.Ok, Icon.Info).Show();
                    _ErrorUser++;
                }
    
            }
        }
        catch (Exception exception)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Ошибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
        }
    }
    
    private void BtnCancel_OnClick(object? sender, RoutedEventArgs e)
    {
        TxbFirstname.Clear();
        TxbLastname.Clear();
        TxbPatronymic.Clear();
        CmbPosition.SelectedItem = -1;
        CmbGender.SelectedItem = -1;
    
        MessageBoxManager.GetMessageBoxStandardWindow("Отмена", "Данные успешно очищены", ButtonEnum.Ok, Icon.Success)
            .Show();
    }

    private async void CheckBlockWindow()
    {
        try
        {
            while (true)
            {
                var CurrentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var StatusWindowBlock = ConnectDb.connect.Users
                    .Select(x => new { x.Id, x.DateTimeWindowBlock })
                    .Any(x => x.DateTimeWindowBlock < CurrentDateTime);

                BtnSave.IsEnabled = StatusWindowBlock;

                if (StatusWindowBlock)
                {
                    BtnSave.IsEnabled = true;
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}