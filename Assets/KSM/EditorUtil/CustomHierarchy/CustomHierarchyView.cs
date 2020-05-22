namespace KSM.Editor.Internal
{
#if UNITY_EDITOR

    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [InitializeOnLoad]
    public class CustomHierarchyView
    {
        private static bool isInit = false;

        private static GameObject hierarchyHelperObj;

        static CustomHierarchyView()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            EditorSceneManager.sceneOpened += FindHierarchyHelperObj;
        }

        static void FindHierarchyHelperObj(Scene scene, OpenSceneMode mode)
        {
            if (isInit) return;
            if (hierarchyHelperObj == null)
            {
                hierarchyHelperObj = HierarchyHelper.Instance?.gameObject;
                if (hierarchyHelperObj == null)
                {
                    Debug.LogWarning("To use hide option, you should add HierarchyHelper Object!");
                    return;
                }
            }

            hierarchyHelperObj?.transform.SetAsLastSibling();

            isInit = true;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (hierarchyHelperObj == null)
            {
                hierarchyHelperObj = HierarchyHelper.Instance?.gameObject;
                if (hierarchyHelperObj == null) return;
            }

            if (obj == hierarchyHelperObj) return;

            if(obj != null)
            {
                bool value = (obj.hideFlags & HideFlags.HideInHierarchy) == HideFlags.HideInHierarchy;

                Rect r = new Rect(selectionRect);

                r.x += r.width - 65;

                EditorGUI.BeginChangeCheck();

                var toggleValue = GUI.Toggle(r, value, "Hide");

                if(EditorGUI.EndChangeCheck())
                {
                    if (toggleValue)
                    {
                        obj.hideFlags |= HideFlags.HideInHierarchy;
                        HierarchyHelper.Instance.hiddenObjects.Add(obj);
                        Selection.activeObject = hierarchyHelperObj;
                        try
                        {
                            EditorApplication.DirtyHierarchyWindowSorting();
                        }
                        catch
                        {
                            // Ignore 
                        }
                    }
                }
            }
        }
    }

#endif
}