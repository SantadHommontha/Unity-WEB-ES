using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
public class WebGLScreenshotSaver : MonoBehaviour
{
    // 1. นำเข้าฟังก์ชัน JavaScript สำหรับการดาวน์โหลด (ต้องสร้างไฟล์ .jslib ในขั้นตอนถัดไป)

    [SerializeField] private GameObject backBTN;
    [SerializeField] private GameObject screenShotBTN;

    [DllImport("__Internal")]
    private static extern void DownloadFile(string name, string base64Data);

    // ฟังก์ชันสาธารณะที่เรียกใช้เมื่อผู้ใช้กดปุ่มเซฟ
    public void CaptureAndSaveScreenshot(string filename)
    {
        // เริ่ม Coroutine เพื่อรอการจบเฟรมและถ่ายภาพ
        StartCoroutine(CaptureScreenshotCoroutine(filename));
    }

    private IEnumerator CaptureScreenshotCoroutine(string filename)
    {
        // 2. รอจนกระทั่งจบเฟรมปัจจุบัน (เพื่อรับประกันว่าภาพหน้าจอถูกเรนเดอร์สมบูรณ์แล้ว)
        backBTN.SetActive(false);
        screenShotBTN.SetActive(false);
        yield return new WaitForEndOfFrame();

        // 3. สร้าง Texture2D เพื่อเก็บภาพหน้าจอทั้งหมด
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // 4. แปลงภาพเป็น Base64 String ในรูปแบบ PNG
        byte[] bytes = ss.EncodeToPNG();
        Destroy(ss); // ทำลาย Texture2D เพื่อเคลียร์หน่วยความจำ

        string base64 = System.Convert.ToBase64String(bytes);
        string fileNameWithExtension = filename + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // 5. เรียกใช้ฟังก์ชัน JavaScript เพื่อเริ่มการดาวน์โหลด
        DownloadFile(fileNameWithExtension, base64);

        backBTN.SetActive(true);
        screenShotBTN.SetActive(true);
    }
}
