using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct State {
	public Vector3 position;
	public Vector3 scale;
	public Vector3 rotation;
	public Color color;
	public List<string> scripts;

	public State(int b1)
    {
		position = new Vector3(0,0,0);
		scale = new Vector3(1,1,1);
		rotation = new Vector3(0,0,0);
		color = new Color(1,1,1);
		scripts = new List<string>();
    }
}