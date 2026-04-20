using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask raycastLayerMask = ~0; // Mặc định là hit mọi layer

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform handSlot;

    // Caches
    private Camera _mainCamera;

    // State
    private ItemObject _currentItem;   
    private ItemObject _itemInHand;    
    private LandController _pendingLand; 
    private CropPlot _pendingCropPlot; // Hỗ trợ lưu trữ cho animation event (nếu cần)

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Đảm bảo thiết bị đầu vào khả dụng
        if (Keyboard.current == null) return;
        HandleInput();
    }

    private void HandleInput()
    {
        // 1. Nhặt đồ (Phím E)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPickUp();
            return; // Ưu tiên xử lý 1 action mỗi frame để tránh conflict
        }

        // 2. Trồng / Thu hoạch (Phím F)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryInteractWithCrop();
            return;
        }

        // 3. Sử dụng Tool - Cuốc đất (Phím Space)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryUseTool();
        }
    }

    #region Core Interactions

    private void TryPickUp()
    {
        if (_currentItem != null)
        {
            LookAtTarget(_currentItem.transform.position);
            animator.SetTrigger("isPicking");
        }
    }

    private void TryInteractWithCrop()
    {
        if (!TryGetRaycastHit(out RaycastHit hit)) return;
        Debug.Log("Hit: " + hit.collider.name);

        // Ưu tiên tìm component CropPlot
        if (!hit.collider.TryGetComponent<CropPlot>(out var cropPlot))
        {
            if (hit.collider.TryGetComponent<LandController>(out var landController)) cropPlot = landController.GetComponent<CropPlot>();
        }

        if (cropPlot == null || !IsWithinDistance(hit.point)) return;

        if (cropPlot.currentState == PlotState.Fruiting)
        {
            Debug.Log("Harvesting crop");
            LookAtTarget(hit.point);
            animator.SetTrigger("isPicking"); 
            cropPlot.Harvest();
        }
        else if (cropPlot.currentState == PlotState.Empty && HasSeedInHand())
        {
            Debug.Log("Planting crop");
            _pendingLand = cropPlot.GetComponent<LandController>();
            _pendingCropPlot = cropPlot;
            
            LookAtTarget(hit.point);
            animator.SetTrigger("isPlanting");

            // Có thể dời logic này vào OnPlantEffect() (Animation Event) để sync với hoạt ảnh
            PerformPlanting(); 
        }
    }

    private void TryUseTool()
    {
        if (!HasToolInHand()) return;

        if (!TryGetRaycastHit(out RaycastHit hit)) return;

        LandController land = hit.collider.GetComponent<LandController>();
        if (land != null && IsWithinDistance(hit.point))
        {
            _pendingLand = land;
            LookAtTarget(hit.point);
            animator.SetTrigger("isDigging");
        }
    }

    #endregion

    #region Animation Events

    // Gắn vào Animation Event cho Frame nhặt đồ
    public void OnPickUpEffect() 
    {
        Debug.Log("Picking up item" + _currentItem);
        if (_currentItem != null)
        {
            HandleItemPickUp(_currentItem);
            _currentItem = null;
        }
    }

    // Gắn vào Animation Event cho Frame cuốc đất chạm đất
    public void OnDigEffect() 
    {
        if (_pendingLand != null)
        {
            _pendingLand.SetState();
            _pendingLand = null;
        }
    }

    // Tùy chọn: Gắn vào Animation Event lúc tay rắc hạt
    public void OnPlantEffect()
    {
        // Khi hoạt ảnh hoàn thiện, uncomment hàm bên dưới và gọi từ anim event.
        // PerformPlanting();
    }

    #endregion

    #region Action Logic

    private void HandleItemPickUp(ItemObject itemToPickUp)
    {
        if (itemToPickUp.itemData == null) return;

        var type = itemToPickUp.itemData.itemType;
        Debug.Log("Equipping item: " + itemToPickUp);
        if (type == ItemType.Tool || type == ItemType.Seed)
        {
            EquipItem(itemToPickUp);
            if (type == ItemType.Tool) InventoryManager.Instance.AddItem(itemToPickUp.itemData, 1); // Giữ lại công cụ trong inventory để sử dụng tiếp
        }
        else
        {
            // InventoryManager.Instance.AddItem(itemToPickUp.itemData, 1);
            Destroy(itemToPickUp.gameObject);
        }
    }

    private void EquipItem(ItemObject newItem)
    {
        if (_itemInHand != null)
        {
            // Trong thực tế sẽ cất vào balo hoặc Instantiate prefab tương ứng ra đất
            Destroy(_itemInHand.gameObject);
        }

        _itemInHand = newItem;
        Transform itemTransform = _itemInHand.transform;
        
        itemTransform.SetParent(handSlot);
        itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        // Vô hiệu hoá vật lý trên đồ vật đang cầm trên tay
        if (_itemInHand.TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (_itemInHand.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
    }

    private void PerformPlanting()
    {
        if (_pendingCropPlot != null && _itemInHand != null) // double-check an toàn
        {
            _pendingCropPlot.PlantSeed(_itemInHand.itemData.plantData);
            Destroy(_itemInHand.gameObject);
            
            _itemInHand = null;
            _pendingCropPlot = null;
        }
    }

    #endregion

    #region Utility Methods

    private bool TryGetRaycastHit(out RaycastHit hit)
    {
        // Phóng tia từ vị trí player về phía trước thay vì theo hướng chuột
        Vector3 origin = transform.position + Vector3.up * 0.5f; 
        
        // Hướng bắn tia (Dốc nhẹ xuống để dễ chạm các obj nằm sát mặt đất nếu cần)
        Vector3 direction = (transform.forward - Vector3.up * 0.2f).normalized;
        
        return Physics.Raycast(origin, direction, out hit, interactionDistance, raycastLayerMask);
    }

    private bool IsWithinDistance(Vector3 targetPoint)
    {
        return Vector3.Distance(transform.position, targetPoint) <= interactionDistance;
    }

    private bool HasToolInHand()
    {
        return _itemInHand != null && 
               _itemInHand.itemData != null && 
               _itemInHand.itemData.itemType == ItemType.Tool;
    }

    private bool HasSeedInHand()
    {
        return _itemInHand != null && 
               _itemInHand.itemData != null && 
               _itemInHand.itemData.itemType == ItemType.Seed;
    }

    private void LookAtTarget(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;
        
        // Tránh lỗi LookRotation zero vector và hạn chế xoay khi mục tiêu đã quá gần
        if (direction.sqrMagnitude > 0.001f) 
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    #endregion

    #region Physics Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemObject>(out var itemObj))
        {
            if (itemObj != _itemInHand)
            {
                _currentItem = itemObj;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentItem != null && other.gameObject == _currentItem.gameObject)
        {
            _currentItem = null;
        }
    }

    #endregion
}