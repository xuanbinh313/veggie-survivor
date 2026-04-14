using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public Image[] slotIcons; // Kéo 6 cái Image (Icon) con vào đây
    public TMP_Text[] slotTexts; // Kéo 6 cái Text (Số lượng) con vào đây

    void Awake() { Instance = this; }

    public void RefreshRender()
    {
        var slots = InventoryManager.Instance.slots;
        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (i < slots.Count && slots[i].item != null)
            {
                slotIcons[i].sprite = slots[i].item.icon; // Cập nhật Icon từ ItemData
                slotIcons[i].enabled = true; // Hiện icon lên
                slotTexts[i].text = slots[i].count > 1 ? slots[i].count.ToString() : "";
            }
            else
            {
                slotIcons[i].enabled = false; // Ô trống thì ẩn icon
                slotTexts[i].text = "";
            }
        }
    }
}