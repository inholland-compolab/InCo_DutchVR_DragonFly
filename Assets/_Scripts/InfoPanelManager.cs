using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class InfoPanelManager : MonoBehaviour{
    public InfoObject[] infoObjects;
    public int CurrentPanelSelected = -1;
    public GameObject panelParent;
    public CanvasGroup canvasGroup;
    public GameObject textPanel;
    public TextMeshProUGUI titleTextInfo;
    public TextMeshProUGUI textTextInfo;
    public GameObject imgPanel;
    public TextMeshProUGUI titleImgInfo;
    public Image imageImgInfo;
    public GameObject graphPanel;
    public TextMeshProUGUI titleGraphInfo;
    // Start is called before the first frame update
    void Start(){
        if(panelParent != null){
            panelParent.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void TogglePanel(int idToToggle){
        if(CurrentPanelSelected != -1){
            FadeOut();
        }
        if(idToToggle == CurrentPanelSelected){
            CurrentPanelSelected = -1;
            // FadeOut();
        }else{
            foreach (InfoObject infoItem in infoObjects) {
                if(idToToggle == infoItem.id){
                    DisablePanels();

                    CurrentPanelSelected = idToToggle;

                    switch (infoItem.type){
                        case InfoObject.InfoTypes.TEXT:
                            SetTextPanel(infoItem.title, infoItem.infoText);                    
                            FadeIn();
                            break;
                        case InfoObject.InfoTypes.IMAGE:
                            SetImgPanel(infoItem.title, infoItem.image);                   
                            FadeIn();
                            break;
                        case InfoObject.InfoTypes.GRAPH:
                            SetGraphPanel(infoItem.title);                   
                            FadeIn();
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }
        }
    }

    private void FadeOut(){
        if(canvasGroup != null){
            canvasGroup.DOFade(0,0.5f).OnComplete(disableCanvas);
        }
    }
    private void disableCanvas(){
        if(panelParent == null){
            Debug.Log("Please set a panel parent");
            return;
        }
        panelParent.SetActive(false);
    }
    private void FadeIn(){
        if(canvasGroup != null){
            canvasGroup.DOFade(1,0.5f).OnComplete(enableCanvas);
        }
    }    
    private void enableCanvas(){
        if(panelParent == null){
            Debug.Log("Please set a panel parent");
            return;
        }
        panelParent.SetActive(true);
    }

    private void SetTextPanel(string newTitle, string newInfoText){
        if(textPanel != null){
            textPanel.SetActive(true);
        }
        if(titleTextInfo != null){
            titleTextInfo.text = newTitle;
        }

        if(textTextInfo != null){
            textTextInfo.text = newInfoText;
        }
    }

    private void SetImgPanel(string newTitle, Texture2D newTexture){
        if(imgPanel != null){
            imgPanel.SetActive(true);
        }
        if(titleImgInfo != null){
            titleImgInfo.text = newTitle;
        }

        // if(imageImgInfo != null){
        //     imageImgInfo.set = newTexture;
        // }
    }
    private void SetGraphPanel(string newTitle){
        if(graphPanel != null){
            graphPanel.SetActive(true);
        }
        if(titleGraphInfo != null){
            titleGraphInfo.text = newTitle;
        }

        // if(imageImgInfo != null){
        //     imageImgInfo.set = newTexture;
        // }
    }
    private void DisablePanels(){
        if(textPanel != null){
            textPanel.SetActive(false);
        }
        if(imgPanel != null){
            imgPanel.SetActive(false);
        }
        if(graphPanel != null){
            graphPanel.SetActive(false);
        }
    }
    public void CloseParent(){
        Debug.Log("close panel");
        CurrentPanelSelected = -1;
        FadeOut();
    }

}
[System.Serializable]
public class InfoObject{
    public string title;
    public int id;
    public enum InfoTypes
    {
        TEXT,IMAGE,GRAPH
    }
    public InfoTypes type = InfoTypes.TEXT;
    public Texture2D image;
    [TextArea]
    public string infoText;
}
