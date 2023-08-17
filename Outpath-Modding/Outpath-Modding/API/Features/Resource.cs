//-----------------------------------
//Waiting for a new generation system
//-----------------------------------
//
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.AccessControl;
//using UnityEngine;
//using static Outpath_Modding.API.Features.ResourcePrefab;
//using static TakeOutResource;

//namespace Outpath_Modding.API.Features
//{
//    public class Resource
//    {
//        public static List<Resource> Resources { get; private set; } = new List<Resource>();

//        public TakeOutResource ResourceBase { get; private set; }

//        public ResourceType Type { get; private set; }

//        public string PropName
//        {
//            get => ResourceBase.propName;
//            set => ResourceBase.propName = value;
//        }

//        public float MaxHealth
//        {
//            get => ResourceBase.health;
//            set => ResourceBase.health = value;
//        }

//        public float Health
//        {
//            get => ResourceBase.currHealth;
//            set => ResourceBase.currHealth = value;
//        }

//        public List<ItemDropGroup> ItemLootGroups
//        {
//            get => ResourceBase.itemPoolGroups.ToList();
//            set => ResourceBase.itemPoolGroups = value.ToArray();
//        }

//        public Vector3 Position
//        {
//            get => ResourceBase.transform.position;
//            set => ResourceBase.transform.position = value;
//        }

//        public Quaternion Rotation
//        {
//            get => ResourceBase.transform.rotation;
//            set => ResourceBase.transform.rotation = value;
//        }

//        public Vector3 Scale
//        {
//            get => ResourceBase.transform.localScale;
//            set => ResourceBase.transform.localScale = value;
//        }

//        public Resource(TakeOutResource resource)
//        {
//            ResourceBase = resource;
//            Resources.Add(this);
//        }

//        public void AddItemGroupToLoot(List<ItemChance> ItemsChance, int minQuantityPerItem, int maxQuantityPerItem, int ItemCount)
//        {
//            List<ItemDropGroup> tempProps = ItemLootGroups;
//            List<ItemDrop> itemDrops = new List<ItemDrop>();
//            foreach (ItemChance ItemChance in ItemsChance)
//            {
//                if (ItemChance.Chance < 0 || ItemChance.Chance > 1) ItemChance.Chance = 1;
//                itemDrops.Add(new ItemDrop() { itemInfo = ItemChance.ItemInfo, score = ItemChance.Chance });
//            }
//            tempProps.Add(new ItemDropGroup() { quantityToSpawn = new Vector2(minQuantityPerItem, maxQuantityPerItem), quantityItemDrops = ItemCount, itemPool = itemDrops.ToArray() });
//            ItemLootGroups = tempProps;
//        }

//        public void ClearLoot()
//        {
//            ItemLootGroups = new List<ItemDropGroup>();
//        }
//    }
//}
