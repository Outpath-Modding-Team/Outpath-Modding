using Outpath_Modding.API.Enums;
using Outpath_Modding.API.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static TakeOutResource;
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

        public Texture Texture
        {
            get => _texture;
            private set
            {
                _texture = value;
                OnChangeSprite();
            }
        }

        public List<ItemDropGroup> ItemLootGroups
        {
            get => ResourceBase.itemPoolGroups.ToList();
            set => ResourceBase.itemPoolGroups = value.ToArray();
        }

        private Texture _texture;

        public ResourcePrefab(TakeOutResource resource)
        {
            ResourceBase = resource;
            Type = resource.SFX_hit.Split('_')[2].ToEnum<ResourceType>();
            ResourcePrefabs.Add(this);
        }

        private void OnChangeSprite()
        {
            GameObject MaterialObj = ResourceBase.transform.GetChild(0).GetChild(0).gameObject;
            Material newMat = new Material(MaterialObj.GetComponent<MeshRenderer>().material);
            newMat.SetTexture("_BaseMap", _texture);
            //MaterialObj.GetComponent<MeshRenderer>().material.mainTexture = _texture;
            MaterialObj.GetComponent<MeshRenderer>().material = newMat;
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
            resource.SetActive(false);
            return resourcePrefab;
        }

        public static ResourcePrefab CreatePrefab(ResourcePrefabSettings resourcePrefabSettings)
        {
            GameObject resource = GameObject.Instantiate(MainResourcePrefab.ResourceBase, new Vector3(0, -100, 0), Quaternion.identity).gameObject;
            GameObject.DontDestroyOnLoad(resource);
            ResourcePrefab resourcePrefab = new ResourcePrefab(resource.GetComponent<TakeOutResource>());
            resourcePrefab.Type = resourcePrefabSettings.Type;
            resourcePrefab.PropName = resourcePrefabSettings.PropName;
            resourcePrefab.MaxHealth = resourcePrefabSettings.MaxHealth;
            resourcePrefab.Texture = resourcePrefabSettings.Texture;
            resource.SetActive(false);
            return resourcePrefab;
        }

        public class ResourcePrefabSettings
        {
            public ResourcePrefabSettings(string propName, float maxHealth, ResourceType resourceType, Texture texture)
            {
                PropName = propName;
                MaxHealth = maxHealth;
                Type = resourceType;
                Texture = texture;
            }

            public ResourceType Type { get; set; }

            public string PropName { get; set; }

            public float MaxHealth { get; set; }

            public Texture Texture { get; set; }
        }

        [System.Serializable]
        public class ItemChance
        {
            public ItemInfo ItemInfo { get; set; }
            public int Chance { get; set; }
        }
    }
}
