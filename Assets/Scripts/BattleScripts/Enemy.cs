using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{

    public GameObject BoardObject;

    public Character charScript;

    public string unitName;



    public List<Attack> attacks = new List<Attack> { };



    public void StartTriggered()
    {
        BoardObject = transform.parent.gameObject;
        // CharInfoPanel = GameObject.Find("CharInfoPanel");
        charScript = gameObject.GetComponent<Character>();



        // hitPoints = (int)enemyStats.maxHP;

        AssignAttacksByType();

    }


    void Update()
    {

    }

    public void TakeTurn()
    {
        if (charScript.isAlive)
        {
            BoardObject.GetComponent<BattleManager>().nextTurnButton.GetComponent<Button>().interactable = false;
            Debug.Log("Scary Enemy Warning!!: " + unitName);
            BoardObject.GetComponent<BoardController>().selectedEntity = gameObject;
            BecomeSelected();
            AIForAttack();

        }
        else
        {
            Debug.Log(unitName + " is dead -- moving along");


        }


    }


    public void BecomeSelected()
    {
        // tell Animator Script to use highlighted pngs
        gameObject.GetComponent<SpriteAnimator>().isOutlined = true;

        charScript.CharInfoPanel.SetActive(true);
        charScript.CharInfoPanel.GetComponent<CharacterInfoPanel>().UpdateInformation("Enemy", unitName, (int)charScript.charStats.maxHP, charScript.hitPoints, 0, null, null, null);

    }



    public class TurnOption
    {
        public (int x, int y) moveCoords;

        public Attack attack;
        public (int x, int y) AttackCoords;

        public List<(int x, int y)> AOETiles;

        public bool targetsHostile;

        public TurnOption((int x, int y) moveCoordsarg, Attack attackarg, (int x, int y) AttackCoordsarg, bool targetsHostilearg)
        {
            moveCoords = moveCoordsarg;
            attack = attackarg;
            AttackCoords = AttackCoordsarg;
            targetsHostile = targetsHostilearg;

        }

    }
    public IEnumerator waitCoroutine;
    public IEnumerator attackCoroutine;
    public IEnumerator endTurnCoroutine;
    public void AIForAttack()
    {

        List<TurnOption> OptionList = new List<TurnOption> { };


        List<(int x, int y)> moveLocationList = BoardObject.GetComponent<HighlightMap>().HighlightMovementSquares((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, charScript.charStats.speed);
        foreach ((int x, int y) moveOption in moveLocationList)
        {
            foreach (Attack atk in attacks)
            {
                //oh shit this gonna need a lot of work
                if (atk.typeOfAttack == "singleTargetMelee" || atk.typeOfAttack == "singleTargetRanged")
                {
                    List<(int x, int y, bool enemypresent)> attackLocationList = atk.DetermineAttackTiles((int)moveOption.x, (int)moveOption.y);

                    foreach ((int x, int y, bool enemypresent) attackLocation in attackLocationList)
                    {


                        OptionList.Add(new TurnOption(moveOption, atk, (attackLocation.x, attackLocation.y), attackLocation.enemypresent));
                        // Debug.Log(moveOption);
                        // Debug.Log(attackLocation);

                    }
                }
                else if (atk.typeOfAttack == "DirectionalPattern")
                {

                    foreach ((int x, int y) cardDir in new List<(int x, int y)> { (0, 1), (1, 0), (0, -1), (-1, 0) })
                    {
                        TurnOption directionOption = new TurnOption(moveOption, atk, (moveOption.x + cardDir.x, moveOption.y + cardDir.y), false);
                        directionOption.AOETiles = atk.CreateDirectionTilesFromPattern(atk.pattern, cardDir, moveOption);
                        OptionList.Add(directionOption);


                    }





                }
            }
        }
        //At this point the option list has been completed, so now I will rate all the options and then carry out
        List<TurnOption> TiedOptions = new List<TurnOption> { };
        bool isATie = false;
        TurnOption chosenOption = null;
        float highestRating = 0;

        foreach (TurnOption option in OptionList)
        {

            float viabilityRating = CalculateRating(option);
            if (chosenOption == null)
            {
                chosenOption = option;
                highestRating = viabilityRating;
            }
            else
            {
                if (viabilityRating == highestRating)
                {
                    isATie = true;
                    TiedOptions.Add(option);
                    highestRating = viabilityRating;
                }
                else if (viabilityRating > highestRating)
                {
                    isATie = false;
                    chosenOption = option;
                    highestRating = viabilityRating;
                    TiedOptions.Clear();
                }
            }
        }

        if (isATie)
        {
            //pick one of the tied options at random
            int randToSelectOption = (int)Random.Range(0, TiedOptions.Count - 1);
            chosenOption = TiedOptions[randToSelectOption];

        }

        // Debug.Log(chosenOption.attack);
        // Debug.Log(highestRating);
        // Debug.Log(chosenOption.moveCoords);
        // Debug.Log(chosenOption.AttackCoords);
        Debug.Log(chosenOption.moveCoords);
        waitCoroutine = WaitAndMove(0.8f, chosenOption);
        StartCoroutine(waitCoroutine);
        attackCoroutine = WaitAndAttack(1.6f, chosenOption);
        StartCoroutine(attackCoroutine);
        endTurnCoroutine = WaitAndEndTurn(1.8f);
        StartCoroutine(endTurnCoroutine);



    }


    // Start is called before the first frame update




    private IEnumerator WaitAndMove(float waitTime, TurnOption chosenOption)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log(chosenOption.moveCoords);
        charScript.Move(chosenOption.moveCoords.x, chosenOption.moveCoords.y);

    }
    private IEnumerator WaitAndAttack(float waitTime, TurnOption chosenOption)
    {
        yield return new WaitForSeconds(waitTime);
        if (chosenOption.targetsHostile)
        {
            if (chosenOption.AOETiles == null)
            {
                BoardObject.GetComponent<BoardController>().boardData[chosenOption.AttackCoords.x, chosenOption.AttackCoords.y].GetComponent<Character>().TakeDamage(chosenOption.attack.damage, chosenOption.attack.damageType);
            }
            else
            {
                foreach ((int x, int y) tile in chosenOption.AOETiles)
                {
                    if (chosenOption.attack.DetermineHostilePresenceOnTile(tile.x, tile.y))
                        BoardObject.GetComponent<BoardController>().boardData[tile.x, tile.y].GetComponent<Character>().TakeDamage(chosenOption.attack.damage, chosenOption.attack.damageType);

                }
            }


            chosenOption.attack.AnimateAttack(BoardObject.GetComponent<BoardController>().boardData[chosenOption.AttackCoords.x, chosenOption.AttackCoords.y]);

        }
    }
    private IEnumerator WaitAndEndTurn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BoardObject.GetComponent<BattleManager>().nextTurnButton.GetComponent<Button>().interactable = true;
        BoardObject.GetComponent<BattleManager>().NextTurn();


    }

    //This is basically where the behavior of enemies will be determined. Desired patters must be incentivized by increased rating.
    public float CalculateRating(TurnOption option)
    {
        float rating = 0;
        if (option.AOETiles == null)
        {
            if (option.targetsHostile)
                rating += option.attack.damage;
        }
        else
        {
            foreach ((int x, int y) tile in option.AOETiles)
            {
                if (BoardObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>().HasTile(new Vector3Int(tile.x, tile.y, 0)))
                {
                    if (option.attack.DetermineHostilePresenceOnTile(tile.x, tile.y))
                    {
                        rating += option.attack.damage;
                        option.targetsHostile = true;

                    }
                }
            }
        }

        if (!option.targetsHostile)
        {
            int distanceFromNearestHostile = 1000;
            foreach (GameObject character in BoardObject.GetComponent<BattleManager>().CharacterListByTurnOrder)
            {
                if (character.CompareTag("Unit"))
                {
                    int distance = ((int)(Mathf.Abs(option.moveCoords.x - character.gameObject.transform.position.x) + Mathf.Abs(option.moveCoords.y - character.gameObject.transform.position.y)));

                    if (distance < distanceFromNearestHostile)
                    {
                        distanceFromNearestHostile = distance;
                    }
                }

            }
            rating -= distanceFromNearestHostile;
        }
        // Debug.Log(distanceFromNearestHostile);

        return rating;
    }


    public void AssignAttacksByType()
    {
        if (charScript.characterName == "BirdMonster")
        {
            attacks.Add(new Attack(BoardObject, true, "Thwack", 35, 1.6f, new List<string> { "strength" }, "physical", "singleTargetMelee"));
            attacks.Add(new Attack(BoardObject, true, "Javelin", 25, 2.5f, new List<string> { "dexterity" }, "physical", "singleTargetRanged"));

        }
        if (charScript.characterName == "Boglin")
        {
            attacks.Add(new Attack(BoardObject, true, "Slash", 50, 1, new List<string> { "strength", "dexterity" }, "physical", "singleTargetMelee"));
            attacks.Add(new Attack(BoardObject, true, "Hatchet Throw", 25, 2.5f, new List<string> { "dexterity" }, "physical", "singleTargetRanged"));

        }
        if (charScript.characterName == "Minotaur")
        {
            attacks.Add(new Attack(BoardObject, true, "Gore", 60, 1, new List<string> { "strength" }, "physical", "singleTargetMelee"));
            attacks.Add(new Attack(BoardObject, true, "HornThrust", 45, 0, new List<string> { "strength" }, "physical", "DirectionalPattern", patternArg: new List<(int x, int y)> { (0, 1), (1, 1), (-1, 1), (0, 2), (1, 2), (-1, 2) }));

        }



    }



    public void SetEnemyStats(string name, bool isBasic)
    {
        unitName = name;
        gameObject.name = name;
        if (isBasic)
        {
            if (name == "BirdMonster")
            {
                charScript.charStats = new Stats(MaxHParg: 150, maxMovementarg: 3, initiativearg: 3, maxActionPointsarg: 1, strengtharg: 3, dexterityarg: 3, potencyarg: 2, physicalDefensearg: 3, magicDefensearg: 3);
            }
            else if (name == "Boglin")
            {
                charScript.charStats = new Stats(MaxHParg: 210, maxMovementarg: 4, initiativearg: 4, maxActionPointsarg: 1, strengtharg: 4, dexterityarg: 4, potencyarg: 3, physicalDefensearg: 3, magicDefensearg: 3);
            }
            else if (name == "Minotaur")
            {
                charScript.charStats = new Stats(MaxHParg: 320, maxMovementarg: 2, initiativearg: 1, maxActionPointsarg: 1, strengtharg: 5, dexterityarg: 2, potencyarg: 1, physicalDefensearg: 4, magicDefensearg: 2);
            }

        }

    }
}
