using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAfterManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You Fart";
        if (BattleManager.savesDidWin.didwin == true)
        {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You Won";

        }
        else
        {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lost";
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
