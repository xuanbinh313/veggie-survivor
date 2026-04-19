using UnityEngine;

public enum ItemType { Generic, Seed, Tool, Crop }
public enum ToolType { None, Hammer, WateringCan, Hoe, Sickle }

[CreateAssetMenu(fileName = "New Item Data", menuName = "Farming/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Core Information")]
    public string id; 
    public string itemName;
    [TextArea] public string description;
    public Sprite icon; 

    [Header("Item Configuration")]
    public ItemType itemType = ItemType.Generic;
    public ToolType toolType = ToolType.None; // Chỉ áp dụng nếu ItemType = Tool
    public int maxStack = 20;

    [Header("Visual Prefabs")]
    [Tooltip("Model 3D khi vứt trên mặt đất")]
    public GameObject worldPrefab;
    
    [Tooltip("Model 3D cầm trên tay nhân vật")]
    public GameObject holdPrefab;

    [Header("Farming Linking")]
    [Tooltip("Dữ liệu hạt giống - Chỉ kéo vào khi ItemType = Seed")]
    public PlantData plantData; 
}
