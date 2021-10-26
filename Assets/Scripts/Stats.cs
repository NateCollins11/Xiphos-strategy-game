using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float upgradePoints = 0;
    public float upgradePointsGainedThisBattle = 0;
    public float maxHP;
    public float speed;
    public float initiative;
    public int maxActionPoints;

    public float strength;

    public float dexterity;
    public float potency;

    public float toughness;
    public float resistance;


    public Stats(float MaxHParg, float maxMovementarg, float initiativearg, int maxActionPointsarg, float strengtharg, float dexterityarg, float potencyarg, float physicalDefensearg, float magicDefensearg)
    {
        maxHP = MaxHParg;
        speed = maxMovementarg;
        initiative = initiativearg;
        maxActionPoints = maxActionPointsarg;
        strength = strengtharg;
        dexterity = dexterityarg;
        potency = potencyarg;

        toughness = physicalDefensearg;
        resistance = magicDefensearg;

    }

    //Making A cost Stateline for upgrading stats
    public Stats(Stats unitStats, bool isNormalCost)
    {
        maxHP = 1;
        if (unitStats.speed > 4)
            speed = 3;
        else
            speed = 2;

        if (unitStats.strength < 4)
            strength = 1;
        else if (unitStats.strength < 7)
            strength = 2;
        else if (unitStats.strength < 10)
            strength = 3;

        if (unitStats.dexterity < 4)
            dexterity = 1;
        else if (unitStats.dexterity < 7)
            dexterity = 2;
        else if (unitStats.dexterity < 10)
            dexterity = 3;

        if (unitStats.potency < 4)
            potency = 1;
        else if (unitStats.potency < 7)
            potency = 2;
        else if (unitStats.potency < 10)
            potency = 3;


        if (unitStats.toughness < 4)
            toughness = 1;
        else if (unitStats.toughness < 7)
            toughness = 2;
        else if (unitStats.toughness < 10)
            toughness = 3;

        if (unitStats.resistance < 4)
            resistance = 1;
        else if (unitStats.resistance < 7)
            resistance = 2;
        else if (unitStats.resistance < 10)
            resistance = 3;
    }

    // code to make copy of stats for preUpgradeStats
    public Stats(Stats unitStats)
    {
        maxHP = unitStats.maxHP;
        speed = unitStats.speed;
        strength = unitStats.strength;
        dexterity = unitStats.dexterity;
        potency = unitStats.potency;
        toughness = unitStats.toughness;
        resistance = unitStats.resistance;

    }

}

