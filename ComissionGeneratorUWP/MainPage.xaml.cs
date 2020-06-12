using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Documents;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Appointments.DataProvider;
using System.ComponentModel;
using Windows.UI.ViewManagement;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            
            this.InitializeComponent();
            var view = ApplicationView.GetForCurrentView();
            view.SetPreferredMinSize(new Size(500, 600));
        }

        public string WindowSize22  { get; set; } = "blablabla";

        private readonly List<_NavigationItem> _navigationPages = new List<_NavigationItem>()
        {
            new _NavigationItem(0,typeof(CompanyPage),"company"),
            new _NavigationItem(1,typeof(ClientPage),"client"),
            new _NavigationItem(2,typeof(CommissionPage),"commission"),
        };

       



        /// <summary>
        /// If Other frame is selected, navigate to it
        /// </summary>
        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                NavigationViewControl_Navigate(args.SelectedItemContainer.Tag.ToString(),
                    args.RecommendedNavigationTransitionInfo);
            }
        }

        /// <summary>
        /// If selectedItemTag is valid _NavigationItem Tag, and if it points to diffrent frame than current,
        /// navigate to it
        /// </summary>
        private void NavigationViewControl_Navigate(string selectedItemTag, NavigationTransitionInfo recommendedNavigationTransitionInfo)
        {
            
            var item = _navigationPages.FirstOrDefault(p => p.Tag.Equals(selectedItemTag));

            if (item != null)
            {
                Type _page = item.PageType;

                //select current frame Type
                var preNavPageType = Frame.CurrentSourcePageType;

                if (!(_page is null) && !Type.Equals(preNavPageType, _page))
                {
                    SetButtonsEnabledState(item);
                    contentFrame.Navigate(_page);
                }
            }

           
        }

        /// <summary>
        /// If it's not first frame, moves backwards
        /// </summary>
        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Type actualFrameType = contentFrame.SourcePageType;
            var item = _navigationPages.FirstOrDefault(p => p.PageType == actualFrameType);
            if(item!= null)
            {
                int index = item.Id - 1;
                if(index >= 0)
                {
                    NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[index];
                    contentFrame.Navigate(_navigationPages[index].PageType);
                }    
                    
            }
        }
        /// <summary>
        /// If it's not last frame, moves forwards
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Type actualFrameType = contentFrame.SourcePageType;
            var item = _navigationPages.FirstOrDefault(p => p.PageType == actualFrameType);
            if (item != null)
            {
                int index = item.Id + 1;
                if (index < 3)
                {
                    NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[index];
                    contentFrame.Navigate(_navigationPages[index].PageType);
                }

            }
        }

        /// <summary>
        /// Load first page on start
        /// </summary>
        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            //set selection bar to first page
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];

            contentFrame.Navigate(_navigationPages[0].PageType);
        }
        /// <summary>
        /// Sets buttons enabled state for NavigationItem passed in parameter
        /// </summary>
        void SetButtonsEnabledState(_NavigationItem item)
        {
            //If it's first page disable backButton
            if (item.Tag == _navigationPages[0].Tag)
            {
                NavigationViewControl.IsBackEnabled = false;
                forwardButton.IsEnabled = true;
                forwardButton.Visibility = Visibility.Visible;
            }

            //If it's last page disable forwardButton
            else if (item.Tag == _navigationPages[2].Tag)
            {
                NavigationViewControl.IsBackEnabled = true;
                forwardButton.IsEnabled = false;
                forwardButton.Visibility = Visibility.Collapsed;
            }
            //enable buttons otherwise
            else
            {
                forwardButton.IsEnabled = true;
                NavigationViewControl.IsBackEnabled = true;
                forwardButton.Visibility = Visibility.Visible;
            
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            
            
            
        }
    }
}
