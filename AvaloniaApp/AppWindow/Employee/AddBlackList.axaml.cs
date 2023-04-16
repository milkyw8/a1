using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.AppWindow.Employee;

public partial class AddBlackList : Window
{
    public AddBlackList()
    {
        try
        {
            InitializeComponent();

            TxtbFirstname.Text = HeapUserGroup.UserGroupHeap.Person.Firstname;
            TxtbLastname.Text = HeapUserGroup.UserGroupHeap.Person.Lastname;
            TxtbPatronymic.Text = HeapUserGroup.UserGroupHeap.Person.Patronymic;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

#if DEBUG
        this.AttachDevTools();
#endif
    }


    private async void BtnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var TxbReason = this.TxbReason?.Text ?? string.Empty;
            var result = await MessageBoxManager.GetMessageBoxStandardWindow("Вопрос", "Вы уверены ?",
                ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Question).Show();

            if (TxbReason != "" && (result & ButtonResult.Yes) != 0)
            {
                var DataUser = ConnectDb.connect.Users.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.UserId);
                var TimeBan = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 2678400;

                DataUser.IsBlocked = true;

                var BlockUser = new ListUser
                {
                    UserId = DataUser.Id,
                    TimeBanUser = TimeBan,
                    Reason = TxbReason
                };

                ConnectDb.connect.ListUsers.Add(BlockUser);
                ConnectDb.connect.SaveChanges();

                await MessageBoxManager.GetMessageBoxStandardWindow("Успех",
                    "Пользователь успешно добавлен в ЧС",
                    ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Success).Show();
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                    "Вы не указали причину добавления в черный список", ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error).Show();
            }
        }
        catch (Exception exception)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Info).Show();
            Console.WriteLine(exception);
        }
    }
}