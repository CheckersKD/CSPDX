using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class minishipscript : MonoBehaviour
{
    public GameObject ship;
    ShipScript shipScript;
    public GameObject greenDotPrefab;
    public int numLookaheads;
    public int numSteps;
    GameObject[] greenDots;
    public Vector3 pointSave1;
    public Vector3 pointSave2;
    public Vector3 pointSave3;
    // Start is called before the first frame update
    void Start()
    {
        shipScript = ship.GetComponent<ShipScript>();
        greenDots = new GameObject[numLookaheads];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(ship.transform.position.x / 100, ship.transform.position.y / 100, ship.transform.position.z / 100);
        Vector3 tempPos = ship.transform.position;
        Vector3 tempVel = shipScript.velocity;
        Vector3 tempMoonPos = shipScript.moonPos;
        float moonStartTime = ship.GetComponent<ShipScript>().moon.GetComponent<moonscript>().startTime;
        float tempTime = ship.GetComponent<ShipScript>().moon.GetComponent<moonscript>().currentObservedTime;
        for(int i = 0; i < numLookaheads; i++){
            Destroy(greenDots[i]);
        }
        for(int i = 0; i < numLookaheads; i++){
            for(int o = 0; o < numSteps; o++){
                tempTime += 0.02f;
                tempMoonPos = new Vector3(Mathf.Sin(((tempTime - moonStartTime) / 542.7456f) * 2 * Mathf.PI) * 3000, 0, Mathf.Cos(((tempTime - moonStartTime) / 542.7456f) * 2 * Mathf.PI) * -3000);
                float gravAccelMagniEarth = 0.006f * (shipScript.massShip * shipScript.massEarth) / (Mathf.Pow(tempPos.x, 2) + Mathf.Pow(tempPos.y, 2) + Mathf.Pow(tempPos.z, 2));
                tempVel = new Vector3(tempVel.x - (gravAccelMagniEarth * (tempPos.x / Mathf.Sqrt(Mathf.Pow(tempPos.x, 2) + Mathf.Pow(tempPos.y, 2) + Mathf.Pow(tempPos.z, 2)))),
                                            tempVel.y - (gravAccelMagniEarth * (tempPos.y / Mathf.Sqrt(Mathf.Pow(tempPos.x, 2) + Mathf.Pow(tempPos.y, 2) + Mathf.Pow(tempPos.z, 2)))),
                                            tempVel.z - (gravAccelMagniEarth * (tempPos.z / Mathf.Sqrt(Mathf.Pow(tempPos.x, 2) + Mathf.Pow(tempPos.y, 2) + Mathf.Pow(tempPos.z, 2)))));
                if(Vector3.Distance(tempPos, tempMoonPos) < 700){
                    float gravAccelMagniMoon = 0.006f * (shipScript.massShip * shipScript.massMoon) / Mathf.Pow(Vector3.Distance(tempPos, tempMoonPos), 2);
                    tempVel = new Vector3(tempVel.x - (gravAccelMagniMoon * ((tempPos.x - tempMoonPos.x) / Vector3.Distance(tempPos, tempMoonPos))),
                                            tempVel.y - (gravAccelMagniMoon * ((tempPos.y - tempMoonPos.y) / Vector3.Distance(tempPos, tempMoonPos))),
                                            tempVel.z - (gravAccelMagniMoon * ((tempPos.z - tempMoonPos.z) / Vector3.Distance(tempPos, tempMoonPos))));
                }
                tempPos += tempVel * 0.02f;
            }
            GameObject newInstantiation = (GameObject) Instantiate(greenDotPrefab);
            newInstantiation.transform.position = tempPos  / 100;
            greenDots[i] = newInstantiation;
            if(tempPos.magnitude < 900 || Vector3.Distance(tempMoonPos, tempPos) < 200){
                i = numLookaheads;
            }
        }
        if(greenDots[0] != null)
            pointSave1 = greenDots[0].transform.position;
        if(greenDots[1] != null)
            pointSave2 = greenDots[1].transform.position;
        if(greenDots[2] != null)
            pointSave3 = greenDots[2].transform.position;
    }
}
