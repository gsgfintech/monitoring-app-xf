﻿using MonitoringApp.XF.Components.MainMenu;
using MonitoringApp.XF.Components.SystemsStatus;
using System;

using Xamarin.Forms;

namespace MonitoringApp.XF
{
    public class MainPage : MasterDetailPage
    {
        private MainMenuPage mainMenuPage;

        public MainPage()
        {
            mainMenuPage = new MainMenuPage();
            Master = mainMenuPage;
            Detail = new NavigationPage(new SystemsStatusesListPage());

            mainMenuPage.ListView.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuPageItem;

            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                mainMenuPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
