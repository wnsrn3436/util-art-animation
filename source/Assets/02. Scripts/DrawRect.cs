using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
public class DrawRect : MonoBehaviour
{
    public float width = 567;
    public float height = 627;
    public static Rect _cropArea = new Rect(0, 0, 0, 0);
    public Rect cropArea
    {
        get { return _cropArea; }
        set
        {
            _cropArea = value;

            lineRenderer.Points[0] = new Vector2(value.x, -value.y);
            lineRenderer.Points[1] = new Vector2(value.x + value.width, -value.y);
            lineRenderer.Points[2] = new Vector2(value.x + value.width, -(value.y + value.height));
            lineRenderer.Points[3] = new Vector2(value.x, -(value.y + value.height));
            lineRenderer.Points[4] = lineRenderer.Points[0];

            lineRenderer.RelativeSize = false;
            lineRenderer.drivenExternally = true;
        }
    }

    Vector2 mousePosition
    {
        get { return new Vector2(Mathf.Clamp(Input.mousePosition.x, 5.0f, Screen.width - 5.0f), Mathf.Clamp(Screen.height - Input.mousePosition.y, 5.0f, Screen.height - 305.0f)); }
    }

    UILineRenderer lineRenderer;
    bool isDrag;

    void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();
    }
    private void Start()
    {
        // Debug.Log(Screen.width/2 - width/2);
        cropArea = new Rect(new Vector2(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2), new Vector2(width, height));
    }
    void OnDestroy()
    {
        _cropArea = new Rect(0, 0, 0, 0);


    }

    /* void Update()
     {
         if (Input.GetMouseButtonDown(0) && (Screen.height - Input.mousePosition.y) < (Screen.height - 300))
         {
             isDrag = true;
             cropArea = new Rect(mousePosition.x, mousePosition.y, 0, 0);
         }
         else if (isDrag && Input.GetMouseButton(0))
         {
             cropArea = new Rect()
             {
                 position = cropArea.position,
                 size = new Vector2(mousePosition.x - cropArea.position.x, mousePosition.y - cropArea.position.y)
             };
         }
         else if (Input.GetMouseButtonUp(0))
         {
             isDrag = false;
         }
     }*/
}
