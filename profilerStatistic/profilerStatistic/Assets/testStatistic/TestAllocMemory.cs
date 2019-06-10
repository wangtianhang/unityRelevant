using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Test
{
    class TestAllocMemory : MonoBehaviour
    {
        private void Update()
        {
            List<string> test = new List<string>();
            test.Add("haha");

            Test();
        }

        void Test()
        {
            //List<string> test = new List<string>();
            //test.Add("gaga");
            for(int i = 0; i < 10; ++i)
            {
                Debug.Log("gag" + i);
            }
            
        }
    }
}
