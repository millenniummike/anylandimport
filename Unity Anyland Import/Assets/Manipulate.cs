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

	[MenuItem ("Anyland/Create Object - 1")]
	static void MenuLoad1()
	{
		Manipulate.CreateObject(1,0,0,0,1,1,1,0,0,0,1,1,1);
	}

    [MenuItem ("Anyland/Create Object - 2")]
	static void MenuLoad2()
	{
		Manipulate.CreateObject(2,0,0,0,1,1,1,0,0,0,1,1,1);
	}

     [MenuItem ("Anyland/Create Object - 1 - spin")]
	static void MenuLoad3()
	{
		float rx, ry, rz, px, py, pz;
		for (int i = 0; i < 360; i=i+10)
		{
			rx = i;
			ry = 0;
			rz = 0;
			px = i / 10;
			py = i / 10;
			pz = 0;
			Manipulate.CreateObject(1,px,py,pz,1,1,1,rx,ry,rz,1,1,1);
		}
	}

	[MenuItem ("Anyland/Create Object - 1 - grid")]
	static void MenuLoad4()
	{
		float sx, sy, sz, rx, ry, rz, px, py, pz;
		float cx = 0f;
		float cy = 0f; 
		float cz = 0f;
		int b = 1;
		sx = 0.4f;
		sy = 0.4f;
		sz = 0.4f;

		for (int x = 0; x < 10; x++){
				for (int y = 0; y < 10; y++){
					rx = 0f;
					ry = 0f;
					rz = 0f;
					px = 0f + x;
					py = 0f + y;
					pz = 0f;
				b++;
				if (b > 17) {
					b = 1;
				}
                cx = cx + 0.01f;
                if (cx>1) {
                    cx = 0;
                }
				Manipulate.CreateObject(b,px,py,pz,sx,sy,sz,rx,ry,rz,cx,cy,cz);
				}
			}
	}

	static void CreateObject (int b,float px,float py, float pz, float sx, float sy, float sz,float rx, float ry, float rz, float cx, float cy, float cz)
	{
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
