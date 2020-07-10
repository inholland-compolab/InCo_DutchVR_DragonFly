using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    float startDist = 0f;
    Vector3 initialScale;
    float curDist = 0f;
    float distDelta = 0f;

    public Transform scalableObject;
    public bool _canScale;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Scale();
    }

    public void Scale(){
        if(_canScale){
            int fingersOnScreen = 0;
            // If there are two touches on the device...
            foreach (Touch touch in Input.touches)
            {
                fingersOnScreen++;
    
                if (fingersOnScreen == 2)
                {
                    //First set the initial distance between fingers so you can compare.
                    if (touch.phase == TouchPhase.Began)
                    {
                        startDist = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        initialScale = scalableObject.transform.localScale;
                    }
                    else
                    {
                        float currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                        float scaleFactor = currentFingersDistance / startDist;
                        scalableObject.transform.localScale = initialScale * scaleFactor;
                    }
                }
            }
        }
    }
}
