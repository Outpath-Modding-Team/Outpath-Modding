//-----------------------------------
//Waiting for a new generation system
//-----------------------------------
//
//using Outpath_Modding.API.Enums;
//using Outpath_Modding.API.Extensions;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Outpath_Modding.API.Features
//{
//    public class IslandBlock
//    {
//        public static List<IslandBlock> IslandBlocks { get; private set; } = new List<IslandBlock>();

//        public Block Block { get; private set; }

//        public BlockType Type { get; private set; }

//        public BiomeType BiomeType { get; private set; }

//        public Vector3 Position
//        {
//            get => Block.transform.position;
//            set => Block.transform.position = value;
//        }

//        public Quaternion Rotation
//        {
//            get => Block.transform.rotation;
//            set => Block.transform.rotation = value;
//        }

//        public Vector3 Scale
//        {
//            get => Block.transform.localScale;
//            set => Block.transform.localScale = value;
//        }

//        public List<Block.Prop> PropToSpawn
//        {
//            get => Block.propToSpawn.ToList();
//            set => Block.propToSpawn = value.ToArray();
//        }

//        public IslandBlock(Block block)
//        {
//            Block = block;
//            Type = Block.name.Split('_')[2].ToEnum<BlockType>();
//            BiomeType = Block.name.Split('_')[1].ToEnum<BiomeType>();
//            IslandBlocks.Add(this);
//        }

//        public void AddPropToSpawn(Block.Prop prop)
//        {
//            List<Block.Prop> tempProps = PropToSpawn;
//            tempProps.Add(prop);
//            PropToSpawn = tempProps;
//        }

//        public void AddPropToSpawn(ResourcePrefab resourcePrefab, float firstChance, float secondChance)
//        {
//            List<Block.Prop> tempProps = PropToSpawn;
//            if (firstChance < 0 || firstChance > 1) firstChance = 1;
//            if (secondChance < 0 || secondChance > 1) secondChance = 1;
//            tempProps.Add(new Block.Prop() { itemPrefab = resourcePrefab.ResourceBase.gameObject, propTypes = new string[] { resourcePrefab.Type.ToString() }, probToSpawn = secondChance, score = firstChance });
//            PropToSpawn = tempProps;
//        }
//    }
//}
