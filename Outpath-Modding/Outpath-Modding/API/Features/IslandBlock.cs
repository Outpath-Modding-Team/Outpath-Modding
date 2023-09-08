using Mono.Cecil;
using Outpath_Modding.API.Enums;
using Outpath_Modding.API.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outpath_Modding.API.Features
{
    public class IslandBlock
    {
        public static List<IslandBlock> IslandBlocks { get; private set; } = new List<IslandBlock>();
        private static List<AddPropToSpawnData> addPropToSpawnDatas = new List<AddPropToSpawnData>();

        public Block Block { get; private set; }

        public BlockType Type { get; private set; }

        public BiomeType BiomeType { get; private set; }

        public Vector3 Position
        {
            get => Block.transform.position;
            set => Block.transform.position = value;
        }

        public Quaternion Rotation
        {
            get => Block.transform.rotation;
            set => Block.transform.rotation = value;
        }

        public Vector3 Scale
        {
            get => Block.transform.localScale;
            set => Block.transform.localScale = value;
        }

        public List<Block.Prop> PropToSpawn
        {
            get => Block.propToSpawn.ToList();
            set => Block.propToSpawn = value.ToArray();
        }

        public IslandBlock(Block block)
        {
            Block = block;
            Type = Block.name.Split('_')[3].ToEnum<BlockType>();
            BiomeType = Block.name.Split('_')[2].ToEnum<BiomeType>();
            IslandBlocks.Add(this);
        }

        public static void AddAllPropsToSpawn()
        {
            foreach (var data in addPropToSpawnDatas)
            {
                Block block = ArchipelagoManager.instance.blocksList.First(x => x.name.Contains(data.BiomeType.ToString()) && x.name.Contains(data.BlockType.ToString()));
                List<Block.Prop> tempProps = block.propToSpawn.ToList();
                foreach (var prop in data.Props)
                {
                    tempProps.Add(prop);
                }
                block.propToSpawn = tempProps.ToArray();
            }
        }

        public void AddPropToSpawn(Block.Prop prop)
        {
            List<Block.Prop> tempProps = PropToSpawn;
            tempProps.Add(prop);
            PropToSpawn = tempProps;
        }

        public void AddPropToSpawn(ResourcePrefab resourcePrefab, float firstChance, float secondChance)
        {
            List<Block.Prop> tempProps = PropToSpawn;
            if (firstChance < 0 || firstChance > 1) firstChance = 1;
            if (secondChance < 0 || secondChance > 1) secondChance = 1;
            tempProps.Add(new Block.Prop() { itemPrefabs = new TakeOutResource[] { resourcePrefab.ResourceBase }, propTypes = new string[] { resourcePrefab.Type.ToString() }, probToSpawn = secondChance, score = firstChance });
            PropToSpawn = tempProps;
        }

        public void AddPropToSpawn(List<ResourcePrefab> resourcePrefabs, float firstChance, float secondChance)
        {
            List<Block.Prop> tempProps = PropToSpawn;
            List<TakeOutResource> props = new List<TakeOutResource>();
            List<string> propTypes = new List<string>();
            foreach (var resource in resourcePrefabs)
            {
                props.Add(resource.ResourceBase);
                propTypes.Add(resource.Type.ToString());
            }
            if (firstChance < 0 || firstChance > 1) firstChance = 1;
            if (secondChance < 0 || secondChance > 1) secondChance = 1;
            tempProps.Add(new Block.Prop() { itemPrefabs = props.ToArray(), propTypes = propTypes.ToArray(), probToSpawn = secondChance, score = firstChance });
            PropToSpawn = tempProps;
        }

        public static void AddPropToSpawn(BiomeType biomeType, BlockType blockType, ResourcePrefab resourcePrefab, float firstChance, float secondChance)
        {
            List<Block.Prop> tempProps = new List<Block.Prop>();
            if (firstChance < 0 || firstChance > 1) firstChance = 1;
            if (secondChance < 0 || secondChance > 1) secondChance = 1;
            tempProps.Add(new Block.Prop() { itemPrefabs = new TakeOutResource[] { resourcePrefab.ResourceBase }, propTypes = new string[] { resourcePrefab.Type.ToString() }, probToSpawn = secondChance, score = firstChance });
            addPropToSpawnDatas.Add(new AddPropToSpawnData(biomeType, blockType, tempProps));
        }

        public static void AddPropToSpawn(BiomeType biomeType, BlockType blockType, List<ResourcePrefab> resourcePrefabs, float firstChance, float secondChance)
        {
            List<Block.Prop> tempProps = new List<Block.Prop>();
            List<TakeOutResource> props = new List<TakeOutResource>();
            List<string> propTypes = new List<string>();
            foreach (var resource in resourcePrefabs)
            {
                props.Add(resource.ResourceBase);
                propTypes.Add(resource.Type.ToString());
            }
            if (firstChance < 0 || firstChance > 1) firstChance = 1;
            if (secondChance < 0 || secondChance > 1) secondChance = 1;
            tempProps.Add(new Block.Prop() { itemPrefabs = props.ToArray(), propTypes = propTypes.ToArray(), probToSpawn = secondChance, score = firstChance });
            addPropToSpawnDatas.Add(new AddPropToSpawnData(biomeType, blockType, tempProps));
        }

        public class AddPropToSpawnData
        {
            public BiomeType BiomeType { get; set; }
            
            public BlockType BlockType { get; set; }

            public List<Block.Prop> Props { get; set; }

            public AddPropToSpawnData(BiomeType biomeType, BlockType blockType, List<Block.Prop> props)
            {
                BiomeType = biomeType;
                BlockType = blockType;
                Props = props;
            }
        }
    }
}
