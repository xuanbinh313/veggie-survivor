using UnityEngine;

[RequireComponent(typeof(Collider))] // Đảm bảo luôn có Collider để Raycast hoặc Trigger phát hiện
public class ItemObject : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("Thông tin vật phẩm này mang theo")]
    public ItemData itemData; 
    
    [Header("Visual Effects")]
    [SerializeField] private bool rotateOverTime = true;
    [SerializeField] private float rotationSpeed = 50f;

    private void Update()
    {
        // Hiệu ứng xoay vòng tạo cảm giác game nhập vai (RPG style)
        if (rotateOverTime)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
        }
    }
}
