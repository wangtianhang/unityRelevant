using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGyro : MonoBehaviour {

    // Faces for 6 sides of the cube
    private GameObject[] quads = new GameObject[6];

    // Textures for each quad, should be +X, +Y etc
    // with appropriate colors, red, green, blue, etc
    public Texture[] labels;

    //public Quaternion m_initQua;
    public GameObject m_parent = null;
    public GameObject m_subGo = null;

    void Start()
    {
        Input.gyro.enabled = true;
        StartCoroutine(GetGyro());

        // make camera solid colour and based at the origin
        GetComponent<Camera>().backgroundColor = new Color(49.0f / 255.0f, 77.0f / 255.0f, 121.0f / 255.0f);
        GetComponent<Camera>().transform.position = new Vector3(0, 0, 0);
        GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;

        // create the six quads forming the sides of a cube
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quads[0] = createQuad(quad, new Vector3(1, 0, 0), new Vector3(0, 90, 0), "plus x",
            new Color(0.90f, 0.10f, 0.10f, 1), labels[0]);
        quads[1] = createQuad(quad, new Vector3(0, 1, 0), new Vector3(-90, 0, 0), "plus y",
            new Color(0.10f, 0.90f, 0.10f, 1), labels[1]);
        quads[2] = createQuad(quad, new Vector3(0, 0, 1), new Vector3(0, 0, 0), "plus z",
            new Color(0.10f, 0.10f, 0.90f, 1), labels[2]);
        quads[3] = createQuad(quad, new Vector3(-1, 0, 0), new Vector3(0, -90, 0), "neg x",
            new Color(0.90f, 0.50f, 0.50f, 1), labels[3]);
        quads[4] = createQuad(quad, new Vector3(0, -1, 0), new Vector3(90, 0, 0), "neg y",
            new Color(0.50f, 0.90f, 0.50f, 1), labels[4]);
        quads[5] = createQuad(quad, new Vector3(0, 0, -1), new Vector3(0, 180, 0), "neg z",
            new Color(0.50f, 0.50f, 0.90f, 1), labels[5]);

        GameObject.Destroy(quad);
    }

    IEnumerator GetGyro()
    {
        //yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1);

        Quaternion initQua = GyroToUnity(Input.gyro.attitude);
        m_parent = new GameObject("parent");
        m_parent.transform.rotation = initQua;
        m_subGo = new GameObject("subGo");
        m_subGo.transform.parent = m_parent.transform;
        m_subGo.transform.localRotation = Quaternion.identity;
    }

    // make a quad for one side of the cube
    GameObject createQuad(GameObject quad, Vector3 pos, Vector3 rot, string name, Color col, Texture t)
    {
        Quaternion quat = Quaternion.Euler(rot);
        GameObject GO = Instantiate(quad, pos, quat);
        GO.name = name;
        GO.GetComponent<Renderer>().material.color = col;
        GO.GetComponent<Renderer>().material.mainTexture = t;
        GO.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
        return GO;
    }

    protected void Update()
    {
        GyroModifyCamera();
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
        GUILayout.Label("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
        if (m_parent != null)
        {
            GUILayout.Label("Parent Orientation: " + m_parent.transform.rotation);
            GUILayout.Label("SubGo Orientation: " + m_subGo.transform.rotation);
            GUILayout.Label("SubGo LocalOrientation: " + m_subGo.transform.localRotation);
        }
    }

    /********************************************/

    // The Gyroscope is right-handed.  Unity is left handed.
    // Make the necessary change to the camera.
    void GyroModifyCamera()
    {
        if(m_subGo != null)
        {
            Quaternion curQua = GyroToUnity(Input.gyro.attitude);
            m_subGo.transform.rotation = curQua;
            transform.rotation = m_subGo.transform.localRotation;
        }
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
