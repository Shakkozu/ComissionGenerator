﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class ClientPage : Page
    {
        public ClientPage()
        {
            this.InitializeComponent();
        }

        async protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await viewModel.SaveXml();
            base.OnNavigatedFrom(e);
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            await viewModel.LoadXml();
            base.OnNavigatedTo(e);
        }




    }
}
