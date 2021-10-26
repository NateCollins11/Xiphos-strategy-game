using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Attack
{

    public string attackName;
    public bool isEnemy;
    public int damage;
    public float range;

    public List<string> modifiers;

    public string damageType;

    public float AOERadius;
    public string typeOfAttack;

    //So the plan in terms of dealing aoe damage is first to determine whether 
    public string aoeType = null;

    public bool isHeal;

    public List<(int x, int y)> pattern;

    public bool hoverSelectionOn = false;

    GameObject BoardObject;

    public Attack()
    {
        attackName = null;

    }

    public Attack(GameObject BoardObjectarg, bool isEnemyarg, string name, int damageargument, float rangeargument, List<string> modifiersarg, string damageTypearg, string typeOfAttackargument, float AOERadiusArgument = 0, List<(int x, int y)> patternArg = null, bool isHealarg = false)
    {
        attackName = name;
        isEnemy = isEnemyarg;
        damage = damageargument;
        range = rangeargument;
        modifiers = modifiersarg;
        damageType = damageTypearg;
        typeOfAttack = typeOfAttackargument;
        BoardObject = BoardObjectarg;
        AOERadius = AOERadiusArgument;
        pattern = patternArg;
        isHeal = isHealarg;

    }
    public List<(int x, int y, bool enemypresent)> DetermineAttackTiles(int xPos, int yPos)
    {
        Tilemap groundmap = BoardObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();

        var attackLocations = new List<(int x, int y, bool enemypresent)> { };
        if (typeOfAttack == "singleTargetMelee")
        {
            aoeType = null;

            for (int y = yPos + Mathf.CeilToInt(range); y >= yPos - Mathf.CeilToInt(range); y--)
            {
                for (int x = xPos + Mathf.CeilToInt(range); x >= xPos - Mathf.CeilToInt(range); x--)
                {

                    if (groundmap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        if (Mathf.Pow(y - yPos, 2) + Mathf.Pow(x - xPos, 2) <= Mathf.Pow((range), 2))
                        {
                            attackLocations.Add((x, y, DetermineHostilePresenceOnTile(x, y)));

                        }
                    }
                }
            }
            return attackLocations;
        }
        else if (typeOfAttack == "singleTargetRanged")
        {
            aoeType = null;
            for (int y = yPos + Mathf.CeilToInt(range); y >= yPos - Mathf.CeilToInt(range); y--)
            {
                for (int x = xPos + Mathf.CeilToInt(range); x >= xPos - Mathf.CeilToInt(range); x--)
                {
                    if (groundmap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        if (Mathf.Abs(y - yPos) > 1 || Mathf.Abs(x - xPos) > 1)
                        {
                            if (Mathf.Pow(y - yPos, 2) + Mathf.Pow(x - xPos, 2) <= Mathf.Pow((range), 2))
                            {
                                attackLocations.Add((x, y, DetermineHostilePresenceOnTile(x, y)));
                            }
                        }
                    }
                }
            }
            return attackLocations;
        }
        else if (typeOfAttack == "MeleeAOE")

        {
            aoeType = "all";
            for (int y = yPos + Mathf.CeilToInt(range); y >= yPos - Mathf.CeilToInt(range); y--)
            {
                for (int x = xPos + Mathf.CeilToInt(range); x >= xPos - Mathf.CeilToInt(range); x--)
                {
                    if (groundmap.HasTile(new Vector3Int(x, y, 0)))
                    {

                        if (Mathf.Pow(y - yPos, 2) + Mathf.Pow(x - xPos, 2) <= Mathf.Pow((range), 2))
                        {


                            attackLocations.Add((x, y, true));


                        }
                    }
                }
            }
            return attackLocations;

        }
        else if (typeOfAttack == "RemoteRadiusAOE") //still need to write this
        {
            aoeType = "radius";
            hoverSelectionOn = true;

            for (int y = yPos + Mathf.CeilToInt(range); y >= yPos - Mathf.CeilToInt(range); y--)
            {
                for (int x = xPos + Mathf.CeilToInt(range); x >= xPos - Mathf.CeilToInt(range); x--)
                {
                    if (groundmap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        if (Mathf.Abs(y - yPos) > 1 || Mathf.Abs(x - xPos) > 1)
                        {
                            if (Mathf.Pow(y - yPos, 2) + Mathf.Pow(x - xPos, 2) <= Mathf.Pow((range), 2))
                            {
                                // Debug.Log("figuring it out");
                                attackLocations.Add((x, y, false));

                            }
                        }

                    }
                }
            }

            return attackLocations;

        }

        else if (typeOfAttack == "DirectionalPattern")
        {
            aoeType = "pattern";
            hoverSelectionOn = true;

            foreach ((int x, int y) direction in new List<(int x, int y)> { (0, 1), (1, 0), (0, -1), (-1, 0) })
            {
                List<(int x, int y)> directionTiles = CreateDirectionTilesFromPattern(pattern, direction, (xPos, yPos));
                foreach ((int x, int y) tile in directionTiles)
                {
                    if (groundmap.HasTile(new Vector3Int(tile.x, tile.y, 0)))
                    {
                        attackLocations.Add((tile.x, tile.y, false));
                    }
                }

            }



            return attackLocations;
        }


        else
        {
            return null;
        }


    }
    public void AnimateAttack(GameObject target)
    {
        Unit selectedUnit = BoardObject.GetComponent<BoardController>().selectedEntity.GetComponent<Unit>();
        GameObject Effect = new GameObject();
        Effect.AddComponent<SpriteRenderer>();
        Effect.GetComponent<SpriteRenderer>().sortingLayerName = "Effects";
        Effect.AddComponent<EffectAnimator>();

        Effect.transform.parent = BoardObject.GetComponent<BoardController>().selectedEntity.gameObject.transform;
        if (attackName == "Eviscerate")
        {
            string direction = "";
            direction = DetermineDirectionOfSingleTargetCardinalAttack(selectedUnit, target);
            Effect.GetComponent<EffectAnimator>().SlashConstructor("Slash", direction, 64);
        }
        if (attackName == "Longshot" || attackName == "Fireball" || attackName == "Javelin")
        {

            Effect.GetComponent<EffectAnimator>().ProjectileConstructor(attackName, target, 80);


        }
    }

    public string DetermineDirectionOfSingleTargetCardinalAttack(Unit selectedUnit, GameObject target)
    {
        string direction = "";
        if (attackName == "Eviscerate")
        {
            if (target.gameObject.transform.position.x > selectedUnit.gameObject.transform.position.x)
            {
                direction = "right";
            }
            if (target.gameObject.transform.position.x < selectedUnit.gameObject.transform.position.x)
            {
                direction = "left";
            }
            if (target.gameObject.transform.position.y > selectedUnit.gameObject.transform.position.y)
            {
                direction = "top";
            }
            if (target.gameObject.transform.position.y < selectedUnit.gameObject.transform.position.y)
            {
                direction = "bottom";
            }

        }
        return direction;
    }

    public bool DetermineHostilePresenceOnTile(int x, int y)
    {
        if (BoardObject.GetComponent<BoardController>().boardData[x, y] != null)
        {

            //now add a "hostile" condition if the attack belongs to the enemy
            if ((!isEnemy && !isHeal) || (isEnemy && isHeal))
            {
                if (BoardObject.GetComponent<BoardController>().boardData[x, y].CompareTag("Enemy"))
                {
                    return true;
                }
            }
            else if ((isEnemy && !isHeal) || (!isEnemy && isHeal))
            {
                if (BoardObject.GetComponent<BoardController>().boardData[x, y].CompareTag("Unit"))
                {
                    return true;
                }
            }
        }

        return false;
    }



    public List<(int x, int y)> CreateDirectionTilesFromPattern(List<(int x, int y)> pattern, (int x, int y) direction, (int x, int y) position)
    {
        List<(int x, int y)> tileList = new List<(int x, int y)> { };

        foreach ((int x, int y) patternTile in pattern)
        {

            if (direction == (0, 1))
                tileList.Add((patternTile.x + position.x, patternTile.y + position.y));
            else if (direction == (1, 0))
                tileList.Add((patternTile.y + position.x, patternTile.x * -1 + position.y));
            else if (direction == (0, -1))
                tileList.Add((patternTile.x * -1 + position.x, patternTile.y * -1 + position.y));
            else if (direction == (-1, 0))
                tileList.Add((patternTile.y * -1 + position.x, patternTile.x + position.y));
        }


        return tileList;
    }

    public (int x, int y) FindCardinalDirection((int x, int y) charPosition, (float x, float y) mousePosition)
    {
        if (Mathf.Abs(mousePosition.x - charPosition.x) > Mathf.Abs(mousePosition.y - charPosition.y))
        {
            if (mousePosition.x > charPosition.x)
                return (1, 0);
            else if (mousePosition.x < charPosition.x)
                return (-1, 0);
        }
        else
        {
            if (mousePosition.y > charPosition.y)
                return (0, 1);
            else if (mousePosition.y < charPosition.y)
                return (0, -1);
        }
        return (-1, 0);



    }

}



