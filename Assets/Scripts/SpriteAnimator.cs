using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{

    public Enemy enemyScript;
    public Unit unitScript;

    public Character charScript;
    public bool isUnit;

    public bool isOutlined = false;
    public List<Sprite> spriteList = new List<Sprite> { };
    public List<Sprite> spriteListH = new List<Sprite> { };


    public int animationTimer = 0;

    public string charName;

    public bool inMenu = false;


    // Start is called before the first frame update
    public void SetUpInBattle(string charNamearg)
    {

        charScript = gameObject.GetComponent<Character>();

        charName = charNamearg;
        isUnit = false;


        isUnit = true;

        Debug.Log(charName);


        for (int i = 0; i < 3; i++)
        {

            spriteList.Add(Resources.LoadAll(charName + "Spritesheet/")[i + 1] as Sprite);
            spriteListH.Add(Resources.LoadAll(charName + "HSpritesheet/")[i + 1] as Sprite);
        }


    }

    public void SetUpUnitInMenu(string unitName)
    {

        charName = unitName;
        inMenu = true;




        for (int i = 0; i < 3; i++)
        {

            spriteList.Add(Resources.LoadAll(charName + "Spritesheet/")[i + 1] as Sprite);
            spriteListH.Add(Resources.LoadAll(charName + "HSpritesheet/")[i + 1] as Sprite);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!isOutlined)
        {
            if (animationTimer < 30)
                ChangeImage(spriteList[0]);
            if (30 < animationTimer && animationTimer < 60)
                ChangeImage(spriteList[1]);
            if (animationTimer > 60 && animationTimer < 90)
                ChangeImage(spriteList[2]);
            if (animationTimer > 90)
                ChangeImage(spriteList[1]);
        }
        else
        {
            if (animationTimer < 30)
                ChangeImage(spriteListH[0]);
            if (30 < animationTimer && animationTimer < 60)
                ChangeImage(spriteListH[1]);
            if (animationTimer > 60 && animationTimer < 90)
                ChangeImage(spriteListH[2]);
            if (animationTimer > 90)
                ChangeImage(spriteListH[1]);


        }

        animationTimer++;
        if (animationTimer > 120)
            animationTimer = 0;
    }

    void ChangeImage(Sprite spr)
    {
        if (!inMenu)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spr;

        }
        else
        {
            gameObject.GetComponent<Image>().sprite = spr;

        }


    }
}
