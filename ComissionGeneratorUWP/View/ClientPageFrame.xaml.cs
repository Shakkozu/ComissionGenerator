using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ComissionGeneratorUWP
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class ClientPage : Page
    {
        public ClientPage()
        {
            this.InitializeComponent();
        }

        async protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await viewModel.SaveJson();
            base.OnNavigatedFrom(e);
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            await viewModel.LoadJson();
            base.OnNavigatedTo(e);
        }




    }
}
