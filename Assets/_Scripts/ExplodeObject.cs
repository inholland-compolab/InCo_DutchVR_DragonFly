using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplodeObject : MonoBehaviour
{
    public float scale = 1;
    public ExplodingItem[] explodingItems;
    // Start is called before the first frame update
    void Start(){
        SetupItems();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            ExplodeAllItems();
        }else if(Input.GetKeyDown(KeyCode.R)){
            ResetAllItems();
        }else if(Input.GetKeyDown(KeyCode.Space)){
            ResetQuick();
        }
    }

    public void SetupItems(){
        foreach (ExplodingItem item in explodingItems){
            item.startPosition = item.transform.localPosition;
        }      
    }

    public void ExplodeAllItems(){
        foreach (ExplodingItem item in explodingItems){
            item.transform.DOLocalMove(item.MoveToPosition,0.5f).SetRelative(true);
        }
    }

    public void ResetAllItems(){
        foreach (ExplodingItem item in explodingItems){
            item.transform.DOLocalMove(item.startPosition,0.5f);
        }
    }

    public void ResetQuick(){
        foreach (ExplodingItem item in explodingItems){
            item.transform.localPosition = item.startPosition;
        }
    }

}

[System.Serializable]
public class ExplodingItem{
    public Transform transform;
    public Vector3 startPosition;
    public Vector3 MoveToPosition;
}
