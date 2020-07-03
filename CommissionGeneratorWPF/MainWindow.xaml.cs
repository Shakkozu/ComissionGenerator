using CommissionGeneratorWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommissionGeneratorWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CompanyPage _companyPage = new CompanyPage();
        private ClientPage _clientPage = new ClientPage();
        public MainWindow()
        {
            InitializeComponent();
            companyPageLabel_MouseLeftButtonDown(this, null);
            LoadCompanyPage();
            
            FontSize = 14;
            FontFamily = new FontFamily("Calibri");
        }

        protected override void OnDeactivated(EventArgs e)
        {
            SaveCompanyPage();
        }

        private void LoadCompanyPage()
        {
            _companyPage.viewModel.LoadJson();
        }
        private void SaveCompanyPage()
        {
            _companyPage.viewModel.SaveJson();
        }

        

        private void companyPageLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            companyPageLabel.Background = Brushes.Wheat;
        }

        private void companyPageLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            companyPageLabel.Background = Brushes.White;
        }

        private void companyPageLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _companyPage;
        }

        private void clientPageLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _clientPage;
        }

        private void clientPageLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            clientPageLabel.Background = Brushes.Wheat;
        }

        private void clientPageLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            clientPageLabel.Background = Brushes.White;
        }
    }
}
