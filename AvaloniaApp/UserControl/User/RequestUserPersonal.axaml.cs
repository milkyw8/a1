using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace AvaloniaApp.UserControl.User;

public partial class RequestUserPersonal : Avalonia.Controls.UserControl
{
    private List<string> _EmployeeList;
    public RequestUserPersonal()
    {
        InitializeComponent();

        CmbTargetVisit.Items = ConnectDb.connect.TargetVisits.Select(x => x.Name).Distinct().ToList();
        CmbSubdivision.Items = ConnectDb.connect.OfficeInfos.Select(x => x.Subdivision).Distinct().ToList();
    }

    private void BtnBack_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new ChooseVisitType();
    }


    private void CmbSubdivision_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            var Employee = ConnectDb.connect.UserGroups
                .Select(x => new
                {
                    x.Person.Id,
                    Fio = x.Person.Lastname + " " + x.Person.Firstname + " " + x.Person.Patronymic,
                    x.TargetVisitId, x.Employee.Subdivision
                })
                .Where(x => x.TargetVisitId == 2 && x.Subdivision == CmbSubdivision.SelectedItem)
                .ToList();

            if (Employee != null)
                TxbFioEmployee.Text = $"{Employee[0].Fio}";

            else
                CmbSubdivision.PlaceholderText = "Сотрудник не найден";
        }
        catch (Exception exception)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "ОШибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }

    private void CheckData()
    {
        try
        {
            if (!string.IsNullOrEmpty(CdpValidFrom.Text) && !string.IsNullOrEmpty(CdpValidUntil.Text) &&
                !string.IsNullOrEmpty(CmbTargetVisit.SelectedItem.ToString()) &&
                !string.IsNullOrEmpty(TxbFirstname.Text) && !string.IsNullOrEmpty(TxbLastname.Text) &&
                !string.IsNullOrEmpty(TxbEmail.Text) && !string.IsNullOrEmpty(TxbComment.Text) &&
                !string.IsNullOrEmpty(TxbPasportNumber.Text) && !string.IsNullOrEmpty(TxbPasportSeries.Text) &&
                !string.IsNullOrEmpty(CdpBirthday.Text))
                return;
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Заполнены не все поля, заполните поля и повторите попытку")
                .Show();
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "При работе приложения возникло непредвиденное исключение", ButtonEnum.Ok, Icon.Warning).Show();
            Console.WriteLine(e);
        }
    }


    private void BtnClearForm_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            CdpValidFrom.Text = null;
            CdpValidUntil.Text = null;
            CmbTargetVisit.SelectedItem = -1;

            CmbSubdivision.SelectedItem = -1;
            TxbFioEmployee.Text = null;

            TxbFirstname.Text = null;
            TxbLastname.Text = null;
            TxbPatronymic.Text = null;
            TxbEmail.Text = null;
            TxbPhone.Text = null;
            TxbComment.Text = null;
            TxbOrganization.Text = null;
            TxbPasportSeries.Text = null;
            TxbPasportNumber.Text = null;
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "При работе приложения возникло непредвиденное исключение", ButtonEnum.Ok, Icon.Warning).Show();
            Console.WriteLine(exception);
        }
    }

    private void BtnSaveRequest_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckData();

            var VisitorData = new PersonalDatum
            {
                Firstname = TxbFirstname.Text,
                Lastname = TxbLastname.Text,
                Patronymic = TxbPatronymic.Text,
                Phone = TxbPhone.Text,
                Email = TxbEmail.Text,
                DateBirthday = CdpBirthday.Text,
                PassportSeries = TxbPasportSeries.Text,
                PassportNumber = TxbPasportNumber.Text
            };
            ConnectDb.connect.PersonalData.Add(VisitorData);
            ConnectDb.connect.SaveChanges();

            var PersonId = VisitorData.Id;

            var TargetVisitId = ConnectDb.connect.TargetVisits
                .Where(x => x.Name == CmbTargetVisit.SelectedItem)
                .Select(x => x.Id)
                .FirstOrDefault();

            Console.WriteLine(TargetVisitId);

            var EmployeeId = ConnectDb.connect.OfficeInfos
                .Where(x => x.Subdivision == CmbSubdivision.SelectedItem)
                .Select(x => x.Id)
                .FirstOrDefault();

            Console.WriteLine(EmployeeId);

            var UserVisitor = new UserGroup
            {
                PersonId = PersonId,
                EmployeeId = EmployeeId,
                TypeVisit = "Личное",
                RequestStatusId = 3,
                Organization = TxbOrganization.Text,
                Comment = TxbComment.Text,
                TargetVisitId = TargetVisitId,
                DateRequest = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ValidFrom = ConverDateTimeUnix(ref CdpValidFrom),
                ValidUntil = ConverDateTimeUnix(ref CdpValidUntil)
            };

            ConnectDb.connect.Add(UserVisitor);
            ConnectDb.connect.SaveChanges();

            MessageBoxManager.GetMessageBoxStandardWindow("Успех",
                    $"Пользователь {UserVisitor.Person.Lastname} {UserVisitor.Person.Firstname} {UserVisitor.Person.Patronymic} создан")
                .Show();
            // MessageBoxManager.GetMessageBoxStandardWindow("Успех", $"Пользователь  создан").Show();
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok,
                Icon.Error, WindowStartupLocation.CenterOwner).Show();
            Console.WriteLine(exception);
        }
    }

    private long ConverDateTimeUnix(ref CalendarDatePicker date)
    {
        var Year = date.SelectedDate.Value.Year;
        var Month = date.SelectedDate.Value.Month;
        var Day = date.SelectedDate.Value.Day;

        var DateTime = new DateTime(Year, Month, Day);
        var DateTimeUnix = new DateTimeOffset(DateTime).ToUnixTimeMilliseconds();

        return DateTimeUnix;
    }
    
}