using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Data", menuName = "Farming/Plant Data")]
public class PlantData : ScriptableObject
{
    [Header("Core Info")]
    public string plantName;

    [Header("Growth Timers")]
    [Tooltip("Thời gian (giây) từ Hạt -> Cây lớn")]
    public float timeToGrowTree = 10f;  
    
    [Tooltip("Thời gian (giây) từ Cây lớn -> Ra quả")]
    public float timeToBearFruit = 15f; 

    [Header("Visual Progression")]
    [Tooltip("Model lúc vừa gieo hạt")]
    public GameObject seedPrefab;     
    
    [Tooltip("Model khi đang là cây non")]
    public GameObject treePrefab1;     
    
    [Tooltip("Model khi ra quả (Sẵn sàng thu hoạch)")]
    public GameObject fruitPrefab;    
    
    [Tooltip("Model gốc cây sau khi đã thu hoạch xong")]
    public GameObject harvestedPrefab; 

    [Header("Harvest Rewards")]
    [Tooltip("Vật phẩm sẽ rớt ra hoặc tự chui vào túi khi thu hoạch")]
    public ItemData itemToGive;           
    
    [Tooltip("Số lượng vật phẩm rơi ra")]
    public int yieldAmount = 1;
}
