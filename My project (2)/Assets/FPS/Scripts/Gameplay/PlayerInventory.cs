

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Unity.FPS.Gameplay
{
    public class PlayerInventory : MonoBehaviour
    {
        [Serializable]
        public class Slot
        {
            public InventoryItem Item;
            public int Quantity;
        }

        [Range(1, 24)] public int SlotCount = 24;
        [SerializeField] List<Slot> m_Slots = new List<Slot>();

        readonly List<Image> m_Icons = new List<Image>();
        readonly List<TextMeshProUGUI> m_Labels = new List<TextMeshProUGUI>();
        readonly List<Button> m_Buttons = new List<Button>();
        GameObject m_UiRoot;
        GameObject m_Panel;

        void Awake()
        {
            while (m_Slots.Count < SlotCount)
                m_Slots.Add(new Slot());

            if (m_Slots.Count > SlotCount)
                m_Slots.RemoveRange(SlotCount, m_Slots.Count - SlotCount);

            BuildUi();
        }

        void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.tabKey.wasPressedThisFrame)
                SetInventoryOpen(!m_Panel.activeSelf);

            if (m_Panel.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
                SetInventoryOpen(false);
        }

        public bool AddItem(InventoryItem item, int amount = 1)
        {
            if (item == null || amount <= 0)
                return false;

            int maxStack = Mathf.Max(1, item.MaxStack);
            int availableSpace = 0;
            foreach (Slot slot in m_Slots)
            {
                if (slot.Item == item)
                    availableSpace += maxStack - slot.Quantity;
                else if (slot.Item == null)
                    availableSpace += maxStack;
            }

            if (availableSpace < amount)
                return false;

            int remaining = amount;
            foreach (Slot slot in m_Slots)
            {
                if (slot.Item != item || slot.Quantity >= maxStack)
                    continue;

                int added = Mathf.Min(remaining, maxStack - slot.Quantity);
                slot.Quantity += added;
                remaining -= added;
                if (remaining == 0)
                    break;
            }

            foreach (Slot slot in m_Slots)
            {
                if (slot.Item != null)
                    continue;

                int added = Mathf.Min(remaining, maxStack);
                slot.Item = item;
                slot.Quantity = added;
                remaining -= added;
                if (remaining == 0)
                    break;
            }

            RefreshUi();
            return true;
        }

        public bool RemoveItem(InventoryItem item, int amount = 1)
        {
            if (item == null || amount <= 0 || CountItem(item) < amount)
                return false;

            int remaining = amount;
            foreach (Slot slot in m_Slots)
            {
                if (slot.Item != item)
                    continue;

                int removed = Mathf.Min(remaining, slot.Quantity);
                slot.Quantity -= removed;
                remaining -= removed;
                if (slot.Quantity == 0)
                    slot.Item = null;
                if (remaining == 0)
                    break;
            }

            RefreshUi();
            return true;
        }

        public int CountItem(InventoryItem item)
        {
            int count = 0;
            foreach (Slot slot in m_Slots)
            {
                if (slot.Item == item)
                    count += slot.Quantity;
            }

            return count;
        }

        void UseSlot(int index)
        {
            Slot slot = m_Slots[index];
            if (slot.Item == null || !slot.Item.Use(this))
                return;

            slot.Quantity--;
            if (slot.Quantity == 0)
                slot.Item = null;

            RefreshUi();
        }

        void BuildUi()
        {
            if (EventSystem.current == null)
            {
                GameObject eventSystem = new GameObject("Inventory EventSystem", typeof(EventSystem),
                    typeof(InputSystemUIInputModule));
                eventSystem.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
            }

            m_UiRoot = new GameObject("Inventory UI", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = m_UiRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;

            CanvasScaler scaler = m_UiRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            RectTransform panel = CreateRect("Inventory Panel", m_UiRoot.transform);
            panel.sizeDelta = new Vector2(760f, 470f);
            Image panelImage = panel.gameObject.AddComponent<Image>();
            panelImage.color = new Color32(19, 25, 31, 245);
            m_Panel = panel.gameObject;

            RectTransform titleRect = CreateRect("Title", panel);
            titleRect.anchorMin = new Vector2(0f, 1f);
            titleRect.anchorMax = new Vector2(1f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.offsetMin = new Vector2(28f, -62f);
            titleRect.offsetMax = new Vector2(-28f, -22f);
            TextMeshProUGUI title = titleRect.gameObject.AddComponent<TextMeshProUGUI>();
            title.text = "INVENTORY    |    TAB / ESC TO CLOSE";
            title.fontSize = 22f;
            title.color = new Color32(229, 238, 241, 255);
            title.alignment = TextAlignmentOptions.Left;

            RectTransform gridRect = CreateRect("Slots", panel);
            gridRect.anchorMin = new Vector2(0.5f, 0f);
            gridRect.anchorMax = new Vector2(0.5f, 0f);
            gridRect.pivot = new Vector2(0.5f, 0f);
            gridRect.sizeDelta = new Vector2(652f, 332f);
            gridRect.anchoredPosition = new Vector2(0f, 28f);
            GridLayoutGroup grid = gridRect.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(102f, 76f);
            grid.spacing = new Vector2(8f, 8f);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 6;

            for (int i = 0; i < SlotCount; i++)
                CreateSlot(gridRect, i);

            m_Panel.SetActive(false);
            RefreshUi();
        }

        void CreateSlot(Transform parent, int index)
        {
            GameObject slotObject = new GameObject("Slot " + (index + 1), typeof(RectTransform), typeof(Image),
                typeof(Button));
            slotObject.transform.SetParent(parent, false);

            Image background = slotObject.GetComponent<Image>();
            background.color = new Color32(39, 49, 56, 255);
            Button button = slotObject.GetComponent<Button>();
            button.targetGraphic = background;
            int slotIndex = index;
            button.onClick.AddListener(() => UseSlot(slotIndex));

            RectTransform iconRect = CreateRect("Icon", slotObject.transform);
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.sizeDelta = new Vector2(38f, 38f);
            iconRect.anchoredPosition = new Vector2(0f, 7f);
            Image icon = iconRect.gameObject.AddComponent<Image>();
            icon.preserveAspect = true;
            icon.raycastTarget = false;

            RectTransform labelRect = CreateRect("Name", slotObject.transform);
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 0f);
            labelRect.pivot = new Vector2(0.5f, 0f);
            labelRect.offsetMin = new Vector2(4f, 3f);
            labelRect.offsetMax = new Vector2(-4f, 22f);
            TextMeshProUGUI label = labelRect.gameObject.AddComponent<TextMeshProUGUI>();
            label.fontSize = 11f;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;
            label.enableWordWrapping = false;
            label.raycastTarget = false;

            m_Icons.Add(icon);
            m_Labels.Add(label);
            m_Buttons.Add(button);
        }

        void RefreshUi()
        {
            if (m_Panel == null)
                return;

            for (int i = 0; i < m_Slots.Count; i++)
            {
                Slot slot = m_Slots[i];
                m_Icons[i].sprite = slot.Item != null ? slot.Item.Icon : null;
                m_Icons[i].enabled = slot.Item != null && slot.Item.Icon != null;
                m_Labels[i].text = slot.Item != null
                    ? slot.Item.DisplayName + " x" + slot.Quantity
                    : string.Empty;
                m_Buttons[i].interactable = slot.Item != null;
            }
        }

        void SetInventoryOpen(bool isOpen)
        {
            m_Panel.SetActive(isOpen);
            Cursor.visible = isOpen;
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        static RectTransform CreateRect(string objectName, Transform parent)
        {
            GameObject child = new GameObject(objectName, typeof(RectTransform));
            child.transform.SetParent(parent, false);
            return child.GetComponent<RectTransform>();
        }

        void OnDestroy()
        {
            if (m_UiRoot != null)
                Destroy(m_UiRoot);
        }
    }
}