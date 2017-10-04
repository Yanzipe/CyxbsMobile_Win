﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Volunteer_LoginPage : Page
    {
        public Volunteer_LoginPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
              {
                  viewModel.ElementWidth1 =(int) e.NewSize.Width;
              };
        }
        ViewModels.VolunteerPageViewModel viewModel = new ViewModels.VolunteerPageViewModel();
        private static string resourceName = "ZSCY_Volunteer";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = viewModel;
            
        }
      
        private async void GetAsync(string user_name,string user_password)
        {

            httpclientPost httpclient = new httpclientPost();
                string json = await httpclient.PostHttpClient(user_name, user_password);
            viewModel.Rootobject = JsonConvert.DeserializeObject<Models.VolunteerModel.Rootobject>(json);
        }
        private async void login_Button_Click(object sender, RoutedEventArgs e)
        {
            user_name.IsEnabled = false;
            user_password.IsEnabled = false;
            loading_progress.IsActive = true;
            login_Button.Visibility = Visibility.Collapsed;
            viewModel.Rootobject = new Models.VolunteerModel.Rootobject();
            var vault = new Windows.Security.Credentials.PasswordVault();

            await Task.Delay(2000);

            GetAsync(user_name.Text, user_password.Password);
            if (viewModel.Rootobject.status==200)
            {
                vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, user_name.Text, user_password.Password));
                this.Frame.Navigate((typeof(VolunteerPage)));
            }
            if(viewModel.Rootobject.status == 001)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "账号或者密码错误";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.status == 002)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "帐号不存在";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.status == 003)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "请输入密码";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.status == 004)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "请输入帐号";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            user_name.IsEnabled = true;
            user_password.IsEnabled = true;
            loading_progress.IsActive = false;
            login_Button.Visibility = Visibility.Visible;

        }


    }
    }
