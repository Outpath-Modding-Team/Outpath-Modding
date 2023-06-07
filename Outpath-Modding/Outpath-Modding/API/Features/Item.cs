using HarmonyLib;
using Outpath_Modding.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ItemInfo;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.API.Features
{
    public class Item
    {
        public static List<ItemInfo> CustomItemInfos = new List<ItemInfo>();
        public static List<Item> Items = new List<Item>();

        public GameObject GameObject;

        public Item(ItemPrefab itemPrefab)
        {
            GameObject = itemPrefab.gameObject;
        }

        public static ItemInfo AddNewMaterial(string itemName, Sprite icon)
        {
            ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Wood").Clone();

            newItemInfo.itemName = itemName;
            newItemInfo.itemID = GetNewItemId();
            newItemInfo.itemIcon = icon;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList = itemInfos.ToArray();
            return newItemInfo;
        }

        public static ItemInfo AddNewMaterial(string itemName, string itemDesc, Sprite icon)
        {
            ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Wood").Clone();

            newItemInfo.itemName = itemName;
            newItemInfo.itemDesc = itemDesc;
            newItemInfo.itemID = GetNewItemId();
            newItemInfo.itemIcon = icon;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList = itemInfos.ToArray();
            return newItemInfo;
        }

        public static ItemInfo AddNewMaterial(ItemMaterialSettings itemMaterialSettings)
        {
            ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Wood").Clone();

            newItemInfo.itemName = itemMaterialSettings.itemName;
            newItemInfo.itemDesc = itemMaterialSettings.itemDescription;
            newItemInfo.itemID = GetNewItemId();
            newItemInfo.itemIcon = itemMaterialSettings.itemIcon;
            newItemInfo.itemMaterialList = itemMaterialSettings.itemToCraft;
            newItemInfo.secondsToCraft = itemMaterialSettings.secondsToCraft;
            newItemInfo.quantityWhenCrafted = itemMaterialSettings.quantityWhenCrafted;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[(int)ItemType.Material].itemsList = itemInfos.ToArray();
            if (itemMaterialSettings.itemCraftingType != ItemCraftingType.None)
                if (!AddToCraftTable(newItemInfo, itemMaterialSettings.itemCraftingType))
                    Logger.Error("Failed to add crafting for item named \"" + itemMaterialSettings.itemName + "\"");
            return newItemInfo;
        }

        public static GameObject SpawnItem(ItemInfo itemInfo)
        {
            return ItemList.instance.SpawnItemPrefab(itemInfo, Vector3.zero);
        }

        public static GameObject SpawnItem(ItemInfo itemInfo, Vector3 position)
        {
            return ItemList.instance.SpawnItemPrefab(itemInfo, position);
        }
        public static GameObject SpawnItem(ItemInfo itemInfo, int quantity)
        {
            return ItemList.instance.SpawnItemPrefab(itemInfo, Vector3.zero, quantity);
        }

        public static GameObject SpawnItem(ItemInfo itemInfo, Vector3 position, int quantity)
        {
            return ItemList.instance.SpawnItemPrefab(itemInfo, position, quantity);
        }

        private static bool AddToCraftTable(ItemInfo itemInfo, ItemCraftingType itemCraftingType)
        {
            try
            {
                GardenItemInfo gardenItemInfo = null;
                foreach (var item in InventoryManager.instance.buildCategories)
                    if (item.buildInCategoryList.ToList().Count(x => x.itemName.Replace(" ", "") == itemCraftingType.ToString()) > 0)
                        gardenItemInfo = item.buildInCategoryList.ToList().First(x => x.itemName.Replace(" ", "") == itemCraftingType.ToString());

                if (!gardenItemInfo) return false;

                List<ItemInfo> itemInfos = gardenItemInfo.prefabToPlace.GetComponent<Build_Craft>().craftRecipesList.ToList();
                itemInfos.Add(itemInfo);
                gardenItemInfo.prefabToPlace.GetComponent<Build_Craft>().craftRecipesList = itemInfos.ToArray();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while adding crafting to an item named \"" + itemInfo.itemName + "\":" + ex.ToString());
                return false;
            }
        }

        private static int GetNewItemId()
        {
            int lastItemId = 0;
            foreach (var item in ItemList.instance.itemList)
            {
                if (item.itemID > lastItemId)
                    lastItemId = item.itemID;
            }
            return lastItemId + 1;
        }

        [System.Serializable]
        public class ItemMaterialSettings
        {
            public ItemMaterialSettings(string itemName, Sprite itemIcon)
            {
                this.itemName = itemName;
                this.itemIcon = itemIcon;
            }

            public ItemMaterialSettings(string itemName, string itemDescription, Sprite itemIcon)
            {
                this.itemName = itemName;
                this.itemDescription = itemDescription;
                this.itemIcon = itemIcon;
            }

            public ItemMaterialSettings(string itemName, Sprite itemIcon, ItemMaterial[] itemToCraft, int secondsToCraft, int quantityWhenCrafted, ItemCraftingType itemCraftingType)
            {
                this.itemName = itemName;
                this.itemIcon = itemIcon;
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }

            public ItemMaterialSettings(string itemName, string itemDescription, Sprite itemIcon, ItemMaterial[] itemToCraft, int secondsToCraft, int quantityWhenCrafted, ItemCraftingType itemCraftingType)
            {
                this.itemName = itemName;
                this.itemDescription = itemDescription;
                this.itemIcon = itemIcon;
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }

            public string itemName;
            public string itemDescription = string.Empty;
            public Sprite itemIcon;
            public ItemMaterial[] itemToCraft = new ItemMaterial[0];
            public int secondsToCraft = 0;
            public int quantityWhenCrafted = 1;
            public ItemCraftingType itemCraftingType = ItemCraftingType.None;
        }

        public enum ItemCraftingType
        {
            None = 0,
            Workstation,
            Furnace,
            ResearchTable,
            Anvil,
            Cauldron,
            CookingPot,
            Imbuing,
            InscriptionTable,
            Mill,
            Recycler,
            Sorcery,
            SpinningWheel
        }
    }
}
