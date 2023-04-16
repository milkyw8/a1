using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.AppWindow.EmployeeGeneral;

public partial class CheckVisitor : Window
{
    public CheckVisitor()
    {
        try
        {
            InitializeComponent();
            BtnShowUserInformation();
            CheckUserBan();

            if (TxtbRequestStatus.Text == "Отклонена")
            {
                BtnRejectRequest.IsEnabled = false;
                BtnSaveDt.IsEnabled = false;
            }
            else
            {
                BtnRejectRequest.IsEnabled = true;
                BtnSaveDt.IsEnabled = true;
            }
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error).Show();
            Console.WriteLine(e);
        }


#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void BtnSaveDt_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Year = DtDateVisit.SelectedDate.Value.Year;
            var Month = DtDateVisit.SelectedDate.Value.Month;
            var Day = DtDateVisit.SelectedDate.Value.Day;
            var TimeToSeconds = TpTimeVisit.SelectedTime.Value.Seconds;

            if (DtDateVisit.SelectedDate != null && TpTimeVisit.SelectedTime != null)
            {
                var Date = new DateTime(Year, Month, Day);
                var DateUnix = new DateTimeOffset(Date).ToUnixTimeSeconds() + TimeToSeconds;

                var Data = ConnectDb.connect.UserGroups.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.Id);

                Data.VisitTime = DateUnix;
                ConnectDb.connect.SaveChanges();

                MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Дата и ввремя добавлены").Show();
            }
            else
            {
                MessageBoxManager
                    .GetMessageBoxStandardWindow("Ошибка", "Поля с датой и врменем обязательны для заполонения").Show();
            }
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error).Show();
            Console.WriteLine(ex);
        }
    }

    private async void BtnRejectRequest_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var result = await MessageBoxManager.GetMessageBoxStandardWindow("Отклонение заявки", "Вы уверены?",
                ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Question).Show();
            if (result == ButtonResult.Yes)
            {
                var Data = ConnectDb.connect.UserGroups.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.Id);
                Data.RequestStatusId = 2;
                TxtbRequestStatus.Text = Data.RequestStatus.Name;
                ConnectDb.connect.SaveChanges();
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandardWindow("Отклонение заявки", "Действия отменены",
                    ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Success).Show();
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error).Show();
            Console.WriteLine(ex);
        }
    }

    private void BtnShowUserInformation()
    {
        try
        {
            TxbFirstname.Text = HeapUserGroup.UserGroupHeap.Person.Firstname;
            TxbLastname.Text = HeapUserGroup.UserGroupHeap.Person.Lastname;
            TxbPatronymic.Text = HeapUserGroup.UserGroupHeap.Person.Patronymic;
            TxbPasportSeries.Text = HeapUserGroup.UserGroupHeap.Person.PassportSeries;
            TxbPasportNumber.Text = HeapUserGroup.UserGroupHeap.Person.PassportNumber;
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error).Show();
            Console.WriteLine(e);
        }
    }

    private void CheckUserBan()
    {
        try
        {
            var UserBan =
                ConnectDb.connect.ListUsers.FirstOrDefault(x => x.UserId == HeapUserGroup.UserGroupHeap.UserId);
            var data = ConnectDb.connect.UserGroups.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.Id);
            if (UserBan != null)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Формальная проверка",
                        "Пользователь находится в черном списке", ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Info)
                    .Show();
               
                data.RequestStatusId = 2;
                ConnectDb.connect.SaveChanges();

                var status = data.RequestStatus.Name;
                TxtbRequestStatus.Text = status;

                if (status == "Отклонена")
                {
                    BtnRejectRequest.IsEnabled = false;
                    BtnSaveDt.IsEnabled = false;
                }
                else
                {
                    BtnRejectRequest.IsEnabled = true;
                    BtnSaveDt.IsEnabled = true;
                }
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Формальная проверка",
                        "Пользователь в черном списке не найден", ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Info)
                    .Show();
                data.RequestStatusId = 1;
                ConnectDb.connect.SaveChanges();
                TxtbRequestStatus.Text = data.RequestStatus.Name;
            }
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обаботке данных",
                ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error).Show();
            Console.WriteLine(e);
        }
    }
}

