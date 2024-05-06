using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;


public class OptionViewHelper : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public Color normalColor;
    public Color highlightedColor;

    private bool lastSelected=false;
    public bool selected=false;

    private OptionView optionView;

    private void Start() {
        optionView=GetComponent<OptionView>();
    }
    private void LateUpdate() {
        if(lastSelected && !selected){
            OptionView[] optionViews=FindObjectsOfType<OptionView>();
            bool optionSelected=false;
            foreach(var optionView in optionViews){
                if(optionView.GetComponent<OptionViewHelper>().selected){
                    optionSelected=true;
                    break;
                }
            }
            if(!optionSelected){
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            lastSelected=false;
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        optionView.targetGraphic.color=normalColor;
        lastSelected=true;
        selected=false;
        Debug.Log("deselect");
    }

    public void OnSelect(BaseEventData data){
        optionView.targetGraphic.color=highlightedColor;
        selected=true;
        Debug.Log("select");
    }
}
