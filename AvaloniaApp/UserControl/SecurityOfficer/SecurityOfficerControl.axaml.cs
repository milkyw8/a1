using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Interactivity;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.UserControl.SecurityOfficer;

public partial class SecurityOfficerControl : Avalonia.Controls.UserControl
{
    private List<UserGroup> _UsersList;
    public SecurityOfficerControl()
    {
        InitializeComponent();
        LblFioUser.Content = $"{HeapUserGroup.UserGroupHeap.Person?.Lastname}." +
                             $"{HeapUserGroup.UserGroupHeap.Person?.Firstname?[0]}." +
                             $"{HeapUserGroup.UserGroupHeap.Person?.Patronymic?[1]}.";
        LoadData();

        DgUserVerification.Items = _UsersList;
        DgPositionRule.Items = _UsersList;
        
    }

    private void BtnExit_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }

    private void LoadData()
    {
        _UsersList = ConnectDb.connect.UserGroups
            .Include(x => x.User)
            .Include(x => x.Person)
            .Include(x => x.Employee)
            .ToList();
    }

    private void BtnSaveMandate_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var Data = ConnectDb.connect.UserGroups.FirstOrDefault(x => x.Id == HeapUserGroup.UserGroupHeap.Id);

            var _PositionRule = new PositionRule();
        }
        catch (Exception exception)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }

    private void BtnSaveVerify_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
        }
        catch (Exception exception)
        {
            MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", "Ошибка в обработке данных", ButtonEnum.Ok, Icon.Error).Show();
            Console.WriteLine(exception);
        }
    }
}