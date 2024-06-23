using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public class DamageElementManager : MonoBehaviour
{
    public static DamageElementManager instance;
    public enum DamageElement { Acid, Fire, Lightning, Physical }
    [Header("Acid")]
    public float enhanceDamageAdd = 5;
    public float enhanceDamageMultiplyLimit = 30;
    public float enhanceCooldown = 1;
    public float enhanceDuration = 5;
    private float currentAddedDamage = 0;
    private float enhanceInternalCooldown = 0;
    private float enhanceDurationResetTime = 0;
    private Coroutine currentEnhance;

    [Header("Fire")]
    public int dotTicks = 3;
    public float dotTickTime = 1;
    public float damageDecay = 50;
    private int remainingStacks;
    private float lastDamageInstance;
    private Coroutine currentBurn;

    [Header("Lightning")]
    public int stunChance = 10;
    public float stunDuration = 1;
    public float stunCooldown = 3;
    private float stunInternalCooldown = 0;

    [Header("Physical")]
    public int criticalChance = 20;
    public int criticalMultiplier = 2;

    private void Awake()
    {
        instance = this;
    }

    public void ApplyDamageEffect(EnemyBehaviour enemy, DamageElement hitElementDamage, float damage)
    {
        switch (hitElementDamage)
        {
            default:
                {
                    PhysicalEffect(enemy, damage);
                    break;
                }
            case DamageElement.Acid:
                {
                    AcidEffect(enemy, damage);
                    break;
                }
            case DamageElement.Fire:
                {
                    FireEffect(enemy, damage);
                    break;
                }
            case DamageElement.Lightning:
                {
                    LightningEffect(enemy, damage);
                    break;
                }
            case DamageElement.Physical:
                {
                    PhysicalEffect(enemy, damage);
                    break;
                }
        }
    }
    private void AcidEffect(EnemyBehaviour enemy, float damage)
    {
        if(currentEnhance != null)
        {
            enhanceDurationResetTime = enhanceDuration;
        }
        else
        {
            enhanceInternalCooldown = Time.time + enhanceCooldown;
            currentEnhance = StartCoroutine(DamageEnhanceUptime());
        }

        if(enhanceInternalCooldown <= Time.time)
        {
            if(currentAddedDamage < enhanceDamageMultiplyLimit)
            {
                currentAddedDamage += enhanceDamageAdd;
            }
            enhanceInternalCooldown = Time.time + enhanceCooldown;
        }

        enemy.InflictDirectDamage(currentAddedDamage * PlayerStats.instance.GetAcidDamageMultiplier());
        enemy.InstantiateText(damage + currentAddedDamage, Color.green);
    }

    private void FireEffect(EnemyBehaviour enemy, float damage)
    {
        enemy.InstantiateText(damage, new Color(1, 0.5f, 0));

        if (currentBurn != null)
        {
            remainingStacks = dotTicks;
            lastDamageInstance = damage;
            return;
        }

        currentBurn = StartCoroutine(BurnEnemy(enemy, damage));
    }

    private void LightningEffect(EnemyBehaviour enemy, float damage)
    {
        int aux = Random.Range(0, 100 / stunChance);

        if (aux == 0 && stunInternalCooldown <= Time.time)
        {
            StartCoroutine(StunEnemy(enemy));
            enemy.InstantiateText(damage, Color.blue);
            float lightningDamage = damage * 0.1f * PlayerStats.instance.GetLightningDamageMultiplier();
            enemy.InflictDirectDamage(lightningDamage);
            enemy.InstantiateText(lightningDamage, Color.blue);

            stunInternalCooldown = stunCooldown + Time.time;
        }
        else
        {
            enemy.InstantiateText(damage, Color.cyan);
        }
    }

    private void PhysicalEffect(EnemyBehaviour enemy, float damage)
    {
        int aux = Random.Range(0, 100 / criticalChance);

        if (aux == 0)
        {
            enemy.InflictDirectDamage(damage * (Mathf.Clamp(criticalMultiplier - 1, 0f, 10f) * PlayerStats.instance.GetPhysicalDamageMultiplier()));
            enemy.InstantiateText(criticalMultiplier * damage, Color.yellow);
        }
        else
        {
            enemy.InstantiateText(damage, Color.white);
        }
    }

    private IEnumerator DamageEnhanceUptime()
    {
        
        enhanceDurationResetTime = enhanceDuration;
        while (enhanceDurationResetTime >= 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            enhanceDurationResetTime -= Time.deltaTime;
        }
        currentAddedDamage = 0;
        currentEnhance = null;
    }

    private IEnumerator BurnEnemy(EnemyBehaviour enemy, float baseDamage)
    {
        lastDamageInstance = baseDamage;
        remainingStacks = dotTicks;

        while (remainingStacks > 0)
        {
            yield return new WaitForSeconds(dotTickTime);
            float burnDamage = lastDamageInstance / (100 / damageDecay);

            lastDamageInstance = burnDamage;

            enemy.InflictDirectDamage(burnDamage * PlayerStats.instance.GetFireDamageMultiplier());
            enemy.InstantiateText(burnDamage, Color.red);

            remainingStacks--;
        }
        currentBurn = null;
    }
    private IEnumerator StunEnemy(EnemyBehaviour enemy)
    {
        enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
       // enemy.isAttacking = false;
        yield return new WaitForSeconds(stunDuration);
        enemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
