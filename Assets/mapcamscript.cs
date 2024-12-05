using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapcamscript : MonoBehaviour
{
    public GameObject miniearth;
    public float thetaX;
    public float thetaY;
    // Start is called before the first frame update
    void Start()
    {
        thetaX = 0;
        thetaY = 0.65f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1)){
            thetaX -= Input.GetAxis("Mouse X") * Time.deltaTime * 7;
            thetaY -= Input.GetAxis("Mouse Y") * Time.deltaTime * 7;
            float thetaYT = Mathf.Clamp(thetaY, -Mathf.PI/2 + 0.1f, Mathf.PI/2 - 0.1f);
            thetaY = thetaYT;
        }
        transform.position = new Vector3(Mathf.Cos(thetaX) * Mathf.Cos(thetaY) * 50, Mathf.Sin(thetaY) * 50, Mathf.Sin(thetaX) * Mathf.Cos(thetaY) * 50);
        transform.LookAt(Vector3.zero);
    }
}
