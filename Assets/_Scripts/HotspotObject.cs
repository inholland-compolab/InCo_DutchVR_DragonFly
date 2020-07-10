using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotspotObject : MonoBehaviour
{
    public UnityEvent ExecuteOnclick;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnMouseDown(){
        if(ExecuteOnclick != null){
            ExecuteOnclick.Invoke();
        }
    }

}
