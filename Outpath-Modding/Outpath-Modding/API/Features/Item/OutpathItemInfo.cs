using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outpath_Modding.API.Features.Item
{
    public class OutpathItemInfo
    {
        public static List<OutpathItemInfo> ItemInfos { get; } = new List<OutpathItemInfo>();

        public OutpathItemInfo(ItemInfo itemInfo)
        {
            Base = itemInfo;

            if (ItemInfos.Count(x => x.Base == itemInfo) > 0)
                return;

            ItemInfos.Add(this);
        }

        public ItemInfo Base { get; private set; }

        public string Name
        {
            get { return Base.itemName; }
            set { Base.itemName = value; }
        }

        public string Description
        {
            get { return Base.itemDesc; }
            set { Base.itemDesc = value; }
        }

        public Sprite Icon
        {
            get { return Base.itemIcon; }
            set { Base.itemIcon = value; }
        }

        public static GameObject SpawnItem(ItemInfo itemInfo)
        {
            GameObject item = ItemList.instance.SpawnItemPrefab(itemInfo, Vector3.zero);
            new Item(item.GetComponent<ItemPrefab>());
            return item;
        }

        public static GameObject SpawnItem(ItemInfo itemInfo, Vector3 position)
        {
            GameObject item = ItemList.instance.SpawnItemPrefab(itemInfo, position);
            new Item(item.GetComponent<ItemPrefab>());
            return item;
        }

        public static GameObject SpawnItem(ItemInfo itemInfo, int quantity)
        {
            GameObject item = ItemList.instance.SpawnItemPrefab(itemInfo, Vector3.zero, quantity);
            new Item(item.GetComponent<ItemPrefab>());
            return item;
        }

        public static GameObject SpawnItem(ItemInfo itemInfo, Vector3 position, int quantity)
        {
            GameObject item = ItemList.instance.SpawnItemPrefab(itemInfo, position, quantity);
            new Item(item.GetComponent<ItemPrefab>());
            return item;
        }
    }
}
