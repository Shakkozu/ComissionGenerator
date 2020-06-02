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
            
        }
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>()
        {
            ("company",typeof(CompanyPage)),
            ("client",typeof(ClientPage)),
            ("commission",typeof(CommissionPage))
        };



        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItemTag = args.SelectedItemContainer.Tag.ToString();
            NavigationViewControl_Navigate(selectedItemTag,
                                           args.RecommendedNavigationTransitionInfo);

        }

        
        private void NavigationViewControl_Navigate(string selectedItemTag, NavigationTransitionInfo recommendedNavigationTransitionInfo)
        {
            Type _page = null;
            var item = _pages.FirstOrDefault(p => p.Tag.Equals(selectedItemTag));
            _page = item.Page;

            var preNavPageType = Frame.CurrentSourcePageType;

            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                contentFrame.Navigate(_page);
            }
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(_pages[0].Page);
        }
    }
}
