using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải có dòng này để dùng hệ thống mới

public class LandController : MonoBehaviour
{
    [Header("Cấu hình các loại đất")]
    public Material grassMaterial;
    public Material sandMaterial;
    public Material waterMaterial;
    private int currentState = 0; // 0: Khô, 1: Xới, 2: Tưới
    private Renderer _renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = grassMaterial;
    }

    public void SetState()
    {
        currentState = (currentState + 1) % 3;
        _renderer.material = currentState == 0 ? grassMaterial : currentState == 1 ? sandMaterial : waterMaterial;
    }
}
