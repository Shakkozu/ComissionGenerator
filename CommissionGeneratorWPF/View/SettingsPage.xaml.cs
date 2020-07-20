using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CommissionGeneratorWPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        public SettingsViewModel viewModel = new SettingsViewModel();

        public SettingsPage()
        {

            InitializeComponent();
            InitializeBinding();
        }

        public void InitializeBinding()
        {
            mainGrid.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Documents (*.docx) | *.docx";
            fileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    viewModel.TemplateFilePath = file;
                    templateFilepathBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    MessageBox.Show("Template File Wasn't Selected", "Template Loading Error");
                    break;
                default:
                    viewModel.TemplateFilePath = "";
                    templateFilepathBox.ToolTip = null;
                    break;
            }
        }

        private void removeFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveFilePath();
        }
    }
}
