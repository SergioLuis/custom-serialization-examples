using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectHelper helper = new ObjectHelper();
            long ini, millis;

            List<SeaMonster> testData = new List<SeaMonster>(TEST_SIZE);

            ini = Environment.TickCount;
            for (int i = 0; i < TEST_SIZE; i++)
            {
                testData.Add(helper.CreateSeaMonster());
            }
            millis = Environment.TickCount - ini;

            Console.WriteLine(string.Format(
                "Created {0} SeaMonster objects in {1} ms", TEST_SIZE, millis));

            BinaryFormatter formatter = new BinaryFormatter();
            Stream st = new MemoryStream();

            ini = Environment.TickCount;
            formatter.Serialize(st, testData);
            millis = Environment.TickCount - ini;

            Console.WriteLine(string.Format(
                "Serialized {0} SeaMonster objects the C# way in {1} ms.",
                TEST_SIZE, millis));

            Console.WriteLine(string.Format(
                "The stream has {0} bytes written.", st.Position));

            st.Position = 0;

            ini = Environment.TickCount;
            List<SeaMonster> deserTestData = formatter.Deserialize(st) as List<SeaMonster>;
            millis = Environment.TickCount - ini;

            Console.WriteLine(string.Format(
                "Deserialized {0} SeaMonster objects the C# way in {1} ms.",
                TEST_SIZE, millis));

            for (int i = 0; i < testData.Count; i++)
            {
                if (!helper.CompareSeaMonster(testData[i], deserTestData[i]))
                    throw new Exception(string.Format(
                        "The SeaMonster[{0}] is different after the deserialization.", i));
            }

            st = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(st);

            ini = Environment.TickCount;
            SerializeSeaMonsterList(testData, writer);
            millis = Environment.TickCount - ini;

            Console.WriteLine(string.Format(
                "Serialized {0} SeaMonster objects the custom way in {1} ms.",
                TEST_SIZE, millis));

            Console.WriteLine(string.Format(
                "The stream has {0} bytes written.", st.Position));

            st.Position = 0;
            BinaryReader reader = new BinaryReader(st);

            ini = Environment.TickCount;
            deserTestData = DeserializeSeaMonsterList(reader);
            millis = Environment.TickCount - ini;

            Console.WriteLine(string.Format(
                "Deserialized {0} SeaMonster objects the custom way in {1} ms.",
                TEST_SIZE, millis));

            for (int i = 0; i < testData.Count; i++)
            {
                if (!helper.CompareSeaMonster(testData[i], deserTestData[i]))
                    throw new Exception(string.Format(
                        "The SeaMonster[{0}] is different after the deserialization.", i));
            }

            Console.ReadLine();

            
        }

        static void SerializeSeaMonsterList(List<SeaMonster> monsters, BinaryWriter writer)
        {
            writer.Write(monsters.Count);
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].Serialize(writer);
            }
        }

        static List<SeaMonster> DeserializeSeaMonsterList(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            List<SeaMonster> result = new List<SeaMonster>(len);
            for (int i = 0; i < len; i++)
            {
                SeaMonster mons = new SeaMonster();
                mons.Deserialize(reader);
                result.Add(mons);
            }

            return result;
        }

        static readonly int TEST_SIZE = 100000;
    }
}
