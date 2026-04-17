using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Farming/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public GameObject seedPrefab;     // Hình ảnh/Model hạt trên đất
    public GameObject treePrefab1;     // Hình ảnh/Model cây đang lớn
    public GameObject treePrefab2;     // Hình ảnh/Model cây đang lớn (phiên bản khác, nếu cần)
    public GameObject fruitPrefab;    // Hình ảnh/Model cây đã có quả
    public GameObject cropPrefab;     // Hình ảnh/Model cây đã thu hoạch (nếu khác với treePrefab)
    public GameObject harvestedPrefab; // Hình ảnh/Model cây sau khi thu hoạch (nếu cần)
    public ItemData itemToGive;           // Vật phẩm nhận được khi thu hoạch (nếu có hệ thống Inventory)
    public float timeToGrowTree = 10f;  // Thời gian (giây) từ Hạt -> Cây
    public float timeToBearFruit = 15f; // Thời gian (giây) từ Cây -> Ra quả

}
