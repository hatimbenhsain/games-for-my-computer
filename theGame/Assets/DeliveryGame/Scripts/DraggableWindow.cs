using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 ogPosition;

    private RectTransform rectTransform;
    private Vector2 offset;

    public bool shakeImmune=true;
    public bool scatterImmune=true;

    public Vector2 scatterTarget;
    public Vector2 currentOrigin;

    public Vector2 scatterDirection;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ogPosition=rectTransform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
        shakeImmune=true;
        scatterImmune=true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint - offset;
        }
    }

    private void Update() {
        if(Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.KeypadEnter)){
            ResetPosition();
        }
    }

    private void ResetPosition(){
        rectTransform.localPosition=ogPosition;
    }
}
