using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData; // Tham chiếu đến ItemData ScriptableObject
    void Update()
    {
        // Xoay vật phẩm quanh trục Y với tốc độ 50 độ mỗi giây
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }
}
