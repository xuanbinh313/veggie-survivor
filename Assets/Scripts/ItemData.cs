using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string id; // ID duy nhất để phân biệt
    public string itemName;
    public Sprite icon; // Hình ảnh hiển thị trong ô Inventory
    public int maxStack = 20;
    public GameObject worldPrefab; // Model 3D để vứt ra thế giới (nếu cần)

}
