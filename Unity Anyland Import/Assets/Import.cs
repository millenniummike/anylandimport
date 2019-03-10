using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class Import : MonoBehaviour {

    [MenuItem("Anyland/Import Json")]
    static void MenuLoad()
    {
        Import.LoadJson();
    }
    static void LoadJson()
    {
		string path = EditorUtility.OpenFilePanel("Open json", "", "json");
        if (path.Length != 0)
        {		
			using (StreamReader r = new StreamReader(path))
			{
			string jsontext = r.ReadToEnd();
			 Debug.Log(jsontext);
			 var json = new JSONObject(jsontext);
			 GameObject parent = new GameObject();
			 string name = "Thing Name";
			 parent.name = name;
			foreach(JSONObject o in json["p"])
			{
				float px = o["s"][0]["p"][0].n;
				float py = o["s"][0]["p"][1].n;
				float pz = o["s"][0]["p"][2].n;

				float sx = o["s"][0]["s"][0].n;
				float sy = o["s"][0]["s"][1].n;
				float sz = o["s"][0]["s"][2].n;

				float rx = o["s"][0]["r"][0].n;
				float ry = o["s"][0]["r"][1].n;
				float rz = o["s"][0]["r"][2].n;

				float cx = o["s"][0]["c"][0].n;
				float cy = o["s"][0]["c"][1].n;
				float cz = o["s"][0]["c"][2].n;

				int b = 1;
				if (o["b"] != null) {
					b = (int) o["b"].i;
				}
					GameObject objectLoaded = Resources.Load("" + b, typeof(GameObject)) as GameObject;
					GameObject newObject = Instantiate(objectLoaded, new Vector3(px, py, pz), Quaternion.identity);
					newObject.transform.localScale = new Vector3(sx, sy, sz);
					newObject.transform.localEulerAngles = new Vector3(rx, ry, rz);
					newObject.transform.SetParent(parent.transform);
					newObject.name = ""+b;
					newObject.tag = "Part";
					newObject.AddComponent<Thing>();
					foreach(JSONObject s in o["s"]){
						newObject.GetComponent<Thing>().states.Add(s);
						if (s["b"]) {
							foreach(JSONObject script in s["b"]){
							newObject.GetComponent<Thing>().scripts.Add(script.str);
							}
						}
					}
					Renderer rend = newObject.transform.GetChild(0).GetComponent<Renderer>();
					 var tempMaterial = new Material(rend.sharedMaterial);
 					tempMaterial.color = new Color(cx,cy,cz);
 					rend.sharedMaterial = tempMaterial;
				}
        	}
        }
    }
}
