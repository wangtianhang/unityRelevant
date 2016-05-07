using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Util
{
    public static T DeepClone<T>(T item)
        where T : class
    {
        T result = default(T);
        if (null != item)
        {
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = 
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf.Serialize(ms, item);
            ms.Seek(0, SeekOrigin.Begin);
            result = bf.Deserialize(ms) as T;
        }
        return result;
    }
}

