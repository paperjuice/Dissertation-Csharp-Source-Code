﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMeleeWeaponCollider : MonoBehaviour {

    debuff mcDebuffs;
    mcStats _mcStats;
    mcMovementBehaviour mcMovement;
    [SerializeField] GameObject enemyPosition;
  //  [SerializeField] float eDamage;
    [SerializeField] float interruptTime;
    [SerializeField] float pushBackForce;

    //enemy amount of damage it deals
    [SerializeField]private float enemyDamage=0f;

    
    void Awake()
    {
        mcDebuffs = GameObject.FindGameObjectWithTag("Player").GetComponent<debuff>();
        _mcStats = GameObject.FindGameObjectWithTag("Player").GetComponent<mcStats>();
        mcMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<mcMovementBehaviour>();
    }


    void Start()
    {
        enemyDamage += controller.dungeonLevel * 1.9f;
//        print(enemyDamage);
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!mcMovement.isInvincible) //put it outside "if(col.gameObject==player)"
            {
                 GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("shakePlayer");
                _mcStats.IncrementAgeOnDamageReceived(enemyDamage + Random.Range(-0.1f*enemyDamage, 0.1f*enemyDamage));
                mcDebuffs.SecondsInterrupted = interruptTime; //Interrupt - this will be iterated based on enemy type
                mcDebuffs.PushBack(enemyPosition.transform.position, mcDebuffs.transform.position, pushBackForce); //PushBack - iterate force
            }
        }
    }



}
