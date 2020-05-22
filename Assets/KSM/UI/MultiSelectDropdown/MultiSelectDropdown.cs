namespace KSM.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [System.Serializable] public class UnityEvent_Int : UnityEvent<int> { }

    /// <summary>
    /// Allow multiple select for dropdown.
    /// </summary>
    public class MultiSelectDropdown : MonoBehaviour, IPointerClickHandler, ISubmitHandler
    {
        [Header("Dropdown UI - Steal dropdown's one when Awake()")]
        [SerializeField] private Text captionText;

        [Header("Variables")]
        [SerializeField] private int _value;
        public int value
        {
            get => _value;
            set
            {
                _value = value;
                onValueChanged?.Invoke(value);
            }
        }

        [MyBox.ConditionalField(nameof(allowAllSwitchOff), inverse:true), SerializeField] private int defaultValue = 0;

        [SerializeField] private string emptyLabelText = "NONE";

        [Header("Settings")]
        public bool allowAllSwitchOff = true;

        [Header("Events")]
        [SerializeField] private UnityEvent_Int onValueChanged = new UnityEvent_Int();
        public UnityEvent_Int OnDropdownClosed = new UnityEvent_Int();

        private Dropdown dropdown;

#if UNITY_EDITOR
        private void Reset()
        {
            dropdown = GetComponent<Dropdown>();
            if (dropdown == null)
            {
                Debug.LogError(nameof(MultiSelectDropdown) + " need Dropdown component in the same GameObject!");
                DestroyImmediate(this);
            }
            
            captionText = dropdown.captionText;

            dropdown.captionText = null;
        }
#endif

        private void Awake()
        {
            if(dropdown == null)
            {
                dropdown = GetComponent<Dropdown>();
                if(dropdown == null)
                {
                    Debug.LogError(nameof(MultiSelectDropdown) + " need Dropdown component in the same GameObject!");
                    Destroy(this);
                    return;
                }
            }

            captionText = dropdown.captionText;
            dropdown.captionText = null;

            onValueChanged.AddListener(v => SetCaptionText(v));
            value = defaultValue;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Invoke(nameof(InitDropdown), Time.deltaTime);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Invoke(nameof(InitDropdown), Time.deltaTime);
        }

        private void InitDropdown()
        {
            Toggle[] toggles = transform.GetChild(3).GetComponentsInChildren<Toggle>();

            int len = toggles.Length;

            for(int i = 0; i < len; ++i)
            {
                // j is one hot encoding of i (i th index is on)
                int j = 1 << (i);

                int _i = i; // assign new memory for maintain the value of i in onValueChanged callback

                // Remove toggle onValueChanged listeners
                toggles[i].onValueChanged.RemoveAllListeners();

                // Add Listener to onValueChanged, which change the "value" in this component not dropdown's.
                toggles[i].onValueChanged.AddListener((isOn) =>
                {
                    if(isOn)
                    {
                        value |= j;
                    }
                    else
                    {
                        if (!allowAllSwitchOff && (value & ~j) == 0)
                        {
                            // remain the toggle isOn
                            toggles[_i].isOn = true;
                        }
                        else
                        {
                            value &= ~j;
                        }
                    }
                });

                // turn on toggle w.r.t. value
                toggles[i].isOn = (value & j) != 0;
            }
        }

        private void SetCaptionText(int value)
        {
            string text = "";

            if(value == 0)
            {
                text = emptyLabelText;
            }
            else
            {
                int len = dropdown.options.Count;

                List<string> selectedLabel = new List<string>();

                for(int i = 0; i < len; ++i)
                {
                    if((value & (1 << i)) != 0)
                    {
                        selectedLabel.Add(dropdown.options[i].text);
                    }
                }

                text = string.Join(" & ", selectedLabel);
            }

            captionText.text = text;
        }
    }
}