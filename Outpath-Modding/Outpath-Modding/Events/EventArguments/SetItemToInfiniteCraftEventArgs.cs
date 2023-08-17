using System;

namespace Outpath_Modding.Events.EventArguments
{
    public class SetItemToInfiniteCraftEventArgs : EventArgs
    {
        public SetItemToInfiniteCraftEventArgs(ItemInfo item)
        {
            Item = item;
        }

        public ItemInfo Item { get; }
        public bool IsAllowed { get; set; } = true;
    }
}
