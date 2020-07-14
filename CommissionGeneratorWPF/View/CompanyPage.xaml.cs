using ClassLibrary;
using System.Windows.Controls;

namespace CommissionGeneratorWPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy CompanyPage.xaml
    /// </summary>
    public partial class CompanyPage : Page
    {
        public CompanyViewModel viewModel = new CompanyViewModel();
        public CompanyPage()
        {
            InitializeComponent();
            InitializeBinding();
        }

        public void InitializeBinding()
        {
            mainGrid.DataContext = viewModel;
        }
    }
}
