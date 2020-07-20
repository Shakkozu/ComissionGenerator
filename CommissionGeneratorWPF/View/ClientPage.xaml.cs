using ClassLibrary;
using ClassLibrary.Data;
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
            viewModel.ClientLoaded += ViewModel_ClientLoaded;

        }

        private void ViewModel_ClientLoaded(object sender, System.EventArgs e)
        {
            UpdateStackPanelsVisibility();
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
            UpdateStackPanelsVisibility();
        }

        private void UpdateStackPanelsVisibility()
        {
            //viewModel.Client.Company = (bool)companyRadioButton.IsChecked;
            if (viewModel.Client.Company)
            {
                companyRadioButton.IsChecked = true;
                if (companyStackPanel != null && personStackPanel != null)
                {
                    personStackPanel.Visibility = Visibility.Collapsed;
                    companyStackPanel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                personRadioButton.IsChecked = true;
                if (companyStackPanel != null && personStackPanel != null)
                {
                    personStackPanel.Visibility = Visibility.Visible;
                    companyStackPanel.Visibility = Visibility.Collapsed;
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

        private void removeClientFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if(clientsList.SelectedItem is ClientModel selectedClient)
            {
                viewModel.RemoveClient(selectedClient);
            }
        }

        private void loadClientFromDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientsList.SelectedItem is ClientModel selectedClient)
            {
                viewModel.LoadClient(selectedClient);
            }
        }

        private void addClientToDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddClient();
        }

        private void clientsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (clientsList.SelectedItem is ClientModel selectedClient)
            {
                viewModel.LoadClient(selectedClient);
            }
        }
    }
}
