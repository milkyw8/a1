using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.AppWindow.SecurityGuard;

public partial class AllowAccess : Window
{
    public AllowAccess()
    {
        InitializeComponent();
        var validFrom = HeapUserGroup.UserGroupHeap.ValidFrom;
        var validUntil = HeapUserGroup.UserGroupHeap.ValidUntil;

        var validFromDateTime = DateTimeOffset.FromUnixTimeSeconds(validFrom);
        var validUntilDateTime = DateTimeOffset.FromUnixTimeSeconds(validUntil);
        
        LblFirstname.Content = HeapUserGroup.UserGroupHeap.Person.Firstname;
        LblLastname.Content = HeapUserGroup.UserGroupHeap.Person.Lastname;
        LblPatronymic.Content = HeapUserGroup.UserGroupHeap.Person.Patronymic;
        LblTypeVisit.Content = HeapUserGroup.UserGroupHeap.TypeVisit;
        LblValidFrom.Content = validFromDateTime;
        LblValidUntil.Content = validUntilDateTime;
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void BtnAllow_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (TxbIntervalAllowTime.Text != null)
            {
                var CurrentDateTime = DateTimeOffset.UtcNow.LocalDateTime.ToString("F");
                Console.WriteLine(CurrentDateTime);

                var UnixDateTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                Console.WriteLine(UnixDateTimeNow);

                var UnixDateTimeLeaving = DateTimeOffset.UtcNow.ToUnixTimeSeconds() +
                                          Convert.ToInt64(TxbIntervalAllowTime.Text) * 60;
                Console.WriteLine(UnixDateTimeLeaving);

                var DateLeaving = DateTimeOffset.FromUnixTimeSeconds(UnixDateTimeLeaving).LocalDateTime.ToString("F");
                Console.WriteLine(DateLeaving);

                var data =
                    ConnectDb.connect.UserGroups.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.Id);


                data.VisitTime = UnixDateTimeNow;
                data.LeavingTime = UnixDateTimeLeaving;
                ConnectDb.connect.SaveChanges();

                MessageBoxManager
                        .GetMessageBoxStandardWindow("Успех",
                            "Данные успешно добавлены." +
                            $"Время входа {CurrentDateTime}." +
                            $" Время убытия {DateLeaving}",
                            ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Success)
                        .Show();
            }
            else
            {
                MessageBoxManager
                    .GetMessageBoxStandardWindow("Ошибка",
                        "Укажите временной интервал на доступ",
                        ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Success)
                    .Show();
            }
     
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok,
                MessageBox.Avalonia.Enums.Icon.Error);
            Console.WriteLine(exception);
            throw;
        }
      
    }
}