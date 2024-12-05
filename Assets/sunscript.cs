using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sunscript : MonoBehaviour
{
    public GameObject ship;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + 11917.54f, ship.transform.position.z - 10000f);
    }
}
