using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class PartialScreenshotCaptureAndUpload : MonoBehaviour
{
    public Camera captureCamera; // 캡처할 카메라
    public static PartialScreenshotCaptureAndUpload instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        captureCamera = Camera.main;
    }

    void Update()
    {

    }

    public IEnumerator CaptureAndUpload(int index)
    {
        int captureWidth = Screen.width;
        int captureHeight = Screen.height;

        // RenderTexture 설정
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        captureCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        // 카메라에 현재 프레임 렌더링
        captureCamera.Render();
        RenderTexture.active = rt;

        // 캡처 영역의 왼쪽 상단에서 읽어오기
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        screenShot.Apply();

        // PNG로 인코딩
        byte[] bytes = screenShot.EncodeToPNG();

        // RenderTexture와 Texture2D 해제
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        yield return null;
        // 이미지 업로드 코루틴 호출
        StartCoroutine(UploadImage(bytes, index));
    }

    IEnumerator UploadImage(byte[] imageData, int index)
    {
        WWWForm form = new WWWForm();

        // 서버 URL 설정
        string serverUrl = "deleted";
        form.AddField("id", 3); // 일단 빌드할 때 id값만 변경해서 프로토타입 만드는걸로 할거임 0은 승현, 1 민주, 2 길쌍, 3준호
        form.AddField("tour_num", index); // 값 추가 필요

        // HTTP POST 요청 생성
        form.AddBinaryData("file", imageData, "PartialScreenshot123.png", "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Image uploaded successfully!");
            }
            else
            {
                Debug.LogError("Error uploading image: " + www.error);
            }
        }
    }
}
