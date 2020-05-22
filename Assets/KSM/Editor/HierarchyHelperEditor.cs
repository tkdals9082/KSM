namespace KSM.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(HierarchyHelper))]
    public class HierarchyHelperEditor : Editor
    {
        private List<GameObject> removeObjs = new List<GameObject>();

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This is Hierarchy Helper Class\nYou can reveal hidden objects.", MessageType.Info);
            
            HierarchyHelper myTarget = (HierarchyHelper)target;

            // Header
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Hide Objects", EditorStyles.boldLabel);
            foreach(var hiddenObj in myTarget.hiddenObjects)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(hiddenObj.name);
                if(GUILayout.Button("Reveal"))
                {
                    hiddenObj.hideFlags &= ~HideFlags.HideInHierarchy;
                    removeObjs.Add(hiddenObj);
                }
                EditorGUILayout.EndHorizontal();
            }

            int cnt = removeObjs.Count;

            if (cnt != 0)
            {
                for (int i = 0; i < cnt; ++i)
                    myTarget.hiddenObjects.Remove(removeObjs[i]);
                removeObjs.Clear();
            }
        }
    }
}