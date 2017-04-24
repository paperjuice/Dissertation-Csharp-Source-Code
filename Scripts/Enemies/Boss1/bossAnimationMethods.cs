﻿using UnityEngine;

public class bossAnimationMethods : MonoBehaviour {

    [SerializeField] bossGeneralBehaviour _bossGeneralBehaviour;
    [SerializeField] bossMeleeAttackBehaviour _bossMeleeAttackBehaviour;
    [SerializeField] Collider[] bossWeapon;

    public void DecrementNumberOfCombos()
    {
        _bossMeleeAttackBehaviour.numberOfCombos--;

        if (_bossMeleeAttackBehaviour.numberOfCombos == 0)
        {
            _bossGeneralBehaviour.enabled = true;
        }
    }

    void AttackMovement(float force)
    {
        _bossMeleeAttackBehaviour.GetComponent<Rigidbody>().AddForce(transform.forward * force * 60000f * Time.fixedDeltaTime);
    }

    //activate weapon in right hand
    void ActivateWeaponCollider(int a)
    {
        if (a == 1)
            bossWeapon[0].enabled = true;
        else if (a == 0)
            bossWeapon[0].enabled = false;
    }
}
