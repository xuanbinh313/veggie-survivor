using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ForestSpawner : MonoBehaviour
{
    public Tilemap forestZoneTilemap; // Kéo Tilemap ForestZone vào đây
    public TileBase markerTile;       // Kéo MarkerTile vào đây
    public GameObject treePrefab;     // Prefab cây (3D) của bạn
    public LayerMask treeLayer; // Khai báo thêm biến này và gán trong Inspector
    [Header("Cấu hình mọc cây")]
    public int maxTreesToSpawn = 50;  // Số lượng cây tối đa muốn mọc
    public float spawnInterval = 5f; // Cứ mỗi 5 giây mọc thêm 1 cây (nếu muốn tự mọc)

    private List<Vector3Int> validSpawnPositions = new List<Vector3Int>();

    void Start()
    {
        // 1. Tìm tất cả các ô có Marker
        FindValidPositions();

        // 2. Mọc một lượng cây ban đầu
        for (int i = 0; i < maxTreesToSpawn / 2; i++)
        {
            SpawnTreeRandomly();
        }

        // 3. (Tùy chọn) Chạy hàm tự mọc cây theo thời gian
        InvokeRepeating("SpawnTreeRandomly", spawnInterval, spawnInterval);
        Debug.Log("Spawned " + validSpawnPositions.Count + " trees");
        // 4. Ẩn Tilemap Marker đi để người chơi không thấy các ô hồng
        forestZoneTilemap.GetComponent<TilemapRenderer>().enabled = false;
    }

    void FindValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt bounds = forestZoneTilemap.cellBounds;
        Debug.Log("Bounds: " + bounds);
        // Quét toàn bộ Tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Debug.Log("Checking position: " + pos);
                if (forestZoneTilemap.GetTile(pos) == markerTile)
                {
                    validSpawnPositions.Add(pos);
                }
            }
        }
    }

    public void SpawnTreeRandomly()
{
    if (validSpawnPositions.Count == 0) return;

    int currentTrees = GameObject.FindGameObjectsWithTag("Tree").Length;
    if (currentTrees >= maxTreesToSpawn) return;

    Vector3Int randomCell = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];
    Vector3 spawnWorldPos = forestZoneTilemap.GetCellCenterWorld(randomCell);
    
    // Ép tọa độ để khớp với mặt đất 3D (Thường là X và Z)
    // Nếu map của bạn nằm ngang, hãy thử: spawnWorldPos.z = spawnWorldPos.y; spawnWorldPos.y = 0;

    if (!IsPositionOccupied(spawnWorldPos))
    {
        GameObject tree = Instantiate(treePrefab, spawnWorldPos, Quaternion.identity);
        tree.name = "Tree_Spawned_" + Time.time; // Dễ tìm trong Hierarchy
        
        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
        tree.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
        
        Debug.Log("<color=green>Cây đã tạo tại: </color>" + spawnWorldPos);
    }
    else 
    {
        Debug.Log("<color=yellow>Vị trí bị chặn bởi vật thể khác!</color>");
    }
}

    bool IsPositionOccupied(Vector3 pos)
    {
        // Dùng Physics để check xem có cây nào ở đó chưa
        return Physics.CheckSphere(pos, 0.5f,treeLayer);
    }
}