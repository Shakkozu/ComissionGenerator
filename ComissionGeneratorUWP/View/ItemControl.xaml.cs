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

// Szablon elementu Kontrolka użytkownika jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234236

namespace ComissionGeneratorUWP
{
    public sealed partial class ItemControl : UserControl
    {
        #region Properties 

        public int Id { get { return _id; } private set { _id = value; } }

        private int _id;

        #endregion

        //*********************

        #region Constructors 

        public ItemControl(int id)
        {
            this.Id = id;
            this.InitializeComponent();
        }

        #endregion

       



        
       
    }
}
