using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moonscript : MonoBehaviour
{
    //34.73 velocity
    public float startTime;
    public float currentObservedTime;
    public Vector3 velEvaluated;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentObservedTime = Time.time;
        transform.position = new Vector3(Mathf.Sin(((Time.time - startTime) / 542.7456f) * 2 * Mathf.PI) * 3000, 0, Mathf.Cos(((Time.time - startTime) / 542.7456f) * 2 * Mathf.PI) * -3000);
        velEvaluated = (transform.position - new Vector3(Mathf.Sin(((Time.time - 0.02f - startTime) / 542.7456f) * 2 * Mathf.PI) * 3000, 0, Mathf.Cos(((Time.time - 0.02f - startTime) / 542.7456f) * 2 * Mathf.PI) * -3000)) * 50;
    }
}
