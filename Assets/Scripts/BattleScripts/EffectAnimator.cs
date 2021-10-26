using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimator : MonoBehaviour
{

    public int animationTimer = 0;
    public int animationLength;
    public float[] destination = { 0, 0 };
    public float[] origin = { 0, 0 };

    public string animationType;
    public string direction;

    public bool HasAnimation = false;



    public List<Sprite> spriteList = new List<Sprite> { };


    public void SlashConstructor(string nameArg, string directionArg, int animationLengthArg)
    {
        HasAnimation = true;
        animationType = "singleTileDirectional";
        gameObject.name = nameArg;
        direction = directionArg;
        animationLength = animationLengthArg;


        for (int i = 0; i < Resources.LoadAll(gameObject.name + "Spritesheet/").GetLength(0); i++)
        {
            spriteList.Add(Resources.LoadAll(gameObject.name + "Spritesheet/")[i] as Sprite);
        }

        if (direction == "bottom")
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 0));
            gameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
        }
        if (direction == "top")
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 180f));
            gameObject.transform.localPosition = new Vector3(1f, 1.5f, 0);
        }
        if (direction == "right")
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 90f));
            gameObject.transform.localPosition = new Vector3(1.5f, 0f, 0);
        }
        if (direction == "left")
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 270f));
            gameObject.transform.localPosition = new Vector3(-.5f, 1, 0);
        }
    }


    public void ProjectileConstructor(string nameArg, GameObject target, int animationLengthArg)
    {
        HasAnimation = true;
        animationType = "projectile";
        gameObject.name = nameArg;
        animationLength = animationLengthArg;
        for (int i = 1; i < Resources.LoadAll(gameObject.name + "Spritesheet/").GetLength(0); i++)
        {

            spriteList.Add(Resources.LoadAll(gameObject.name + "Spritesheet/")[i] as Sprite);
        }



        destination[0] = target.transform.position.x;
        destination[1] = target.transform.position.y;
        origin[0] = gameObject.transform.parent.position.x;
        origin[1] = gameObject.transform.parent.position.y;

        OrientRotationToTarget();


    }
    void Start()
    {


    }

    void Update()
    {
        if (HasAnimation)
        {

            gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[Mathf.FloorToInt((((float)animationTimer / (animationLength + 1)) * spriteList.Count))];
            animationTimer++;

            if (animationTimer > animationLength)
            {
                Destroy(gameObject);
                // animationTimer = 0;
            }

            if (animationType == "projectile")
            {
                gameObject.transform.localPosition = new Vector3(((destination[0] - origin[0]) * ((float)animationTimer / animationLength)) + 0.5f, ((destination[1] - origin[1]) * ((float)animationTimer / animationLength)) + 0.5f, 0);

            }
        }

    }

    public void OrientRotationToTarget()
    {

        Vector3 difference = new Vector3(destination[0], destination[1], 0) - new Vector3(origin[0], origin[1], 0);
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);

    }

}
