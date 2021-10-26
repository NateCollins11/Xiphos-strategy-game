using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{


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



    public void UpdateInformation(string unitType, string unitName, int maxHP, int currentHP, int movementRemaining, string attack1Name, string attack2Name, string attack3Name)
    {
        gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = unitName;
        gameObject.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = " " + currentHP + "/" + maxHP;


        if (attack1Name != null)
        {
            gameObject.transform.GetChild(5).gameObject.SetActive(true);
            gameObject.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = attack1Name;
        }
        else
        {
            gameObject.transform.GetChild(5).gameObject.SetActive(false);

        }
        if (attack2Name != null)
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(true);
            gameObject.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = attack2Name;
        }
        else
        {
            gameObject.transform.GetChild(6).gameObject.SetActive(false);

        }
        if (attack3Name != null)
        {
            gameObject.transform.GetChild(7).gameObject.SetActive(true);
            gameObject.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = attack3Name;
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
