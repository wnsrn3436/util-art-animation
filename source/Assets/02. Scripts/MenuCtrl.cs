using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCtrl : MonoBehaviour
{
    public GameObject clickMenu;
    public GameObject menuPanel;
    public GameObject infoPanel;
    public GameObject leftImages;
    public GameObject centerImages;
    public GameObject lightPanel;
    public GameObject pic;
    public GameObject captureMenu;
    public GameObject captureImg;
    public GameObject artBone;
    public GameObject artBone2;
    public GameObject ArtAnimationCtrl;
    public GameObject ArtAnimationCtrl2;

    public Text text;

    public ArtApply art;
    public ArtApply art2;
    public Dropdown dropdown;
    public GameObject lineRender;
    public ScreenShot screenshot;
    public Slider progress;

    List<GameObject> imageOne = new List<GameObject>();
    List<GameObject> imageTwo = new List<GameObject>();

    List<GameObject> centerimageList = new List<GameObject>();
    List<Sprite> sprites = new List<Sprite>();

    GameObject infoExit;
    GameObject infoPrev;
    GameObject infoNext;

    Color color;
    Renderer render;

    int length;
    public static int count = 0;
    bool menuing = false;
    bool infoing = false;
    bool lighting = false;
    bool isCapture = false;

    //ARCamera position
    public GameObject cam;
    public static Vector3 camPosition;

    private float timer;
    private bool starting = false;
    private AndroidUtils androidUtils;
    [SerializeField]
    private GameObject setMenu1, setMenu2;

    public void OnClickStartRecord()
    {
        timer = 0;
        starting = true;
        setMenu1.SetActive(false);
        setMenu2.SetActive(false);
        androidUtils.PrepareRecorder();
        StartCoroutine(DelayCallRecord());
    }
    private IEnumerator DelayCallRecord()
    {
        yield return new WaitForSeconds(0.1f);
        androidUtils.StartRecording();
    }
    public void OnClickStopRecord()
    {
        timer = 0;
        starting = false;
        setMenu1.SetActive(true);
        setMenu2.SetActive(true);
        androidUtils.StopRecording();
        StartCoroutine(DelaySaveVideo());
    }
    private IEnumerator DelaySaveVideo()
    {
        yield return new WaitForSeconds(1);
        androidUtils.SaveVideoToGallery();
    }

    private void Start()
    {
        // Switch to 1920 x 1080 full-screen
        Screen.SetResolution(1920, 1080, true);

        for (int i = 0; i < DropdownCtrl.lengthIndex; i++)
        {
            length = leftImages.transform.GetChild(i).transform.childCount;
            for (int j = 0; j < length; j++)
            {
                if (i == 0)
                {
                    imageOne.Add(leftImages.transform.GetChild(i).transform.GetChild(j).gameObject);
                }
                else if (i == 1)
                {
                    imageTwo.Add(leftImages.transform.GetChild(i).transform.GetChild(j).gameObject);
                }
            }
        }

        infoExit = infoPanel.transform.GetChild(3).gameObject;
        infoNext = infoPanel.transform.GetChild(4).gameObject;
        infoPrev = infoPanel.transform.GetChild(5).gameObject;

        if (!AndroidUtils.IsPermitted(AndroidPermission.ACCESS_FINE_LOCATION))//test request permission
            AndroidUtils.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION);
        androidUtils = FindObjectOfType<AndroidUtils>();

        render = pic.GetComponent<Renderer>();
    }

    private void Update()
    {
        //text.text = count.ToString();
        if (starting)
            timer += Time.deltaTime;
        if (timer > 10.0f)
            OnClickStopRecord();
        if (Input.GetKeyDown(KeyCode.Escape) && starting)
            OnClickStopRecord();

        camPosition = cam.transform.position;
    }

    public void MenuClickBtn()
    {
        if (!menuing)
        {
            menuing = true;
            clickMenu.SetActive(true);
            menuPanel.SetActive(true);
        }
        else
        {
            menuing = false;
            clickMenu.SetActive(false);
            menuPanel.SetActive(false);
        }
    }

    public void LightBtnClick()
    {
        if (!lighting)
        {
            lighting = true;
            menuing = false;
            clickMenu.SetActive(false);
            menuPanel.SetActive(false);
            lightPanel.SetActive(true);
            infoPanel.SetActive(false);
            centerImages.SetActive(false);
            infoing = false;
        }
        else
        {
            lighting = false;
            menuing = false;
            clickMenu.SetActive(false);
            menuPanel.SetActive(false);
            lightPanel.SetActive(false);
            infoPanel.SetActive(false);
            centerImages.SetActive(false);
            infoing = false;
        }
    }

    public void CaptureClick()
    {
        if(!isCapture)
        {
            isCapture = true;
            menuing = false;
            lineRender.SetActive(true);
            clickMenu.SetActive(false);
            menuPanel.SetActive(false);
            captureMenu.SetActive(true);
            //screenshot.StartShot();
        }
        else
        {
            isCapture = false;
            menuing = false;
            lineRender.SetActive(false);
            clickMenu.SetActive(false);
            menuPanel.SetActive(false);
            captureMenu.SetActive(false);
            //screenshot.StartShot();
        }
    }

    public void CaptureBtnClick()
    {
        lineRender.SetActive(false);
        captureImg.SetActive(true);
        //artBone.SetActive(true);
        screenshot.StartShot();
        //StartCoroutine(Delay());
    }

    public void AnimationBtnClick()
    {
        captureMenu.SetActive(false);

        if (dropdown.value == 0)
        {
            TextureLoader.Copy(art.source, art.artTerrain);
            TextureLoader.Copy(art.source, art.artBone);
            TextureLoader.ClearOverlap(art.artTerrain, art.artBone);

            artBone.SetActive(true);
            ArtAnimationCtrl.SetActive(true);
        }
        else if (dropdown.value == 1)
        {
            TextureLoader.Copy(art2.source, art2.artTerrain);
            TextureLoader.Copy(art2.source, art2.artBone);
            TextureLoader.ClearOverlap(art2.artTerrain, art2.artBone);

            artBone2.SetActive(true);
            ArtAnimationCtrl2.SetActive(true);
        }
    }

    public void OnDropDown(int value)
    {
        artBone.SetActive(false);
        ArtAnimationCtrl.SetActive(false);

        artBone2.SetActive(false);
        ArtAnimationCtrl2.SetActive(false);
    }

    public void QuitBtnClick()
    {
        Application.Quit();
    }

    public void InfoPrevClick()
    {
        //if (count != 0)
        if (count > 0)
        {
            if (DropdownCtrl.selectIndex == 0)
            {
                imageOne[count].gameObject.SetActive(false);
                count--;
                render.material.mainTexture = imageOne[count].GetComponent<Image>().sprite.texture;
                imageOne[count].gameObject.SetActive(true);
            }
            else if (DropdownCtrl.selectIndex == 1)
            {
                imageTwo[count].gameObject.SetActive(false);
                count--;
                render.material.mainTexture = imageTwo[count].GetComponent<Image>().sprite.texture;
                imageTwo[count].gameObject.SetActive(true);
            }
        }
    }

    public void InfoNextClick()
    {

        if (DropdownCtrl.selectIndex == 0)
        {
            if (count < imageOne.Count - 1)
            {
                imageOne[count].gameObject.SetActive(false);
                count++;
                //leftImages.transform.GetChild(i).GetComponent<Image>().sprite
                render.material.mainTexture = imageOne[count].GetComponent<Image>().sprite.texture;
                imageOne[count].gameObject.SetActive(true);
            }
        }
        else if (DropdownCtrl.selectIndex == 1)
        {
            if (count < imageTwo.Count - 1)
            {
                imageTwo[count].gameObject.SetActive(false);
                count++;
                render.material.mainTexture = imageTwo[count].GetComponent<Image>().sprite.texture;
                //render.material.mainTexture = sprites[count].texture;
                imageTwo[count].gameObject.SetActive(true);
            }
        }

    }

    public void InfoExitClick()
    {
        infoPanel.SetActive(false);
        centerImages.SetActive(false);
        infoing = false;
    }

    public void ShareBtnClick()
    {
        /*menuing = false;
        clickMenu.SetActive(false);
        menuPanel.SetActive(false);*/
        AndroidUtils.OpenGallery();
    }

    IEnumerator PrevNextCtrl()
    {
        while (infoing)
        {
            if (count == 0)
            {
                infoPrev.GetComponent<Button>().interactable = false;
                infoNext.GetComponent<Button>().interactable = true;
            }
            else if (count == length - 1)
            {
                infoPrev.GetComponent<Button>().interactable = true;
                infoNext.GetComponent<Button>().interactable = false;
            }
            else
            {
                infoPrev.GetComponent<Button>().interactable = true;
                infoNext.GetComponent<Button>().interactable = true;
            }
            yield return null;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        captureImg.SetActive(false);
    }
}
