using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{

    public int upgradePoints;

    public UnitIcon unitIcon;
    public Stats unitStats;

    public Stats upgradeCosts;





    public GameObject hideableUpgradesPanel;
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI UP;
    public TMPro.TextMeshProUGUI HP;
    public TMPro.TextMeshProUGUI Speed;
    public TMPro.TextMeshProUGUI Strength;
    public TMPro.TextMeshProUGUI Dexterity;
    public TMPro.TextMeshProUGUI Potency;
    public TMPro.TextMeshProUGUI Toughness;
    public TMPro.TextMeshProUGUI Resistance;





    void Start()
    {
        hideableUpgradesPanel.SetActive(false);
    }

    public void ReceiveUnit(UnitIcon unitIconarg)
    {
        hideableUpgradesPanel.SetActive(true);
        unitIcon = unitIconarg;

        upgradeCosts = new Stats(unitIcon.myUnitData.unitStats, true);

        Name.text = unitIcon.myUnitData.unitName;

        UP.text = ("UP: " + ((int)unitIcon.myUnitData.unitStats.upgradePoints).ToString());

        HP.text = ((int)unitIcon.myUnitData.unitStats.maxHP).ToString();
        HP.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.maxHP.ToString();

        Speed.text = ((int)unitIcon.myUnitData.unitStats.speed).ToString();
        Speed.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.speed.ToString();

        Strength.text = ((int)unitIcon.myUnitData.unitStats.strength).ToString();
        Strength.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.strength.ToString();

        Dexterity.text = ((int)unitIcon.myUnitData.unitStats.dexterity).ToString();
        Dexterity.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.dexterity.ToString();

        Potency.text = ((int)unitIcon.myUnitData.unitStats.potency).ToString();
        Potency.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.potency.ToString();

        Toughness.text = ((int)unitIcon.myUnitData.unitStats.toughness).ToString();
        Toughness.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.toughness.ToString();

        Resistance.text = ((int)unitIcon.myUnitData.unitStats.resistance).ToString();
        Resistance.gameObject.transform.parent.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Cost: " + upgradeCosts.resistance.ToString();


        RefreshallStatChangeButtonAvailability(unitIconarg, (int)unitIcon.myUnitData.unitStats.upgradePoints);

    }

    public void LoseUnit()
    {
        hideableUpgradesPanel.SetActive(false);

    }

    public void RefreshallStatChangeButtonAvailability(UnitIcon unit, int upgradePoints)
    {
        float upgradeCost;
        float currentStat;
        float preupgradeStat;


        //for each stat-change panel, check for both of its buttons whether they can be validly pressed, and then make them uninteractible if not
        for (int i = 0; i < 7; i++)
        {
            upgradeCost = 1000;
            currentStat = 0;
            preupgradeStat = 0;
            if (i == 0)
            {
                upgradeCost = upgradeCosts.maxHP;
                currentStat = unit.myUnitData.unitStats.maxHP;
                preupgradeStat = unit.preupgradeUnitStats.maxHP;
            }
            if (i == 1)
            {
                upgradeCost = upgradeCosts.speed;
                currentStat = unit.myUnitData.unitStats.speed;
                preupgradeStat = unit.preupgradeUnitStats.speed;
            }
            if (i == 2)
            {
                upgradeCost = upgradeCosts.strength;
                currentStat = unit.myUnitData.unitStats.strength;
                preupgradeStat = unit.preupgradeUnitStats.strength;
            }
            if (i == 3)
            {
                upgradeCost = upgradeCosts.dexterity;
                currentStat = unit.myUnitData.unitStats.dexterity;
                preupgradeStat = unit.preupgradeUnitStats.dexterity;
            }
            if (i == 4)
            {
                upgradeCost = upgradeCosts.potency;
                currentStat = unit.myUnitData.unitStats.potency;
                preupgradeStat = unit.preupgradeUnitStats.potency;
            }
            if (i == 5)
            {
                upgradeCost = upgradeCosts.toughness;
                currentStat = unit.myUnitData.unitStats.toughness;
                preupgradeStat = unit.preupgradeUnitStats.toughness;
            }
            if (i == 6)
            {
                upgradeCost = upgradeCosts.resistance;
                currentStat = unit.myUnitData.unitStats.resistance;
                preupgradeStat = unit.preupgradeUnitStats.resistance;
            }


            if (upgradeCost > upgradePoints)
            {
                gameObject.transform.GetChild(1).GetChild(i).GetChild(1).GetComponent<Button>().interactable = false;
            }
            else
            {
                gameObject.transform.GetChild(1).GetChild(i).GetChild(1).GetComponent<Button>().interactable = true;
            }


            if (currentStat == preupgradeStat)
            {
                gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
            }
            else
            {
                gameObject.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
            }


        }



    }
    public void PositiveStatButtonPressed(int index)
    {
        AnyStatButtonPressed(index, -1);

    }
    public void NegativeStatButtonPressed(int index)
    {
        AnyStatButtonPressed(index, 1);
    }


    public void AnyStatButtonPressed(int index, int rightOrLeft)
    {
        float cost = upgradeCosts.maxHP;
        if (index == 1)
            cost = upgradeCosts.speed;
        if (index == 2)
            cost = upgradeCosts.strength;
        if (index == 3)
            cost = upgradeCosts.dexterity;
        if (index == 4)
            cost = upgradeCosts.potency;
        if (index == 5)
            cost = upgradeCosts.toughness;
        if (index == 6)
            cost = upgradeCosts.resistance;

        unitIcon.myUnitData.unitStats.upgradePoints += (cost * rightOrLeft);
        UP.text = ("UP: " + ((int)unitIcon.myUnitData.unitStats.upgradePoints).ToString());


        RefreshallStatChangeButtonAvailability(unitIcon, (int)unitIcon.myUnitData.unitStats.upgradePoints);
    }


    public void HPUp()
    {
        unitIcon.myUnitData.unitStats.maxHP += 10;
        HP.text = ((int)unitIcon.myUnitData.unitStats.maxHP).ToString();

    }

    public void HPDown()
    {
        unitIcon.myUnitData.unitStats.maxHP -= 10;
        HP.text = ((int)unitIcon.myUnitData.unitStats.maxHP).ToString();
    }

    public void SpeedUp()
    {
        unitIcon.myUnitData.unitStats.speed++;
        Speed.text = ((int)unitIcon.myUnitData.unitStats.speed).ToString();
    }

    public void SpeedDown()
    {
        unitIcon.myUnitData.unitStats.speed--;
        Speed.text = ((int)unitIcon.myUnitData.unitStats.speed).ToString();
    }
    public void StrengthUp()
    {
        unitIcon.myUnitData.unitStats.strength++;
        Strength.text = ((int)unitIcon.myUnitData.unitStats.strength).ToString();
    }

    public void StrengthDown()
    {
        unitIcon.myUnitData.unitStats.strength--;
        Strength.text = ((int)unitIcon.myUnitData.unitStats.strength).ToString();
    }
    public void DexterityUp()
    {
        unitIcon.myUnitData.unitStats.dexterity++;
        Dexterity.text = ((int)unitIcon.myUnitData.unitStats.dexterity).ToString();
    }

    public void DexterityDown()
    {
        unitIcon.myUnitData.unitStats.dexterity--;
        Dexterity.text = ((int)unitIcon.myUnitData.unitStats.dexterity).ToString();
    }

    public void PotencyUp()
    {
        unitIcon.myUnitData.unitStats.potency++;
        Potency.text = ((int)unitIcon.myUnitData.unitStats.potency).ToString();
    }

    public void PotencyDown()
    {
        unitIcon.myUnitData.unitStats.potency--;
        Potency.text = ((int)unitIcon.myUnitData.unitStats.potency).ToString();
    }
    public void ToughnessUp()
    {
        unitIcon.myUnitData.unitStats.toughness++;
        Toughness.text = ((int)unitIcon.myUnitData.unitStats.toughness).ToString();
    }

    public void ToughnessDown()
    {
        unitIcon.myUnitData.unitStats.toughness--;
        Toughness.text = ((int)unitIcon.myUnitData.unitStats.toughness).ToString();
    }

    public void ResistanceUp()
    {
        unitIcon.myUnitData.unitStats.resistance++;
        Resistance.text = ((int)unitIcon.myUnitData.unitStats.resistance).ToString();
    }

    public void ResistanceDown()
    {
        unitIcon.myUnitData.unitStats.resistance--;
        Resistance.text = ((int)unitIcon.myUnitData.unitStats.resistance).ToString();
    }



}
