using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MemeryTouchTest
{
    [TestFixture()]
    public class RandomUtilTest
    {
        [Test()]
        public void TestRandomInt()
        {
            for (int i = 0; i < 100; i++) {
                int rand = RandomUtil.RandomInt(0, 10);
                System.Console.Out.WriteLine(rand);
                Assert.IsTrue(0 <= rand && rand <= 10);
            }
        }

        [Test()]
        public void TestRandomFloat()
        {
            for (int i = 0; i < 100; i++) {
                float rand = RandomUtil.RandomFloat(0f, 10f);
                System.Console.Out.WriteLine(rand);
                Assert.IsTrue(0f <= rand && rand <= 10f);
            }
        }

        [Test()]
        public void TestRandomColor()
        {
            for (int i = 0; i < 100; i++) {
                Color color = RandomUtil.RandomColor();
                System.Console.Out.WriteLine(color.grayscale);
            }
        }

        [Test()]
        public void TestDraw()
        {
            Dictionary<float, int> dict = new Dictionary<float, int>(){ { 0.2f,1 }, { 0.4f,1 }, { 0.7f,3 }, };
            for (int i = 0; i < 100; i++) {
                float rand = RandomUtil.Draw(dict);
                System.Console.Out.WriteLine(rand.ToString());
                Assert.IsTrue(rand == 0.2f || rand == 0.4f || rand == 0.7f);
            }
        }

        [Test()]
        public void TestRandomChar()
        {
            System.Console.Out.WriteLine(RandomUtil.RandomChar());
        }

        [Test()]
        public void TestGenerateRandomStr()
        {
            string rand = RandomUtil.GenerateRandomStr(50);
            System.Console.Out.WriteLine(rand);
            Assert.IsTrue(rand.Length == 50);
        }

    }
}

