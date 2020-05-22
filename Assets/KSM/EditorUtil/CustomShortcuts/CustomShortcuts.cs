namespace KSM.Utility
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    public class EditorUtility
    {
        [MenuItem("GameObject/Active All Parent Object %q")]
        private static void ActiveAllParentObj()
        {
            Transform currentTf = Selection.activeGameObject.transform;
            Transform rootTf = currentTf.root;

            while (currentTf != rootTf)
            {
                SetActiveAndDirty(currentTf.gameObject);
                currentTf = currentTf.parent;
            }
            SetActiveAndDirty(currentTf.gameObject);
        }

        private static void SetDirtyGameObejct(GameObject go)
        {
            UnityEditor.EditorUtility.SetDirty(go);
        }

        private static void SetActiveAndDirty(GameObject go)
        {
            if (!go.activeInHierarchy)
            {
                Undo.RecordObject(go, "Undo " + go.name);
                go.SetActive(true);
                //SetDirtyGameObejct(go);
            }
        }
    }
#endif
}