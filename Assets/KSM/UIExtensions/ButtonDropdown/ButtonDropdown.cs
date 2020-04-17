using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace KSM.UI
{
    public class ButtonDropdown : Dropdown
    {
        #region Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Extensions/Dropdown - Button")]
        static void CreateButtonDropdown()
        {
            EditorApplication.ExecuteMenuItem("GameObject/UI/Dropdown");

            GameObject dropdownObj = Selection.activeGameObject;
            dropdownObj.name = "ButtonDropdown"; // TODO : implement naming function for duplicated obj.

            DestroyImmediate(dropdownObj.GetComponent<Dropdown>());

            // disable toggle, add child object with Button.
            Toggle item = dropdownObj.transform.GetComponentInChildren<Toggle>(true);
            item.enabled = false;

            // remove children
            while (item.transform.childCount > 0)
                DestroyImmediate(item.transform.GetChild(0).gameObject);

            // Add Button object
            GameObject button = new GameObject("Button", typeof(Image), typeof(Button));
            button.transform.SetParent(item.transform, false);

            // Adjust sizeDelta
            RectTransform tempRtf = button.GetComponent<RectTransform>();
            tempRtf.sizeDelta = new Vector2(160, 20);

            // Add Text object
            GameObject itemTextObj = new GameObject("Item Label", typeof(Text));
            itemTextObj.transform.SetParent(button.transform, false);

            // Adjust sizeDelta, anchors
            tempRtf = itemTextObj.GetComponent<RectTransform>();
            tempRtf.anchorMin = Vector2.zero;
            tempRtf.anchorMax = Vector2.one;
            tempRtf.sizeDelta = Vector2.zero;

            Text itemText = itemTextObj.GetComponent<Text>();
            itemText.color = Color.black;
            itemText.alignment = TextAnchor.MiddleCenter;

            // Add ButtonDropdown component
            ButtonDropdown buttonDropdown = dropdownObj.AddComponent<ButtonDropdown>();

            // Add Options
            buttonDropdown.options.Add(new Dropdown.OptionData("Option A"));
            buttonDropdown.options.Add(new Dropdown.OptionData("Option B"));
            buttonDropdown.options.Add(new Dropdown.OptionData("Option C"));

            // Assign Components to buttonDropdown Component
            buttonDropdown.captionText = dropdownObj.transform.GetChild(0).GetComponent<Text>();
            buttonDropdown.template = dropdownObj.transform.GetChild(2).GetComponent<RectTransform>();
            buttonDropdown.itemText = itemText;
        }
#endif
        #endregion

        public List<Button> buttons = new List<Button>();

        public UnityEvent OnFinishCreateList = new UnityEvent();

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            base.DestroyDropdownList(dropdownList);
            buttons.Clear();
        }

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            var itemIns = base.CreateItem(itemTemplate);
            Button itemButton = itemIns.GetComponentInChildren<Button>();
            buttons.Add(itemButton);
            return itemIns;
        }

        protected override GameObject CreateBlocker(Canvas rootCanvas)
        {
            // Blocker is instantiated after the list.
            OnFinishCreateList?.Invoke();
            return base.CreateBlocker(rootCanvas);
        }
    }
}