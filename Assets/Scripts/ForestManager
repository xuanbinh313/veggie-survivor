using UnityEngine;
using System.Collections;

public class ForestManager : MonoBehaviour
{
    public GameObject treePrefab;
    public int maxTrees = 20;
    public float spawnRate = 10f; // 10 giây mọc 1 cây
    public Vector2 xRange = new Vector2(-20, 20); // Phạm vi bản đồ X
    public Vector2 zRange = new Vector2(-20, 20); // Phạm vi bản đồ Z

    void Start()
    {
        StartCoroutine(SpawnTrees());
    }

    IEnumerator SpawnTrees()
    {
        while (true)
        {
            // Đếm số lượng cây hiện có trong Scene
            int currentTreeCount = GameObject.FindGameObjectsWithTag("Tree").Length;

            if (currentTreeCount < maxTrees)
            {
                SpawnSingleTree();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnSingleTree()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(xRange.x, xRange.y),
            0, // Giả sử mặt đất ở y = 0
            Random.Range(zRange.x, zRange.y)
        );

        // Kiểm tra xem vị trí có bị trùng với vật thể khác không (tùy chọn)
        Instantiate(treePrefab, spawnPos, Quaternion.identity);
    }
}