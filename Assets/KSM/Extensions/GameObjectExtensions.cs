namespace KSM.Utility
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class GameObjectExtensions
    {
        public static void SetUniqueName(this GameObject go, string name)
        {
            Transform parentTf = go.transform.parent;

            int siblingCount = parentTf.childCount;

            List<string> siblingNames = new List<string>();

            for(int i = 0; i < siblingCount; ++i)
            {
                siblingNames.Add(parentTf.GetChild(i).name);
            }

            int postFix = 1;
            string originName = name;

            if(siblingNames.Contains(name))
            {
                name = originName + $" ({postFix})";
            }

            while(siblingNames.Contains(name))
            {
                ++postFix;
                name = originName + $" ({postFix})";
            }

            go.name = name;
        }
    }
}