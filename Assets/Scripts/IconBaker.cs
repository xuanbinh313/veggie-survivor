using UnityEngine;
using System.IO;

public class IconBaker : MonoBehaviour
{
    public Camera studioCam;
    public string fileName = "WeaponIcon";
    public int resWidth = 512;
    public int resHeight = 512;

    [ContextMenu("Capture Icon")] // Nhấn chuột phải vào Script trong Inspector để chạy
    public void CaptureIcon()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        studioCam.targetTexture = rt;
        
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        studioCam.Render();
        
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        
        studioCam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        
        byte[] bytes = screenShot.EncodeToPNG();
        string path = Application.dataPath + "/" + fileName + ".png";
        File.WriteAllBytes(path, bytes);
        
        Debug.Log("Đã lưu Icon tại: " + path);
    }
}