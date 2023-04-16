using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.AppWindow.EmployeeGeneral;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.UserControl.EmployeeGeneral;

/// <summary>
///     Логика ркаботы окна сотрудниика общего отдела
/// </summary>
public partial class EmployeeGeneralMain : Avalonia.Controls.UserControl
{
    private List<UserGroup> UserGroupList;
    private readonly List<string> _TypeVisit = new() { "Персональный", "Групповой" };
    public EmployeeGeneralMain()
    {
        try
        {
            InitializeComponent();
            CmbTypeVisit.Items = _TypeVisit;
            CmbRequestStatus.Items = ConnectDb.connect.RequestStatuses.Select(x => x.Name).Distinct().ToList();
            CmbSubdivision.Items = ConnectDb.connect.UserGroups.Select(x => x.Employee.Subdivision).Distinct().ToList();
            LoadDataVisitUser();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private void BtnSignOut_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }

    /// <summary>
    ///     Логика работы поиска
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSearch_OnClick(object? sender, RoutedEventArgs e)
    {
        var TypeVisit = CmbTypeVisit.SelectedItem?.ToString() ?? string.Empty;
        var Subdivision = CmbSubdivision.SelectedItem?.ToString() ?? string.Empty;
        var RequestStatus = CmbRequestStatus.SelectedItem?.ToString() ?? string.Empty;


        var Filter = UserGroupList?.Where(x =>
            x.TypeVisit == TypeVisit ||
            x.Employee.Subdivision == Subdivision ||
            x.RequestStatus.Name == RequestStatus)?.ToList() ?? UserGroupList;

        if (Filter.Count() == 0)
            MessageBoxManager
                .GetMessageBoxStandardWindow("Поиск завершен", "Совпадений не найдено", ButtonEnum.Ok, Icon.Info)
                .Show();
        else
            VisitUserList.Items = Filter;
    }

    /// <summary>
    ///     Логика работы с данными выбранного пользователя
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void BtnInfoUser_OnClick(object? sender, RoutedEventArgs e)
    {
        HeapUserGroup.UserGroupHeap = (sender as Button).DataContext as UserGroup;
        new CheckVisitor().Show();
        LoadDataVisitUser();
    }

    /// <summary>
    ///     Метод загрузки данных
    /// </summary>
    private void LoadDataVisitUser()
    {
        var UserGroup = ConnectDb.connect.UserGroups
            .Include(x => x.User)
            .Include(x => x.Employee)
            .Include(x => x.Person)
            .Include(x => x.RequestStatus)
            .OrderBy(x => x.UserId)
            .ToList();
        UserGroupList = UserGroup;
        VisitUserList.Items = UserGroupList;
    }

    /// <summary>
    ///     Метод отмены всех фильтров
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void BtnCancelFilter_OnClick(object? sender, RoutedEventArgs e)
    {
        CmbTypeVisit.SelectedItem = -1;
        CmbSubdivision.SelectedItem = -1;
        CmbRequestStatus.SelectedItem = -1;

        VisitUserList.Items = UserGroupList;
    }

}