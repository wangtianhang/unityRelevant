using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class AssetHelper
{
    public static void Assert(bool result, string tag)
    {
        if(!result)
        {
            StackTrace stackTrace = new StackTrace();
            string stackInfo = stackTrace.ToString();
            //StackFrame[] stackFrame = stackTrace.GetFrames();
            //string stackInfo = StackFramesToString(stackFrame);
            Console.WriteLine(tag + stackInfo);
        }

//         try
//         {
//             throw new Exception("test");
//         }
//         catch(System.Exception ex)
//         {
//             int test = 0;
// 
//             StackTrace stackTrace = new StackTrace();
//             //string stackInfo = stackTrace.ToString();
//             StackFrame[] stackFrame = stackTrace.GetFrames();
//             string stackInfo = StackFramesToString(stackFrame);
//             Console.WriteLine(stackInfo);
//         }
// 
//         try
//         {
//             throw new Exception("test");
//         }
//         catch (System.Exception ex)
//         {
//             StackTrace st = new StackTrace();
//             StackTrace st1 = new StackTrace(new StackFrame(true));
//             Console.WriteLine(" Stack trace for Main: {0}",
//                st1.ToString());
//             Console.WriteLine(st.ToString());
//         }
    }

//     static string StackFramesToString(StackFrame[] stacks)
//     {
//         string result = string.Empty;
//         foreach (StackFrame stack in stacks)
//         {
//             result += string.Format("{0} {1} {2} {3}\r\n", stack.GetFileName(),
//                 stack.GetFileLineNumber(),
//                 stack.GetFileColumnNumber(),
//                 stack.GetMethod().ToString());
//         }
//         return result;
//     }
}

