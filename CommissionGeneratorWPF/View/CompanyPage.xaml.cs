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
    /// Logika interakcji dla klasy CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : Page
    {
        public CompanyViewModel viewModel;
        
        public CompanyPage()
        {
            InitializeComponent();
            InitializeBinding();
        }

        public void InitializeBinding()
        {
            viewModel = new CompanyViewModel();
            mainGrid.DataContext = viewModel;
        }
        
        
    }
}
