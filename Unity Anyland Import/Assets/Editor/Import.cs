using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
                var json = new JSONObject(jsontext);
                GameObject parent = new GameObject();
                string name = json["n"].str;
                parent.name = name;

                if (!parent.GetComponent<Thing>())
                {
                    parent.AddComponent<Thing>();
                }
                Thing thing = parent.GetComponent<Thing>();
                if (json["inc"]) {
                    foreach (JSONObject inc in json["inc"])
                    {
                        thing.inc.Add(inc[0].str);
                        thing.inc.Add(inc[1].str);
                    }
                }
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

				if (b>17 && b<24) {b=1;}
				if (b>89 && b<144) {b=1;}
				if (b>205 && b<207) {b=1;}
					GameObject objectLoaded = Resources.Load("" + b, typeof(GameObject)) as GameObject;
					GameObject newObject = Instantiate(objectLoaded, new Vector3(px, py, pz), Quaternion.identity);
					newObject.transform.localScale = new Vector3(sx, sy, sz);
					newObject.transform.localEulerAngles = new Vector3(rx, ry, rz);
					newObject.transform.SetParent(parent.transform);
					newObject.name = ""+b;

					newObject.tag = "Part";
					if (!newObject.GetComponent<Part>()) {
						newObject.AddComponent<Part>();
					}

					foreach(JSONObject s in o["s"]){
						State state = new State(b);

						float px1 = s["p"][0].n;
						float py1 = s["p"][1].n;
						float pz1 = s["p"][2].n;

						float sx1 = s["s"][0].n;
						float sy1 = s["s"][1].n;
						float sz1 = s["s"][2].n;

						float rx1 = s["r"][0].n;
						float ry1 = s["r"][1].n;
						float rz1 = s["r"][2].n;

						float red = s["c"][0].n;
						float green = s["c"][1].n;
						float blue = s["c"][2].n;
						state.color.r=red;
						state.color.g=green;
						state.color.b=blue;
						state.position.x=px1;
						state.position.y=py1;
						state.position.z=pz1;
						state.scale.x=sx1;
						state.scale.y=sy1;
						state.scale.z=sz1;
						state.rotation.x=rx1;
						state.rotation.y=ry1;
						state.rotation.z=rz1;

						newObject.GetComponent<Part>().states.Add(state);
						if (s["b"]) {
							foreach(JSONObject script in s["b"]){
							state.scripts.Add(script.str);
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
