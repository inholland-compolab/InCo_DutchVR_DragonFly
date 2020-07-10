using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotationSpeed = 1;


    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up,  -RotationSpeed);
    }
}
