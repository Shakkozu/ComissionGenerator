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

        //flag is used to control IncorrectDataDialog
        private bool navigationFlag = false;

        //for future use
        private CompanyViewModel _viewModel = new CompanyViewModel();

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
        /// If entered data is incorrect prompts the user if he's sure he wants to change page
        /// </summary>
        async protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //if any of data entered by user is invalid
            if (viewModel.CheckValidation() == false)
            {
                if (navigationFlag == false)
                {
                    e.Cancel = true;
                    //Show dialog asking user if he's sure that he wants to continue despite the fact
                    //that the entered data is incorrect
                    IncorrectDataDialog incorrectDataDialog = new IncorrectDataDialog();
                    await incorrectDataDialog.ShowAsync();
                    IncorrectDataDialogResult result = incorrectDataDialog.Result;

                    switch (result)
                    {
                        //navigate to next page
                        case IncorrectDataDialogResult.AcceptCurrentData:
                            navigationFlag = true;
                            this.Frame.Navigate(e.SourcePageType);
                            break;
                        //return to current page
                        case IncorrectDataDialogResult.DontAcceptCurrentData:
                            break;
                        default:
                            break;
                    }
                }
            }
            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Renews local flag
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationFlag = false;
            base.OnNavigatedTo(e);
        }

        #endregion





    }
}
