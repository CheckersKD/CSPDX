using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public GameObject ShipModel;
    public float thetaX;
    public float thetaY;
    public float angleDebug;
    // Start is called before the first frame update
    void Start()
    {
        thetaX = 0.01f;
        thetaY = Mathf.Atan(2/3f);
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
        Vector3 angleToRotate = new Vector3(Mathf.Asin(ShipModel.transform.position.z / Mathf.Sqrt(Mathf.Pow(ShipModel.transform.position.z, 2) + Mathf.Pow(ShipModel.transform.position.y, 2))), 0, Mathf.Asin(-ShipModel.transform.position.x / Mathf.Sqrt(Mathf.Pow(ShipModel.transform.position.x, 2) + Mathf.Pow(ShipModel.transform.position.y, 2))));
        Debug.Log(angleToRotate.x);
        Debug.Log(angleToRotate.z);
        Vector3 unRotatedPosition = new Vector3(Mathf.Cos(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52), Mathf.Sin(thetaY) * Mathf.Sqrt(52), Mathf.Sin(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52));
        float xBeforeTransf = (unRotatedPosition.x * (float) (Mathf.Cos(angleToRotate.z) * Mathf.Cos(angleToRotate.y))) + (unRotatedPosition.y * (float) ((Mathf.Cos(angleToRotate.z) * Mathf.Sin(angleToRotate.y) * Mathf.Sin(angleToRotate.x)) - (Mathf.Sin(angleToRotate.z) * Mathf.Cos(angleToRotate.x)))) + (unRotatedPosition.z * (float) ((Mathf.Cos(angleToRotate.z) * Mathf.Sin(angleToRotate.y) * Mathf.Cos(angleToRotate.x)) + (Mathf.Sin(angleToRotate.z) * Mathf.Sin(angleToRotate.x))));
        float yBeforeTransf = (unRotatedPosition.x * (float) (Mathf.Sin(angleToRotate.z) * Mathf.Cos(angleToRotate.y))) + (unRotatedPosition.y * (float) ((Mathf.Sin(angleToRotate.z) * Mathf.Sin(angleToRotate.y) * Mathf.Sin(angleToRotate.x)) + (Mathf.Cos(angleToRotate.z) * Mathf.Cos(angleToRotate.x)))) + (unRotatedPosition.z * (float) ((Mathf.Sin(angleToRotate.z) * Mathf.Sin(angleToRotate.y) * Mathf.Cos(angleToRotate.x)) - (Mathf.Cos(angleToRotate.z) * Mathf.Sin(angleToRotate.x))));
        float zBeforeTransf = (unRotatedPosition.x * (float) (-1 * Mathf.Sin(angleToRotate.y))) + (unRotatedPosition.y * (float) (Mathf.Cos(angleToRotate.y) * Mathf.Sin(angleToRotate.x))) + (unRotatedPosition.z * (float) (Mathf.Cos(angleToRotate.y) * Mathf.Cos(angleToRotate.x)));
        // transform.position = new Vector3(xBeforeTransf + ShipModel.transform.position.x, yBeforeTransf + ShipModel.transform.position.y, zBeforeTransf + ShipModel.transform.position.z);
        transform.position = new Vector3(Mathf.Cos(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.x, Mathf.Sin(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.y, Mathf.Sin(thetaX) * Mathf.Cos(thetaY) * Mathf.Sqrt(52) + ShipModel.transform.position.z);
        transform.LookAt(ShipModel.transform);
    }
}
