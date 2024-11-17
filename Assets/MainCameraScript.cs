using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public GameObject ShipModel;
    public float thetaX;
    public float thetaY;
    // Start is called before the first frame update
    void Start()
    {
        thetaX = 0;
        thetaY = Mathf.Atan(2/3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1)){
            thetaX -= Input.GetAxis("Mouse X") * Time.deltaTime * 10;
            thetaY -= Input.GetAxis("Mouse Y") * Time.deltaTime * 10;
            float thetaYT = Mathf.Clamp(thetaY, -Mathf.Atan(2/3f), Mathf.Atan(2/3f));
            thetaY = thetaYT;
        }
        transform.position = new Vector3(Mathf.Cos(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.x, Mathf.Sin(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.y, Mathf.Sin(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.z);
        transform.LookAt(ShipModel.transform);
    }
}
