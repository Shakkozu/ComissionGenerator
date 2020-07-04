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
        #region Private Members

        private CompanyPage _companyPage = new CompanyPage();
        private ClientPage _clientPage = new ClientPage();
        private CommissionPage _commissionPage = new CommissionPage();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            InitializeMainWindow();
        }


        private void InitializeMainWindow()
        {

            LoadCompanyPage();
            SetPage("companyPageLabel");

            FontSize = 14;
            FontFamily = new FontFamily("Calibri");
        }

        #endregion

        #region Methods

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

        
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock block)
                block.Background = Brushes.Wheat;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock block)
                block.Background = Brushes.White;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                SetPage(textBlock.Name);
            }
        }

        /// <summary>
        /// Sets frame content using appropriate Page labels
        /// </summary>
        /// <param name="pageLabel"></param>
        private void SetPage(string pageLabel)
        {
            switch (pageLabel)
            {
                case "companyPageLabel":
                    frame.Content = _companyPage;
                    break;
                case "clientPageLabel":
                    frame.Content = _clientPage;
                    break;
                case "commissionPageLabel":
                    frame.Content = _commissionPage;
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
