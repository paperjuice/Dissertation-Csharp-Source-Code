﻿using System.Collections;
using UnityEngine;

public class enemyBeenBlocked : MonoBehaviour {

    private mcStats _mcStats;
    private Animator _camera;

    [SerializeField] private bool isBoss;
    
    [Header("Components")]
    [SerializeField] private Rigidbody pushback;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Animator _anim;
    [SerializeField] GameObject stunIcon;

    [Header("ONLY FOR LESSER")]
    [SerializeField] private enemyMeleeBehaviour _enemyMeleeBehaviour;

    [Header("ONLY FOR BOSS")]
    [SerializeField] private MonoBehaviour _bossGeneralBehaviour;

    [Header("Values")]
    [SerializeField] private float timeIncapacitated;
    [SerializeField] private float pushbackForce;

    private void Awake()
    {
        _mcStats = GameObject.FindGameObjectWithTag("Player").GetComponent<mcStats>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    void Update()
    {
        RotateStunIcon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shield")
        {
            _anim.SetTrigger("blocked");
           // _mcStats.Knowledge(1 + controller.dungeonLevel);
            _camera.SetTrigger("shake");
            _particle.transform.position = other.gameObject.transform.position + new Vector3(0,1,0) + transform.forward *-1.2f;
            _particle.Play();
            _particle.transform.parent=null;
            pushback.AddForce((-other.gameObject.transform.position+pushback.transform.position) * pushbackForce * 1000000 * Time.deltaTime);
            GetComponent<Collider>().enabled = false;


            if (!isBoss)
            {
                _enemyMeleeBehaviour.attackBehaviour = 0;
                _enemyMeleeBehaviour.enabled = false;
                foreach (string a in _enemyMeleeBehaviour.animationString)
                    _anim.ResetTrigger(a);
            }
            else
            {
//                print("intra");
                _bossGeneralBehaviour.enabled = false;
            }

            if(!isBoss)
                StartCoroutine(ActivateBehaviourLesser());
            else
                StartCoroutine(ActivateBehaviourBoss());
        }
    }

    IEnumerator ActivateBehaviourLesser()
    {
        yield return new WaitForSeconds(timeIncapacitated);
        _enemyMeleeBehaviour.enabled = true;
    }

    IEnumerator ActivateBehaviourBoss()
    {
        stunIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeIncapacitated);
        _bossGeneralBehaviour.enabled = true;
        stunIcon.gameObject.SetActive(false);
    }

    void RotateStunIcon()
    {
        if(stunIcon != null)
        {
            if( stunIcon.gameObject.activeInHierarchy)
            {
                stunIcon.transform.Rotate(Vector3.forward * Time.deltaTime * 200f);
            }
        }
    }



}
