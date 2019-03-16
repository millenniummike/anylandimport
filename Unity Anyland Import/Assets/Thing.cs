using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Thing:MonoBehaviour
{
   public List<string> inc;
   public Thing(){
       inc = new List<string>();
   }
}