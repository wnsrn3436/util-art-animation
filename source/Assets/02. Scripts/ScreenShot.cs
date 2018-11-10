using UnityEngine;
public class ScreenShot : MonoBehaviour
{
    //보여지는 카메라.
    public UnityEngine.UI.RawImage img;
    public GameObject pic;
    public ArtApply art;
    public ArtApply art2;


    public void StartShot()
    {
        //Capture((int)(Screen.width / 2.0f), (int)(Screen.height / 2.0f), DrawRect._cropArea);
        Capture(567,627, DrawRect._cropArea);
        art.source = img.texture as Texture2D;
        art2.source = img.texture as Texture2D;
    }

    public Texture2D Capture(int width, int height, Rect rect)
    {


        Camera mainCamera = Camera.main;

        //캡처할 텍스쳐 생성
        RenderTexture subScreen = new RenderTexture(mainCamera.pixelWidth, mainCamera.pixelHeight, 24);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);

        //카메라 설정 기억
        float beforeAspect = mainCamera.aspect;
        RenderTexture beforeTexture = mainCamera.targetTexture;
        RenderTexture beforeActive = RenderTexture.active;

        //카메라 랜더링 대상을 텍스처로 변경
        mainCamera.targetTexture = subScreen;

        //캡처해서 텍스처로 저장
        RenderTexture.active = subScreen;
        mainCamera.Render();
        texture.ReadPixels(rect, 0, 0);

        Debug.Log(rect);
        texture.Apply();

        //카메라를 원래 설정으로 돌림
        mainCamera.targetTexture = beforeTexture;
        mainCamera.aspect = beforeAspect;
        RenderTexture.active = beforeActive;
        img.texture = texture;

        Debug.Log(texture.width + "  " + texture.height);
        /*
        string path;
#if UNITY_EDITOR
        path = Application.dataPath + "/a.jpg";
#else
       path = Application.persistentDataPath + "/a.jpg";
#endif

        System.IO.File.WriteAllBytes(path, texture.EncodeToJPG());*/
        return texture;
    }
}