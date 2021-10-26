using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{

    public Button nextLevelButton;
    public GameObject activeUnitsPanel;
    public GameObject inactiveUnitsPanel;

    public GameObject upgradePanel;
    public IconSlot upgradeSlot;
    public GameObject UnitIcon;
    public UnitIcon pickedUpUnit = null;
    public LevelLoader levelLoader;

    public static Party playerParty = null;



    // Start is called before the first frame update
    void Start()
    {
        if (playerParty == null)
            CreateBasicParty();


        int slotIndex = 0;
        foreach (UnitData activeUnit in playerParty.ActiveUnits)
        {


            GameObject unitIcon = Instantiate(UnitIcon);
            unitIcon.GetComponent<UnitIcon>().myUnitData = activeUnit;
            unitIcon.GetComponent<UnitIcon>().preupgradeUnitStats = new Stats(activeUnit.unitStats);
            unitIcon.GetComponent<UnitIcon>().inPartyManagement = true;


            unitIcon.transform.SetParent(activeUnitsPanel.transform.GetChild(slotIndex).GetChild(0), false);
            activeUnitsPanel.transform.GetChild(slotIndex).GetComponent<IconSlot>().unitContained = unitIcon.transform.GetComponent<UnitIcon>();
            unitIcon.transform.GetComponent<SpriteAnimator>().SetUpUnitInMenu(activeUnit.unitName);
            unitIcon.GetComponent<UnitIcon>().partyManager = this;

            slotIndex++;
        }

        SetUpUnitInventorySlots();
        SetUpUpgradeUnitSlot();

    }

    public void SetUpUnitInventorySlots()
    {
        int slotIndex = 0;
        foreach (UnitData inventoryUnit in playerParty.InventoryUnits)
        {
            GameObject unitIcon = Instantiate(UnitIcon);
            unitIcon.GetComponent<UnitIcon>().myUnitData = inventoryUnit;
            unitIcon.GetComponent<UnitIcon>().preupgradeUnitStats = new Stats(inventoryUnit.unitStats);
            unitIcon.GetComponent<UnitIcon>().inPartyManagement = true;


            unitIcon.transform.SetParent(inactiveUnitsPanel.transform.GetChild(slotIndex).GetChild(0), false);
            inactiveUnitsPanel.transform.GetChild(slotIndex).GetComponent<IconSlot>().unitContained = unitIcon.transform.GetComponent<UnitIcon>();
            unitIcon.transform.GetComponent<SpriteAnimator>().SetUpUnitInMenu(inventoryUnit.unitName);
            unitIcon.GetComponent<UnitIcon>().partyManager = this;

            slotIndex++;
        }


        for (int i = 0; i < inactiveUnitsPanel.transform.childCount - 1; i++)
        {
            if (inactiveUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().unitContained == null)
                inactiveUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().ToggleRaycast(true);

        }

    }

    public void SetUpUpgradeUnitSlot()
    {
        upgradeSlot = upgradePanel.transform.GetChild(0).GetComponent<IconSlot>();
        upgradeSlot.ToggleRaycast(true);
        upgradeSlot.isUpgradeSlot = true;


    }


    public void CreateBasicParty()
    {
        playerParty = new Party("basic");

    }

    public void PickUpIcon(UnitIcon unitIcon)
    {

        pickedUpUnit = unitIcon;

    }

    public void ClickedOnSlot(IconSlot slot)
    {

        if (slot.unitContained == pickedUpUnit)
        {
            if (slot.isUpgradeSlot)
                upgradePanel.GetComponent<UpgradeScript>().ReceiveUnit(pickedUpUnit);
            pickedUpUnit = null;
        }

        else if (slot.unitContained == null)
        {
            pickedUpUnit.gameObject.transform.parent.parent.GetComponent<IconSlot>().unitContained = null;
            pickedUpUnit.gameObject.transform.parent.parent.GetComponent<IconSlot>().ToggleRaycast(true);


            pickedUpUnit.gameObject.transform.SetParent(slot.gameObject.transform.GetChild(0), false);
            slot.unitContained = pickedUpUnit;

            if (slot.isUpgradeSlot)
                upgradePanel.GetComponent<UpgradeScript>().ReceiveUnit(pickedUpUnit);

            pickedUpUnit = null;

            RefreshPartyLists();
            RefreshContinueButtonAvailability();
        }



    }
    public void RefreshPartyLists()
    {
        playerParty.ActiveUnits.Clear();
        for (int i = 0; i < activeUnitsPanel.transform.childCount - 2; i++)
        {

            if (activeUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().unitContained != null)
                playerParty.ActiveUnits.Add(activeUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().unitContained.myUnitData);
        }
        playerParty.InventoryUnits.Clear();
        for (int i = 0; i < inactiveUnitsPanel.transform.childCount - 2; i++)
        {
            if (inactiveUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().unitContained != null)
                playerParty.InventoryUnits.Add(inactiveUnitsPanel.transform.GetChild(i).GetComponent<IconSlot>().unitContained.myUnitData);
        }



    }
    public void StartBattle()
    {
        StartCoroutine(levelLoader.LoadNextScene("Battle"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshContinueButtonAvailability()
    {
        if (upgradeSlot.unitContained != null)
        {
            nextLevelButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            nextLevelButton.GetComponent<Button>().interactable = true;
        }

    }

}

public class UnitData
{
    public string unitName;
    public Stats unitStats;

    public UnitData(string name, bool isBasic)
    {
        unitName = name;
        if (isBasic)
        {
            if (name == "Archer")
            {
                unitStats = new Stats(150, 4, 4, 1, 3, 6, 3, 3, 2);
            }
            else if (name == "Mage")
            {
                unitStats = new Stats(100, 3, 3, 1, 2, 3, 5, 1, 3);
            }
            else if (name == "Knight")
            {
                unitStats = new Stats(230, 2, 1, 1, 6, 2, 1, 4, 2);
            }
            else if (name == "Paladin")
            {
                unitStats = new Stats(200, 3, 6, 1, 4, 2, 4, 4, 3);
            }
            else if (name == "Sorceress")
            {
                unitStats = new Stats(90, 3, 4, 1, 1, 2, 4, 2, 4);
            }

        }


    }



}
// ok so i didn't realize that in order to work with prefabs, i cant use unnassigned monobehaviors. Instead, im making a new class, which will contain all of the information about a given unit, and then then the Party will be populated with THESE. When the battle is being set up. These UnitInfo instances will be copied to create the actual unit gameObjects

public class Party
{
    public int levelID;


    public List<UnitData> ActiveUnits = new List<UnitData> { };

    public List<UnitData> InventoryUnits = new List<UnitData> { };

    public Party(string basicParty)
    {
        if (basicParty == "basic")
        {
            ActiveUnits.Add(new UnitData("Archer", true));
            ActiveUnits.Add(new UnitData("Mage", true));
            ActiveUnits.Add(new UnitData("Paladin", true));
            InventoryUnits.Add(new UnitData("Sorceress", true));
            InventoryUnits.Add(new UnitData("Knight", true));

            levelID = 1;

        }

    }











}