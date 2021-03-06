﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.ServiceReference1;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP
{
    public sealed partial class MainPage : Page
    {
        EmployeeClient webService = new EmployeeClient();
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += ManPage_Loaded;
        }
        void ManPage_Loaded(object sender, RoutedEventArgs e)
        {
            getEmployee();
        }
        async void getEmployee()
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                GridViewEmployee.ItemsSource = await webService.GetProductListAsync();
                ProgressBar.IsIndeterminate = false;
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Employee newEmployee = new Employee();
                newEmployee.firstName = TextBoxName.Text;
                newEmployee.lastName = TextBoxSurname.Text;
                newEmployee.Age = Int32.Parse(TextBoxAge.Text);
                newEmployee.address = TextBoxCity.Text;
                bool result = await webService.AddEmployeeAsync(newEmployee);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if(result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Inserted Successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't Insert");
                    await messageDialog.ShowAsync();
                }
                getEmployee();
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }
        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if(GridViewEmployee.SelectedItem != null)
            {
                try
                {
                    ProgressBar.IsIndeterminate = true;
                    ProgressBar.Visibility = Visibility.Visible;
                    bool result = await webService.DeleteEmployeeAsync((GridViewEmployee.SelectedItem as Employee).empID);
                    if(result == true)
                    {
                        MessageDialog messageDialog = new MessageDialog("Delete successfully!");
                        await messageDialog.ShowAsync();
                        Reset();
                    }
                    else
                    {
                        MessageDialog messageDialog = new MessageDialog("Can't delete!");
                        await messageDialog.ShowAsync();
                    }
                    getEmployee();
                }
                catch(Exception ex)
                {
                    MessageDialog messageDialog = new MessageDialog(ex.Message);
                    await messageDialog.ShowAsync();
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                }
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Choise record to delete!");
                await messageDialog.ShowAsync();
            }
        }

        void Reset()
        {
            TextBoxName.Text = string.Empty;
            TextBoxSurname.Text = string.Empty;
            TextBoxCity.Text = string.Empty;
            TextBoxAge.Text = string.Empty;
        }
        private async void ButtonModifi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Employee newEmployee = new Employee();
                newEmployee.empID = (GridViewEmployee.SelectedItem as Employee).empID;
                newEmployee.firstName = TextBoxName.Text;
                newEmployee.lastName = TextBoxSurname.Text;
                newEmployee.Age = Int32.Parse(TextBoxAge.Text);
                newEmployee.address = TextBoxCity.Text;

                bool result = await webService.UpdateEmployeeAsync(newEmployee);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if(result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Edited successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't delete!");
                    await messageDialog.ShowAsync();
                }
                getEmployee();
            }
            catch
            {
                MessageDialog messageDialog = new MessageDialog("Choise Employee!");
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }

    }
}
