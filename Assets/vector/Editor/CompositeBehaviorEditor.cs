using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CompositeBehavior))]
public class CompositeBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompositeBehavior cb = (CompositeBehavior) target;

        Rect r = EditorGUILayout.BeginHorizontal();
        r.height = EditorGUIUtility.singleLineHeight;

        if (cb.behavior == null || cb.behavior.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
            r = EditorGUILayout.BeginHorizontal();
            r.height = EditorGUIUtility.singleLineHeight;
        }
        else
        {
            r.x = 30f;
            r.width = EditorGUIUtility.currentViewWidth - 95;
            EditorGUI.LabelField(r, "Behaviors");
            r.x = EditorGUIUtility.currentViewWidth - 65;
            r.width = 60f;
            EditorGUI.LabelField(r, "Weights");
            r.y += EditorGUIUtility.singleLineHeight * 1.2f;

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behavior.Length; i++)
            {
                r.x = 5f;
                r.width = 20f;
                EditorGUI.LabelField(r, i.ToString());
                r.x = 30f;
                r.width = EditorGUIUtility.currentViewWidth - 95;
                cb.behavior[i] = (FlockBehavior) EditorGUI.ObjectField(r, cb.behavior[i], typeof(FlockBehavior), false);
                r.x = EditorGUIUtility.currentViewWidth - 65;
                r.width = 60f;
                cb.weights[i] = EditorGUI.FloatField(r, cb.weights[i]);
                r.y += EditorGUIUtility.singleLineHeight * 1.1f;
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }

                EditorGUILayout.EndHorizontal();
                r.x = 5f;
                r.width = EditorGUIUtility.currentViewWidth - 10f;
                r.y += EditorGUIUtility.singleLineHeight * 0.5f;
                if (GUI.Button(r, "Add Behavior"))
                {
                    AddBehavior(cb);
                    EditorUtility.SetDirty(cb);
                }
                
                r.y += EditorGUIUtility.singleLineHeight * 1.5f;
                if (cb.behavior != null && cb.behavior.Length > 0)
                {
                    if (GUI.Button(r, "Remove Behavior"))
                    {
                        RemoveBehavior(cb);
                        EditorUtility.SetDirty(cb);
                    }
                }
            
        
    }

    void AddBehavior(CompositeBehavior cb)
    {
        int oldCount = (cb.behavior != null) ? cb.behavior.Length : 0;
        FlockBehavior[] newBehavoirs = new FlockBehavior[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehavoirs[i] = cb.behavior[i];
            newWeights[i] = cb.weights[i];
        }

        newWeights[oldCount] = 1f;
        cb.behavior = newBehavoirs;
        cb.weights = newWeights;
    }
    
    void RemoveBehavior(CompositeBehavior cb)
    {
        int oldCount = cb.behavior.Length;
        if (oldCount == 1)
        {
            cb.behavior = null;
            cb.weights = null;
            return;
        }
        FlockBehavior[] newBehavoirs = new FlockBehavior[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehavoirs[i] = cb.behavior[i];
            newWeights[i] = cb.weights[i];
        }

        cb.behavior = newBehavoirs;
        cb.weights = newWeights;
    }
}
