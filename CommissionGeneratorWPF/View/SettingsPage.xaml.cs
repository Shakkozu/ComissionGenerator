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
using ClassLibrary;

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
            fileDialog.Filter = "Dokumenty (*.docx) | *.docx";
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
                    MessageBox.Show("Nie wybrano pliku szablonu",  "Template Loading Error");
                    break;
                default:
                    viewModel.TemplateFilePath = "";
                    templateFilepathBox.ToolTip = null;
                    break;
            }
        }
    }
}
