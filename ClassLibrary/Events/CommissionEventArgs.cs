using System;
using System.Collections.Generic;

namespace ClassLibrary.Events
{
    public class CommissionEventArgs : EventArgs
    {
        public CommissionEventArgs()
        {

        }

        public CommissionEventArgs(List<ItemModel> items, string resultPath)
        {
            Items = items;
            ResultPath = resultPath;
        }

        public List<ItemModel> Items { get; }
        public string ResultPath { get; }

    }
}
