using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LerpVisualizer))]
public class LerpVisualizerEditor : Editor
{
    private SerializedProperty FollowTasks;
    private SerializedProperty DisplayFollowTasks;
    private bool DisplayFollowTasksOpen = true;
    private SerializedProperty UseFPSLimit;
    private SerializedProperty UseRandomFPS;
    private SerializedProperty ObjectPrefab;
    private SerializedProperty Speed;
    private SerializedProperty LerpSpeed;
    private SerializedProperty FPSLimit;
    private SerializedProperty RandomFPSMin;
    private SerializedProperty RandomFPSMax;
    private SerializedProperty RandomizationTime;
    private Dictionary<string, GUIContent> GUIContentDict = new();
    private void OnEnable()
    {
        // Initialization of all serialized variables, make a GUIContent and add it to the dictionary
        FollowTasks = serializedObject.FindProperty("followTasks");
        GUIContent _followTasks = new GUIContent("Follow Tasks", "Optional. Leave blank to auto-assign, or assign custom follow gameobject here.");
        GUIContentDict.Add("FollowTasks", _followTasks);

        DisplayFollowTasks = serializedObject.FindProperty("DisplayFollowTasks");
        
        ObjectPrefab = serializedObject.FindProperty("FollowObjectPrefab");
        GUIContent _objectPrefab = new GUIContent("Follow Object Prefab", "Assign GameObject that will be used as the prefab for auto-assigned follow objects");
        GUIContentDict.Add("ObjectPrefab", _objectPrefab);

        Speed = serializedObject.FindProperty("Speed");
        GUIContent _speed = new GUIContent("Speed", "Movement speed of the main GameObject");
        GUIContentDict.Add("Speed", _speed);

        LerpSpeed = serializedObject.FindProperty("LerpSpeed");
        GUIContent _lerpSpeed = new GUIContent("Lerp Speed", "Movement speed for all follow objects");
        GUIContentDict.Add("LerpSpeed", _lerpSpeed);
         
        UseFPSLimit = serializedObject.FindProperty("UseFPSLimit");
        GUIContent _useFPSLimit = new GUIContent("Use FPS Limit","Simulate locked FPS at a given amount");
        GUIContentDict.Add("UseFPSLimit", _useFPSLimit);

        FPSLimit = serializedObject.FindProperty("FPSLimit");
        GUIContent _FPSLimit = new GUIContent("","");
        GUIContentDict.Add("FPSLimit", _FPSLimit);

        UseRandomFPS = serializedObject.FindProperty("UseRandomFPS");
        GUIContent _useRandomFPS = new GUIContent();
        GUIContentDict.Add("UseRandomFPS", _useRandomFPS);

        RandomFPSMin = serializedObject.FindProperty("RandomFPSMin");
        GUIContent _randomFPSMin = new GUIContent();
        GUIContentDict.Add("RandomFPSMin", _randomFPSMin);

        RandomFPSMax = serializedObject.FindProperty("RandomFPSMax");
        GUIContent _randomFPSMax = new GUIContent();
        GUIContentDict.Add("RandomFPSMax", _randomFPSMax);

        RandomizationTime = serializedObject.FindProperty("RandomizationTime");
        GUIContent _randomizationTime = new GUIContent();
        GUIContentDict.Add("RandomizationTime", _randomizationTime);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LerpVisualizer visualizer = (LerpVisualizer)target;
        
        // EditorStyles.objectField.normal.textColor = Color.white;
        EditorGUILayout.PropertyField(ObjectPrefab, GUIContentDict["ObjectPrefab"]);
        
        DisplayFollowTasksOpen = EditorGUILayout.Foldout(DisplayFollowTasksOpen,"FollowTasks");
        if(DisplayFollowTasksOpen) {

            for (int i = 0; i < FollowTasks.arraySize; i++) {
                SerializedProperty task = FollowTasks.GetArrayElementAtIndex(i);
                SerializedProperty _object = task.FindPropertyRelative("_Object");
                
                SerializedProperty color = task.FindPropertyRelative("_Color");
                if(color.colorValue == null) color.colorValue = Color.white;
                SerializedProperty interpolation = task.FindPropertyRelative("Interpolation");
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (_object != null) {
                    // EditorStyles.objectField.normal.textColor = color.colorValue;
                    EditorGUILayout.PropertyField(_object, new GUIContent($"Follower object"));
                }
               
                EditorGUILayout.PropertyField(color, new GUIContent($"Color of the object and associated UI"));
                EditorGUILayout.PropertyField(interpolation, new GUIContent("Interpolation Type"));
                // Add dropdown or some indicator for functions (delegates aren't directly serialized)
                EditorGUILayout.LabelField($"Follow Task: {Enum.GetName(typeof(InterpolationType), interpolation.intValue)}", EditorStyles.miniLabel);
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add New Follow Task")) {
                FollowTasks.InsertArrayElementAtIndex(FollowTasks.arraySize);
            }

            if (GUILayout.Button("Remove Last Follow Task")) {
                if (FollowTasks.arraySize > 0)
                    FollowTasks.DeleteArrayElementAtIndex(FollowTasks.arraySize - 1);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        EditorGUILayout.PropertyField(Speed, GUIContentDict["Speed"]);
        
        EditorGUILayout.PropertyField(LerpSpeed, GUIContentDict["LerpSpeed"]);
        
        GUIContent _useFPSInfo = new GUIContent("Toggle FPS Limit", "Toggles simulated FPS limit for all interpolations");
        UseFPSLimit.boolValue = GUILayout.Toggle(UseFPSLimit.boolValue, _useFPSInfo);
        
        if (UseFPSLimit.boolValue)
        {
            UseRandomFPS.boolValue = false;
            FPSLimit.intValue = EditorGUILayout.IntSlider("FPS Limit", FPSLimit.intValue, 1, 200);
        }

        GUIContent _useRandomFPSInfo = new GUIContent("Toggle random FPS", "Toggles simulated FPS with random intervals");
        UseRandomFPS.boolValue = GUILayout.Toggle(UseRandomFPS.boolValue, _useRandomFPSInfo);
        
        if (UseRandomFPS.boolValue)
        {
            UseFPSLimit.boolValue = false;
            RandomFPSMin.intValue = EditorGUILayout.IntSlider("Random FPS minimum value", RandomFPSMin.intValue, 1, 200);
            RandomFPSMax.intValue = EditorGUILayout.IntSlider("Random FPS maximum value", RandomFPSMax.intValue, 1, 200);

            //EditorGUILayout.PropertyField(RandomizationTime, GUIContentDict["RandomizationTime"]);

        }
        serializedObject.ApplyModifiedProperties();
    }
}
