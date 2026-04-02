using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Animator animator;
    private GameObject currentItem; // Vật phẩm đang đứng gần

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Nhấn E để Nhặt (Pickup)
        if (Keyboard.current.eKey.wasPressedThisFrame && currentItem != null && currentItem.CompareTag("Seed"))
        {
            animator.SetTrigger("isPicking");
            Debug.Log("Picked up: " + currentItem.name);
            Destroy(currentItem, 0.5f); // Xóa vật phẩm sau khi nhặt
        }

        // 2. Nhấn F để Trồng (Planting)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            // Kiểm tra nếu đang đứng ở ô đất (mã hóa logic địa hình ở đây)
            animator.SetTrigger("isPlanting");
            Debug.Log("Planted a seed!");
            // Gọi hàm tạo cây tại đây
        }
    }

    // Kiểm tra khi đi vào vùng của vật phẩm
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed") )
        {
            currentItem = other.gameObject;
            // Bạn có thể hiện chữ Tiếng Anh ở đây: "Press E to Pick up Seeds"
            Debug.Log("Standing near: " + currentItem.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItem) currentItem = null;
    }
}