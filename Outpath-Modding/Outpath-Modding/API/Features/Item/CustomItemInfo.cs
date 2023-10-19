using Outpath_Modding.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ItemInfo;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.API.Features.Item
{
    public class CustomItemInfo
    {
        public static List<CustomItemInfo> CustomItemInfos { get; } = new List<CustomItemInfo>();

        public CustomItemInfo(ItemInfo itemInfo)
        {
            Base = itemInfo;
            Settings = null;
            CustomItemInfos.Add(this);
        }

        public CustomItemInfo(ItemInfo itemInfo, ItemSettings itemSettings)
        {
            Base = itemInfo;
            new OutpathItemInfo(Base);
            Settings = itemSettings;
            CustomItemInfos.Add(this);
        }

        public CustomItemInfo(ItemSettings itemSettings)
        {
            Base = null;
            Settings = itemSettings;
            CustomItemInfos.Add(this);
        }

        public ItemInfo Base { get; private set; }

        public ItemSettings Settings { get; private set; }

        public void RegistrateItemInfo()
        {
            if (Base == null)
            {
                if (Settings is ItemMaterialSettings itemMaterialSettings)
                {
                    ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Wood").Clone();
                    newItemInfo.itemName = itemMaterialSettings.itemName;
                    newItemInfo.itemDesc = itemMaterialSettings.itemDescription;
                    newItemInfo.itemID = GetNewItemId();
                    newItemInfo.itemIcon = itemMaterialSettings.itemIcon;
                    newItemInfo.itemMaterialList = ConvertItemCraft(itemMaterialSettings.itemToCraft);
                    newItemInfo.secondsToCraft = itemMaterialSettings.secondsToCraft;
                    newItemInfo.xpWhenCrafted = itemMaterialSettings.xpWhenCrafted;
                    newItemInfo.quantityWhenCrafted = itemMaterialSettings.quantityWhenCrafted;
                    Base = newItemInfo;
                }
                else if (Settings is ItemWeaponSettings itemWeaponSettings)
                {
                    ItemInfo newItemInfo = ItemList.instance.itemList.First(x => x.name == "Sword Flint").Clone();
                    newItemInfo.itemName = itemWeaponSettings.itemName;
                    newItemInfo.itemDesc = itemWeaponSettings.itemDescription;
                    newItemInfo.itemID = GetNewItemId();
                    newItemInfo.itemIcon = itemWeaponSettings.itemIcon;
                    newItemInfo.toolTier = itemWeaponSettings.toolTier;
                    newItemInfo.efficiency = itemWeaponSettings.damage;
                    newItemInfo.itemMaterialList = ConvertItemCraft(itemWeaponSettings.itemToCraft);
                    newItemInfo.secondsToCraft = itemWeaponSettings.secondsToCraft;
                    newItemInfo.xpWhenCrafted = itemWeaponSettings.xpWhenCrafted;
                    newItemInfo.quantityWhenCrafted = itemWeaponSettings.quantityWhenCrafted;
                    Base = newItemInfo;
                }
            }

            List<ItemInfo> itemInfos = ItemList.instance.itemList.ToList();
            itemInfos.Add(Base);
            ItemList.instance.itemList = itemInfos.ToArray();
            itemInfos = ItemList.instance.itemsTypes[Base.itemTypeIndex].itemsList.ToList();
            itemInfos.Add(Base);
            ItemList.instance.itemsTypes[Base.itemTypeIndex].itemsList = itemInfos.ToArray();
            if (Settings.itemCraftingType != ItemCraftingType.None)
                if (!AddToCraftTable(Base, Settings.itemCraftingType))
                    Logger.Error("Failed to add crafting for item named \"" + Settings.itemName + "\"");
        }

        public static void RegisterAllItems()
        {
            foreach (var customItem in CustomItemInfos)
                customItem.RegistrateItemInfo();
        }

        public static void AddNewMaterial(string itemName, Sprite icon)
        {
            new CustomItemInfo(new ItemMaterialSettings(itemName, icon));
        }

        public static void AddNewMaterial(ItemMaterialSettings itemMaterialSettings)
        {
            new CustomItemInfo(itemMaterialSettings);
        }

        public static void AddNewWeapon(string itemName, Sprite icon)
        {
            new CustomItemInfo(new ItemWeaponSettings(itemName, icon));
        }

        public static void AddNewWeapon(ItemWeaponSettings itemWeaponSettings)
        {
            new CustomItemInfo(itemWeaponSettings);
        }

        private static bool AddToCraftTable(ItemInfo itemInfo, ItemCraftingType itemCraftingType)
        {
            try
            {
                GardenItemInfo gardenItemInfo = GetGardenItem(itemCraftingType);
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

        private static GardenItemInfo GetGardenItem(ItemCraftingType itemCraftingType)
        {
            foreach (var buildCategory in InventoryManager.instance.buildCategories)
                foreach (var buildSubCategory in buildCategory.CategoriesList)
                {
                    foreach (var buildInCategory in buildSubCategory.buildsInCategoryList)
                    {
                        Logger.Debug("Sub cat name: " + buildSubCategory.buildSubCatName + " build in cat: " + buildInCategory.itemName);
                    }
                }

            foreach (var buildCategory in InventoryManager.instance.buildCategories)
                foreach (var buildSubCategory in buildCategory.CategoriesList)
                    if (buildSubCategory.buildsInCategoryList.ToList().Count(x => x.itemName.Replace(" ", "") == itemCraftingType.ToString()) > 0)
                        return buildSubCategory.buildsInCategoryList.ToList().First(x => x.itemName.Replace(" ", "") == itemCraftingType.ToString());
            return null;
        }

        private static ItemMaterial[] ConvertItemCraft(ItemCraftMaterial[] itemCraftMaterials)
        {
            List<ItemMaterial> itemMaterials = new List<ItemMaterial>();
            foreach (var item in itemCraftMaterials)
                itemMaterials.Add(new ItemMaterial() { itemInfo = ItemList.instance.itemList.First(x => x.itemName == item.itemInfoName), quantityNeeded = item.quantityNeeded });
            return itemMaterials.ToArray();
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

        [Serializable]
        public class ItemSettings
        {
            public string itemName;
            public string itemDescription;
            public Sprite itemIcon;
            public ItemCraftMaterial[] itemToCraft;
            public int secondsToCraft;
            public int xpWhenCrafted;
            public int quantityWhenCrafted;
            public ItemCraftingType itemCraftingType;
        }

        [Serializable]
        public class ItemMaterialSettings : ItemSettings
        {
            public ItemMaterialSettings(string itemName, Sprite itemIcon, string itemDescription = "", ItemCraftMaterial[] itemToCraft = null, int secondsToCraft = 2, int quantityWhenCrafted = 1, int xpWhenCrafted = 0, ItemCraftingType itemCraftingType = ItemCraftingType.None)
            {
                this.itemName = itemName;
                this.itemDescription = itemDescription;
                this.itemIcon = itemIcon;
                if (itemToCraft == null) itemToCraft = new ItemCraftMaterial[0];
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.xpWhenCrafted = xpWhenCrafted;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }
        }

        [Serializable]
        public class ItemWeaponSettings : ItemSettings
        {
            public ItemWeaponSettings(string itemName, Sprite itemIcon, string itemDescription = "", int toolTier = 1, int damage = 1, ItemCraftMaterial[] itemToCraft = null, int secondsToCraft = 2, int quantityWhenCrafted = 0, int xpWhenCrafted = 1, ItemCraftingType itemCraftingType = ItemCraftingType.None)
            {
                this.itemName = itemName;
                this.itemIcon = itemIcon;
                this.itemDescription = itemDescription;
                this.toolTier = toolTier;
                this.damage = damage;
                if (itemToCraft == null) itemToCraft = new ItemCraftMaterial[0];
                this.itemToCraft = itemToCraft;
                this.secondsToCraft = secondsToCraft;
                this.xpWhenCrafted = xpWhenCrafted;
                this.quantityWhenCrafted = quantityWhenCrafted;
                this.itemCraftingType = itemCraftingType;
            }

            public int toolTier;
            public int damage;
        }

        [Serializable]
        public class ItemCraftMaterial
        {
            public string itemInfoName;

            public int quantityNeeded = 1;
        }

        public enum ItemCraftingType
        {
            None = 0,
            Workbench,
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
