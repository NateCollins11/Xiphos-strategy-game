using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameObject UnitIcon;

    // Start is called before the first frame update
    void Start()
    {


        int slotIndex = 0;
        foreach (UnitData activeUnit in PartyManager.playerParty.ActiveUnits)
        {
            GameObject unitIcon = Instantiate(UnitIcon);
            unitIcon.transform.SetParent(gameObject.transform.GetChild(0).GetChild(slotIndex).GetChild(0), false);
            // gameObject.transform.GetChild(0).GetChild(slotIndex).GetComponent<IconSlot>().unitContained = unitIcon.transform.GetComponent<UnitIcon>();
            unitIcon.transform.GetComponent<SpriteAnimator>().SetUpUnitInMenu(activeUnit.unitName);
            unitIcon.GetComponent<Image>().raycastTarget = false;

            // unitIcon.GetComponent<UnitIcon>().partyManager = this;


            activeUnit.unitStats.upgradePointsGainedThisBattle++;
            gameObject.transform.GetChild(0).GetChild(slotIndex).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ("Upgrade Points: +" + activeUnit.unitStats.upgradePointsGainedThisBattle.ToString());

            activeUnit.unitStats.upgradePoints += activeUnit.unitStats.upgradePointsGainedThisBattle;

            activeUnit.unitStats.upgradePointsGainedThisBattle = 0;


            slotIndex++;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GoToPartyManager()
    {
        StartCoroutine(levelLoader.LoadNextScene("PartyManagement"));
    }

}
