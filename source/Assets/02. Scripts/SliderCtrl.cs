using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCtrl : MonoBehaviour {

    public GameObject infoPanel;
    public GameObject img;
    public Slider progress;

    Color color;
    Color tmp;
    Material mat;

	void OnEnable () {
        infoPanel.SetActive(true);
        img.transform.GetChild(DropdownCtrl.selectIndex).gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    private void OnDisable()
    {
        infoPanel.SetActive(false);
        img.transform.GetChild(DropdownCtrl.selectIndex).gameObject.SetActive(false);
    }

    IEnumerator Delay()
    {
        mat = GetComponent<Renderer>().material;
        color = mat.color;

        tmp = color;
        while (true)
        {
            tmp.a = progress.value;
            mat.color = tmp;
            yield return null;
        }
    }
}
