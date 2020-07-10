using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionRotation : MonoBehaviour
{
    public AirflowObj[] airflowObjs;
    // Start is called before the first frame update
    void Start() {
        if(airflowObjs.Length > 0){
            AirflowObject[] AirflowObjects = FindObjectsOfType<AirflowObject>();
            foreach (AirflowObject airflowObject in AirflowObjects){
                airflowObjs[airflowObject.ID].airflowObject = airflowObject.transform;
                airflowObjs[airflowObject.ID].airFlowChildGO = airflowObject.childGo;
            }

            SetAirflow();
        }
    }

    void OnEnable(){
        Invoke("SetAirflow",0.1f);
        // SetAirflow();
    }

    void OnDisable() {
        DisableAirflow();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetAirflow(){
        foreach (AirflowObj airflowObj in airflowObjs){
            if(airflowObj.airFlowChildGO != null){
                airflowObj.airFlowChildGO.SetActive(true);
            }
            airflowObj.airflowObject.position = airflowObj.moveLocation.position;
            airflowObj.airflowObject.rotation = airflowObj.moveLocation.rotation;
        }
    }
    public void DisableAirflow(){
        foreach (AirflowObj airflowObj in airflowObjs){
            if(airflowObj.airFlowChildGO != null){
                airflowObj.airFlowChildGO.SetActive(false);
            }
        }
    }
}
[System.Serializable]
public class AirflowObj{
    public Transform airflowObject;
    public Transform moveLocation;
    public GameObject airFlowChildGO;
}
