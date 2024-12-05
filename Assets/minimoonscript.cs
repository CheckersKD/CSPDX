using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimoonscript : MonoBehaviour
{
    public GameObject moon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = moon.transform.position / 100;
    }
}
