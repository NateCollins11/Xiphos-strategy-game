using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isEnemy;

    public Unit unitScript;

    public Enemy enemyScript;

    public Stats charStats;

    public int hitPoints;

    public string characterName;

    public GameObject BoardObject;
    public GameObject TextEffect;

    public GameObject CharInfoPanel;

    public bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int ApplyDefenseModifiers(int damage, string damageType)
    {
        float modifiedDMG = 0;
        if (damageType == "physical")
            modifiedDMG = damage * (1.4f - .1f * charStats.toughness);
        if (damageType == "magic")
            modifiedDMG = damage * (1.8f - .175f * charStats.resistance);

        return (int)modifiedDMG;
    }

    public void TakeDamage(int damage, string damageType)
    {
        int modifiedDamage = ApplyDefenseModifiers(damage, damageType);

        hitPoints -= modifiedDamage;

        AnimateText("damage", modifiedDamage, damageType);

        if (hitPoints <= 0)
        {
            Die();
        }
        gameObject.transform.GetChild(0).GetComponent<HealthBar>().DrawHealthBar((int)charStats.maxHP, hitPoints, isEnemy);

    }

    public void GetHealed(int healing, string damageType)
    {

        //making sure that healing doesnt surpass max 
        int modifiedHealing = Mathf.Min((int)(charStats.maxHP - hitPoints), (int)(healing));

        hitPoints += modifiedHealing;

        AnimateText("healing", modifiedHealing, damageType);

        gameObject.transform.GetChild(0).GetComponent<HealthBar>().DrawHealthBar((int)charStats.maxHP, hitPoints, isEnemy);

    }



    public void Die()
    {

        BoardObject.GetComponent<BoardController>().boardData[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y] = null;
        isAlive = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }
        BoardObject.GetComponent<BattleManager>().CheckIfAllUnitsDead();
        BoardObject.GetComponent<BattleManager>().CheckIfAllEnemiesDead();


    }

    public void AnimateText(string effectName, int intarg, string damageType)
    {
        GameObject textObject = Instantiate(TextEffect);
        textObject.transform.SetParent(gameObject.transform, false);

        string textvariable = "";
        if (effectName == "UP")
            textvariable = "+" + intarg + " UP";

        if (effectName == "damage")
        {
            textvariable = intarg.ToString();


            if (damageType == "physical")
            {
                textObject.GetComponent<TMPro.TextMeshPro>().color = new Color32(140, 20, 50, 255);
                textObject.GetComponent<TMPro.TextMeshPro>().outlineColor = new Color32(200, 60, 20, 255);
            }


            if (damageType == "magic")
            {
                textObject.GetComponent<TMPro.TextMeshPro>().color = new Color32(35, 14, 80, 255);
                textObject.GetComponent<TMPro.TextMeshPro>().outlineColor = new Color32(125, 35, 220, 255);
            }


        }
        if (effectName == "healing")
        {
            textvariable = intarg.ToString();

            textObject.GetComponent<TMPro.TextMeshPro>().color = new Color32(40, 140, 50, 255);
            textObject.GetComponent<TMPro.TextMeshPro>().outlineColor = new Color32(60, 180, 70, 255);

        }

        textObject.GetComponent<TMPro.TextMeshPro>().text = textvariable;
        Destroy(textObject, 1.5f);

    }

    public int ApplyStatModifier(Attack atk)
    {
        Debug.Log(atk.damage);
        float dmg = atk.damage;
        foreach (string mod in atk.modifiers)

        //determine which stat the attack uses and then apply that modification
        {
            // define how damage scales differently for each stat
            if (mod == "strength")
                dmg *= (.6f + .1f * charStats.strength);
            if (mod == "dexterity")
                dmg *= (.4f + .125f * charStats.dexterity);
            if (mod == "potency")
                dmg *= (.2f + .15f * charStats.potency);

        }
        return (int)dmg;
    }
    public void RefreshPosition()
    {

        if (isAlive)
            BoardObject.GetComponent<BoardController>().boardData[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y] = gameObject;

    }

    public void Unselect()
    {

        // tell Animator Script to use non-highlighted pngs
        gameObject.GetComponent<SpriteAnimator>().isOutlined = false;
        BoardObject.GetComponent<HighlightMap>().ClearHighlights();
        CharInfoPanel.SetActive(false);

        if (!isEnemy)
            unitScript.inAttackMode = false;
    }

    public void BecomeSelected()
    {
        if (isEnemy)
            enemyScript.BecomeSelected();

        else
            unitScript.BecomeSelected();



    }

    public void Move(int DestX, int DestY)
    {

        //Subtract distance moved from moveRemaining
        if (!isEnemy)
            unitScript.movementRemaining = unitScript.movementRemaining - (Mathf.Sqrt(Mathf.Pow(DestX - gameObject.transform.position.x, 2) + Mathf.Pow(DestY - gameObject.transform.position.y, 2)));

        //Update BoardData to reflect location change
        BoardObject.GetComponent<BoardController>().boardData[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y] = null;
        BoardObject.GetComponent<BoardController>().boardData[DestX, DestY] = gameObject;

        //Change Location of Object
        gameObject.transform.position = new Vector3(DestX, DestY, 0);

        BoardObject.GetComponent<HighlightMap>().ClearHighlights();


    }

    public void StartTriggered()
    {
        BoardObject = transform.parent.gameObject;
        CharInfoPanel = GameObject.Find("CharInfoPanel");

        if (isEnemy)
        {
            enemyScript = gameObject.GetComponent<Enemy>();
        }

        else
        {
            unitScript = gameObject.GetComponent<Unit>();
        }

        hitPoints = (int)charStats.maxHP;

        gameObject.transform.GetChild(0).GetComponent<HealthBar>().DrawHealthBar((int)charStats.maxHP, hitPoints, isEnemy);
        // AssignAttacksByType();

    }

}
