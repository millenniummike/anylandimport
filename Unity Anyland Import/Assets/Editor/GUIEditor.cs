using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Part)), CanEditMultipleObjects]
public class GUIEditor : Editor
{
    static int b = 1;
    static Vector3 max = new Vector3(5f,5f,5f);
    static Vector3 spacing = new Vector3(1f,1f,1f);
    static float radius = 0.1f;
    static float circleSize = 20;
    static float theta_scale = 0.02f;
    static Vector3 rotationOffset = new Vector3(0,0,0);
    static Vector3 scaleOffset = new Vector3(0,0,0);
    static Vector3 positionOffset = new Vector3(0.2f,0.2f,0.2f);
    static int iterations = 5;
    static float speed = 0.2f;
    static int currentState = 0;
    public override void OnInspectorGUI()
    {
        b = EditorGUILayout.IntField("B - part number:", b);
        iterations = EditorGUILayout.IntField("Iterations:", iterations);
        speed = EditorGUILayout.FloatField("Speed - forward direction:", speed);
        positionOffset = EditorGUILayout.Vector3Field("Position offset:", positionOffset);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation offset:", rotationOffset);
        scaleOffset = EditorGUILayout.Vector3Field("Scale offset:", scaleOffset);
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
            State newState = cloneExistingState();
		    CreateObject(newState,cloneExistingB());
        }
        GUILayout.EndHorizontal();

         GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Grid", GUILayout.Width(100), GUILayout.Height(20))){
        State originalState = cloneExistingState();
        State newState = originalState;
        int b = cloneExistingB();
            for (float x = 0; x < max.x; x = x + spacing.x){
                for (float y = 0; y < max.y; y = y + spacing.y){
                    for (float z = 0; z < max.z; z = z + spacing.z){
                        newState.position.x = originalState.position.x + x;
                        newState.position.y = originalState.position.y + y;
                        newState.position.z = originalState.position.z + z;
                        newState.scale.x += scaleOffset.x;
                        newState.scale.y += scaleOffset.y;
                        newState.scale.z += scaleOffset.z;
                        CreateObject(newState,b);
                    }
                }
            }
        }

        if(GUILayout.Button("Create Circle", GUILayout.Width(100), GUILayout.Height(20))){
        State newState = cloneExistingState();
        float theta = 0f;

        for (int i = 0; i < circleSize; i++)
            {
                theta += (4.0f * Mathf.PI * theta_scale);
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                newState.position.x = newState.position.x + x;
                newState.position.y = newState.position.y + y;
                CreateObject(newState,cloneExistingB());
            }
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Extrude"))
        {
        Transform t = Selection.activeGameObject.transform;
        State newState = cloneExistingState();
        for (int i = 0; i < iterations; i++)
            {
                Vector3 newPosition =  t.TransformDirection(Vector3.forward) * speed;
                newState.position.x += newPosition.x;
                newState.position.y += newPosition.y;
                newState.position.z += newPosition.z;
                
                newState.position += positionOffset;
                newState.rotation += rotationOffset;
                newState.scale.x += scaleOffset.x;
                newState.scale.y += scaleOffset.y;
                newState.scale.z += scaleOffset.z;
                GameObject go = CreateObject(newState,cloneExistingB());
                t = go.transform; newState.position = t.position; //mutate
            }
        }

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add State", GUILayout.Width(100), GUILayout.Height(20))){
            State originalState = cloneExistingState();
            Part p =Selection.activeGameObject.GetComponent<Part>();
            p.states.Add(originalState);
        }

        if(GUILayout.Button("Previous State", GUILayout.Width(100), GUILayout.Height(20))){
            currentState--;
            Part p =Selection.activeGameObject.GetComponent<Part>();
            if (currentState<0) {currentState=p.states.Count-1;}
            updateState();
        }

        if(GUILayout.Button("Next State", GUILayout.Width(100), GUILayout.Height(20))){
            currentState++;
            Part p =Selection.activeGameObject.GetComponent<Part>();
            if (currentState>=p.states.Count) {currentState=0;}
            updateState();
        }
        GUILayout.EndHorizontal();
        currentState = EditorGUILayout.IntField("Current State:", currentState);

        DrawDefaultInspector ();
    }

    void updateState(){
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

    void saveState(){
        Part p = Selection.activeGameObject.GetComponent<Part>();
        //p.states[currentState].position =  Selection.activeGameObject.transform.position;
    }

    static GameObject CreateObject (State state,int b)
	{
        //**TODO clone existing state */
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
        return newObject;
	}

    static State cloneExistingState(){
        //**TODO enhance to clone all substates */
		int b = int.Parse(Selection.activeGameObject.name);
		State newState = new State(b);
		newState.position = Selection.activeGameObject.transform.position;
		newState.scale = Selection.activeGameObject.transform.localScale;
		newState.rotation = Selection.activeGameObject.transform.eulerAngles;
		Renderer r = Selection.activeGameObject.transform.GetChild(0).GetComponent<Renderer>();
		newState.color = r.sharedMaterial.color;
		return newState;
	}

	static int cloneExistingB(){
		int b = int.Parse(Selection.activeGameObject.name);
		return b;
	}
}