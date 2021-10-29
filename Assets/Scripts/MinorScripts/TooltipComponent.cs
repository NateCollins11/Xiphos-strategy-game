using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tooltip;
    public Tooltip tooltipComp;



    public void OnPointerEnter(PointerEventData eventData){

        tooltipComp.ShowTooltip(tooltip);

        // Debug.Log("im being moused over!!!");

    }

    public void OnPointerExit(PointerEventData eventData){

        tooltipComp.HideTooltip();

    }






    void Awake()
    {
        tooltipComp = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
