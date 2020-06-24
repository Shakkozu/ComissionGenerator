using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ComissionGeneratorUWP.ViewModel;
using ComissionGeneratorUWP.View;
using Windows.UI.Xaml.Documents;
using System.ServiceModel.Channels;
using Windows.UI.Input.Spatial;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class CompanyPage : Page, IValidator
    {

        #region Properties

      
        /// <summary>
        /// Checks if data entered on this page is correct
        /// </summary>
        public bool DataValidated { get { return viewModel.CheckValidation(); } }

        #endregion

        //****************************

        #region Constructors

        public CompanyPage()
        {
            this.InitializeComponent();

        }

        #endregion

        //****************************

        #region Methods
          
        /// <summary>
        /// During frame change saves actual data
        /// </summary>
        /// <param name="e"></param>
        async protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await viewModel.SaveJson();
            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// During frame load tries to load data stored in file
        /// </summary>
        /// <param name="e"></param>
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            await viewModel.LoadJson();
            base.OnNavigatedTo(e);
        }

        



        #endregion





    }
}
