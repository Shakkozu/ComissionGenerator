using Windows.UI.Xaml.Controls;

// Szablon elementu Kontrolka użytkownika jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234236

namespace ComissionGeneratorUWP
{
    public sealed partial class ItemControl : UserControl
    {
        #region Properties 

        public int Id { get { return _id; } private set { _id = value; } }

        private int _id;

        #endregion

        //*********************

        #region Constructors 

        public ItemControl(int id)
        {
            this.Id = id;
            this.InitializeComponent();
        }

        #endregion







    }
}
