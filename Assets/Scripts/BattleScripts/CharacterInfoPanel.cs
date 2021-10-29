using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    public Tooltip tooltipObject;

    public GameObject Board;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Board.GetComponent<BoardController>().selectedEntity == null)
        {
            gameObject.SetActive(false);
        }
    }



    public void UpdateInformation(string unitType, string unitName, int maxHP, int currentHP, int movementRemaining, Attack attack1, Attack attack2, Attack attack3)
    {
        gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = unitName;
        gameObject.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = " " + currentHP + "/" + maxHP;


        if (attack1 != null)
        {
            gameObject.transform.GetChild(5).gameObject.SetActive(true);
            gameObject.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = attack1.attackName;
            TooltipComponent tooltip = gameObject.transform.GetChild(5).gameObject.GetComponent<TooltipComponent>();
            tooltip.enabled = true;
            tooltip.tooltipComp = tooltipObject;

            tooltip.tooltip = attack1.tooltipText;
        }   
        else
        {
            gameObject.transform.GetChild(5).gameObject.SetActive(false);

        }
        if (attack2 != null)
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(true);
            gameObject.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = attack2.attackName;
            TooltipComponent tooltip = gameObject.transform.GetChild(6).gameObject.GetComponent<TooltipComponent>();
            tooltip.enabled = true;
            tooltip.tooltipComp = tooltipObject;

            tooltip.tooltip = attack2.tooltipText;
        }
        else
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(false);

        }
        if (attack3 != null)
        {
            gameObject.transform.GetChild(7).gameObject.SetActive(true);
            gameObject.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = attack3.attackName;
            TooltipComponent tooltip = gameObject.transform.GetChild(7).gameObject.GetComponent<TooltipComponent>();
            tooltip.enabled = true;
            tooltip.tooltipComp = tooltipObject;
            tooltip.tooltip = attack3.tooltipText;
        }
        else
        {
            gameObject.transform.GetChild(7).gameObject.SetActive(false);

        }


        if (unitType == "Unit")
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            if (movementRemaining >= 1)
            {
                gameObject.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = "Movement: ";
                gameObject.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = movementRemaining.ToString();
            }
            else
            {

                gameObject.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = "No Movement Remaining ";
                gameObject.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }

        }
        else if (unitType == "Enemy")
        {
            gameObject.GetComponent<Image>().color = new Color32(221, 124, 124, 255);


            gameObject.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = "Special Abilities:";
            gameObject.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "";

        }








    }
}
