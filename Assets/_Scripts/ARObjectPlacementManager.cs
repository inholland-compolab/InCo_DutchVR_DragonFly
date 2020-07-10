using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class ARObjectPlacementManager : MonoBehaviour
{
    public Transform camera;
    public GameObject prefab;
    public Transform content;
    // public bool _canPlace = true;
    public ARPlaneManager arPlaneManager;
    public enum PlacementStates {place, rotate, correct};
    public PlacementStates currentPlaceMentState = PlacementStates.place;
    public RotateObject rotateObject;
    public ScaleObject scaleObject;
    public TextMeshProUGUI StateInfoText;
    private ExplodeObject explodeObject;
    private GameObject objectCanvas;

    public bool _IsInitialized = false;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        rotateObject = GetComponent<RotateObject>();
        scaleObject = GetComponent<ScaleObject>();
    }
    void Start() {
        camera = m_SessionOrigin.camera.transform;
    }

    private void OnEnable() {
        currentPlaceMentState = PlacementStates.place;
        ShowActivePlane();
    }

    private void OnDisable() {
        hideVisualPlanes();
    }

    void Update()
    {
        if (Input.touchCount == 0 )
            return;

        var touch = Input.GetTouch(0);
        
        if(IsPointerOverGameObject()){
            return;
        }

        if(currentPlaceMentState == PlacementStates.place){
            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon)) {
                if(content == null)
                    return;

                if(_IsInitialized == false){
                    InitializePlacement();
                }

                var hitPose = s_Hits[0].pose;

                Vector3 newRot = Vector3.zero;
                newRot = content.eulerAngles;
                // content.LookAt(camera);

                // newRot = new Vector3(newRot.x,content.eulerAngles.y+180,newRot.z);

                content.position = hitPose.position;
                // content.eulerAngles = newRot;
            }
        }else if(currentPlaceMentState == PlacementStates.rotate){
            if(rotateObject != null){
                if(rotateObject._canMove == false){
                    rotateObject.transformToRotate = content;
                    rotateObject._canMove = true;
                }

                if(scaleObject != null){
                    if(scaleObject._canScale == false){
                        scaleObject.scalableObject = content;
                        scaleObject._canScale = true;
                    }
                }
            }else{
                currentPlaceMentState = PlacementStates.correct;
            }
        }else if(currentPlaceMentState == PlacementStates.correct){
            if(rotateObject != null){
                if(rotateObject._canMove == true){
                    rotateObject.transformToRotate = content;
                    rotateObject._canMove = false;
                }
            }

            if(scaleObject != null){
                if(scaleObject._canScale == true){
                    scaleObject.scalableObject = content;
                    scaleObject._canScale = false;
                }
            }
        }

        Debug.Log(currentPlaceMentState.ToString());
    }

    public void AcceptPlacement(){
        currentPlaceMentState = PlacementStates.correct;
        hideVisualPlanes();
        SetFeedBackText("");
        if (objectCanvas != null){
            objectCanvas.SetActive(true);
        }
    }

    public void SetRotation(){
        currentPlaceMentState = PlacementStates.rotate;
        SetFeedBackText("Rotate the Dragonfly to swipe, scale the Dragonfly to pinch.");
        // if(explodeObject != null){
        //     explodeObject.SetupItems();
        // }
    }

    public void ResetPlacement(){
        if(explodeObject != null){
            explodeObject.ResetQuick();
        }
        if (objectCanvas != null){
            objectCanvas.SetActive(false);
        }
        currentPlaceMentState = PlacementStates.place;
        ShowActivePlane();
        SetFeedBackText("Tap on the floor plane to move the Dragonfly.");
    }

    // public void Move(){

    // }

    public void hideVisualPlanes(){
        if(arPlaneManager ==null){
            return;
        }

        foreach (ARPlane plane in arPlaneManager.trackables) {
            plane.gameObject.SetActive(false);
        }

        arPlaneManager.enabled = false;
    }

    public void ShowActivePlane(){
        if(arPlaneManager ==null){
            return;
        }

        arPlaneManager.enabled = true;
        
        foreach (ARPlane plane in arPlaneManager.trackables) {
            plane.gameObject.SetActive(true);
        }
    }

    public bool IsPointerOverGameObject(){
        //check mouse
        if(EventSystem.current.IsPointerOverGameObject())
            return true;
             
            //check touch
        if(Input.touchCount > 0 ) {
            if(EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }
             
        return false;
    } 
    
    public void InitializePlacement(){
        if(prefab != null){
            GameObject tempGo = Instantiate(prefab,content);
            _IsInitialized = true;
            ExplodeObject tempExpObj = tempGo.GetComponent<ExplodeObject>();
            if(tempExpObj != null){
                explodeObject = tempExpObj;
            }

            SetFeedBackText("Tap on the floor plane to move the Dragonfly.");

            GameObject tempCanvas = GameObject.FindGameObjectWithTag("PlaneCanvas");

            if (tempCanvas != null){
                objectCanvas = tempCanvas;
                if (objectCanvas != null){
                    objectCanvas.SetActive(false);
                }
            }
        }
    }

    public void SetFeedBackText(string text){
        if(StateInfoText!=null){
            StateInfoText.text = text;
        }
    }
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARSessionOrigin m_SessionOrigin;
    ARRaycastManager m_RaycastManager;
}
