using Outpath_Modding.API.Enums;
using Outpath_Modding.API.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static TakeOutResource;
using Logger = Outpath_Modding.GameConsole.Logger;
using Resources = UnityEngine.Resources;

namespace Outpath_Modding.API.Features
{
    public class ResourcePrefab
    {
        public static List<ResourcePrefab> ResourcePrefabs { get; private set; } = new List<ResourcePrefab>();

        public static ResourcePrefab MainResourcePrefab { get; private set; } = new ResourcePrefab(Resources.Load<GameObject>(Path.Combine("Props", "Prop_Prefab")).GetComponent<TakeOutResource>());

        public TakeOutResource ResourceBase { get; private set; }

        public ResourceType Type { get; private set; }

        public string PropName
        {
            get => ResourceBase.propName;
            set => ResourceBase.propName = value;
        }

        public float MaxHealth
        {
            get => ResourceBase.health;
            set => ResourceBase.health = value;
        }

        public float Health
        {
            get => ResourceBase.currHealth;
            set => ResourceBase.currHealth = value;
        }

        public Sprite Sprite
        {
            get => _sprite;
            private set
            {
                _sprite = value;
                OnChangeSprite();
            }
        }

        public List<ItemDropGroup> ItemLootGroups
        {
            get => ResourceBase.itemPoolGroups.ToList();
            set => ResourceBase.itemPoolGroups = value.ToArray();
        }

        private Sprite _sprite;

        public ResourcePrefab(TakeOutResource resource)
        {
            ResourceBase = resource;
            Type = resource.SFX_hit.Split('_')[2].ToEnum<ResourceType>();
            ResourcePrefabs.Add(this);
        }

        private void OnChangeSprite()
        {
            try
            {
                GameObject MaterialObj = ResourceBase.transform.GetChild(0).GetChild(0).gameObject;
                GameObject.Destroy(MaterialObj.GetComponent<RandomMeshSprite>());
                MaterialObj.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Sprite.texture);
            }
            catch (Exception ex)
            {
                Logger.Error("Error texture: " + ex.ToString());
            }
        }

        public void AddItemGroupToLoot(List<ItemChance> ItemsChance, int minQuantityPerItem, int maxQuantityPerItem, int ItemCount)
        {
            List<ItemDropGroup> tempProps = ItemLootGroups;
            List<ItemDrop> itemDrops = new List<ItemDrop>();
            foreach (ItemChance ItemChance in ItemsChance)
            {
                if (ItemChance.Chance < 0 || ItemChance.Chance > 1) ItemChance.Chance = 1;
                itemDrops.Add(new ItemDrop() { itemInfo = ItemChance.ItemInfo, score = ItemChance.Chance });
            }
            tempProps.Add(new ItemDropGroup() { quantityToSpawn = new Vector2(minQuantityPerItem, maxQuantityPerItem), quantityItemDrops = ItemCount, itemPool = itemDrops.ToArray() });
            ItemLootGroups = tempProps;
        }

        public void ClearLoot()
        {
            ItemLootGroups = new List<ItemDropGroup>();
        }

        public static ResourcePrefab CreatePrefab(string propName, float maxHealth, ResourceType resourceType)
        {
            GameObject resource = GameObject.Instantiate(MainResourcePrefab.ResourceBase, new Vector3(0, -100, 0), Quaternion.identity).gameObject;
            GameObject.DontDestroyOnLoad(resource);
            ResourcePrefab resourcePrefab = new ResourcePrefab(resource.GetComponent<TakeOutResource>());
            resourcePrefab.Type = resourceType;
            resourcePrefab.PropName = propName;
            resourcePrefab.MaxHealth = maxHealth;
            //resource.SetActive(false);
            return resourcePrefab;
        }

        public static ResourcePrefab CreatePrefab(ResourcePrefabSettings resourcePrefabSettings)
        {
            GameObject resource = GameObject.Instantiate(MainResourcePrefab.ResourceBase, new Vector3(0, -100, 0), Quaternion.identity).gameObject;
            GameObject.DontDestroyOnLoad(resource);
            ResourcePrefab resourcePrefab = new ResourcePrefab(resource.GetComponent<TakeOutResource>());
            resourcePrefab.Sprite = resourcePrefabSettings.Sprite;
            resourcePrefab.Type = resourcePrefabSettings.Type;
            resourcePrefab.PropName = resourcePrefabSettings.PropName;
            resourcePrefab.MaxHealth = resourcePrefabSettings.MaxHealth;
            //resource.SetActive(false);
            return resourcePrefab;
        }

        public class ResourcePrefabSettings
        {
            public ResourcePrefabSettings(string propName, float maxHealth, ResourceType resourceType, Sprite texture)
            {
                PropName = propName;
                MaxHealth = maxHealth;
                Type = resourceType;
                Sprite = texture;
            }

            public ResourceType Type { get; set; }

            public string PropName { get; set; }

            public float MaxHealth { get; set; }

            public Sprite Sprite { get; set; }
        }

        [System.Serializable]
        public class ItemChance
        {
            public ItemInfo ItemInfo { get; set; }
            public int Chance { get; set; }
        }
    }
}
