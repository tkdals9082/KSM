namespace KSM.UI
{
    using MyBox;
    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class EnumDropdown : MonoBehaviour
    {
        [Tooltip("Enum name must contain its namespace too!")]
        public string enumClassName; // ex. KSM.UI.Example.Test

        [ReadOnly] public string selectedEnumName;
        [ReadOnly] public int selectedEnumValue;

        private Dropdown dropdown;

        private Type enumType = null;
        private string[] names = null;
        private int[] values = null;

#if UNITY_EDITOR
        private void Reset()
        {
            dropdown = GetComponent<Dropdown>();
            if (dropdown == null)
            {
                Debug.LogError(nameof(EnumDropdown) + " need Dropdown component in the same GameObject!");
                DestroyImmediate(this);
            }
        }
#endif

        private void Awake()
        {
            if (dropdown == null)
            {
                dropdown = GetComponent<Dropdown>();
                if (dropdown == null)
                {
                    Debug.LogError(nameof(EnumDropdown) + " need Dropdown component in the same GameObject!");
                    Destroy(this);
                    return;
                }
            }

            if(!InitEnum()) return;

            SetDropdownOptions();

            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private bool InitEnum()
        {
            enumType = Type.GetType(enumClassName);

            if (enumType == null)
            {
                Debug.LogError($"Cannot find {enumClassName}! Check whether you insert its namespace");
                return false;
            }

            names = Enum.GetNames(enumType);
            values = Enum.GetValues(enumType).Cast<int>().ToArray();

            return true;
        }

        private void SetDropdownOptions()
        {
            dropdown.ClearOptions();
            
            // foreach is not a harm for performance when the enumerable is Array. [Tested]
            foreach(var name in names)
            {
                dropdown.options.Add(new Dropdown.OptionData(name));
            }
        }

        private void OnDropdownValueChanged(int idx)
        {
            selectedEnumName = names[idx];
            selectedEnumValue = values[idx];
        }
    }
}