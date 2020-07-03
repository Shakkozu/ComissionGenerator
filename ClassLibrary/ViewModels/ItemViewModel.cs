using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ItemViewModel
    {
        public ObservableCollection<ItemModel> ItemModels { get; set; } = new ObservableCollection<ItemModel>();
    }
}
