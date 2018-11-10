using UnityEngine;

public class WindCtrl : MonoBehaviour
{
    public Transform[] targets;
    public float speed = 10.0f;

    float time;

    void Update()
    {
        foreach (Transform target in targets)
        {
            Vector3 rotation = target.transform.rotation.eulerAngles;
            rotation.z += (Mathf.Sin(time + Time.deltaTime) - Mathf.Sin(time)) * speed;
            target.transform.rotation = Quaternion.Euler(rotation);
        }

        time += Time.deltaTime;
    }
}