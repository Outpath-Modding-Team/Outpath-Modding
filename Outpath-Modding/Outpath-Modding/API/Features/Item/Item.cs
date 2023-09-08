using System.Collections.Generic;
using UnityEngine;

namespace Outpath_Modding.API.Features.Item
{
    public class Item
    {
        public static List<ItemInfo> CustomItemInfos { get; private set; } = new List<ItemInfo>();
        public static List<Item> Items { get; private set; } = new List<Item>();

        public GameObject GameObject;

        public Item(ItemPrefab itemPrefab)
        {
            GameObject = itemPrefab.gameObject;
        }
    }
}
