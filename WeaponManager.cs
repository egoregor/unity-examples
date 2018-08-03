using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    private Hashtable CurrentWeapon = new Hashtable();
    private float BreakKoef = 5f;

    public AllSlotsData AllSlots;

    public float PercentGood = 60f;
    public float PercentTricky = 85f;
    public int DamageSimpleMin = 1;
    public int DamageSimpleMax = 2;
    public int DamageGoodMin = 2;
    public int DamageGoodMax = 3;
    public int DamageTrickyMin = 3;
    public int DamageTrickyMax = 4;
    public float Strength = 666f;
    
    void Start()
    {

    }
    
    void Update()
    {

    }
    public void SetStartWeaponForEnemy()
    {
        SetWeapon(
            PercentGood,
            PercentTricky,
            DamageSimpleMin,
            DamageSimpleMax,
            DamageGoodMin,
            DamageGoodMax,
            DamageTrickyMin,
            DamageTrickyMax,
            Strength);
    }

    public Hashtable GetCurrentWeapon()
    {
        return CurrentWeapon;
    }

    public void SetWeapon(
        float percentGood,
        float percentPerfect,
        float damageSimpleMin,
        float damageSimpleMax,
        float damageGoodMin,
        float damageGoodMax,
        float damagePerfectMin,
        float damagePerfectMax,
        float strength)
    {
        if (CurrentWeapon.ContainsKey("PercentGood")) { CurrentWeapon["PercentGood"] = percentGood; }
        else CurrentWeapon.Add("PercentGood", percentGood);

        if (CurrentWeapon.ContainsKey("PercentPerfect")) { CurrentWeapon["PercentPerfect"] = percentPerfect; }
        else CurrentWeapon.Add("PercentPerfect", percentPerfect);

        if (CurrentWeapon.ContainsKey("DamageSimpleMin")) { CurrentWeapon["DamageSimpleMin"] = damageSimpleMin; }
        else CurrentWeapon.Add("DamageSimpleMin", damageSimpleMin);

        if (CurrentWeapon.ContainsKey("DamageSimpleMax")) { CurrentWeapon["DamageSimpleMax"] = damageSimpleMax; }
        else CurrentWeapon.Add("DamageSimpleMax", damageSimpleMax);

        if (CurrentWeapon.ContainsKey("DamageGoodMin")) { CurrentWeapon["DamageGoodMin"] = damageGoodMin; }
        else CurrentWeapon.Add("DamageGoodMin", damageGoodMin);

        if (CurrentWeapon.ContainsKey("DamageGoodMax")) { CurrentWeapon["DamageGoodMax"] = damageGoodMax; }
        else CurrentWeapon.Add("DamageGoodMax", damageGoodMax);

        if (CurrentWeapon.ContainsKey("DamagePerfectMin")) { CurrentWeapon["DamagePerfectMin"] = damagePerfectMin; }
        else CurrentWeapon.Add("DamagePerfectMin", damagePerfectMin);

        if (CurrentWeapon.ContainsKey("DamagePerfectMax")) { CurrentWeapon["DamagePerfectMax"] = damagePerfectMax; }
        else CurrentWeapon.Add("DamagePerfectMax", damagePerfectMax);

        if (CurrentWeapon.ContainsKey("Strength")) { CurrentWeapon["Strength"] = strength; }
        else CurrentWeapon.Add("Strength", strength);
    }

    public void BreakWeapon()
    {
        bool sameWeapon = false;
        bool similarWeapon = false;
        ItemClass tempItem;

        if ((float)CurrentWeapon["Strength"] != 666)
        {
            CurrentWeapon["Strength"] = (float)CurrentWeapon["Strength"] - BreakKoef;
        }

        if ((float)CurrentWeapon["Strength"] <= 0)
        {

            for (int i = 0; i < 12; i++)
            {
                if (AllSlots.AllSlotsItems[i].Type == "Weapon" && AllSlots.AllSlotsItems[i].Description1 == AllSlots.AllSlotsItems[40].Description1)
                {
                    AllSlots.AllSlotsItems[40].Type = "Empty";
                    tempItem = AllSlots.AllSlotsItems[i];
                    AllSlots.AllSlotsItems[i] = AllSlots.AllSlotsItems[40];
                    AllSlots.AllSlotsItems[40] = tempItem;

                    sameWeapon = true;
                    break;
                }
            }
            if (!sameWeapon)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (AllSlots.AllSlotsItems[i].Type == "Weapon")
                    {
                        AllSlots.AllSlotsItems[40].Type = "Empty";
                        tempItem = AllSlots.AllSlotsItems[i];
                        AllSlots.AllSlotsItems[i] = AllSlots.AllSlotsItems[40];
                        AllSlots.AllSlotsItems[40] = tempItem;

                        similarWeapon = true;
                        break;
                    }
                }
            }
            if (!sameWeapon && !similarWeapon)
            {
                AllSlots.AllSlotsItems[40].Type = "Empty";
            }

            AllSlots.CleanupEmptySlots();
            AllSlots.Refresh();
        }
        if (AllSlots.AllSlotsItems[40].Type == "Weapon") AllSlots.AllSlotsItems[40].DurabilityCurrent = (float)CurrentWeapon["Strength"];

    }

    public void FixWeapon()
    {
        CurrentWeapon["Strength"] = 100f;
    }
}
