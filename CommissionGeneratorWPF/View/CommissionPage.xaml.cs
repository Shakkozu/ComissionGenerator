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
using ClassLibrary.Events;

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
            CommissionGenerated.Invoke(this, new CommissionEventArgs(viewModel.ItemList.ToList()));

            //string result = "";
            //foreach(var item in viewModel.ItemList)
            //{
            //    if(item.ItemName != "")
            //        result += $"{item.Id}: {item.ItemName} costs {item.ItemPrice}" + Environment.NewLine;
            //}
            //MessageBox.Show(result);
        }
    }
}
