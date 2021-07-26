using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Banking_Calculator.Resources;

namespace Banking_Calculator
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        
        private void OrdLoan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoanPivot1.xaml?parameter=ordinary", UriKind.RelativeOrAbsolute));
        }

        private void AmortLoan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoanPivot1.xaml?parameter=amortized", UriKind.RelativeOrAbsolute));

        }

        private void CompoundInt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/DepositPivot1.xaml?parameter=compoundi", UriKind.RelativeOrAbsolute));

        }

        private void SimpleInt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/DepositPivot1.xaml?parameter=simple", UriKind.RelativeOrAbsolute));

        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}