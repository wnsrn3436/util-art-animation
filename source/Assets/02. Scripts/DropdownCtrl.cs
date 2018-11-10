using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownCtrl : MonoBehaviour
{
    public Slider progress;
    public Sprite[] images;
    public GameObject parentImg;
    public Image img;
    public GameObject pic;
    public SliderCtrl sCtrl;

    public static int selectIndex = 0;
    public static int lengthIndex;

    Renderer renderer;
    Dropdown dropdown;
    List<Dropdown.OptionData> options;

    void Start()
    {
        options = new List<Dropdown.OptionData>();

        for(int i = 0; i<images.Length;i++)
        {
            var a = new Dropdown.OptionData("Picture"+i);
            options.Add(a);
            
        }

        lengthIndex = images.Length;
        dropdown = GetComponent<Dropdown>();
        dropdown.options = options;
        img.sprite = images[0];

        renderer = pic.GetComponent<Renderer>();
        renderer.material.mainTexture = img.sprite.texture;
        dropdown.onValueChanged.AddListener(delegate { ValueChanged(dropdown); });
        
    }

    void ValueChanged(Dropdown _dropdown)
    {
        MenuCtrl.count = 0;
        img.sprite = images[_dropdown.value];
        renderer.material.mainTexture =images[_dropdown.value].texture;
        parentImg.transform.GetChild(selectIndex).gameObject.SetActive(false);
        selectIndex = _dropdown.value;
        parentImg.transform.GetChild(selectIndex).gameObject.SetActive(true);
    }
}
