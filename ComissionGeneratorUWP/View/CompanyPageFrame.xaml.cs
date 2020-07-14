using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class CompanyPage : Page, IValidator
    {

        #region Properties


        /// <summary>
        /// Checks if data entered on this page is correct
        /// </summary>
        public bool DataValidated { get { return viewModel.CheckValidation(); } }

        #endregion

        //****************************

        #region Constructors

        public CompanyPage()
        {
            this.InitializeComponent();

        }

        #endregion

        //****************************

        #region Methods

        /// <summary>
        /// During frame change saves actual data
        /// </summary>
        /// <param name="e"></param>
        async protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await viewModel.SaveJson();
            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// During frame load tries to load data stored in file
        /// </summary>
        /// <param name="e"></param>
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            await viewModel.LoadJson();
            base.OnNavigatedTo(e);
        }





        #endregion





    }
}
