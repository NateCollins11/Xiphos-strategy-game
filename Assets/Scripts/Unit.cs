using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public GameObject BoardObject;

    public Character charScript;
    public GameObject TextEffect;

    public int unitIndex;



    public float movementRemaining;


    public List<(int x, int y)> movementLocations;
    public List<(int x, int y, bool enemypresent)> attackLocations;
    public bool inAttackMode;
    // public int maxHP = 100;
    public int ActionPoints = 0;
    public GameObject CharInfoPanel;
    public Attack Attack1;
    public Attack Attack2;
    public Attack Attack3;

    public Attack preparedAttack;

    public GameObject Effect;






    // Start is called before the first frame update
    public void StartTriggered()
    {
        BoardObject = transform.parent.gameObject;
        charScript = gameObject.GetComponent<Character>();

        CharInfoPanel = GameObject.Find("CharInfoPanel");

        charScript.hitPoints = (int)charScript.charStats.maxHP;

        gameObject.transform.GetChild(0).GetComponent<HealthBar>().DrawHealthBar((int)charScript.charStats.maxHP, charScript.hitPoints, false);


        AssignAttacksByClass();
    }




    // Update is called once per frame
    void Update()
    {

    }



    public void TakeTurn()
    {
        if (charScript.isAlive)
        {
            movementRemaining = charScript.charStats.speed;
            ActionPoints = 1;

            BoardObject.GetComponent<BoardController>().selectedEntity = gameObject;
            BecomeSelected();
            Debug.Log("Friendly Turn: " + charScript.characterName + "(is alive)");
        }
        else
        {
            Debug.Log(charScript.characterName + " is dead :<");
        }

    }




    public void BecomeSelected()
    {
        // tell Animator Script to use highlighted pngs
        gameObject.GetComponent<SpriteAnimator>().isOutlined = true;
        movementLocations = BoardObject.GetComponent<HighlightMap>().HighlightMovementSquares((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, movementRemaining);
        CharInfoPanel.SetActive(true);
        CharInfoPanel.GetComponent<CharacterInfoPanel>().UpdateInformation("Unit", charScript.characterName, (int)charScript.charStats.maxHP, charScript.hitPoints, (int)movementRemaining, Attack1.attackName, Attack2.attackName, Attack3.attackName);

    }







    public void PrepareAttack(int attackNumber)
    {
        if (ActionPoints > 0)
        {
            Attack atk;
            if (attackNumber == 1)
            {
                atk = Attack1;
            }
            else if (attackNumber == 2)
            {
                atk = Attack2;
            }
            else
            {
                atk = Attack3;
            }

            attackLocations = atk.DetermineAttackTiles((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
            gameObject.transform.parent.GetComponent<HighlightMap>().HighlightAttackSquares(attackLocations, atk);
            preparedAttack = atk;
            inAttackMode = true;
        }
    }


    public void ExecuteAttack(GameObject target)
    {
        if (preparedAttack.aoeType == null)
        {


            DealDamageToEnemy(preparedAttack, target.GetComponent<Character>());

        }
        else if (preparedAttack.aoeType == "all")
        {

            foreach ((int x, int y, bool enemypresent) tile in attackLocations)
            {

                if (BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y] != null)
                {
                    // Debug.Log("looking for enemy");
                    if (BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].tag == "Enemy")
                    {
                        DealDamageToEnemy(preparedAttack, BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].GetComponent<Character>());

                    }
                }

            }


        }
        else if (preparedAttack.aoeType == "radius")
        {

            foreach ((int x, int y) tile in BoardObject.GetComponent<BoardController>().hoverSelectionTiles)
            {

                if (BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y] != null)
                {
                    // Debug.Log("looking for enemy");
                    if (BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].tag == "Enemy")
                    {
                        DealDamageToEnemy(preparedAttack, BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].GetComponent<Character>());

                    }
                }
            }
        }
        else if (preparedAttack.aoeType == "pattern")
        {

            foreach ((int x, int y) tile in BoardObject.GetComponent<BoardController>().hoverSelectionTiles)
            {

                if (BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y] != null)
                {
                    // Debug.Log("looking for enemy");
                    if (preparedAttack.DetermineHostilePresenceOnTile(tile.x, tile.y))
                    {
                        DealDamageToEnemy(preparedAttack, BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].GetComponent<Character>());

                    }
                }
            }
        }

        preparedAttack.AnimateAttack(target);
        preparedAttack = null;
        BoardObject.GetComponent<BoardController>().hoverSelectionTiles = null;
        movementLocations = BoardObject.GetComponent<HighlightMap>().HighlightMovementSquares((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, movementRemaining);
        inAttackMode = false;
        ActionPoints--;

    }





    public void DealDamageToEnemy(Attack atk, Character target)
    {

        if (atk.isHeal)
        {
            Debug.Log("Healing should be :" + charScript.ApplyStatModifier(atk));
            target.GetHealed(charScript.ApplyStatModifier(atk), atk.damageType);
        }
        else
        {
            target.TakeDamage(charScript.ApplyStatModifier(atk), atk.damageType);
        }

        Debug.Log(target.characterName + " now has " + target.hitPoints + " health");

        if (target.hitPoints <= 0)
        {
            PartyManager.playerParty.ActiveUnits[unitIndex].unitStats.upgradePointsGainedThisBattle++;


            charScript.AnimateText("UP", 1, atk.damageType);
        }

    }



    public void AssignAttacksByClass()
    {
        if (charScript.characterName == "Archer")
        {
            Attack1 = new Attack(BoardObject, false, "Powershot", 55, 4, new List<string> { "strength", "dexterity" }, "physical", "singleTargetRanged");
            Attack2 = new Attack(BoardObject, false, "Longshot", 30, 6, new List<string> { "dexterity" }, "physical", "singleTargetRanged");
            Attack3 = new Attack(BoardObject, false, "Arrow Stab", 20, 1, new List<string> { "strength", "dexterity" }, "physical", "singleTargetMelee");

        }

        if (charScript.characterName == "Knight")
        {

            Attack1 = new Attack(BoardObject, false, "Eviscerate", 85, 1, new List<string> { "strength" }, "physical", "singleTargetMelee");
            Attack2 = new Attack(BoardObject, false, "Slash", 40, 1.6f, new List<string> { "strength" }, "physical", "MeleeAOE");
            Attack3 = new Attack(BoardObject, false, "Javelin", 40, 2, new List<string> { "dexterity" }, "physical", "singleTargetMelee");
        }

        if (charScript.characterName == "Mage")
        {

            Attack1 = new Attack(BoardObject, false, "Lightning", 40, 5, new List<string> { "potency" }, "magic", "RemoteRadiusAOE", AOERadiusArgument: 1);
            Attack2 = new Attack(BoardObject, false, "Fireball", 50, 4, new List<string> { "potency" }, "magic", "singleTargetRanged");
            Attack3 = new Attack(BoardObject, false, "Bonk", 15, 1.6f, new List<string> { "strength" }, "physical", "singleTargetMelee");

        }

        if (charScript.characterName == "Paladin")
        {

            Attack1 = new Attack(BoardObject, false, "HammerBlast", 35, 0, new List<string> { "strength", "potency" }, "physical", "DirectionalPattern", patternArg: new List<(int x, int y)> { (0, 1), (1, 1), (-1, 1), (0, 2), (0, 3) });
            Attack2 = new Attack(BoardObject, false, "Bless", 50, 0, new List<string> { "potency" }, "", "DirectionalPattern", patternArg: new List<(int x, int y)> { (0, 1), (0, 2) }, isHealarg: true);
            Attack3 = new Attack(BoardObject, false, "Pound", 50, 1.6f, new List<string> { "strength" }, "physical", "singleTargetMelee");

        }
        if (charScript.characterName == "Sorceress")
        {

            Attack1 = new Attack(BoardObject, false, "Arcane Ray", 35, 0, new List<string> { "potency" }, "magic", "DirectionalPattern", patternArg: new List<(int x, int y)> { (0, 1), (0, 2), (0, 3), (0, 4) });
            Attack2 = new Attack(BoardObject, false, "Overwhelm", 50, 2.5f, new List<string> { "potency" }, "magic", "MeleeAOE");
            Attack3 = new Attack(BoardObject, false, "placeholder", 50, 1.6f, new List<string> { "strength" }, "physical", "singleTargetMelee");

        }

    }





}
