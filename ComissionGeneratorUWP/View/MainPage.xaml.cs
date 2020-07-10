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
using ComissionGeneratorUWP.View;
using System.ServiceModel.Channels;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        #region Properties

        /// <summary>
        /// This list contains all NavigationPages, in case new Page is added that list needs to be modified!
        /// </summary>
        private readonly List<_NavigationItem> _navigationPages = new List<_NavigationItem>()
        {
            new _NavigationItem(0,typeof(CompanyPage),"company"),
            new _NavigationItem(1,typeof(ClientPage),"client"),
            new _NavigationItem(2,typeof(CommissionPage),"commission"),
        };

        #endregion

        //****************

        #region Constructors

        public MainPage()
        {

            this.InitializeComponent();
            var view = ApplicationView.GetForCurrentView();
            view.SetPreferredMinSize(new Size(500, 600));
        }

        #endregion

        //****************

        #region Methods

        /// <summary>
        /// If Other frame is selected, navigate to it
        /// </summary>
        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            
            sender.UpdateLayout();
            FocusManager.FindNextElement(FocusNavigationDirection.Next);
            //OnPointerCaptureLost(new PointerRoutedEventArgs("this"));
            this.ReleasePointerCaptures();
            if (args.SelectedItemContainer != null)
            {
                //NavigationViewControl.SelectedItem = args.SelectedItemContainer.Tag.ToString();
                NavigationViewControl_Navigate(args.SelectedItemContainer.Tag.ToString());
            }
        }

        /// <summary>
        /// If selectedItemTag is valid _NavigationItem Tag, and if it points to diffrent frame than current,
        /// navigate to it
        /// </summary>
        private void NavigationViewControl_Navigate(string selectedItemTag)
        {

            var item = _navigationPages.FirstOrDefault(p => p.Tag.Equals(selectedItemTag));

            if (item != null)
            {
                Type _page = item.PageType;

                //select current frame Type
                var preNavPageType = Frame.CurrentSourcePageType;

                //if selected page is different than current
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
            if (item != null)
            {
                int index = item.Id - 1;
                if (index >= 0)
                {
                    NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[index];
                }

            }
        }
        /// <summary>
        /// Moves to the next page
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //search pages by 'PageType'
            Type actualFrameType = contentFrame.SourcePageType;
            //select _NavigationItem with specified PageType
            var item = _navigationPages.FirstOrDefault(p => p.PageType == actualFrameType);

            if (item != null)
            {
                if (item.Id < 2)
                {
                    //Changes Selected item and navigates to it
                    NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[item.Id + 1];
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















        #endregion

       
    }
}
