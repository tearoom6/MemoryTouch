using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace MemeryTouchTest
{
    [TestFixture()]
    public class EncryptUtilTest
    {
        [Test()]
        public void TestObjectToJson()
        {
            string json = EncryptUtil.ObjectToJson(new List<int>(new int []{ 1, 2, 3, 5 }));
            System.Console.Out.WriteLine(json);

            string json2 = EncryptUtil.ObjectToJson(4.3);
            System.Console.Out.WriteLine(json2);

            List<RankingRecord> records = new List<RankingRecord>();
            records.Add(new RankingRecord("easy", "murota", 1, 1080));
            records.Add(new RankingRecord("easy", "sekine", 2, 990));
            records.Add(new RankingRecord("easy", "saito", 3, 770));
            records.Add(new RankingRecord("easy", "koga", 4, 520));
            records.Add(new RankingRecord("easy", "atsukawa", 5, 410));
            string json3 = EncryptUtil.ObjectToJson(records);
            System.Console.Out.WriteLine(json3);
        }

        [Test()]
        public void TestJsonToObject()
        {
            List<int> obj = EncryptUtil.JsonToObject<List<int>>("[1,2,3,5]");
            foreach (int value in obj) {
                System.Console.Out.WriteLine(value);
            }

            float obj2 = EncryptUtil.JsonToObject<float>("4.3");
            System.Console.Out.WriteLine(obj2);

            List<RankingRecord> obj3 = EncryptUtil.JsonToObject<List<RankingRecord>>("[{\"category\":\"easy\",\"name\":\"murota\",\"rank\":1,\"point\":1080},{\"category\":\"easy\",\"name\":\"sekine\",\"rank\":2,\"point\":990},{\"category\":\"easy\",\"name\":\"saito\",\"rank\":3,\"point\":770},{\"category\":\"easy\",\"name\":\"koga\",\"rank\":4,\"point\":520},{\"category\":\"easy\",\"name\":\"atsukawa\",\"rank\":5,\"point\":410}]");
            foreach (RankingRecord value in obj3) {
                System.Console.Out.WriteLine(value.category);
                System.Console.Out.WriteLine(value.name);
                System.Console.Out.WriteLine(value.rank);
                System.Console.Out.WriteLine(value.point);
            }
        }

        [Test()]
        public void TestEncrypt()
        {
            string encrypted = EncryptUtil.Encrypt(new List<int>(new int []{ 1, 2, 3, 5 }), "fgeEe4gN");
            System.Console.Out.WriteLine(encrypted);
        }

        [Test()]
        public void TestDecrypt()
        {
            List<int> decrypted = EncryptUtil.Decrypt<List<int>>("QocK4XqBhGyfAbJPsaRI5A==", "fgeEe4gN");
            foreach (int value in decrypted) {
                System.Console.Out.WriteLine(value);
            }
        }

        [Test()]
        public void TestEncryptString()
        {
            string encrypted = EncryptUtil.EncryptString("10", "fgeEe4gN");
            System.Console.Out.WriteLine(encrypted);
        }

        [Test()]
        public void TestDecryptString()
        {
            string decrypted = EncryptUtil.DecryptString("0snlHO/3AvD8t7RBKR2dtA==", "fgeEe4gN");
            System.Console.Out.WriteLine(decrypted);
        }
    }
}

