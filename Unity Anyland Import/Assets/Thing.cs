using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Thing : MonoBehaviour
{	
   public List<string> scripts = new List<string>();
   public List<JSONObject> states = new List<JSONObject>();
}