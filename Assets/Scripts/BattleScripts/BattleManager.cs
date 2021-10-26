using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is attached to the Board GameObject and manages battle logic and events, such as battle set-up and ending as well as turns

public class BattleManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameObject nextTurnButton;
    public GameObject UnitObject;
    public GameObject EnemyObject;
    public static SavesInfo savesDidWin;
    public BoardController boardControl;
    public List<GameObject> CharacterListByTurnOrder;
    public int turnWithinCycle = 0;


    public IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {

        boardControl = gameObject.GetComponent<BoardController>();

        savesDidWin = new SavesInfo();



        SetUpUnitsForBattle();
        SetUpEnemiesForBattle();

        CharacterListByTurnOrder = DetermineTurnOrder();


        // this coroutine is necessary because the first turn cannot start until

        coroutine = WaitAndPrint(0.05f);
        StartCoroutine(coroutine);

    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
        NextTurn();
    }





    public void DealWithDestoyedCharacters()
    {


    }

    public void SetUpUnitsForBattle()
    {

        Party unitParty;

        // in case party not yet available (if partyManagement was skipped), create basic party

        if (PartyManager.playerParty == null)
        {
            Debug.Log("Default Party created because no party can be found");
            unitParty = new Party("basic");

        }
        else
        {
            unitParty = PartyManager.playerParty;
        }

        //Unit GameObjects will be created from unitDatas and place on bottom row starting at tempPlacePosition
        int temporaryPlacingPosition = 4;
        int index = 0;
        foreach (UnitData unitData in unitParty.ActiveUnits)
        {
            //likely imperfect way of setting up unit gameObjects

            GameObject unitObject = Instantiate(UnitObject);
            unitObject.transform.parent = gameObject.transform;
            unitObject.name = unitData.unitName;

            unitObject.GetComponent<Character>().charStats = unitData.unitStats;
            unitObject.GetComponent<Character>().characterName = unitData.unitName;


            unitObject.GetComponent<Unit>().StartTriggered();
            unitObject.GetComponent<Unit>().unitIndex = index;

            unitObject.GetComponent<Character>().StartTriggered();

            unitObject.transform.position = (new Vector3(temporaryPlacingPosition + index, 0, 0));
            unitObject.GetComponent<SpriteAnimator>().SetUpInBattle(unitData.unitName);

            index++;
        }


    }


    public void SetUpEnemiesForBattle()
    {
        //creates list of enemy name strings based on levelID

        List<string> enemySet;

        if (PartyManager.playerParty != null)
        {
            enemySet = CreateEnemySquad(PartyManager.playerParty.levelID);

        }
        else
        {
            enemySet = CreateEnemySquad(1);
        }

        //then uses those strings to instantiate and set up enemy game Objects

        foreach (string enemy in enemySet)
        {
            GameObject enemyObject = Instantiate(EnemyObject);

            enemyObject.transform.parent = gameObject.transform;
            enemyObject.GetComponent<Character>().isEnemy = true;
            enemyObject.GetComponent<Character>().characterName = enemy;

            enemyObject.GetComponent<Enemy>().StartTriggered();
            enemyObject.GetComponent<Enemy>().SetEnemyStats(enemy, true);

            enemyObject.GetComponent<Character>().StartTriggered();

            // provide random coordinate in the top two rows of battlefield
            (int x, int y) randomPosition = ((int)Random.Range(2, 10), (int)Random.Range(10, 12));

            while (boardControl.boardData[randomPosition.x, randomPosition.y] != null)
            {
                randomPosition = ((int)Random.Range(2, 10), (int)Random.Range(10, 11));
            }
            // Debug.Log(randomPosition);
            enemyObject.transform.position = (new Vector3(randomPosition.x, randomPosition.y, 0));
            boardControl.boardData[randomPosition.x, randomPosition.y] = enemyObject;
            enemyObject.GetComponent<SpriteAnimator>().SetUpInBattle(enemy);

        }

    }

    public void EndAllTurns()
    {
        //this function simply makes it so that when a new turn starts, no other character is able to act

        foreach (GameObject character in CharacterListByTurnOrder)
        {
            if (character.CompareTag("Unit"))
            {
                character.GetComponent<Unit>().movementRemaining = 0;
                character.GetComponent<Unit>().ActionPoints = 0;
            }
            else
            {
            }

        }



    }


    public void NextTurn()
    {

        //make it so other units are not able to do stuff
        EndAllTurns();



        if (turnWithinCycle < CharacterListByTurnOrder.Count)
        {
            Debug.Log("Turn within cycle: " + turnWithinCycle + ", so it is the turn of " + CharacterListByTurnOrder[turnWithinCycle].gameObject.name);

            //keep moving to the next turn until someone is alive, or until the cycle restarts
            while (!CharacterListByTurnOrder[turnWithinCycle].GetComponent<Character>().isAlive)
            {

                turnWithinCycle++;
                if (turnWithinCycle >= CharacterListByTurnOrder.Count)
                {
                    EndRound(false);
                }
            }

            boardControl.UnselectEverything();

            if (CharacterListByTurnOrder[turnWithinCycle].CompareTag("Unit"))
            {
                CharacterListByTurnOrder[turnWithinCycle].GetComponent<Unit>().TakeTurn();
            }
            else if (CharacterListByTurnOrder[turnWithinCycle].CompareTag("Enemy"))
            {
                CharacterListByTurnOrder[turnWithinCycle].GetComponent<Enemy>().TakeTurn();
            }
            turnWithinCycle++;
        }
        else
        {

            EndRound(true);
        }




    }


    public void EndRound(bool initiateNextTurn)
    {

        turnWithinCycle = 0;

        if (initiateNextTurn)
            NextTurn();


    }


    public List<GameObject> DetermineTurnOrder()
    {
        var unsortedCharacterList = new List<GameObject> { };
        for (int i = 0; i < boardControl.transform.childCount; i++)
        {
            if (boardControl.transform.GetChild(i).CompareTag("Unit") || boardControl.transform.GetChild(i).CompareTag("Enemy"))
            {
                unsortedCharacterList.Add(boardControl.transform.GetChild(i).gameObject);
            }

        }
        var CharacterList = new List<GameObject> { };
        for (int initiativeLevel = 10; initiativeLevel >= 0; initiativeLevel--)
        {
            foreach (GameObject character in unsortedCharacterList)
            {
                if (character.GetComponent<Character>().charStats.initiative == initiativeLevel)
                {
                    CharacterList.Add(character);
                }

            }


        }
        return CharacterList;

    }


    public void CheckIfAllEnemiesDead()
    {
        bool shouldEndBattle = true;

        foreach (GameObject character in CharacterListByTurnOrder)
        {
            if (character.CompareTag("Enemy"))
            {
                if (character.GetComponent<Character>().isAlive)
                {
                    shouldEndBattle = false;
                }
            }
        }
        if (shouldEndBattle == true)
        {
            savesDidWin.didwin = true;
            Debug.Log("all enemies vanquished");
            EndBattle();
        }
    }
    public void CheckIfAllUnitsDead()
    {
        bool shouldEndBattle = true;

        foreach (GameObject character in CharacterListByTurnOrder)
        {
            if (character.CompareTag("Unit"))
            {
                if (character.GetComponent<Character>().isAlive)
                {
                    shouldEndBattle = false;
                }
            }
        }
        if (shouldEndBattle == true)
        {
            Debug.Log("all allies vanquished");
            savesDidWin.didwin = false;
            EndBattle();
        }
    }


    public void EndBattle()
    {

        PartyManager.playerParty.levelID++;
        StartCoroutine(levelLoader.LoadNextScene("Rewards"));

    }

    public List<string> CreateEnemySquad(int levelID)
    {

        List<string> enemyList;

        if (levelID == 1)
            enemyList = new List<string> { "BirdMonster", "BirdMonster", "BirdMonster" };
        else if (levelID == 2)
            enemyList = new List<string> { "BirdMonster", "BirdMonster", "Boglin", "Boglin" };
        else if (levelID == 3)
            enemyList = new List<string> { "BirdMonster", "BirdMonster", "BirdMonster", "Boglin", "Boglin", "Boglin" };
        else if (levelID == 4)
            enemyList = new List<string> { "Minotaur", "Boglin", "Boglin" };
        else if (levelID == 4)
            enemyList = new List<string> { "Minotaur", "Minotaur", "Boglin", "Boglin", "BirdMonster", "BirdMonster" };
        else
        {
            enemyList = new List<string> { "Minotaur" };

        }


        return enemyList;
    }


}

public class SavesInfo
{
    public bool didwin;

}
