using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    private AbilitiesController AbilitiesCtrl;
    public GameObject HpLoss;

    // Use this for initialization
    void Start()
    {
        AbilitiesCtrl = GetComponent<AbilitiesController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(GameObject source, GameObject target, Hashtable weapon)
    {
        /* TODO: replace in the same controller */
        if (target.tag == "Enemy")
        {
            if (target.GetComponent<MoveHelper>())
            {
                target.GetComponent<MoveHelper>().GetHit(CountDamage(source, target, weapon));
            }
            if (target.GetComponent<MoveHelperBoss>())
            {
                target.GetComponent<MoveHelperBoss>().GetHit(CountDamage(source, target, weapon));
            }
        }
        if (target.tag == "Player")
        {
            target.GetComponent<ThirdPersonCharacter>().GetHit(CountDamage(source, target, weapon));
        }
    }

    private int CountDamage(GameObject source, GameObject target, Hashtable weapon)
    {
        int damage = 0;
        Color hpLossColor = new Color(1, 1, 1, 1);

        float percentHitting = Random.Range(0, 100f);

        if (percentHitting < ((float)weapon["PercentGood"] - AbilitiesCtrl.PlusGoodChance))
        {
            damage = (int)Mathf.Round(Random.Range((float)weapon["DamageSimpleMin"], (float)weapon["DamageSimpleMax"]));
            hpLossColor = new Color(1, 1, 1, 1);
            
            damage += AbilitiesCtrl.PlusSimpleDamage;
            damage -= target.GetComponent<AbilitiesController>().MinusSimpleDamage;
        }
        if (percentHitting >= ((float)weapon["PercentGood"] - AbilitiesCtrl.PlusGoodChance) && percentHitting < ((float)weapon["PercentPerfect"] - AbilitiesCtrl.PlusTrickyChance))
        {
            damage = (int)Mathf.Round(Random.Range((float)weapon["DamageGoodMin"], (float)weapon["DamageGoodMax"]));
            hpLossColor = new Color(0.81f, 1, 0.51f, 1);
            
            damage += AbilitiesCtrl.PlusGoodDamage;
            damage -= target.GetComponent<AbilitiesController>().MinusGoodDamage;
        }
        if (percentHitting >= ((float)weapon["PercentPerfect"] - AbilitiesCtrl.PlusTrickyChance))
        {
            damage = (int)Mathf.Round(Random.Range((float)weapon["DamagePerfectMin"], (float)weapon["DamagePerfectMax"]));
            hpLossColor = new Color(1, 0.3f, 0.3f, 1);

            damage += AbilitiesCtrl.PlusTrickyDamage;
            damage -= target.GetComponent<AbilitiesController>().MinusTrickyDamage;
        }
        
        if (damage < 0) damage = 0;

        string hpLossText = "" + damage;
        if (damage == 0)
        {
            hpLossText = "Block";
            hpLossColor = new Color(1, 1, 1, 1);
        };
        GameObject newHpLoss = Instantiate(HpLoss) as GameObject;
        newHpLoss.GetComponent<ShowSomeDamage>().Activate(hpLossText, hpLossColor, new Vector3(target.transform.position.x, target.transform.position.y + Random.Range(0.8f, 1f), target.transform.position.z));

        return damage;
    }
}
