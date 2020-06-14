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

//Szablon elementu Okno dialogowe zawartości jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP.View
{
    public sealed partial class IncorrectDataDialog : ContentDialog
    {

        #region Properties

        public IncorrectDataDialogResult Result { get; set; } = IncorrectDataDialogResult.DontAcceptCurrentData;

        #endregion

        //***********************

        #region Constructors

        public IncorrectDataDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        //***********************

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = IncorrectDataDialogResult.AcceptCurrentData;
            Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = IncorrectDataDialogResult.DontAcceptCurrentData;
            Hide();
        }

        #endregion






    }

    #region Enumerator Types

    public enum IncorrectDataDialogResult
    {
        AcceptCurrentData,
        DontAcceptCurrentData
    }

    #endregion

}


