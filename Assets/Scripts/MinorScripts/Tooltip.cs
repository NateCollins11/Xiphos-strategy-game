using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

    public Camera uiCamera;
    public Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Start(){
        backgroundRectTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        tooltipText = gameObject.transform.GetChild(1).GetComponent<Text>();


        HideTooltip();
    }



    public void ShowTooltip(string tooltipString){
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 3f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;

    }

    public void HideTooltip(){
        gameObject.SetActive(false);
        

    }

    private void Update(){
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
}
