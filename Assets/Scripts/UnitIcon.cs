using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{


    public PartyManager partyManager;
    public UnitData myUnitData;

    public Stats preupgradeUnitStats;

    public bool inPartyManagement = false;



    void Start()
    {

        // partyManager = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<PartyManager>();


    }
    public void ClickEvent()
    {

        partyManager.PickUpIcon(this);

    }

    // Update is called once per frame
    void Update()
    {
        if (inPartyManagement)
        {
            if (partyManager.pickedUpUnit == this)
            {
                gameObject.transform.parent.parent.GetComponent<IconSlot>().LoseIcon();
                ToggleRaycast(false);
                gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            }
            else
            {
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
                ToggleRaycast(true);
            }
        }

    }

    public void ToggleRaycast(bool raycastOn)
    {
        gameObject.GetComponent<Image>().raycastTarget = raycastOn;

    }
}
