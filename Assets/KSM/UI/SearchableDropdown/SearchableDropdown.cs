namespace KSM.UI
{
    using KSM.Utility;
    using MyBox;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    public class SearchableDropdown : MonoBehaviour
    {
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private InputField searchIpf;
        [SerializeField, ReadOnly] private Transform contentTf; 

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/KSM/Dropdown - Searchable")]
        static void CreateButtonDropdown()
        {
            Transform activeTransform = Selection.activeTransform;

            EditorApplication.ExecuteMenuItem("GameObject/UI/Dropdown");

            GameObject dropdownObj = Selection.activeGameObject;
            dropdownObj.SetUniqueName("SearchableDropdown");
            dropdownObj.transform.SetParent(activeTransform == null ? dropdownObj.transform.root : activeTransform, false);

            dropdownObj.AddComponent<SearchableDropdown>(); // this call Reset()
        }

        private void Reset()
        {
            {
                dropdown = GetComponent<Dropdown>();
                if (dropdown == null)
                {
                    Debug.LogError(nameof(MultiSelectDropdown) + " need Dropdown component in the same GameObject!");
                    DestroyImmediate(this);
                }
            }

            EditorApplication.ExecuteMenuItem("GameObject/UI/Input Field");
            RectTransform inputFieldTf = Selection.activeGameObject.GetComponent<RectTransform>();
            inputFieldTf.localPosition = Vector3.zero;
            inputFieldTf.SetParent(dropdown.template, false);
            inputFieldTf.anchorMin = new Vector2(0, 1);
            inputFieldTf.anchorMax = new Vector2(1, 1);
            inputFieldTf.pivot = new Vector2(0.5f, 1);
            inputFieldTf.sizeDelta = new Vector2(0, 30);

            ScrollRect templateScroll = dropdown.template.GetComponent<ScrollRect>();
            templateScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            PushDownTo30(templateScroll.viewport);
            PushDownTo30(templateScroll.verticalScrollbar.GetComponent<RectTransform>());

            void PushDownTo30(RectTransform rectTf)
            {
                rectTf.localPosition = new Vector3(rectTf.localPosition.x, rectTf.localPosition.y - 30, rectTf.localPosition.z);
                rectTf.sizeDelta = new Vector2(rectTf.sizeDelta.x, rectTf.sizeDelta.y - 30);
            }

            VerticalLayoutGroup layoutGroup = templateScroll.content.GetOrAddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlWidth = true;
            ContentSizeFitter sizeFitter = templateScroll.content.GetOrAddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            dropdown.ClearOptions();
            string[] options = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            dropdown.options.AddRange(options.ConvertAll(str => new Dropdown.OptionData(str)));

            searchIpf = inputFieldTf.GetComponent<InputField>();
            searchIpf.onValueChanged.AddPersistentListener(Search);
        }
#endif
        private void Search(string findText)
        {
            if(contentTf == null)
            {
                contentTf = GetComponentInChildren<ScrollRect>().content;
            }

            int itemCount = contentTf.childCount;

            for(int i = 0; i < dropdown.options.Count; ++i)
            {
                contentTf.GetChild(i + 1).gameObject.SetActive(dropdown.options[i].text.Contains(findText));
            }
        }
    }
}