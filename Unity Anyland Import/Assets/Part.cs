using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Part:MonoBehaviour
{
   public List<State> states;
   public Part(){
       states = new List<State>();
   }
}