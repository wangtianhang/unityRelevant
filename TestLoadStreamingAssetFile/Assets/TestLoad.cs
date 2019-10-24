using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestLoad : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		string path = "test/test.txt";
		AndroidJavaClass javaClass = new AndroidJavaClass("com.example.loadstreamingassetfile.LoadStreamingAssetFile");
		byte[] data = javaClass.CallStatic<byte[]>("loadFile", path);
		string str = System.Text.Encoding.UTF8.GetString ( data );
		Debug.Log("TestLoad Start " + str);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
