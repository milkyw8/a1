using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApp.AppWindow.Employee;
using AvaloniaApp.ContentObject;
using AvaloniaApp.Helper.Connect;
using AvaloniaApp.Helper.Navigation;
using AvaloniaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.UserControl.Employee;

public partial class EmployeeMain : Avalonia.Controls.UserControl
{
    private List<UserGroup> _UserGroupList;
    public EmployeeMain()
    {
        InitializeComponent();
        LoadData();
        CreateFolder();
        
    }
    
    private void BtnSignOut_OnClick(object? sender, RoutedEventArgs e)
    {
        FrameNavigation.navigate.Content = new AuthControl();
    }


    private void LoadData()
    {
        var Data = ConnectDb.connect.UserGroups
            .Include(x => x.Employee)
            .Include(x => x.Person)
            .Include(x => x.User)
            .OrderBy(x => x.UserId)
            .ToList();
        _UserGroupList = Data;
        VisitUserList.Items = _UserGroupList;
    }

    private void BtnInfoUser_OnClick(object? sender, RoutedEventArgs e)
    {
        HeapUserGroup.UserGroupHeap = (sender as Button).DataContext as UserGroup;
        new AddBlackList().Show();
    }

    private void CreateFolder()
    {
        var DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var NewFolderName = "Отчеты ТБ";
        var NewFolderPath = Path.Combine(DocumentsPath, NewFolderName);
        Directory.CreateDirectory(NewFolderPath);
    }

    private void BtnCreateReport_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}