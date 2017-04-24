﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class mcStats : MonoBehaviour {

//    private debuff mcDebuff;
    private consumablePotency _cp;
//    private mcWeaponCollision _mcWeapon;
    private fadeOutFadein fadeIn;
    mcMovementBehaviour _mcMsBehaviour;
    GameObject _camera;
//    bool isCameraObjectRefered = false;

    [SerializeField] ParticleSystem blood;
    [Header("Mc Dead Body ")]
    [SerializeField] GameObject deadBody;

    //death
    bool canDieOfOldAge = false;
    float chanceToDieOfOldAge;

    //knowledge
    public static float knowledge = 0f;

    //age
    private float age=0;
    public float Age{
        get{return age;}
        set{age = value;}
    }
    private float agingPerSecond;
    private GameObject guiAge;
    private Text textAge;

    //health
    private GameObject guiHealth; 
    private float currentHealth=50f;
    public  float CurrentHealth{
        get{return currentHealth;}
        set{currentHealth = value;}
    }
    private float maxHealth;
    public float MaxHealth{
        get{return maxHealth;}
        set{maxHealth = value;}
    }

    //Spirit
    private GameObject guiSpirit;
    private float currentSpirit=20f;
    private float maxSpirit;
    public float MaxSpirit{
        get{return maxSpirit;}
        set{maxSpirit = value;}
    }

    
    //youthfulness
    private float youthfulness;
    private float bonusYouthfulness;
    public float BonusYouthfulness
    {
        get{return bonusYouthfulness;}
        set{bonusYouthfulness=value;}
    }

    //fortitude
    private float fortitude;
    private float bonusFortitude;
    public float BonusFortitude
    {
        get{return bonusFortitude;}
        set{bonusFortitude=value;}
    }

    //wisdom
    private float wisdom;
    private float bonusWisdom;
    public float BonusWisdom
    {
        get{return bonusWisdom;}
        set{bonusWisdom =value;}
    }

    //luck
    private float luck;

    //damage
    private float mcDamage;

    //armour
    private float damageProcessedBasedOnArmour;


    //critChance
    private float damageIfCrited;
    
    
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        //mcDebuff = GetComponent<debuff>();
        _cp = GetComponent<consumablePotency>();
        //_mcWeapon = GameObject.FindGameObjectWithTag("mcWeapon").GetComponent<mcWeaponCollision>();
        knowledge = 0;
        _mcMsBehaviour = GetComponent<mcMovementBehaviour>();
    }

    
    void Update()
    {
//        Debug.Log( (_cp.CritChanceLevel * Fortitude())/(1500+ _cp.CritChanceLevel * Fortitude())*100f);
        AgePerSecond();
        CameraGrayScaleOnAging();
//        Debug.Log(((_cp.CritChanceLevel * Fortitude())/(1500+ _cp.CritChanceLevel * Fortitude())*100f)+Endowments.bonusCritChance);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "water")
            DeathBehaviour();
    }

    private void AgePerSecond()
    {
        Age = Mathf.Clamp(Age,0f,float.MaxValue);
        Age += Time.deltaTime * 0.03f;
    }

    public void IncrementAgeOnDamageReceived(float damageReceived)
    {
        if(damageReceived>0 && !_mcMsBehaviour.isInvincible)
        {
            Age += Armour(damageReceived);
            damageReceived = 0f;
            blood.Play();

            if(_cp.PromiseOfLifeLevel==0)
                Death();
        }
    }

    public void Knowledge(float knowledgeGain)
    {
        if (knowledgeGain > 0f)
        {
            knowledge += knowledgeGain;
            knowledgeGain = 0f;
        }
    }

    public float Youthfulness()
    {
        youthfulness = (1f-age/62f>0?1f-age/62f:0) * (knowledge + bonusYouthfulness);
        return youthfulness;
    }

    public float Fortitude()
    {
        if (age <= 22f)
            fortitude = (age/22f) * (knowledge + BonusFortitude);
        else if(age > 22f)
            fortitude = ((22-(age-22))/22>0?(22-(age-22))/22:0) *(knowledge+ BonusFortitude);
        
        return fortitude;
    }

    public float Wisdom()
    {
        wisdom = (age/62<1?age/62:1) * (knowledge + BonusWisdom);
        return wisdom;
    }

    public float Spirit(float decreasAmount = 0)
    {
        if (decreasAmount > 0)
        {
            currentSpirit -= decreasAmount;
            decreasAmount = 0f;
        }

        maxSpirit = 20 + Youthfulness()*0.01f;
        currentSpirit = Mathf.Clamp(currentSpirit, 0f, maxSpirit);

        if (currentSpirit < maxSpirit)
            currentSpirit += Time.fixedDeltaTime*(8f + _cp.SpiritRegen()*0.5f + (Youthfulness()*0.01f));
        
        return currentSpirit;
    }

    public float Luck()
    {
        luck = ((knowledge+1) / (knowledge + 6000f)) * Youthfulness() * 30;
        luck = Mathf.Clamp(luck, 0,25); 
        return luck;
    }

    public float McDamage(float weaponDamage)
    {
        float temp = ((Fortitude() * 0.5f) + weaponDamage);
        mcDamage = temp + Random.Range(-temp*0.1f, temp*0.1f);  
        return CritChance(mcDamage);;
    }

    public float Armour(float damageReceived)
    {
        damageProcessedBasedOnArmour = damageReceived - _cp.ArmourLevel*2f - Fortitude() * 0.005f;
        damageProcessedBasedOnArmour = Mathf.Clamp(damageProcessedBasedOnArmour, 2f, float.MaxValue);
        return damageProcessedBasedOnArmour;
    }

    public float CritChance(float mcDamage)
    {
        float chanceToCrit = Random.Range(1f, 100f);
        if(chanceToCrit <= _cp.CritChanceLevel  +  Youthfulness()*0.1f + 10f)
        {
            mcDamage = mcDamage * Random.Range(1.6f,2f);
        }
        return mcDamage;
    }

    void Death()
    {
        float deathChance = 0f;
        if (Age>=62 )
        {
            deathChance = Random.Range(1f,100f);
            if(deathChance<= Age / (1.5f +Age/41f))
                DeathBehaviour();
        }

        if(age>72 && !canDieOfOldAge)  
        {  
            canDieOfOldAge=true;
            if(gameObject.activeInHierarchy)
                StartCoroutine(DeathByOld());
        }
        else if(age<=72 && canDieOfOldAge){
            canDieOfOldAge = false;
        }
    }
    IEnumerator DeathByOld()
    {
        while(canDieOfOldAge)
        {
            print("tick-tack");
            chanceToDieOfOldAge = Random.Range(1f,100f);
            if(chanceToDieOfOldAge<=(age/(50+age))*100f)
                DeathBehaviour();

            yield return new WaitForSeconds(10f);
        }
    }

    void DeathBehaviour()
    {
        fadeIn = GameObject.FindGameObjectWithTag("fadeIn").GetComponent<fadeOutFadein>();         
        deadBody.transform.parent = null;
        deadBody.gameObject.SetActive(true);
        gameObject.SetActive(false);
        fadeIn.enabled = true;
        fadeIn.SceneName = "enterLeaderboard";
        Destroy(gameObject,5f);
    }
    // IEnumerator DestroyOnDeath()
    // {
    //     yield return new WaitForSeconds(1f)
    // }

    void CameraGrayScaleOnAging()
    {
        if(_camera == null)
            _camera = GameObject.FindGameObjectWithTag("MainCamera");

        if(_camera != null)
            _camera.GetComponent<ColorCorrectionCurves>().saturation = 1f-(age/110f);
    }

    

    
    
}
