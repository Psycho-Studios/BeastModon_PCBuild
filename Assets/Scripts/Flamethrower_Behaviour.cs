using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower_Behaviour : ProjectileData
{
    public void Awake()
    {
        this.bool_playerProjectile = true;
    }
    public override void launchProjectile()
    {
        return;
    }
}
