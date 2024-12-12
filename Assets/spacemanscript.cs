using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacemanscript : MonoBehaviour
{
    public GameObject spacemancam;
    public Vector3 velocity;
    public float gravAccelMagniEarth;
    public float gravAccelMagniMoon;
    public GameObject ship;
    public ShipScript shipScript;
    public GameObject colliderParent;
    public Transform[] colliderVertices;
    public GameObject moon;
    public float thrust;
    public Transform shipLowerSphereTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        shipScript = ship.GetComponent<ShipScript>();
        colliderVertices = colliderParent.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W)){
            transform.forward = -Vector3.Normalize(-transform.forward + 0.5f * (Vector3.Normalize(spacemancam.transform.position - transform.position) - -transform.forward));
            thrust = 0.2f;
        }
        else if(Input.GetKey(KeyCode.S)){
            transform.forward = Vector3.Normalize(transform.forward + 0.5f * (Vector3.Normalize(spacemancam.transform.position - transform.position) - transform.forward));
            thrust = 0.2f;
        }
        else if(Input.GetKey(KeyCode.A)){
            transform.forward = -Vector3.Normalize(-transform.forward + 0.5f * (spacemancam.transform.right - -transform.forward));
            thrust = 0.2f;
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.forward = Vector3.Normalize(transform.forward + 0.5f * (spacemancam.transform.right - transform.forward));
            thrust = 0.2f;
        }
        else{
            thrust = 0.0f;
        }
        gravAccelMagniEarth = 0.006f * (shipScript.massShip * shipScript.massEarth) / (Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2));
        velocity = new Vector3(velocity.x - (gravAccelMagniEarth * (transform.position.x / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.y - (gravAccelMagniEarth * (transform.position.y / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.z - (gravAccelMagniEarth * (transform.position.z / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))));
        if(Vector3.Distance(transform.position, shipScript.moonPos) < 700){
            gravAccelMagniMoon = 0.006f * (shipScript.massShip * shipScript.massMoon) / Mathf.Pow(Vector3.Distance(transform.position, shipScript.moonPos), 2);
            velocity = new Vector3(velocity.x - (gravAccelMagniMoon * ((transform.position.x - shipScript.moonPos.x) / Vector3.Distance(transform.position, shipScript.moonPos))),
                                    velocity.y - (gravAccelMagniMoon * ((transform.position.y - shipScript.moonPos.y) / Vector3.Distance(transform.position, shipScript.moonPos))),
                                    velocity.z - (gravAccelMagniMoon * ((transform.position.z - shipScript.moonPos.z) / Vector3.Distance(transform.position, shipScript.moonPos))));
        }
        if(collidesWithEarth() != null){
            foreach (int i in collidesWithEarth()){
                transform.position += new Vector3(((colliderVertices[i].position.x / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.x, ((colliderVertices[i].position.y / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.y, ((colliderVertices[i].position.z / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.z);
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity, transform.position) < 0){
                velocity = Vector3.zero;
                velocity = new Vector3(velocity.x + (thrust * transform.forward.x * 4f), 
                                velocity.y + (thrust * transform.forward.y * 4f),
                                velocity.z + (thrust * transform.forward.z * 4f));
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
        }
        Debug.DrawRay(transform.position, velocity);
        if(collidesWithMoon() != null){
            foreach (int i in collidesWithMoon()){
                transform.position += new Vector3((((colliderVertices[i].position.x - shipScript.moonPos.x) / Vector3.Distance(colliderVertices[i].position, shipScript.moonPos)) * 200) - (colliderVertices[i].position.x - shipScript.moonPos.x), (((colliderVertices[i].position.y - shipScript.moonPos.y) / Vector3.Distance(colliderVertices[i].position, shipScript.moonPos)) * 200) - (colliderVertices[i].position.y - shipScript.moonPos.y), (((colliderVertices[i].position.z - shipScript.moonPos.z) / Vector3.Distance(colliderVertices[i].position, shipScript.moonPos)) * 200) - (colliderVertices[i].position.z - shipScript.moonPos.z));
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity - moon.GetComponent<moonscript>().velEvaluated, transform.position - shipScript.moonPos) < 0){
                velocity = moon.GetComponent<moonscript>().velEvaluated;
                velocity = new Vector3(velocity.x + (thrust * transform.forward.x * 4f), 
                                velocity.y + (thrust * transform.forward.y * 4f),
                                velocity.z + (thrust * transform.forward.z * 4f));
                Debug.Log("now" + Time.time);
                // transform.position += moon.GetComponent<moonscript>().velEvaluated / 50;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
            if(!shipScript.flagPlanted && shipScript.spacewalking){
                shipScript.plantFlagTextShowCooldown = 1.0f;
                if(Input.GetKey(KeyCode.P)){
                    GameObject plantedFlag = (GameObject) Instantiate(shipScript.flagPrefab);
                    plantedFlag.transform.position = transform.position;
                    plantedFlag.GetComponent<flagscript>().relativeMoonPos = transform.position - shipScript.moonPos;
                    shipScript.flagPlanted = true;
                }
            }
        }
        if(collidesWithShip1() != null){
            foreach (int i in collidesWithShip1()){
                transform.position += new Vector3((((colliderVertices[i].position.x - ship.transform.position.x) / Vector3.Distance(colliderVertices[i].position, ship.transform.position)) * 1) - (colliderVertices[i].position.x - ship.transform.position.x), (((colliderVertices[i].position.y - ship.transform.position.y) / Vector3.Distance(colliderVertices[i].position, ship.transform.position)) * 1) - (colliderVertices[i].position.y - ship.transform.position.y), (((colliderVertices[i].position.z - ship.transform.position.z) / Vector3.Distance(colliderVertices[i].position, ship.transform.position)) * 1) - (colliderVertices[i].position.z - ship.transform.position.z));
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity - shipScript.velocity, transform.position - ship.transform.position) < 0){
                velocity = shipScript.velocity;
                // transform.position += moon.GetComponent<moonscript>().velEvaluated / 50;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
        }
        if(collidesWithShip2() != null){
            foreach (int i in collidesWithShip2()){
                transform.position += new Vector3((((colliderVertices[i].position.x - shipLowerSphereTransform.position.x) / Vector3.Distance(colliderVertices[i].position, shipLowerSphereTransform.position)) * 0.75f) - (colliderVertices[i].position.x - shipLowerSphereTransform.position.x), (((colliderVertices[i].position.y - shipLowerSphereTransform.position.y) / Vector3.Distance(colliderVertices[i].position, shipLowerSphereTransform.position)) * 0.75f) - (colliderVertices[i].position.y - shipLowerSphereTransform.position.y), (((colliderVertices[i].position.z - shipLowerSphereTransform.position.z) / Vector3.Distance(colliderVertices[i].position, shipLowerSphereTransform.position)) * 0.75f) - (colliderVertices[i].position.z - shipLowerSphereTransform.position.z));
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity - shipScript.velocity, transform.position - shipLowerSphereTransform.position) < 0){
                velocity = shipScript.velocity;
                // transform.position += moon.GetComponent<moonscript>().velEvaluated / 50;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
        }
        velocity = new Vector3(velocity.x + (thrust * transform.forward.x * 0.2f), 
                                velocity.y + (thrust * transform.forward.y * 0.2f),
                                velocity.z + (thrust * transform.forward.z * 0.2f));


        transform.position += (velocity * Time.deltaTime);
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
        if(result.Count == 0)
            return null;
        else
            return result;
    }

    List<int> collidesWithMoon(){
        float temp = 32767;
        List<int> result = new List<int>();
        for(int i = 0; i < colliderVertices.Length; i++){
            if (Vector3.Distance(colliderVertices[i].position, shipScript.moonPos) < 200){
                result.Add(i);
            }
            if(Vector3.Distance(colliderVertices[i].position, shipScript.moonPos) < temp){
                temp = Vector3.Magnitude(colliderVertices[i].position);
            }
        }
        if(result.Count == 0)
            return null;
        else
            return result;
    }

    List<int> collidesWithShip1(){
        List<int> result = new List<int>();
        for(int i = 0; i < colliderVertices.Length; i++){
            if (Vector3.Distance(colliderVertices[i].position, ship.transform.position) < 1){
                result.Add(i);
            }
        }
        if(result.Count == 0)
            return null;
        else
            return result;
    }

    List<int> collidesWithShip2(){
        List<int> result = new List<int>();
        for(int i = 0; i < colliderVertices.Length; i++){
            if (Vector3.Distance(colliderVertices[i].position, shipLowerSphereTransform.position) < 0.75f){
                result.Add(i);
            }
        }
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
