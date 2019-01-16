using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class Import : MonoBehaviour {

	public GameObject[] prefab;
	public GameObject group;
    public void LoadJson()
    {
        using (StreamReader r = new StreamReader("Assets/testJSON/truck.json"))
        {
            string jsontext = r.ReadToEnd();
            Debug.Log(jsontext);
			
			 var json = new JSONObject(jsontext);    
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
					GameObject newObject = Instantiate(prefab[b-1], new Vector3(px, py, pz), Quaternion.identity);
					newObject.transform.localScale = new Vector3(sx, sy, sz);
					newObject.transform.rotation = Quaternion.Euler(rx,ry,rz);
					newObject.transform.parent = group.transform;
					Renderer rend = newObject.transform.GetChild(0).GetComponent<Renderer>();
					rend.material.SetColor("_Color", new Color(cx,cy,cz));	
			}
        }
    }
	// Use this for initialization
	void Start () {
		LoadJson();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
