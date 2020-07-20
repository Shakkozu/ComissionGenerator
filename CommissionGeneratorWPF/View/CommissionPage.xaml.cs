using ClassLibrary;
using ClassLibrary.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CommissionGeneratorWPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy CommissionPage.xaml
    /// </summary>
    public partial class CommissionPage : Page
    {
        public CommissionViewModel viewModel = new CommissionViewModel();
        public event EventHandler<CommissionEventArgs> CommissionGenerated;

        public CommissionPage()
        {
            InitializeComponent();
            InitializeBinding();
        }

        public void InitializeBinding()
        {
            mainGrid.DataContext = viewModel;
            itemsList.ItemsSource = viewModel.ItemList;
        }

        private void CommissionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void generateCommissionButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = ("Documents (*.docx) | *.docx");
            saveFileDialog.DefaultExt = "*.docx";
            saveFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            var result = saveFileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    string filePath = saveFileDialog.FileName;
                    CommissionGenerated?.Invoke(this, new CommissionEventArgs(viewModel.ItemList.ToList(), filePath));
                    break;
                default:
                    MessageBox.Show("Save Filepath Wasn't Selected", "Commission Creating Error");
                    break;
            }

        }
    }
}
