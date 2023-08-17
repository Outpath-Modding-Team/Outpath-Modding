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
        public static List<ItemInfo> CustomItemInfos { get; private set; } = new List<ItemInfo>();
        public static List<Item> Items { get; private set; } = new List<Item>();

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
            itemInfos = ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList = itemInfos.ToArray();
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
            newItemInfo.xpWhenCrafted = itemMaterialSettings.xpWhenCrafted;
            newItemInfo.quantityWhenCrafted = itemMaterialSettings.quantityWhenCrafted;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList = itemInfos.ToArray();
            if (itemMaterialSettings.itemCraftingType != ItemCraftingType.None)
                if (!AddToCraftTable(newItemInfo, itemMaterialSettings.itemCraftingType))
                    Logger.Error("Failed to add crafting for item named \"" + itemMaterialSettings.itemName + "\"");
            return newItemInfo;
        }

        public static ItemInfo AddNewWeapon(string itemName, Sprite icon)
        {
            ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Sword Flint").Clone();

            newItemInfo.itemName = itemName;
            newItemInfo.itemID = GetNewItemId();
            newItemInfo.itemIcon = icon;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList = itemInfos.ToArray();
            return newItemInfo;
        }

        public static ItemInfo AddNewWeapon(ItemWeaponSettings itemWeaponSettings)
        {
            ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Sword Flint").Clone();

            newItemInfo.itemName = itemWeaponSettings.itemName;
            newItemInfo.itemDesc = itemWeaponSettings.itemDescription;
            newItemInfo.itemID = GetNewItemId();
            newItemInfo.itemIcon = itemWeaponSettings.itemIcon;
            newItemInfo.toolTier = itemWeaponSettings.toolTier;
            newItemInfo.efficiency = itemWeaponSettings.damage;
            newItemInfo.itemMaterialList = itemWeaponSettings.itemToCraft;
            newItemInfo.secondsToCraft = itemWeaponSettings.secondsToCraft;
            newItemInfo.xpWhenCrafted = itemWeaponSettings.xpWhenCrafted;
            newItemInfo.quantityWhenCrafted = itemWeaponSettings.quantityWhenCrafted;
            CustomItemInfos.Add(newItemInfo);
            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[newItemInfo.itemTypeIndex].itemsList.ToList();
            itemInfos.Add(newItemInfo);
            ItemList.instance.itemsTypes[(int)newItemInfo.itemType].itemsList = itemInfos.ToArray();
            if (itemWeaponSettings.itemCraftingType != ItemCraftingType.None)
                if (!AddToCraftTable(newItemInfo, itemWeaponSettings.itemCraftingType))
                    Logger.Error("Failed to add crafting for item named \"" + itemWeaponSettings.itemName + "\"");
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
            public ItemMaterialSettings(string itemName, Sprite itemIcon, string itemDescription = "", ItemMaterial[] itemToCraft = null, int secondsToCraft = 2, int quantityWhenCrafted = 1, int xpWhenCrafted = 0, ItemCraftingType itemCraftingType = ItemCraftingType.None)
            {
                this.itemName = itemName;
                this.itemDescription = itemDescription;
                this.itemIcon = itemIcon;
                if (itemToCraft == null) itemToCraft = new ItemMaterial[0];
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.xpWhenCrafted = xpWhenCrafted;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }

            public string itemName;
            public string itemDescription;
            public Sprite itemIcon;
            public ItemMaterial[] itemToCraft;
            public int secondsToCraft;
            public int xpWhenCrafted;
            public int quantityWhenCrafted;
            public ItemCraftingType itemCraftingType;
        }

        [System.Serializable]
        public class ItemWeaponSettings
        {
            public ItemWeaponSettings(string itemName, Sprite itemIcon, string itemDescription = "", int toolTier = 1, int damage = 1, ItemMaterial[] itemToCraft = null, int secondsToCraft = 2, int quantityWhenCrafted = 0, int xpWhenCrafted = 1, ItemCraftingType itemCraftingType = ItemCraftingType.None)
            {
                this.itemName = itemName;
                this.itemIcon = itemIcon;
                this.itemDescription = itemDescription;
                this.toolTier = toolTier;
                this.damage = damage;
                if (itemToCraft == null) itemToCraft = new ItemMaterial[0];
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.xpWhenCrafted = xpWhenCrafted;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }

            public string itemName;
            public string itemDescription;
            public Sprite itemIcon;
            public int toolTier;
            public int damage;
            public ItemMaterial[] itemToCraft;
            public int secondsToCraft;
            public int xpWhenCrafted;
            public int quantityWhenCrafted;
            public ItemCraftingType itemCraftingType;
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
