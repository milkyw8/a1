using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.AppWindow.SecurityGuard;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.UserControl.SecurityGuard;

/// <summary>
///     Логика работы окна охранника
/// </summary>
public partial class SecurityGuardMain : Avalonia.Controls.UserControl
{
    private List<UserGroup> UserGroupList;

    /// <summary>
    ///     Загрузка данных для страницы
    /// </summary>
    public SecurityGuardMain()
    {
        try
        {
            InitializeComponent();

            CmbTypeVisit.Items = ConnectDb.connect.UserGroups.Select(x => x.TypeVisit).Distinct().ToList();
            CmbSubdivision.Items = ConnectDb.connect.UserGroups
                .Select(x => x.Employee.Subdivision).Distinct().ToList();

            LoadDataVisitUser();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    ///     Поиск значений
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSearch_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            
            var TypeVisit = Convert.ToString(CmbTypeVisit.SelectedItem);
            var Subdivision = Convert.ToString(CmbSubdivision.SelectedItem);
            var TxtSearch = TxbSearch.Text;

            if (TxtSearch == null)
            {
                if (DpDateRequest != null && TypeVisit != "" && Subdivision != "")
                {
                    var Day = DpDateRequest.SelectedDate.Value.Day;
                    var Month = DpDateRequest.SelectedDate.Value.Month;
                    var Year = DpDateRequest.SelectedDate.Value.Year;
                    
                    var SelectedDate = new DateTime(Year, Month, Day);
                    Console.WriteLine(SelectedDate);

                    var DtRequestDayUnix = new DateTimeOffset(SelectedDate).ToUnixTimeSeconds();
                    Console.WriteLine(DtRequestDayUnix);

                    var DtNextDayRequest = new DateTime(Year, Month, Day + 1);
                    Console.WriteLine(DtNextDayRequest);

                    var DtNextDayRequestUnix = new DateTimeOffset(DtNextDayRequest).ToUnixTimeSeconds();
                    Console.WriteLine(DtNextDayRequestUnix);

                    var Filter = UserGroupList
                        .SkipWhile(x => x.DateRequest >= DtRequestDayUnix)
                        .Skip(1)
                        .TakeWhile(x => x.DateRequest <= DtNextDayRequestUnix)
                        .Where(x =>
                            x.TypeVisit == TypeVisit &&
                            x.Employee.Subdivision == Subdivision)
                        .ToList();

                    VisitUserList.Items = Filter;
                }

            }
            else
            {
                if (TxtSearch == "")
                {
                    VisitUserList.Items = UserGroupList;
                }
                else
                {
                    var Sorting = UserGroupList.Where(x =>
                            x.Person.Firstname == TxtSearch ||
                            x.Person.Lastname == TxtSearch ||
                            x.Person.Patronymic == TxtSearch ||
                            x.Person.PassportNumber == TxtSearch)
                        .ToList();

                    if (Sorting.Count > 0)
                        VisitUserList.Items = Sorting;
                    else
                        MessageBoxManager.GetMessageBoxStandardWindow("Поиск завершен", "Совпадений не найдено",
                            ButtonEnum.Ok, Icon.Info).Show();
                }
            }
        } 
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Неперехваченное исключение", ButtonEnum.Ok,
                Icon.Error);
        }
    }

    /// <summary>
    ///     Загрузка данных в DataGrid
    /// </summary>
    private void LoadDataVisitUser()
    {
        var userGroup = ConnectDb.connect.UserGroups
            .Include(x => x.Employee)
            .Include(x => x.Person)
            .Include(x => x.User)
            .OrderBy(x => x.UserId)
            .Where(x => x.RequestStatusId == 1)
            .ToList();
        UserGroupList = userGroup;
        VisitUserList.Items = UserGroupList;
    }

    /// <summary>
    ///     Вызов доп. окно для указания разрешения по врмени на вход
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnInfoUser_OnClick(object? sender, RoutedEventArgs e)
    {
        HeapUserGroup.UserGroupHeap = (sender as Button).DataContext as UserGroup;
        new AllowAccess().Show();
    }

    /// <summary>
    ///     Выход из учетной записи
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSignOut_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }

    /// <summary>
    ///     Удалить фильтры
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCancelFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        DpDateRequest.SelectedDate = null;
        CmbTypeVisit.SelectedItem = -1;
        CmbSubdivision.SelectedItem = -1;

        VisitUserList.Items = UserGroupList;
    }
}