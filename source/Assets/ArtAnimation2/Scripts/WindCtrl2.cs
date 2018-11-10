using UnityEngine;

public class WindCtrl2 : MonoBehaviour
{
    public Transform[] targets;
    public float[] speed;

    float time;

    void Update()
    {
        int i = 0;
        foreach (Transform target in targets)
        {
            Vector3 position = target.transform.position;
            position.x += (Mathf.Sin(time + Time.deltaTime) - Mathf.Sin(time)) * speed[i] / 10.0f;
            target.transform.position = position;

            Vector3 rotation = target.transform.rotation.eulerAngles;
            rotation.z += (Mathf.Sin(time + Time.deltaTime) - Mathf.Sin(time)) * speed[i] * 70.0f;
            target.transform.rotation = Quaternion.Euler(rotation);

            i++;
        }

        time += Time.deltaTime;
    }
}