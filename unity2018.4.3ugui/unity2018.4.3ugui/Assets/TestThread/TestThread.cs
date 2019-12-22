using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestThread : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("mainThread " + Thread.CurrentThread.ManagedThreadId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
