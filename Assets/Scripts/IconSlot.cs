using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSlot : MonoBehaviour
{

    public PartyManager partyManager;
    public UnitIcon unitContained = null;
    public UpgradeScript upgradeScript;
    public bool isUpgradeSlot = false;





    void Start()
    {

        partyManager = gameObject.transform.parent.parent.parent.GetComponent<PartyManager>();


    }


    // Update is called once per frame
    public void ClickEvent()

    {

        if (unitContained == null || unitContained == partyManager.pickedUpUnit)
        {
            partyManager.ClickedOnSlot(this);
        }

    }

    public void GainIcon(UnitIcon icon)
    {

        ToggleRaycast(false);

    }

    public void LoseIcon()
    {

        ToggleRaycast(true);
        if (isUpgradeSlot)
            upgradeScript.LoseUnit();


    }

    public void ToggleRaycast(bool raycastOn)
    {
        gameObject.GetComponent<Image>().raycastTarget = raycastOn;

    }

}
