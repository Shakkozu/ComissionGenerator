using ClassLibrary;
using System.Collections.ObjectModel;

namespace ComissionGeneratorUWP.ViewModel
{
    public class ItemViewModel
    {
        public ObservableCollection<ItemModel> ItemModels { get; set; } = new ObservableCollection<ItemModel>();
    }
}
