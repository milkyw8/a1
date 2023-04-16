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

public partial class RequestUserGroup : Avalonia.Controls.UserControl
{
    private readonly List<PersonalDatum> _PersonalDataVisitor = new();
    public RequestUserGroup()
    {
        InitializeComponent();

        CmbTargetVisit.Items = ConnectDb.connect.TargetVisits.Select(x => x.Name).Distinct().ToList();

        CmbSubdivision.Items = ConnectDb.connect.OfficeInfos
            .Select(x => x.Subdivision)
            .Distinct()
            .ToList();
        
        
    }

    private long ConvertDateTimeToUnix(ref CalendarDatePicker date)
    {
        var Year = date.SelectedDate.Value.Year;
        var Month = date.SelectedDate.Value.Month;
        var Day = date.SelectedDate.Value.Day;

        var DateTime = new DateTime(Year, Month, Day);

        var DateTimeUnix = new DateTimeOffset(DateTime).ToUnixTimeSeconds();
        return DateTimeUnix;
    }

    private void BtnCreateRequest_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Data = ConnectDb.connect.UserGroups
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .OrderBy(x => x.Name)
                .LastOrDefault();

            GenerateName:
            var rnd = new Random();
            var GroupName = $"ГР{rnd.Next(1, 10000)}";

            if (Data.Name == GroupName) goto GenerateName;


            var TargetVisitId = ConnectDb.connect.TargetVisits
                .Where(x => x.Name == CmbTargetVisit.SelectedItem)
                .Select(x => x.Id)
                .FirstOrDefault();

            var EmployeeId = ConnectDb.connect.OfficeInfos
                .Where(x => x.Subdivision == CmbSubdivision.SelectedItem)
                .Select(x => x.Id)
                .FirstOrDefault();

            var CurrentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            AddVisitorPesronalDataDg(TxbFirstname.Text, TxbLastname.Text,
                TxbPatronymic.Text,
                TxbPhone.Text, TxbEmail.Text, CdpBirthday.Text,
                TxbPasportSeries.Text, TxbPasportNumber.Text);

            foreach (var Person in _PersonalDataVisitor)
            {
                
                int PersonId = AddVisitorPesronalData(Firstname: TxbFirstname.Text, Lastname: TxbLastname.Text,
                    Patronymic: TxbPatronymic.Text,
                    Phone: TxbPhone.Text, Email: TxbEmail.Text, DateBirthday: CdpBirthday.Text,
                    PasportSeries: TxbPasportSeries.Text, PasportNumber: TxbPasportNumber.Text, PersonId: out PersonId);

                var VisitorGroup = new UserGroup
                {
                    Name = GroupName,
                    PersonId = PersonId,
                    Comment = TxbComment.Text,
                    TargetVisitId = TargetVisitId,
                    Organization = TxbOrganization.Text,
                    EmployeeId = EmployeeId,
                    TypeVisit = "Групповой",
                    RequestStatusId = 3,
                    DateRequest = CurrentDateTime,
                    ValidFrom = ConvertDateTimeToUnix(ref CldValidFrom),
                    ValidUntil = ConvertDateTimeToUnix(ref CldValidUntil)
                };

                ConnectDb.connect.UserGroups.Add(VisitorGroup);
                ConnectDb.connect.SaveChanges();
            }
            
            MessageBoxManager
                .GetMessageBoxStandardWindow("Успех", "Запрос успешно отправлен", ButtonEnum.Ok, Icon.Success).Show();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private void BtnClearForm_OnClick(object? sender, RoutedEventArgs e)
    {
        TxbFirstname.Text = null;
        TxbLastname.Text = null;
        TxbPatronymic.Text = null;
        TxbComment.Text = null;
        TxbEmail.Text = null;
        TxbOrganization.Text = null;
        TxbPhone.Text = null;
        CdpBirthday.Text = null;
        TxbPasportNumber.Text = null;
        TxbPasportSeries.Text = null;

        CldValidFrom.Text = null;
        CldValidUntil.Text = null;
        CmbSubdivision.SelectedItem = -1;
        CmbTargetVisit.SelectedItem = -1;
        TxbEmployee.Text = null;
    }

    private void CmbSubdivision_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            // HeapUserGroup.UserGroupHeap = (sender as ComboBox)?.DataContext as UserGroup;
            
            var Employee = ConnectDb.connect.UserGroups
                .Select(x => new
                {
                    x.Person.Lastname, x.Person.Firstname, x.Person.Patronymic, x.TargetVisitId, x.Employee.Subdivision
                })
                .Where(x => x.TargetVisitId == 2 && x.Subdivision == CmbSubdivision.SelectedItem).ToList();

            
            if (Employee != null)
                TxbEmployee.Text = $"{Employee[0].Firstname} {Employee[0].Lastname} {Employee[0].Patronymic}";
            else
                TxbEmployee.Text = "Сотрудник не найден";

        }
        catch (Exception exception)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Ошика в обработке жанных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }

    private void BtnAddVisitor_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {

            CheckData();

            AddVisitorPesronalDataDg(TxbFirstname.Text, TxbLastname.Text,
                Patronymic: TxbPatronymic.Text,
                Phone: TxbPhone.Text, Email: TxbEmail.Text, DateBirthday: CdpBirthday.Text,
                TxbPasportSeries.Text, TxbPasportNumber.Text);
            
        }
        catch (Exception exception)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }

    private bool CheckData()
    {
        try
        {
            if (TxbFirstname.Text != null &&
                TxbLastname.Text != null &&
                TxbEmail.Text != null &&
                TxbOrganization.Text != null &&
                CdpBirthday.Text != null &&
                TxbPasportNumber.Text != null &&
                TxbPasportSeries.Text != null &&
                CldValidFrom.Text != null &&
                CldValidUntil.Text != null &&
                CmbSubdivision.SelectedItem != null &&
                CmbTargetVisit.SelectedItem != null &&
                TxbEmployee.Text != null)
            {
                return true;
            }

            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Заполнены не все поля. Заполните все поля и повторите попытку", ButtonEnum.Ok, Icon.Error).Show();
            return false;
        }
        catch (Exception e)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(e);
            throw;
        }
    }

    private int AddVisitorPesronalData(out int PersonId, string Firstname, string Lastname, string Patronymic,
        string Phone, string Email, string DateBirthday, string PasportSeries, string PasportNumber)
    {
        var VisitorData = new PersonalDatum
        {
            Firstname = Firstname,
            Lastname = Lastname,
            Patronymic = Patronymic,
            Phone = Phone,
            Email = Email,
            DateBirthday = DateBirthday,
            PassportSeries = PasportSeries,
            PassportNumber = PasportNumber
        };
        
        ConnectDb.connect.PersonalData.Add(VisitorData);
        ConnectDb.connect.SaveChanges();

        PersonId = VisitorData.Id;
        return PersonId;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new ChooseVisitType();
    }

    private void AddVisitorPesronalDataDg(string Firstname, string Lastname, string Patronymic,
        string Phone, string Email, string DateBirthday, string PasportSeries, string PasportNumber)
    {
        var count = _PersonalDataVisitor.Count + 1;
        var VisitorData = new PersonalDatum
        {
            Id = count,
            Firstname = Firstname,
            Lastname = Lastname,
            Patronymic = Patronymic,
            Phone = Phone,
            Email = Email,
            DateBirthday = DateBirthday,
            PassportSeries = PasportSeries,
            PassportNumber = PasportNumber
        };

        _PersonalDataVisitor.Add(VisitorData);
        Console.WriteLine("Пользователь добавлен");

        DgVisitorGroup.Items = _PersonalDataVisitor;
    }
    
}   