using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AndroidJavaClass jc = new AndroidJavaClass("com.example.mylibrary.TestUnityCallJava");
        string ret = jc.CallStatic<string>("UnityCallAndroid");
        Debug.Log("ret " + ret);

        Debug.Log("Test Start end");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
