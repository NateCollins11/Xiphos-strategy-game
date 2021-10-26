using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : MonoBehaviour
{
    public int turnNumber;
    public int boardSizex;
    public int boardSizey;
    public GameObject[,] boardData;
    public Camera cam;
    public GameObject selectedEntity = null;
    public Tilemap groundMap;
    public List<(int x, int y)> hoverSelectionTiles;


    // Start is called before the first frame update
    void Start()
    {

        boardSizex = groundMap.cellBounds.xMax;
        boardSizey = groundMap.cellBounds.yMax;
        boardData = new GameObject[boardSizex, boardSizey];
        turnNumber = 1;


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            RefreshCharacterPositions();

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);


            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit == true)
            {
                GameObject clickedOnBoard = hit.collider.gameObject;
            }

            int mousePosx = Mathf.FloorToInt(mousePos.x);
            int mousePosy = Mathf.FloorToInt(mousePos.y);

            if (hit == true)
            {
                if (hoverSelectionTiles != null)
                {
                    selectedEntity.GetComponent<Unit>().ExecuteAttack(null);


                }
                else if (boardData[mousePosx, mousePosy] != null)
                {
                    GameObject clickedOnObject = boardData[mousePosx, mousePosy];

                    if (clickedOnObject.CompareTag("Unit"))
                    {
                        if (selectedEntity != null)
                        {

                            selectedEntity.GetComponent<Character>().Unselect();

                        }

                        if (selectedEntity != clickedOnObject)
                        {
                            selectedEntity = clickedOnObject;
                            clickedOnObject.GetComponent<Unit>().BecomeSelected();
                        }
                        else
                        {

                            selectedEntity = null;

                        }


                    }
                    if (clickedOnObject.CompareTag("Enemy"))
                    {


                        if (selectedEntity != null)
                        {
                            if (selectedEntity.CompareTag("Unit"))
                            {
                                if (selectedEntity.GetComponent<Unit>().inAttackMode == true)
                                {
                                    if (selectedEntity.GetComponent<Unit>().attackLocations.Contains((mousePosx, mousePosy, true)))
                                    {
                                        selectedEntity.GetComponent<Unit>().ExecuteAttack(clickedOnObject);
                                        selectedEntity.GetComponent<Unit>().inAttackMode = false;
                                        gameObject.GetComponent<HighlightMap>().ClearHighlights();
                                    }


                                    selectedEntity.GetComponent<Character>().Unselect();
                                    selectedEntity.GetComponent<Unit>().BecomeSelected();


                                }

                                else
                                {
                                    selectedEntity.GetComponent<Character>().Unselect();
                                    clickedOnObject.GetComponent<Enemy>().BecomeSelected();
                                    selectedEntity = clickedOnObject;
                                }

                            }
                            else if (selectedEntity.CompareTag("Enemy"))
                            {
                                selectedEntity.GetComponent<Character>().Unselect();
                                selectedEntity = clickedOnObject;
                                clickedOnObject.GetComponent<Character>().BecomeSelected();


                            }
                        }
                        else
                        {
                            selectedEntity = clickedOnObject;
                            clickedOnObject.GetComponent<Enemy>().BecomeSelected();

                        }
                    }
                }
                else if (boardData[mousePosx, mousePosy] == null)
                {


                    if (selectedEntity != null)
                    {
                        if (selectedEntity.CompareTag("Unit"))
                        {
                            if (selectedEntity.GetComponent<Unit>().inAttackMode == true)
                            {
                                if (selectedEntity.GetComponent<Unit>().attackLocations.Contains((mousePosx, mousePosy, true)))
                                {
                                    selectedEntity.GetComponent<Unit>().ExecuteAttack(null);
                                }

                            }
                            else
                            {
                                if (selectedEntity.GetComponent<Unit>().movementLocations.Contains((mousePosx, mousePosy)))
                                {
                                    selectedEntity.GetComponent<Character>().Move(mousePosx, mousePosy);
                                }
                                selectedEntity.GetComponent<Character>().Unselect();
                                gameObject.GetComponent<HighlightMap>().ClearHighlights();
                                selectedEntity = null;
                            }

                        }
                    }
                }
            }
        }

        if (selectedEntity != null)
        {
            if (selectedEntity.CompareTag("Unit"))
            {
                if (selectedEntity.GetComponent<Unit>().inAttackMode)
                {
                    if (selectedEntity.GetComponentInParent<Unit>().preparedAttack.hoverSelectionOn)
                    {
                        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

                        if (selectedEntity.GetComponent<Unit>().attackLocations.Contains(((int)mousePos.x, (int)mousePos.y, false)))
                        {
                            hoverSelectionTiles = gameObject.GetComponent<HighlightMap>().UpdateHoverSelection(mousePos.x, mousePos.y, selectedEntity.GetComponent<Unit>());
                        }
                        else
                        {
                            gameObject.GetComponent<HighlightMap>().HighlightAttackSquares(selectedEntity.GetComponent<Unit>().attackLocations, selectedEntity.GetComponent<Unit>().preparedAttack);
                            hoverSelectionTiles = null;
                        }
                    }
                }
            }
        }
    }

    public void RefreshCharacterPositions()
    {

        GameObject[] UnitList = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject Unit in UnitList)
        {
            Unit.GetComponent<Character>().RefreshPosition();
        }

        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Enemy in EnemyList)
        {
            Enemy.GetComponent<Character>().RefreshPosition();
        }


    }




    public void NextTurn()
    {
        turnNumber++;

        //Reset the remaining movement for each Unit
        GameObject[] UnitList = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject Unit in UnitList)
        {
            Unit.GetComponent<Unit>().movementRemaining = Unit.GetComponent<Character>().charStats.speed;

        }
        UnselectEverything();

    }


    public void UnselectEverything()
    {

        if (selectedEntity != null)

            selectedEntity.GetComponent<Character>().Unselect();


        selectedEntity = null;



    }

    public void SelectedUnitPrepareAttack1()
    {
        selectedEntity.GetComponent<Unit>().PrepareAttack(1);
    }
    public void SelectedUnitPrepareAttack2()
    {
        selectedEntity.GetComponent<Unit>().PrepareAttack(2);
    }
    public void SelectedUnitPrepareAttack3()
    {
        selectedEntity.GetComponent<Unit>().PrepareAttack(3);
    }



}
