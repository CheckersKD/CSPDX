using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    public float gravAccelMagniEarth;
    public float massEarth;
    public float massMoon;
    public float massShip;
    public Vector3 velocity;
    public GameObject colliderParent;
    Transform[] colliderVertices;
    public Vector3 rbVelDebug;
    public bool collideEarthDebug;
    Vector3 angularVel;
    public float thrust;
    public Transform upPoint;
    public float closestPointToEarth;

    // Start is called before the first frame update
    void Start()
    {
        gravAccelMagniEarth = 0;
        colliderVertices = colliderParent.GetComponentsInChildren<Transform>();
        thrust = 0;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Z)){
            thrust = 1;
        }
        if(Input.GetKeyDown(KeyCode.X)){
            thrust = 0;
        }
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftShift)){
            thrust += 0.02f;
            thrust = Mathf.Clamp(thrust, 0, 1);
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            thrust -= 0.02f;
            thrust = Mathf.Clamp(thrust, 0, 1);
        }
        if(Input.GetKey(KeyCode.D)){
            angularVel.x += 1f;
        }
        if(Input.GetKey(KeyCode.A)){
            angularVel.x -= 1f;
        }
        if(Input.GetKey(KeyCode.W)){
            angularVel.z += 1f;
        }
        if(Input.GetKey(KeyCode.S)){
            angularVel.z -= 1f;
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)){
            if(Mathf.Abs(angularVel.x) > 0)
                angularVel.x -= Mathf.Min(1, Mathf.Abs(angularVel.x)) * (angularVel.x / Mathf.Abs(angularVel.x));
            if(Mathf.Abs(angularVel.y) > 0)
                angularVel.y -= Mathf.Min(1, Mathf.Abs(angularVel.y)) * (angularVel.y / Mathf.Abs(angularVel.y));
            if(Mathf.Abs(angularVel.z) > 0)
                angularVel.z -= Mathf.Min(1, Mathf.Abs(angularVel.z)) * (angularVel.z / Mathf.Abs(angularVel.z));
        }
        velocity = new Vector3(velocity.x + (thrust * (upPoint.position.x - transform.position.x) * 0.2f), 
                                velocity.y + (thrust * (upPoint.position.y - transform.position.y) * 0.2f),
                                velocity.z + (thrust * (upPoint.position.z - transform.position.z) * 0.2f));
        gravAccelMagniEarth = 0.006f * (massShip * massEarth) / (Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2));
        velocity = new Vector3(velocity.x - (gravAccelMagniEarth * (transform.position.x / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.y - (gravAccelMagniEarth * (transform.position.y / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.z - (gravAccelMagniEarth * (transform.position.z / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))));
        if(collidesWithEarth() != null){
            foreach (int i in collidesWithEarth()){
                transform.position += new Vector3(((colliderVertices[i].position.x / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.x, ((colliderVertices[i].position.y / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.y, ((colliderVertices[i].position.z / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.z);
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity, transform.position) < 0){
                if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                    angularVel = Vector3.zero;
                velocity = Vector3.zero;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
            collideEarthDebug = true;
        }
        else{
            collideEarthDebug = false;
        }
        // Debug.Log(vectorToRadialComponent(velocity, transform.position));
        transform.position += (velocity * Time.deltaTime);
        transform.Rotate(angularVel * Time.deltaTime);
        // Debug.Log(Time.deltaTime);
    }

    List<int> collidesWithEarth(){
        float temp = 32767;
        List<int> result = new List<int>();
        for(int i = 0; i < colliderVertices.Length; i++){
            if (Vector3.Magnitude(colliderVertices[i].position) < 900){
                result.Add(i);
            }
            if(Vector3.Magnitude(colliderVertices[i].position) < temp){
                temp = Vector3.Magnitude(colliderVertices[i].position);
            }
        }
        closestPointToEarth = temp;
        if(result.Count == 0)
            return null;
        else
            return result;
    }

    double vectorToRadialComponent(Vector3 inputVector, Vector3 radialVector){
        Vector3 inputNormalized = new Vector3(inputVector.x / inputVector.magnitude, inputVector.y / inputVector.magnitude, inputVector.z / inputVector.magnitude);
        Vector3 radialNormalized = new Vector3(radialVector.x / radialVector.magnitude, radialVector.y / radialVector.magnitude, radialVector.z / radialVector.magnitude);
        float radialComponentNormalized = (inputNormalized.x * radialNormalized.x) + (inputNormalized.y * radialNormalized.y) + (inputNormalized.z * radialNormalized.z);
        return radialComponentNormalized * inputNormalized.magnitude;
    }
}
