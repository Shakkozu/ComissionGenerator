using ClassLibrary.Events;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using CommissionGeneratorWPF.View;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private readonly SettingsPage _settingsPage = new SettingsPage();

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
                if (File.Exists(_settingsPage.viewModel.TemplateFilePath))
                {
                    DocumentHelper.GenerateDocumentFromTemplate(_settingsPage.viewModel.TemplateFilePath,
                        e.ResultPath, PersonalData, e.Items, _commissionPage.viewModel.ReplaceOnlyValues);
                }
                else
                {
                    DocumentHelper.GenerateNewDocument(e.ResultPath, PersonalData, e.Items);
                }
            }
            catch (FileLoadException)
            {
                MessageBox.Show("Please close your generated Commission before you will create new one!", "Commission Creating Error");
            }

            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message, "Invalid Template file", MessageBoxButton.OK);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Document Creation Error", MessageBoxButton.OK);
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
            _settingsPage.viewModel.LoadJson();
        }
        private void SaveCompanyPage()
        {
            _companyPage.viewModel.SaveJson();
            _clientPage.viewModel.SaveJson();
            _commissionPage.viewModel.SaveJson();
            _settingsPage.viewModel.SaveJson();
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
                case "settingsPageLabel":
                    frame.Content = _settingsPage;
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
