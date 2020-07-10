using ClassLibrary.Events;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using CommissionGeneratorWPF.View;
using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly CompanyPage _companyPage = new CompanyPage();
        private readonly ClientPage _clientPage = new ClientPage();
        private readonly CommissionPage _commissionPage = new CommissionPage();

        private readonly PersonalData PersonalData;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            InitializeMainWindow();
            PersonalData = new PersonalData(_companyPage.viewModel, _clientPage.viewModel);
            _commissionPage.CommissionGenerated += _commissionPage_CommissionGenerated;

        }

        private void _commissionPage_CommissionGenerated(object sender, CommissionEventArgs e)
        {
            try
            {
                if (File.Exists(@"C:\Users\user\Desktop\testHydroDoc.docx"))
                {
                    DocumentHelper.GenerateDocumentFromTemplate(@"C:\Users\user\Desktop\testHydroDoc.docx",
                        @"C:\Users\user\Desktop\result.docx", PersonalData, e.Items, _commissionPage.viewModel.ReplaceOnlyValues);
                }
                else
                {
                    DocumentHelper.GenerateNewDocument(@"C:\Users\user\Desktop\firstTest.docx", PersonalData, e.Items);
                }
            }
            catch(FileLoadException)
            {
                MessageBox.Show("Please close your generated Commission before you will create new one!");
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
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
            //FOR TEST PURPOSES ; TODO DELETE
            _clientPage.viewModel.LoadJson();
            _commissionPage.viewModel.LoadJson();
        }
        private void SaveCompanyPage()
        {
            _companyPage.viewModel.SaveJson();
            _clientPage.viewModel.SaveJson();
            _commissionPage.viewModel.SaveJson();
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
