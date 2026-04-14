using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    [System.Serializable]
    public class InventorySlot {
        public ItemData item;
        public int count;
    }

    public List<InventorySlot> slots = new List<InventorySlot>(6); // 6 ô đồ

    void Awake() { Instance = this; }

    public bool AddItem(ItemData data, int amount)
    {
        // 1. Tìm xem item này đã có trong túi chưa (để cộng dồn stack)
        foreach (var slot in slots) {
            if (slot.item == data && slot.count < data.maxStack) {
                slot.count += amount;
                UpdateUI();
                return true;
            }
        }

        // 2. Nếu chưa có, tìm ô trống (item == null)
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].item == null) {
                slots[i].item = data;
                slots[i].count = amount;
                UpdateUI();
                return true;
            }
        }
        return false; // Túi đầy
    }

    void UpdateUI() {
        // Gọi đến Script hiển thị UI (Bước 4)
        InventoryUI.Instance.RefreshRender();
    }
}