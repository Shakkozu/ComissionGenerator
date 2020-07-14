using System.Collections.ObjectModel;

namespace ClassLibrary
{
    public class ItemViewModel
    {
        public ObservableCollection<ItemModel> ItemModels { get; set; } = new ObservableCollection<ItemModel>();
    }
}
