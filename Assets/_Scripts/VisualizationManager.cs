using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationManager : MonoBehaviour
{
    public VisualizationModeObject[] visualizationModeObjects;
    public int CurrentModeSelected = -1;
    public SetPositionRotation setPositionRotation;

    // Start is called before the first frame update
    void Start() {
        CurrentModeSelected = 0;
        TogglePanel(0);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TogglePanel(int idToToggle){
        // if(CurrentModeSelected != -1){
        //     DisableMode();
        // }
        // if(idToToggle == CurrentModeSelected){
        //     CurrentModeSelected = -1;
        //     // FadeOut();
        // }else{
            foreach (VisualizationModeObject visModeItem in visualizationModeObjects) {
                if(idToToggle == visModeItem.id){
                    DisableMode();

                    CurrentModeSelected = idToToggle;
                    SetMode(visModeItem);
                    break;
                }
            }
        // }
    }

    private void DisableMode(){
        if(CurrentModeSelected == -1){
            return;
        }

        VisualizationModeObject tempItem = visualizationModeObjects[CurrentModeSelected];
        if(tempItem.UIGameobject != null){
            tempItem.UIGameobject.SetActive(false);
        }

        if(tempItem.SceneObject != null){
            tempItem.SceneObject.SetActive(false);
        }

        // if(setPositionRotation != null){
        //     setPositionRotation.DisableAirflow();
        // }
    }

    private void SetMode(VisualizationModeObject item){
        if(item.UIGameobject != null){
            item.UIGameobject.SetActive(true);
        }

        if(item.SceneObject != null){
            item.SceneObject.SetActive(true);
        }
    }

}
[System.Serializable]
public class VisualizationModeObject{
    public int id;
    public GameObject UIGameobject;
    public GameObject SceneObject;
}