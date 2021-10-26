using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Transform fillbar;

    public Sprite UnitFillSprite;
    public Sprite EnemyFillSprite;

    public Sprite HealthBarOutlineGood;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame






    public void DrawHealthBar(int maxhealth, float remaininghealth, bool isEnemy)
    {
        if (!isEnemy)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = UnitFillSprite;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = HealthBarOutlineGood;
        }
        if (isEnemy)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = EnemyFillSprite;
        }

        Transform fillbar = gameObject.transform.GetChild(1);

        fillbar.localScale = new Vector3((remaininghealth / maxhealth) * .8832f, 0.1f, 0f);

    }
}
