using System.Collections;
using UnityEngine;

/// <summary>
/// By adding enemies specific to each level in the 
/// </summary>
public class E_SpawnableObjects
{
    public enum Miscellaneous
    {
        PowerUp_BeastModeUp = 0,
        Effect_BeastModeUp = 1,

        PowerUp_HealthUp = 2,
        Effect_HealthUp = 3,

        PowerUp_LetsGo = 4,
        Effect_LetsGo = 5,

        PowerUp_RankUp = 6,
        Effect_RankUp = 7,

        Explosion_1 = 8,
        Explosion_2 = 9,
        Explosion_3 = 10,
        Explosion_WalknSpy = 11,
        Explosion_Player1Ship = 12,
        VisualEffect_BallisticBeast = 64, //Lives in List_playerGameObjectsToPool
    }
    
    public enum PlayerGameObjects
    { 

    }

    public enum TestingScene
    {
        Gyrogat = 0,
        Gyrogat_Projectile_Flesh = 1,
        Nathaniel_Explosion = 2,
        Nathaniel_LowerScreen_ZigZag_Metal = 3,
        Nathaniel_UpperScreen_ZigZag_Metal = 4,
        WoodenBox = 5,
        Jump_n_Spy = 6,
        Walk_n_Spy = 7,
        Explosion_2_Walk_n_Spy = 8
    }

    public enum Tutorial
    {
            //Tutorial
            Gyrogat_Metal = 0,
            Gyrogat_Projectile_Flesh = 1,
            Nathaniel_Explosion = 2,
            Nathaniel_LowerScreen_ZigZag_Metal = 3,
            Nathaniel_UpperScreen_ZigZag_Metal = 4,
            WoodenBox = 5,
            Jump_n_Spy = 6,
            Walk_n_Spy = 7,
            Explosion_2_Walk_n_Spy = 8,
            Gyrogat_12xLeft_Stationary_Metal = 9,
            Gyrogat_12xRight_Stationary_Metal = 10
    }
}