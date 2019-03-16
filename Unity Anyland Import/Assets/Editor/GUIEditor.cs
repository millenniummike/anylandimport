using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class GUIEditor : Editor
{
    static int b = 1;
    static Vector3 max = new Vector3(5f,5f,5f);
    static Vector3 spacing = new Vector3(1f,1f,1f);
    static float radius = 2f;
    static float circleSize = 20;
    static float theta_scale = 0.02f;
    static Vector3 rotationOffset = new Vector3(0,0,0);
    static Vector3 scaleOffset = new Vector3(0,0,0);
    static Vector3 positionOffset = new Vector3(0.2f,0.2f,0.2f);

    static Vector3 jitter = new Vector3(0.5f,0.5f,0.5f);
    static int iterations = 5;
    static float speed = 1f;
    static int currentState = 0;
    public override void OnInspectorGUI()
    {
        b = EditorGUILayout.IntField("B - part number:", b);
        iterations = EditorGUILayout.IntField("Iterations:", iterations);
        speed = EditorGUILayout.FloatField("Speed:", speed);
        positionOffset = EditorGUILayout.Vector3Field("Position offset:", positionOffset);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation offset:", rotationOffset);
        scaleOffset = EditorGUILayout.Vector3Field("Scale offset:", scaleOffset);
        jitter = EditorGUILayout.Vector3Field("Jitter:", jitter);
        max = EditorGUILayout.Vector3Field("Max grid size:", max); 
        spacing = EditorGUILayout.Vector3Field("Grid spacing:", spacing);  
        radius = EditorGUILayout.FloatField("Radius:", radius);
        circleSize = EditorGUILayout.FloatField("Points of circle:", circleSize);
        theta_scale = EditorGUILayout.FloatField("Theta scale:", theta_scale);
        
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Part", GUILayout.Width(100), GUILayout.Height(20))){
            State newState = new State(1);
            int part = b;
            CreateObject(newState,b);
        }

        if(GUILayout.Button("Duplicate Part", GUILayout.Width(100), GUILayout.Height(20))){
            CopyObject(Selection.activeGameObject);
        }
        GUILayout.EndHorizontal();

         GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Grid", GUILayout.Width(100), GUILayout.Height(20))){
            GameObject originalGo = Selection.activeGameObject;
            for (float x = 0; x < max.x; x = x + spacing.x){
                for (float y = 0; y < max.y; y = y + spacing.y){
                    for (float z = 0; z < max.z; z = z + spacing.z){
                        GameObject go = CopyObject(Selection.activeGameObject);
                        Vector3 newPos = originalGo.transform.position;
                        newPos.x += x;
                        newPos.y += y;
                        newPos.z += z;
                        Vector3 newScale = Selection.activeGameObject.transform.localScale;
                        newScale.x += scaleOffset.x;
                        newScale.y += scaleOffset.y;
                        newScale.z += scaleOffset.z;
                        go.transform.position = newPos;
                        go.transform.localScale = newScale;
                        Selection.activeGameObject = go;
                        saveState(go);
                    }
                }
            }
        }

/* 
        if(GUILayout.Button("Create Clump", GUILayout.Width(100), GUILayout.Height(20))){
        GameObject originalGo = Selection.activeGameObject;
        System.Random random = new System.Random();
        for (int c=0; c< iterations; c++){
            GameObject go = CopyObject(originalGo);
            float x = UnityEngine.Random.Range(0.0f, max.x);
            newState.position.x = x;
            float y = UnityEngine.Random.Range(0.0f, max.y);
            newState.position.y = y;
            float z = UnityEngine.Random.Range(0.0f, max.z);
            newState.position.z = z;

            float rx = UnityEngine.Random.Range(0.0f, rotationOffset.x);
            newState.rotation.x = rx;
            float ry = UnityEngine.Random.Range(0.0f, rotationOffset.y);
            newState.rotation.y = ry;
            float rz = UnityEngine.Random.Range(0.0f, rotationOffset.z);
            newState.rotation.z = rz;

            newState.scale.x += scaleOffset.x;
            newState.scale.y += scaleOffset.y;
            newState.scale.z += scaleOffset.z;
            }
        }
*/
        if(GUILayout.Button("Create Circle", GUILayout.Width(100), GUILayout.Height(20))){
        GameObject originalGo = Selection.activeGameObject;
        float theta = 0f;

        for (int i = 0; i < circleSize; i++)
            {
                GameObject go = CopyObject(originalGo);
                Vector3 newPos = originalGo.transform.position;
                theta += (4.0f * Mathf.PI * theta_scale);
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                newPos.x = newPos.x + x;
                newPos.y = newPos.y + y;
                
                Vector3 newScale = Selection.activeGameObject.transform.localScale;
                newScale.x += scaleOffset.x;
                newScale.y += scaleOffset.y;
                newScale.z += scaleOffset.z;
                go.transform.position = newPos;
                go.transform.localScale = newScale;

            }
        }

        if(GUILayout.Button("Spin", GUILayout.Width(100), GUILayout.Height(20))){
        for (int i = 0; i < iterations; i++)
            {
                GameObject originalGo = Selection.activeGameObject;
                GameObject go = CopyObject(originalGo);
                Vector3 newRotation = go.transform.localEulerAngles;
                newRotation += rotationOffset;
                go.transform.localEulerAngles = newRotation;
                Selection.activeGameObject = go;
            }
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Extrude"))
        {
        for (int i = 0; i < iterations; i++)
            {
                GameObject go = CopyObject(Selection.activeGameObject);
                Vector3 newPosition = go.transform.TransformDirection(Vector3.forward * speed * go.transform.localScale.x);
                Vector3 newRotation =  go.transform.localEulerAngles + rotationOffset;
                Vector3 newScale = Selection.activeGameObject.transform.localScale;
                newScale.x += scaleOffset.x;
                newScale.y += scaleOffset.y;
                newScale.z += scaleOffset.z;
                go.transform.localPosition += newPosition;
                go.transform.localScale = newScale;
                go.transform.localEulerAngles = newRotation;
                saveState(go);
                Selection.activeGameObject = go;
            }
        }

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add State", GUILayout.Width(100), GUILayout.Height(20))){
            Part p =Selection.activeGameObject.GetComponent<Part>();
            p.states.Add(new State());
        }

        if(GUILayout.Button("Previous State", GUILayout.Width(100), GUILayout.Height(20))){
            currentState--;
            Part p =Selection.activeGameObject.GetComponent<Part>();
            if (currentState<0) {currentState=p.states.Count-1;}
            updateCurrentObject();
        }

        if(GUILayout.Button("Next State", GUILayout.Width(100), GUILayout.Height(20))){
            currentState++;
            Part p =Selection.activeGameObject.GetComponent<Part>();
            if (currentState>=p.states.Count) {currentState=0;}
            updateCurrentObject();
        }

        if(GUILayout.Button("Save State", GUILayout.Width(100), GUILayout.Height(20))){
            saveState(Selection.activeGameObject);
        }

        GUILayout.EndHorizontal();
        currentState = EditorGUILayout.IntField("Current State:", currentState);

        if(GUILayout.Button("Undo", GUILayout.Width(100), GUILayout.Height(20))){
                // undo
                }
        DrawDefaultInspector ();
    }

    void updateCurrentObject(){
        Part p = Selection.activeGameObject.GetComponent<Part>();
        Selection.activeGameObject.transform.position = p.states[currentState].position;
        Selection.activeGameObject.transform.eulerAngles = p.states[currentState].rotation;
        Selection.activeGameObject.transform.localScale = p.states[currentState].scale;
        Selection.activeGameObject.transform.position = p.states[currentState].position;

        Renderer rend = Selection.activeGameObject.transform.GetChild(0).GetComponent<Renderer>();
		var tempMaterial = new Material(rend.sharedMaterial);
 		tempMaterial.color = p.states[currentState].color;
 		rend.sharedMaterial = tempMaterial;
    }

    static void saveState(GameObject sourceGo){
        Part p = sourceGo.GetComponent<Part>();
        State c = p.states[currentState];
        c.position =  sourceGo.transform.position;
        c.rotation = sourceGo.transform.eulerAngles;
        c.scale = sourceGo.transform.localScale;
        Renderer rend = sourceGo.transform.GetChild(0).GetComponent<Renderer>();
		var tempMaterial = rend.sharedMaterial;
 		c.color = tempMaterial.color;
        p.states[currentState] = c;
    }

    static GameObject CreateObject (State state, int b)
	{
        GameObject objectLoaded = Resources.Load("" + b, typeof(GameObject)) as GameObject;
        GameObject newObject = Instantiate(objectLoaded, state.position, Quaternion.identity);
        newObject.transform.localScale = state.scale;
        newObject.transform.localEulerAngles = state.rotation;
        //newObject.transform.SetParent(parent.transform);
        newObject.name = "" + b;
        newObject.tag = "Part";
		newObject.AddComponent<Part>();
        Part p = newObject.GetComponent<Part>();
		p.states.Add(state);
        Renderer rend = newObject.transform.GetChild(0).GetComponent<Renderer>();
        var tempMaterial = new Material(rend.sharedMaterial);
        tempMaterial.color = state.color;
        rend.sharedMaterial = tempMaterial;
        Selection.activeGameObject = newObject;
        saveState(Selection.activeGameObject);
        return newObject;
	}

    static GameObject CopyObject(GameObject originalGo){
		GameObject go = Instantiate(originalGo);
        Selection.activeGameObject = go;
        go.name = originalGo.name;
        return go;
    }

    static List<State> cloneExistingStates(){
        Part p = Selection.activeGameObject.GetComponent<Part>();
		List<State> states = p.states;

		return states;
	}

	static int cloneExistingB(){
		int b = int.Parse(Selection.activeGameObject.name);
		return b;
	}
}