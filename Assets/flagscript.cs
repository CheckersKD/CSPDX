using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class flagscript : MonoBehaviour
{
    public Vector3 relativeMoonPos;
    public GameObject moon;
    // Start is called before the first frame update
    void Start()
    {
        moon = GameObject.Find("Moon");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = moon.transform.position + relativeMoonPos;
        transform.LookAt(moon.transform.position);
    }
}
