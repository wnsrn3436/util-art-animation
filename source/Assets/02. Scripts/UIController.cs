using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    private float timer;
    private bool starting = false;
    private AndroidUtils androidUtils;
    [SerializeField] private GameObject startRecordbtn, stopRecordBtn, galleryBtn, shareBtn;
    private void Start()
    {
        if (!AndroidUtils.IsPermitted(AndroidPermission.ACCESS_FINE_LOCATION))//test request permission
            AndroidUtils.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION);
        androidUtils = FindObjectOfType<AndroidUtils>();
    }

    private void Update()
    {
        if (starting)
            timer += Time.deltaTime;
        if (timer > 10.0f)
            OnClickStopRecord();
        if(Input.GetKeyDown(KeyCode.Escape) && starting)
            OnClickStopRecord();
    }

    public void OnClickStartRecord()
    {
        timer = 0;
        starting = true;
        startRecordbtn.SetActive(false);
        galleryBtn.SetActive(false);
        //stopRecordBtn.SetActive(true);
        shareBtn.SetActive(false);
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
        startRecordbtn.SetActive(true);
        //stopRecordBtn.SetActive(false);
        galleryBtn.SetActive(true);
        shareBtn.SetActive(true);
        androidUtils.StopRecording();
        StartCoroutine(DelaySaveVideo());
    }
    private IEnumerator DelaySaveVideo()
    {
        yield return new WaitForSeconds(1);
        androidUtils.SaveVideoToGallery();
    }

    public void OnClickOpenGallery()
    {
        AndroidUtils.OpenGallery();
    }
}
