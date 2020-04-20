namespace KSM.UI.Example
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class Listener : MonoBehaviour
    {
        public ButtonDropdown buttonDropdown;

        List<string> initials = new List<string>() { "K", "S", "M" };

        private void Start()
        {
            AddOptionsButtonDropdown();
        }

        private void AddOptionsButtonDropdown()
        {
            buttonDropdown.ClearOptions();

            List<(string, UnityAction)> options = new List<(string, UnityAction)>();

            int cnt = initials.Count;

            for(int i = 0; i < cnt; ++i)
            {
                int j = i;
                options.Add((initials[i], () => Debug.Log("my " + j + "th initial is " + initials[j])));
            }

            buttonDropdown.AddOptions(options);
        }
    }
}