using System;
using System.IO;
using System.Collections.Generic;

namespace SerializationExamples
{
    [Serializable]
    public class MapCoordinates
    {
        public int X;
        public int Y;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
        }
    }

    [Serializable]
    public enum ItemType
    {
        Food = 0,
        Spell = 1,
        Weapon = 2,
        ConstructionMaterial = 3,
        DecoItem = 4
    }

    [Serializable]
    public class InventoryItem
    {
        public string Name;
        public int Quantity;
        public ItemType Type;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Quantity);
            writer.Write((short)Type);
        }

        public void Deserialize(BinaryReader reader)
        {
            Name = reader.ReadString();
            Quantity = reader.ReadInt32();
            Type = (ItemType)reader.ReadInt16();
        }
    }

    [Serializable]
    public class Monster
    {
        public string Name;
        public int Age;

        internal MapCoordinates mLastSpawn;
        internal List<InventoryItem> mInventory;

        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Age);
            mLastSpawn.Serialize(writer);
            writer.Write(mInventory.Count);
            foreach (InventoryItem item in mInventory)
            {
                item.Serialize(writer);
            }
        }

        public virtual void Deserialize(BinaryReader reader)
        {
            Name = reader.ReadString();
            Age = reader.ReadInt32();
            mLastSpawn = new MapCoordinates();
            mLastSpawn.Deserialize(reader);
            int len = reader.ReadInt32();
            mInventory = new List<InventoryItem>(len);
            for (int i = 0; i < len; i++)
            {
                InventoryItem item = new InventoryItem();
                item.Deserialize(reader);
                mInventory.Add(item);
            }
        }
    }

    [Serializable]
    public class SeaMonster : Monster
    {
        public short SwimSpeed;

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(SwimSpeed);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            SwimSpeed = reader.ReadInt16();
        }
    }

    public class ObjectHelper
    {
        public ObjectHelper()
        {
            mRandom = new Random(Environment.TickCount);
        }

        public SeaMonster CreateSeaMonster()
        {
            Monster tmp = CreateMonster();
            SeaMonster result = new SeaMonster();

            CopyMonster(tmp, result);
            result.SwimSpeed = (short)mRandom.Next();

            return result;
        }

        public bool CompareSeaMonster(SeaMonster lft, SeaMonster rgt)
        {
            if (lft == null || rgt == null)
            {
                return lft == rgt;
            }

            return lft.SwimSpeed == rgt.SwimSpeed &&
                CompareMonster(lft, rgt);
        }

        public Monster CreateMonster()
        {
            Monster result = new Monster();
            result.Name = "Name: " + mRandom.Next();
            result.Age = mRandom.Next();
            result.mLastSpawn = CreateMapCoordinates();
            result.mInventory = new List<InventoryItem>();
            for (int i = 0; i < mRandom.Next(10, 20); i++)
            {
                result.mInventory.Add(CreateInventoryItem());
            }

            return result;
        }

        public bool CompareMonster(Monster lft, Monster rgt)
        {
            if (lft == null || rgt == null)
            {
                return lft == rgt;
            }

            bool equals = lft.Name.Equals(rgt.Name) &&
                lft.Age == rgt.Age &&
                CompareMapCoordinates(lft.mLastSpawn, rgt.mLastSpawn) &&
                lft.mInventory.Count == rgt.mInventory.Count;

            if (!equals)
            {
                return false;
            }

            for (int i = 0; i < lft.mInventory.Count && equals; i++)
            {
                equals = equals &&
                    CompareInventoryItem(lft.mInventory[i], rgt.mInventory[i]);
            }

            return equals;
        }

        public InventoryItem CreateInventoryItem()
        {
            InventoryItem result = new InventoryItem();
            result.Name = "Name: " + mRandom.Next();
            result.Quantity = mRandom.Next();
            result.Type = (ItemType)mRandom.Next(
                (int)ItemType.Food, (int)ItemType.DecoItem);

            return result;
        }

        public bool CompareInventoryItem(InventoryItem lft, InventoryItem rgt)
        {
            if (lft == null || rgt == null)
            {
                return lft == rgt;
            }

            return lft.Name.Equals(rgt.Name) &&
                lft.Quantity == rgt.Quantity &&
                lft.Type == rgt.Type;
        }

        public MapCoordinates CreateMapCoordinates()
        {
            MapCoordinates result = new MapCoordinates();
            result.X = mRandom.Next();
            result.Y = mRandom.Next();

            return result;
        }

        public bool CompareMapCoordinates(MapCoordinates lft, MapCoordinates rgt)
        {
            if (lft == null || rgt == null)
            {
                return lft == rgt;
            }

            return lft.X == rgt.X &&
                lft.Y == rgt.Y;
        }

        void CopyMonster(Monster src, Monster dst)
        {
            dst.Name = src.Name;
            dst.Age = src.Age;
            dst.mLastSpawn = src.mLastSpawn;
            dst.mInventory = src.mInventory;
        }

        Random mRandom;
    }
}
