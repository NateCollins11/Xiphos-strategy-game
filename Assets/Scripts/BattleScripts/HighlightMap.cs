using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightMap : MonoBehaviour
{


    public Tilemap groundMap;
    public Tilemap highlightMap;


    public TileBase HighlightTile;
    public TileBase AttackHighlightTile;
    public TileBase NoAttackHighlightTile;

    public TileBase HealHighlightTile;
    public TileBase NoHealHighlightTile;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public List<(int x, int y)> HighlightMovementSquares(int xPos, int yPos, float movementSpeed)
    {

        ClearHighlights();
        var movementLocations = new List<(int x, int y)> { };
        float distToHighestRow = movementSpeed;

        for (int y = yPos + (int)distToHighestRow; y >= yPos - distToHighestRow; y--)
        {
            for (int x = xPos + (int)distToHighestRow; x >= xPos - distToHighestRow; x--)
            {
                if (groundMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    if (Mathf.Pow(y - yPos, 2) + Mathf.Pow(x - xPos, 2) <= Mathf.Pow((distToHighestRow), 2))
                    {
                        if (gameObject.GetComponent<BoardController>().boardData[x, y] == null)
                        {
                            highlightMap.SetTile(new Vector3Int(x, y, 0), HighlightTile);
                            movementLocations.Add((x, y));

                        }

                    }
                }
            }
        }
        return movementLocations;
    }


    public void HighlightAttackSquares(List<(int x, int y, bool enemypresent)> Tilelist, Attack atk)
    {
        ClearHighlights();

        TileBase PosHighlightTile;
        TileBase NegHighlightTile;

        if (atk.isHeal)
        {
            PosHighlightTile = HealHighlightTile;
            NegHighlightTile = NoHealHighlightTile;
        }
        else
        {
            PosHighlightTile = AttackHighlightTile;
            NegHighlightTile = NoAttackHighlightTile;
        }



        foreach ((int x, int y, bool enemypresent) in Tilelist)
        {
            if (enemypresent)
            {
                HighlightSingleTile(x, y, PosHighlightTile);
            }
            else
            {
                HighlightSingleTile(x, y, NegHighlightTile);
            }

        }

    }

    public List<(int x, int y)> UpdateHoverSelection(float xPos, float yPos, Unit selectedUnit)
    {
        TileBase PosHighlightTile;
        TileBase NegHighlightTile;

        if (selectedUnit.preparedAttack.isHeal)
        {
            PosHighlightTile = HealHighlightTile;
            NegHighlightTile = NoHealHighlightTile;
        }
        else
        {
            PosHighlightTile = AttackHighlightTile;
            NegHighlightTile = NoAttackHighlightTile;
        }


        List<(int x, int y)> selectionTiles = new List<(int x, int y)> { };
        HighlightAttackSquares(selectedUnit.attackLocations, selectedUnit.preparedAttack);

        if (selectedUnit.preparedAttack.aoeType == "radius")
        {

            for (int y = (int)yPos + (int)selectedUnit.preparedAttack.AOERadius; y >= (int)yPos - (int)selectedUnit.preparedAttack.AOERadius; y--)
            {
                for (int x = (int)xPos + (int)selectedUnit.preparedAttack.AOERadius; x >= (int)xPos - (int)selectedUnit.preparedAttack.AOERadius; x--)
                {
                    if (groundMap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        if (Mathf.Pow(y - (int)yPos, 2) + Mathf.Pow(x - (int)xPos, 2) <= Mathf.Pow((selectedUnit.preparedAttack.AOERadius), 2))
                        {

                            highlightMap.SetTile(new Vector3Int(x, y, 0), PosHighlightTile);
                            selectionTiles.Add((x, y));



                        }
                    }
                }
            }
        }
        else if (selectedUnit.preparedAttack.aoeType == "pattern")
        {
            (int x, int y) direction = selectedUnit.preparedAttack.FindCardinalDirection(((int)selectedUnit.gameObject.transform.position.x, (int)selectedUnit.gameObject.transform.position.y), (xPos - .5f, yPos - .5f));
            Debug.Log((Input.mousePosition.x, Input.mousePosition.y));

            List<(int x, int y)> patternTiles = selectedUnit.preparedAttack.CreateDirectionTilesFromPattern(selectedUnit.preparedAttack.pattern, direction, ((int)selectedUnit.gameObject.transform.position.x, (int)selectedUnit.gameObject.transform.position.y));

            foreach ((int x, int y) tile in patternTiles)
            {
                if (groundMap.HasTile(new Vector3Int(tile.x, tile.y, 0)))
                {

                    highlightMap.SetTile(new Vector3Int(tile.x, tile.y, 0), PosHighlightTile);
                    selectionTiles.Add((tile.x, tile.y));


                }
            }


        }
        return selectionTiles;

    }

    public void ClearHighlights()
    {
        highlightMap.ClearAllTiles();



    }


    public void HighlightSingleTile(int x, int y, TileBase tile)
    {

        highlightMap.SetTile(new Vector3Int(x, y, 0), tile);

    }
}
