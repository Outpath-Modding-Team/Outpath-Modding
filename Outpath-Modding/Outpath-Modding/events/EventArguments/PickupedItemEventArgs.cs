using System;

namespace Outpath_Modding.Events.EventArguments
{
    public class PickupedItemEventArgs : EventArgs
    {
        public PickupedItemEventArgs(ItemInfo itemInfo, int quantity)
        {
            ItemInfo = itemInfo;
            Quantity = quantity;
        }

        public ItemInfo ItemInfo { get; }
        public int Quantity { get; }
    }
}