using System;

namespace Outpath_Modding.Events.EventArguments
{
    public class SetItemToCraftEventArgs : EventArgs
    {
        public SetItemToCraftEventArgs(ItemInfo item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public ItemInfo Item { get; }
        public int Quantity { get; set; }
        public bool IsAllowed { get; set; } = true;
    }
}
