﻿using ClassLibrary;
using System.Windows;
using System.Windows.Controls;

namespace CommissionGeneratorWPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientViewModel viewModel = new ClientViewModel();

        public ClientPage()
        {
            InitializeComponent();
            InitializeBinding();
            companyStackPanel.Visibility = Visibility.Collapsed;
        }

        public void InitializeBinding()
        {
            mainGrid.DataContext = viewModel;
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        ///I had to use those functions, because using Visibility Converters doesn't work with stackPanels
        #region StackPanelVisibility

        /// <summary>
        /// Turns companyStackPanel visible
        /// </summary>
        private void companyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (companyRadioButton.IsChecked == true)
            {
                if (companyStackPanel != null && personStackPanel != null)
                {
                    personStackPanel.Visibility = Visibility.Collapsed;
                    companyStackPanel.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Turns personStackPanel visible
        /// </summary>
        private void personRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (personRadioButton.IsChecked == true)
            {
                if (companyStackPanel != null && personStackPanel != null)
                {
                    companyStackPanel.Visibility = Visibility.Collapsed;
                    personStackPanel.Visibility = Visibility.Visible;
                }
            }
        }
        #endregion
    }
}
