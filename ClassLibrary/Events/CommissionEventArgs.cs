using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Events
{
    public class CommissionEventArgs : EventArgs
    {
        public CommissionEventArgs()
        {

        }

        public CommissionEventArgs(List<ItemModel> items)
        {
            Items = items;
        }

        public List<ItemModel> Items { get; }

    }
}
