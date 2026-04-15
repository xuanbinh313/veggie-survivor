using UnityEngine;

public class TreeController : MonoBehaviour
{
    public int health = 6;
    public GameObject woodPrefab; // Kéo Prefab khúc gỗ (có gắn ItemObject) vào đây
    public int woodDropCount = 3;

    public void TakeHit()
    {
        health--;
        // Thêm hiệu ứng rung cây hoặc hạt (Particles) ở đây
        Debug.Log("Cây bị chặt! Máu còn: " + health);

        if (health <= 0)
        {
            FellTree();
        }
    }

    void FellTree()
    {
        Debug.Log("Cây đã đổ!");
        
        // Tạo ra 3 stack gỗ rớt quanh vị trí cây
        for (int i = 0; i < woodDropCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            Instantiate(woodPrefab, transform.position + randomOffset, Quaternion.identity);
        }

        // Thông báo cho ForestManager rằng cây này đã mất (nếu cần)
        Destroy(gameObject);
    }
}