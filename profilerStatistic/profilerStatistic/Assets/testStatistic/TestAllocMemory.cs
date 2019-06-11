using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Test
{
    class TestAllocMemory : MonoBehaviour
    {
        private void Start()
        {
            //Profiler.logFile = "testProfiler.raw";
            //Profiler.enabled = true;
            //Profiler.enableBinaryLog = true;
            //Debug.Log("Start testProfiler.log");
        }

        private void Update()
        {
            Profiler.BeginSample("TestAllocMemory");
            //uint tmp1 = Profiler.GetTempAllocatorSize();
            List<string> test = new List<string>();
            test.Add("haha");

            //uint tmp2 = Profiler.GetTempAllocatorSize();
            Test();

            //uint tmp3 = Profiler.GetTempAllocatorSize();
            Profiler.EndSample();
        }

        void Test()
        {
            //List<string> test = new List<string>();
            //test.Add("gaga");
            //             for(int i = 0; i < 10; ++i)
            //             {
            //                 Debug.Log("gag" + i);
            //             }
            List<string> test = new List<string>();
            test.Add("haha");
        }
    }
}
