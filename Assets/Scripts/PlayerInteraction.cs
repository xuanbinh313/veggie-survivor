using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Animator animator;
    public Transform handSlot; 
    public float interactionDistance = 2.0f; // Tăng lên để dễ tương tác

    private ItemObject currentItem;  // Vật phẩm đứng gần
    private ItemObject itemInHand;   // Vật phẩm đang cầm
    private LandController pendingLand;

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Nhấn E để Nhặt
        if (Keyboard.current.eKey.wasPressedThisFrame && currentItem != null)
        {
            // Xoay nhân vật về phía vật phẩm
            LookAtTarget(currentItem.transform.position);

            if (currentItem.CompareTag("Seed"))
            {
                animator.SetTrigger("isPicking"); 
                // Không dùng Invoke, hãy gọi PickUpItem thông qua Animation Event
            }
            else if (currentItem.CompareTag("Item"))
            {
                animator.SetTrigger("isPicking"); // Giả sử bạn có anim lượm đồ chung
            }
        }

        // 2. Nhấn F để Trồng
        if (Keyboard.current.fKey.wasPressedThisFrame && itemInHand != null)
        {
            animator.SetTrigger("isPlanting");
            // Logic trừ item hoặc tạo cây sẽ nằm trong Animation Event của isPlanting
        }

        // 3. Chuột trái để Cuốc đất
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleDigging();
        }
    }

    void HandleDigging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            LandController land = hit.collider.GetComponent<LandController>();
            if (land != null && Vector3.Distance(transform.position, hit.point) <= interactionDistance)
            {
                pendingLand = land;
                LookAtTarget(hit.point);
                animator.SetTrigger("isDigging");
            }
        }
    }

    void LookAtTarget(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    // --- HÀM GỌI TỪ ANIMATION EVENT ---

    public void OnPickUpEffect() // Gán vào Anim nhặt đồ
    {
        if (currentItem == null) return;

        // Nếu là Seed thì cầm trên tay, nếu là Item thì cho vào Inventory
        if (currentItem.CompareTag("Seed"))
        {
            itemInHand = currentItem;
            
            // Tắt vật lý và script xoay
            if (itemInHand.TryGetComponent<Collider>(out Collider col)) col.enabled = false;
            if (itemInHand.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
            
            itemInHand.transform.SetParent(handSlot);
            itemInHand.transform.localPosition = Vector3.zero;
            itemInHand.transform.localRotation = Quaternion.identity;
        }
        else
        {
            InventoryManager.Instance.AddItem(currentItem.itemData, 1);
            Destroy(currentItem.gameObject);
        }
        currentItem = null;
    }

    public void OnDigEffect() // Gán vào Anim cuốc đất
    {
        if (pendingLand != null)
        {
            pendingLand.SetState();
            pendingLand = null;
        }
    }

    // --- XỬ LÝ VA CHẠM ---

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem object đi vào có script ItemObject không
        ItemObject io = other.GetComponent<ItemObject>();
        if (io != null)
        {
            currentItem = io;
            Debug.Log("Đang đứng gần: " + currentItem.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentItem != null && other.gameObject == currentItem.gameObject)
        {
            currentItem = null;
        }
    }
}