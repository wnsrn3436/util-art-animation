using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightButton : MonoBehaviour
{
    //Create Light Button
    public Button createButton;

    //Delete Light Button
    public Button deleteButton;

    //lights를 담을 GameObject
    public GameObject lightParent;

    //picture를 담을 GamgeObject
    public GameObject picture;

    //mode
    //0 : select light
    //1 : create light
    public static int mode = 0;

    //Button Text
    private readonly string createLightButtonText = "Create Light";
    private readonly string deleteLightButtonText = "Delete Light";

    //light number
    private int number = 0;

    //light range
    private readonly float lightRange = 1.0f;

    //light intensity
    private readonly float lightIntensity = 1.0f;

    //light sphere collider radius
    private readonly float lightColliderRadius = 0.15f;

    //Selected Light
    GameObject selectedLight = null;

    //Selected Light for Delete
    GameObject selectedLightForDelete = null;

    Transform saveSelectedLightTransform;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        //button 초기 Text 설정
        createButton.GetComponentInChildren<Text>().text = createLightButtonText;
        deleteButton.GetComponentInChildren<Text>().text = deleteLightButtonText;
        //light 생성 button listener 추가
        createButton.GetComponent<Button>().onClick.AddListener(ChangeCreateLightMode);
        //light 제거 button listener 추가
        deleteButton.GetComponent<Button>().onClick.AddListener(DeleteLight);
    }

    void ChangeCreateLightMode()
    {
        //생성모드가 아닐경우
        if (mode == 0)
        {
            //click시 button text 변경
            createButton.GetComponentInChildren<Text>().text = "End Create";
            //light 생성 mode로 전환
            mode = 1;
        }
        else if (mode == 1)
        {
            //click시 button text 변경
            createButton.GetComponentInChildren<Text>().text = "Create Light";
            //일반 mode로 전환
            mode = 0;
            //선택되어 있는 selectedLightForDelete 초기화
            selectedLightForDelete = null;
        }
    }

    void FixedUpdate()
    {
        //touch 갯수가 1이상일 경우
        if (Input.touchCount > 0)
        {
            //첫번째 touch 선택
            Touch touch = Input.touches[0];

            switch (mode)
            {
                //light 선택 mode일 경우
                case 0:
                    switch (touch.phase)
                    {
                        //Touch 시작
                        case TouchPhase.Began:
                            //Touch position에 Raycast 생성
                            ray = Camera.main.ScreenPointToRay(touch.position);

                            //Raycast에 충돌한 Object가 존재할 경우 true
                            if (Physics.Raycast(ray, out hit, 10.0f))
                            {
                                //충돌한 Object의 Tag가 Light일 경우 true
                                if (hit.transform.gameObject.CompareTag("Light"))
                                {
                                    //선택된 Light Object를 selectedLight에 기억
                                    selectedLight = hit.transform.gameObject;
                                    //선택된 Light Object를 selectedLightForDelete에 기억
                                    selectedLightForDelete = selectedLight;
                                    //선택된 Light Object의 위치를 saveSelectedLightTransform에 기억
                                    saveSelectedLightTransform = selectedLight.transform;
                                }
                            }
                            break;
                        //Touch 유지 상태에서 움직임
                        case TouchPhase.Moved:
                            //선택된 Light Object가 존재할 경우 true
                            if (selectedLight != null)
                            {
                                //이동한 Touch position을 받아옴. position.z는 Camera에서 얼마나 떨어져있는 WorldPoint인지 입력. 현재 Camera의 z 위치가 -6이면, Camera에서 +6만큼 떨어진 WorldPoint에서 움직여야 함
                                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -(MenuCtrl.camPosition.z)));

                                //Light Object를 이동
                                selectedLight.transform.position = new Vector3(touchedPos.x, touchedPos.y, selectedLight.transform.position.z);
                            }
                            break;
                        //Touch 끝
                        case TouchPhase.Ended:
                            //이동이 끝났으므로 selectedLight를 초기화
                            selectedLight = null;
                            break;
                    }
                    break;

                //light 생성 mode일 경우
                case 1:
                    //touch 시작할 때
                    if (touch.phase == TouchPhase.Began)
                    {
                        //light 생성할 위치를 touch로 입력
                        Vector3 createPosition;

                        //touch의 위치값 받아옴
                        ray = Camera.main.ScreenPointToRay(touch.position);

                        //ray가 도달한 위치가 Picture 내부에 있다면 light 생성
                        if (Physics.Raycast(ray, out hit, 10.0f) && hit.transform.gameObject.CompareTag("Picture"))
                        {
                            createPosition = hit.point;
                            createPosition.z = -0.1f;

                            //light 생성 fucntion 호출
                            CreateLight(createPosition);
                        }
                    }
                    break;
                default: break;
            }
        }
    }

    void CreateLight(Vector3 pos)
    {

        //light를 넣을 GameObject
        GameObject userLight = new GameObject();
        userLight.transform.SetParent(lightParent.transform);
        userLight.name = "userLight" + (++number).ToString("00000");
        userLight.tag = "Light";
        userLight.transform.position = pos;

        //Touch로 Object 선택을 위한 Collider
        userLight.AddComponent<SphereCollider>();
        SphereCollider col = userLight.GetComponent<SphereCollider>();
        col.radius = lightColliderRadius;

        //Light Component
        userLight.AddComponent<Light>();
        Light lightSetting = userLight.GetComponent<Light>();
        lightSetting.type = LightType.Point;
        lightSetting.range = lightRange;
        lightSetting.intensity = lightIntensity;
    }

    void DeleteLight()
    {
        //선택된 selectedLightForDelete 존재할 시
        if (selectedLightForDelete != null)
        {
            //selectedLightForDelete에 저장된 light 제거
            Destroy(selectedLightForDelete);

            //selectedLightForDelete 초기화
            selectedLightForDelete = null;
        }
    }
}
