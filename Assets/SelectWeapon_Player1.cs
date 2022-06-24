using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWeapon_Player1 : MonoBehaviour
{
         Animator animator_weaponSlots;

    private void Awake()
    {
        animator_weaponSlots = this.gameObject.GetComponent<Animator>();
    }

    public void animateWeaponIndicator(int int_targetWeaponSlot)
    {
        animator_weaponSlots.SetInteger("Weapon", int_targetWeaponSlot);
    }
}
