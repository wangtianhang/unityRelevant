/*
   Tuner Data -  Read Static Data in Game Development.
   e-mail : dongliang17@126.com
*/
using System.Diagnostics;
using UnityEngine;
namespace Tuner
{
    public class Singleton<T> where T : new()
    {
        protected Singleton()
        {
            //Debug.Assert(null == instance);
        }
        private static T internalInstance = new T();
        public static T Instance
        {
            get { return internalInstance; }
        }
    }

}
