using UnityEngine;
public enum PlotState { Empty, Seeded, Growing, Fruiting }
public class CropPlot : MonoBehaviour
{
    public PlotState currentState = PlotState.Empty;
    public PlantData currentPlant;

    private float timer = 0f;
    private GameObject currentModel; // Model đang hiển thị trên ô đất

    void Update()
    {
        if (currentState == PlotState.Seeded)
        {
            timer += Time.deltaTime;
            if (timer >= currentPlant.timeToGrowTree)
            {
                GrowToTree();
            }
        }
        else if (currentState == PlotState.Growing)
        {
            timer += Time.deltaTime;
            if (timer >= currentPlant.timeToBearFruit)
            {
                GrowToFruit();
            }
        }
    }

    // Hàm trồng hạt
    public void PlantSeed(PlantData seedData)
    {
        if (currentState != PlotState.Empty) return;

        currentPlant = seedData;
        currentState = PlotState.Seeded;
        timer = 0f;

        // Đổi hình ảnh/mesh ô đất sang trạng thái có hạt
        UpdateModel(currentPlant.seedPrefab);
    }

    // Lớn thành cây
    private void GrowToTree()
    {
        currentState = PlotState.Growing;
        timer = 0f;
        UpdateModel(currentPlant.treePrefab1);
    }

    // Ra quả
    private void GrowToFruit()
    {
        currentState = PlotState.Fruiting;
        UpdateModel(currentPlant.fruitPrefab);
    }

    // Hàm thu hoạch
    public void Harvest()
    {
        if (currentState == PlotState.Fruiting)
        {
            Debug.Log("Đã thu hoạch: " + currentPlant.plantName);
            // Thêm logic add vật phẩm vào Inventory của user ở đây
            
            // Reset lại ô đất (hoặc lùi về trạng thái Growing tùy game)
            ResetPlot();
        }
    }

    private void UpdateModel(GameObject newModelPrefab)
    {
        if (currentModel != null) Destroy(currentModel);
        
        if (newModelPrefab != null)
        {
            // Sinh ra model cây/hạt tại vị trí ô đất
            currentModel = Instantiate(newModelPrefab, transform.position, Quaternion.identity);
            currentModel.transform.SetParent(this.transform);
        }
    }

    private void ResetPlot()
    {
        currentState = PlotState.Empty;
        currentPlant = null;
        timer = 0f;
        UpdateModel(null);
    }
}