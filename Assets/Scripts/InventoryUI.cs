using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; // Cần thiết cho UI Toolkit

[RequireComponent(typeof(UIDocument))]
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    private VisualElement root;
    private List<VisualElement> slotsUI = new();

    void Awake() 
    { 
        Instance = this; 
    }

    void OnEnable()
    {
        // 1. Lấy root từ UIDocument gắn trên cùng GameObject
        root = GetComponent<UIDocument>().rootVisualElement;

        // 2. Tìm tất cả các Element có class là "slot" đã tạo ở file USS
        // Giả sử bạn đặt class cho 6 ô đồ là "slot"
        slotsUI = root.Query<VisualElement>(className: "slot").ToList();
    }

    public void RefreshRender()
    {
        var dataSlots = InventoryManager.Instance.slots;

        for (int i = 0; i < slotsUI.Count; i++)
        {
            // Truy vấn icon và text bên trong mỗi ô slot
            // Trong UI Toolkit, Image thường là một VisualElement với background-image
            VisualElement icon = slotsUI[i].Q<VisualElement>("Icon"); 
            Label countLabel = slotsUI[i].Q<Label>("CountLabel");

            if (i < dataSlots.Count && dataSlots[i].item != null)
            {
                // Hiển thị Icon (Sử dụng style.backgroundImage)
                Debug.Log("Slots UI: " + dataSlots[i].item.icon);
                icon.style.backgroundImage = new StyleBackground(dataSlots[i].item.icon);
                icon.style.display = DisplayStyle.Flex; // Hiện icon
                // Hiển thị số lượng
                countLabel.text = dataSlots[i].count > 1 ? dataSlots[i].count.ToString() : "";
            }
            else
            {
                // Ô trống thì ẩn
                icon.style.display = DisplayStyle.None;
                countLabel.text = "";
            }
        }
    }
}