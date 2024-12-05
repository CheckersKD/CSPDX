using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShipScript : MonoBehaviour
{
    public float gravAccelMagniEarth;
    public float gravAccelMagniMoon;
    public float massEarth;
    public float massMoon;
    public float massShip;
    public Vector3 velocity;
    public GameObject colliderParent;
    Transform[] colliderVertices;
    public Vector3 rbVelDebug;
    public bool collideEarthDebug;
    public bool collideMoonDebug;
    public bool collideMoonDebug2;
    Vector3 angularVel;
    public float thrust;
    public Transform upPoint;
    public float closestPointToEarth;
    public ParticleSystem thrustParticles;
    public Vector3 moonPos;
    public GameObject moon;
    public float velMag;
    public bool inMap;
    public GameObject mainCamera;
    public GameObject mapCamera;
    public GameObject flagPrefab;
    public GameObject plantFlagText;
    public bool flagPlanted;
    public float plantFlagTextShowCooldown;
    public TMP_Text celestialBodyNear;
    public TMP_Text velDisplay;
    public Slider thrustDisplay;

    // Start is called before the first frame update
    void Start()
    {
        gravAccelMagniEarth = 0;
        colliderVertices = colliderParent.GetComponentsInChildren<Transform>();
        thrust = 0;
        inMap = false;
        flagPlanted = false;
    }

    void Update(){
        mainCamera.SetActive(!inMap);
        mapCamera.SetActive(inMap);
        if(Input.GetKeyDown(KeyCode.Z)){
            thrust = 1;
        }
        if(Input.GetKeyDown(KeyCode.X)){
            thrust = 0;
        }
        if(Input.GetKeyDown(KeyCode.M)){
            inMap = !inMap;
        }
        thrustDisplay.value = thrust;
    }

    void FixedUpdate()
    {
        moonPos = moon.transform.position;
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
        var main = thrustParticles.main;
        main.startSpeed = Mathf.Clamp(Mathf.Pow(thrust, 2) * 10 * (1 + UnityEngine.Random.value), 2, 20);
        var emission = thrustParticles.emission;
        emission.rateOverTime = thrust * 30;
        velocity = new Vector3(velocity.x + (thrust * (upPoint.position.x - transform.position.x) * 0.2f), 
                                velocity.y + (thrust * (upPoint.position.y - transform.position.y) * 0.2f),
                                velocity.z + (thrust * (upPoint.position.z - transform.position.z) * 0.2f));
        gravAccelMagniEarth = 0.006f * (massShip * massEarth) / (Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2));
        velocity = new Vector3(velocity.x - (gravAccelMagniEarth * (transform.position.x / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.y - (gravAccelMagniEarth * (transform.position.y / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))),
                                    velocity.z - (gravAccelMagniEarth * (transform.position.z / Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) + Mathf.Pow(transform.position.z, 2)))));
        if(Vector3.Distance(transform.position, moonPos) < 700){
            gravAccelMagniMoon = 0.006f * (massShip * massMoon) / Mathf.Pow(Vector3.Distance(transform.position, moonPos), 2);
            velocity = new Vector3(velocity.x - (gravAccelMagniMoon * ((transform.position.x - moonPos.x) / Vector3.Distance(transform.position, moonPos))),
                                    velocity.y - (gravAccelMagniMoon * ((transform.position.y - moonPos.y) / Vector3.Distance(transform.position, moonPos))),
                                    velocity.z - (gravAccelMagniMoon * ((transform.position.z - moonPos.z) / Vector3.Distance(transform.position, moonPos))));
            celestialBodyNear.text = "Near Moon";
            velDisplay.text = "Velocity Relative to Moon: " + (((int) ((moon.GetComponent<moonscript>().velEvaluated - velocity).magnitude * 10)) / 10.0f);
        }
        else{
            celestialBodyNear.text = "Near Earth";
            velDisplay.text = "Velocity Relative to Earth: " + (((int) (velocity.magnitude * 10)) / 10.0f);
        }
        if(collidesWithEarth() != null){
            foreach (int i in collidesWithEarth()){
                transform.position += new Vector3(((colliderVertices[i].position.x / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.x, ((colliderVertices[i].position.y / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.y, ((colliderVertices[i].position.z / Vector3.Magnitude(colliderVertices[i].position)) * 900) - colliderVertices[i].position.z);
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity, transform.position) < 0){
                velocity = Vector3.zero;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
            collideEarthDebug = true;
        }
        else{
            collideEarthDebug = false;
        }
        Debug.DrawRay(transform.position, velocity);
        if(collidesWithMoon() != null){
            foreach (int i in collidesWithMoon()){
                transform.position += new Vector3((((colliderVertices[i].position.x - moonPos.x) / Vector3.Distance(colliderVertices[i].position, moonPos)) * 200) - (colliderVertices[i].position.x - moonPos.x), (((colliderVertices[i].position.y - moonPos.y) / Vector3.Distance(colliderVertices[i].position, moonPos)) * 200) - (colliderVertices[i].position.y - moonPos.y), (((colliderVertices[i].position.z - moonPos.z) / Vector3.Distance(colliderVertices[i].position, moonPos)) * 200) - (colliderVertices[i].position.z - moonPos.z));
                // Debug.Log("Done.");
            }
            if(vectorToRadialComponent(velocity - moon.GetComponent<moonscript>().velEvaluated, transform.position - moonPos) < 0){
                velocity = moon.GetComponent<moonscript>().velEvaluated;
                Debug.Log("now" + Time.time);
                collideMoonDebug2 = true;
                // transform.position += moon.GetComponent<moonscript>().velEvaluated / 50;
                // Debug.Log(vectorToRadialComponent(velocity, transform.position));
            }
            else{
                collideMoonDebug2 = false;
            }
            collideMoonDebug = true;
            if(!flagPlanted){
                plantFlagTextShowCooldown = 1.0f;
                if(Input.GetKey(KeyCode.P)){
                    GameObject plantedFlag = (GameObject) Instantiate(flagPrefab);
                    plantedFlag.transform.position = transform.position;
                    plantedFlag.GetComponent<flagscript>().relativeMoonPos = transform.position - moonPos;
                    flagPlanted = true;
                }
            }
        }
        else{
            collideMoonDebug = false;
            // plantFlagTextShowCooldown = 0.0f;
        }
        plantFlagText.SetActive(plantFlagTextShowCooldown > 0);
        plantFlagTextShowCooldown -= 0.02f;
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                    angularVel = Vector3.zero;
        // Debug.Log(vectorToRadialComponent(velocity, transform.position));
        velMag = velocity.magnitude;
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

    List<int> collidesWithMoon(){
        float temp = 32767;
        List<int> result = new List<int>();
        for(int i = 0; i < colliderVertices.Length; i++){
            if (Vector3.Distance(colliderVertices[i].position, moonPos) < 200){
                result.Add(i);
            }
            if(Vector3.Distance(colliderVertices[i].position, moonPos) < temp){
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
