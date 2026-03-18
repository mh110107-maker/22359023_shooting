using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float spd = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        // Vector3 direct = Vector3.right * h + Vector3.up * v;
        Vector3 derect = new Vector3(h, v, 0);

        //transform.Translate(derect * spd * Time.deltaTime);
        transform.position = transform.position + derect * spd * Time.deltaTime;

    }
}
