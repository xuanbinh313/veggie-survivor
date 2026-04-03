using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Animator animator;
    public Transform handSlot; // Kéo object HandSlot vào đây trong Inspector
    private GameObject currentItem; // Vật phẩm đang đứng gần
    private GameObject itemInHand;      // Vật phẩm đang cầm trên tay

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Nhấn E để Nhặt (Pickup)
        if (Keyboard.current.eKey.wasPressedThisFrame && currentItem != null && currentItem.CompareTag("Seed"))
        {
            animator.SetTrigger("isPicking");
            Debug.Log("Picked up: " + currentItem.name);
            // Destroy(currentItem, 0.5f); // Xóa vật phẩm sau khi nhặt
            // Gọi hàm nhặt sau một khoảng trễ ngắn (để khớp với lúc tay cúi xuống)
            Invoke("PickUpItem", 0.5f);
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
    void PickUpItem()
    {
        if (currentItem == null) return;

        itemInHand = currentItem;
        // --- THÊM DÒNG NÀY ĐỂ TẮT XOAY ---
        ItemSpin spinScript = itemInHand.GetComponent<ItemSpin>();
        if (spinScript != null)
        {
            spinScript.enabled = false;
        }
        // --------------------------------
        // Vô hiệu hóa vật lý để không bị lỗi khi di chuyển
        if (itemInHand.GetComponent<Collider>())
            itemInHand.GetComponent<Collider>().enabled = false;

        if (itemInHand.GetComponent<Rigidbody>())
            itemInHand.GetComponent<Rigidbody>().isKinematic = true;

        // Gắn vào tay
        itemInHand.transform.SetParent(handSlot);

        // Đưa về vị trí 0 (khớp với HandSlot đã chỉnh ở Bước 1)
        itemInHand.transform.localPosition = Vector3.zero;
        itemInHand.transform.localRotation = Quaternion.identity;

        currentItem = null;
    }

    void DropOrPlantItem()
    {
        // Logic trồng cây: Cho vật phẩm biến mất hoặc đặt xuống đất
        itemInHand.transform.SetParent(null); // Bỏ vật phẩm ra khỏi tay
        // Ví dụ: Đặt vị trí xuống chân nhân vật
        itemInHand.transform.position = transform.position + transform.forward;

        if (itemInHand.GetComponent<Collider>()) itemInHand.GetComponent<Collider>().enabled = true;

        itemInHand = null;
    }
    // Kiểm tra khi đi vào vùng của vật phẩm
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed"))
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