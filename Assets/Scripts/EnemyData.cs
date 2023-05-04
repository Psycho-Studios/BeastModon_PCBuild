using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyData : MonoBehaviour
{
    protected bool bool_defeated;
    public bool bool_bossEnemy, bool_bossWeakpoint,
        bool_slammableWall, bool_slammableEnemy, bool_movableByChargedShot;
    public int int_lifePoints;

    private void Start()
    {
        //No need for normal mode in this switch statement, in that case enemy health is not modified
        switch(GameProperties.DataManagement.GameData.string_currentDifficulty)
        {
            case "Easy":
            {
                int_lifePoints = Convert.ToInt32(int_lifePoints * 0.80f);
                break;
            }
            case "Arcade":
            {
                int_lifePoints = Convert.ToInt32(int_lifePoints * 1.5f);
                break;
            }
            default: //Do nothing, normal mode
            {
                break;
            }
        }
    }

    protected abstract void receiveDamage(int damageTaken);

}
