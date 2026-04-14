using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData; // Tham chiếu đến ItemData ScriptableObject
    // Khi Player chạm vào vật phẩm này, sẽ gọi hàm này để thêm vào kho đồ và xóa khỏi thế giới
    // public void OnTriggerEnter(Collider other)
    // {
    //     // Kiểm tra nếu va chạm với Player
    //     if (other.CompareTag("Player"))
    //     {
    //         // Hiển thị thông tin vật phẩm (có thể là UI hoặc Debug)
    //         Debug.Log("Đã nhặt được: " + itemData.itemName);
    //         bool isPickedUp = InventoryManager.Instance.AddItem(itemData, 1); // Thêm vật phẩm vào kho đồ của Player
    //         if (isPickedUp)
    //         {
    //             // Nếu đã thêm thành công vào kho đồ, xóa vật phẩm khỏi thế giới
    //             // Thêm logic để thêm vật phẩm vào kho đồ của Player ở đây
    //             Destroy(gameObject); // Xóa vật phẩm sau khi nhặt
    //         }
    //     }
    // }
    void Update()
    {
        // Xoay vật phẩm quanh trục Y với tốc độ 50 độ mỗi giây
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }
}
