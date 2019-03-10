using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using Newtonsoft.Json;
using System.Text;

public struct State {
	public int b;
	public Vector3 position;
	public Vector3 scale;
	public Vector3 rotation;
	public Color color;

	public State(int b1)
    {
        b = b1;
		position = new Vector3(0,0,0);
		scale = new Vector3(1,1,1);
		rotation = new Vector3(0,0,0);
		color = new Color(1,1,1);
    }
}
public class Manipulate : MonoBehaviour
{

	static State cloneExistingState(){
		int b = int.Parse(Selection.activeGameObject.name);
		State newState = new State(b);
		newState.position = Selection.activeGameObject.transform.position;
		newState.scale = Selection.activeGameObject.transform.localScale;
		newState.rotation = Selection.activeGameObject.transform.eulerAngles;
		Renderer r = Selection.activeGameObject.transform.GetChild(0).GetComponent<Renderer>();
		newState.color = r.sharedMaterial.color;
		return newState;
	}

	[MenuItem ("Anyland/Create/Create Object - 1")]
	static void MenuCreateObject1()
	{
		State newState = new State(1);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 2")]
	static void MenuLoadCreateObject2()
	{
		State newState = new State(2);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 3")]
	static void MenuLoadCreateObject3()
	{
		State newState = new State(3);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 4")]
	static void MenuLoadCreateObject4()
	{
		State newState = new State(4);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 5")]
	static void MenuLoadCreateObject5()
	{
		State newState = new State(5);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 6")]
	static void MenuLoadCreateObject6()
	{
		State newState = new State(6);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Create/Create Object - 7")]
	static void MenuLoadCreateObject7()
	{
		State newState = new State(7);
		Debug.Log(newState.b);
		Manipulate.CreateObject(newState);
	}

    [MenuItem ("Anyland/Duplicate/Duplicate Selected Object")]
	static void MenuDuplicate()
	{
		State newState = cloneExistingState();
		Manipulate.CreateObject(newState);
	}

	[MenuItem ("Anyland/Generate/All Objects - grid")]
	static void MenuGenerateGridAll()
	{
		State newState = new State(1);
		newState.scale = new Vector3(0.2f,0.2f,0.2f);
		int b = 1;
		for (int x = 0; x < 20; x++){
			for (int y = 0; y < 15; y++){
				newState.position.x = 0f + x;
				newState.position.y = 0f + y;
				newState.b = b;
				Manipulate.CreateObject(newState);
				b++;
				if (b>17 && b<25) {b=25;}
				if (b>89 && b<144) {b=144;}
				if (b==206) {b=207;}
				if (b>251) {b=1;}
			}
		}
	}

     [MenuItem ("Anyland/Generate/Object Selected - spin")]
	static void MenuGenerateObjectSelected()
	{
		State newState = cloneExistingState();
		
		for (int i = 0; i < 360; i=i+30)
		{
			newState.rotation.x = i;
			Manipulate.CreateObject(newState);
		}
	}

    [MenuItem ("Anyland/Generate/Object Selected - circle")]
	static void MenuGenerateCircle()
	{
        State newState = cloneExistingState();
        float theta = 0f;

        float theta_scale = 0.02f;        //Set lower to add more points
        int size = 100; //Total number of points in circle
        float radius = 1f;

        for (int i = 0; i < size; i++)
        {
            theta += (4.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            newState.position.x = newState.position.x + x;
            newState.position.y = newState.position.y + y;
            Manipulate.CreateObject(newState);
        }
	}

	[MenuItem ("Anyland/Generate/Object Selected - grid")]
	static void MenuGenerateGrid()
	{
		State originalState = cloneExistingState();
		State newState = originalState;

		for (int x = 0; x < 10; x++){
			for (int y = 0; y < 10; y++){
				newState.position.x = originalState.position.x  + x;
				newState.position.y = originalState.position.y + y;
				Manipulate.CreateObject(newState);
			}
		}
	}

    [MenuItem ("Anyland/Generate/Object Selected - spiral")]
	static void MenuGenerateSpiral()
	{
        State newState = cloneExistingState();
        float theta = 0f;
        float theta_scale = 0.02f;        //Set lower to add more points
        int size = 200; //Total number of points in circle
        float radius = 1f;

        for (int i = 0; i < size; i++)
        {
            theta += (4.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            newState.position.x = newState.position.x + x;
            newState.position.y = newState.position.y + y;
			newState.position.z = newState.position.z + 0.1f;
            Manipulate.CreateObject(newState);
        }
	}

	static void CreateObject (State state)
	{
        GameObject objectLoaded = Resources.Load("" + state.b, typeof(GameObject)) as GameObject;
        GameObject newObject = Instantiate(objectLoaded, state.position, Quaternion.identity);
        newObject.transform.localScale = state.scale;
        newObject.transform.localEulerAngles = state.rotation;
        //newObject.transform.SetParent(parent.transform);
        newObject.name = ""+state.b;
        newObject.tag = "Part";
        Renderer rend = newObject.transform.GetChild(0).GetComponent<Renderer>();
        var tempMaterial = new Material(rend.sharedMaterial);
        tempMaterial.color = state.color;
        rend.sharedMaterial = tempMaterial;
	}
}
