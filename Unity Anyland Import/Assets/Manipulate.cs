using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using Newtonsoft.Json;
using System.Text;

public class Manipulate : MonoBehaviour
{

	[MenuItem ("Anyland/Create Object")]
	static void MenuLoad ()
	{
		Manipulate.CreateObject(5);
	}

	static void CreateObject (int b)
	{
        int px=0,py=0,pz=0,sx=1,sy=1,sz=1,rx=0,ry=0,rz=0,cx=1,cy=1,cz=1;
        GameObject objectLoaded = Resources.Load("" + b, typeof(GameObject)) as GameObject;
        GameObject newObject = Instantiate(objectLoaded, new Vector3(px, py, pz), Quaternion.identity);
        newObject.transform.localScale = new Vector3(sx, sy, sz);
        newObject.transform.localEulerAngles = new Vector3(rx, ry, rz);
        //newObject.transform.SetParent(parent.transform);
        newObject.name = ""+b;
        newObject.tag = "Part";
        Renderer rend = newObject.transform.GetChild(0).GetComponent<Renderer>();
        var tempMaterial = new Material(rend.sharedMaterial);
        tempMaterial.color = new Color(cx,cy,cz);
        rend.sharedMaterial = tempMaterial;
	}
}
