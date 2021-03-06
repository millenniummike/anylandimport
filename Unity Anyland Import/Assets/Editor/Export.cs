using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using Newtonsoft.Json;
using System.Text;

public class Export : MonoBehaviour
{

	[MenuItem ("Anyland/Export Json")]
	static void MenuLoad ()
	{
		Export.SaveJson ();
	}

	static void SaveJson ()
	{

		var path = EditorUtility.SaveFilePanel (
			           "Save thing as JSON",
			           "",
			           "" + ".png",
			           "json");

		if (path.Length != 0) {
			StringBuilder sb = new StringBuilder ();
			StringWriter sw = new StringWriter (sb);

			using (JsonWriter writer = new JsonTextWriter (sw)) {
				writer.Formatting = Formatting.Indented;
				
				writer.WriteStartObject ();
					writer.WritePropertyName ("n");
					writer.WriteValue ("Thing Name");
					//**TODO add inc */
					writer.WritePropertyName ("p");
					writer.WriteStartArray ();

					GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag("Part");
					foreach (GameObject o in allGameObjects) {

						float px = o.transform.position.x;
						float py = o.transform.position.y;
						float pz = o.transform.position.z;

						float sx = o.transform.localScale.x;
						float sy = o.transform.localScale.y;
						float sz = o.transform.localScale.z;

						float rx = o.transform.localEulerAngles.x;
						float ry = o.transform.localEulerAngles.y;
						float rz = o.transform.localEulerAngles.z;

						Color c = o.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.GetColor("_Color");

						float cx = c.r;
						float cy = c.g;
						float cz = c.b;

						writer.WriteStartObject ();
						writer.WritePropertyName ("b");
						writer.WriteValue (Int32.Parse(o.name));

						writer.WritePropertyName ("s");
							writer.WriteStartArray ();
							//loop
							Part part = o.GetComponent<Part>();
							foreach (State state in part.states) {
							Debug.Log(state);
							Debug.Log(state.position.x);
							writer.WriteStartObject ();
								writer.WritePropertyName ("p");
									writer.WriteStartArray ();
									writer.WriteValue (state.position.x);
									writer.WriteValue (state.position.y);
									writer.WriteValue (state.position.z);
									writer.WriteEnd ();
								
								writer.WritePropertyName ("r");
									writer.WriteStartArray ();
									writer.WriteValue (state.rotation.x);
									writer.WriteValue (state.rotation.y);
									writer.WriteValue (state.rotation.z);
									writer.WriteEnd ();

								writer.WritePropertyName ("s");
									writer.WriteStartArray ();
									writer.WriteValue (state.scale.x);
									writer.WriteValue (state.scale.y);
									writer.WriteValue (state.scale.z);
									writer.WriteEnd ();

								writer.WritePropertyName ("c");
									writer.WriteStartArray ();
									writer.WriteValue (state.color.r);
									writer.WriteValue (state.color.g);
									writer.WriteValue (state.color.b);
									writer.WriteEnd ();

								writer.WritePropertyName ("b");
									writer.WriteStartArray();
									foreach(string script in state.scripts){
										writer.WriteValue(script);
									}
									writer.WriteEndArray();
							writer.WriteEndObject();
							}
						writer.WriteEndArray();
					writer.WriteEndObject();
					}
				writer.WriteEndObject();
			}
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.WriteLine(sb.ToString ());
        	streamWriter.Close();
		}
	}
}
