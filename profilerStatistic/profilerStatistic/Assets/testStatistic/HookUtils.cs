using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookUtils
{
    public static void Begin(string tag)
    {
        Debug.Log("HookUtil begin " + tag);
    }

    public static void End(string tag)
    {
        Debug.Log("HookUtil end " + tag);
    }

    public static void ToMessage()
    {

    }
}
