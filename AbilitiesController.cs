using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AbilitiesController : MonoBehaviour
{

    private StatController StatCtrl;
    public UIController UiCtrl;
    public AudioUiController AudioUiCtrl;

    // Уровни активных реликов (Тамбурин, Ошейник, Подкова)
    public int RelicTambourine = 0;
    public int RelicCollar = 0;
    public int RelicHorseshoe = 0;

    // Плюс к хилу значение
    public float PlusHealPercent = 0;
    // Минус к хилу КД
    public float MinusKdPercent = 0;
    // Плюс к простому удару
    public int PlusSimpleDamage = 0;
    // Плюс к хорошему удару
    public int PlusGoodDamage = 0;
    // Плюс к трики удару
    public int PlusTrickyDamage = 0;
    // Плюс к шансу хорошему удару
    public float PlusGoodChance = 0;
    // Плюс к шансу трики удару
    public float PlusTrickyChance = 0;
    // Минус от простого удара
    public int MinusSimpleDamage = 0;
    // Минус от хорошего удара
    public int MinusGoodDamage = 0;
    // Минус от трики удара
    public int MinusTrickyDamage = 0;

    public float[] Tambourine1;
    public float[] Tambourine2;
    public float[] Tambourine3;
    public float[] Collar1;
    public float[] Collar2;
    public float[] Collar3;
    public float[] HorseShoe1;
    public float[] HorseShoe2;
    public float[] HorseShoe3;

    private bool HealIsReady = true;
    private int RegenerateCount = 1;
    private float RegeneratingTick = 0;
    [Range(1f, 10f)]
    private float HealTimeActivity = 1f;
    private float HealTimeBeforeEnd = 0;
    [Range(1f, 20f)]
    public float HealRestartTime = 1f;
    private float HealBeforeRestart = 0;
    
    public List<GameObject> ActiveAbilityList = new List<GameObject>();
    private int ActiveAbilityNum = 0;

    public AllSlotsData AllData;

    public int SoupHealValue = 10;
    public int JamHealValue = 15;
    public int BananasHealValue = 5;
    public int BerriesHealValue = 5;

    private void Awake()
    {
        /*
         * Heal
         * HealCd
         * Damage1
         * Damage2
         * Damage3
         * Chance2
         * Chance3
         * Def1
         * Def2
         * Def3
         */

        Tambourine1 = new float[10] { 0.1f, 0.1f, 0, 0, 0, 0f, 0f, 1, 1, 1 };
        Tambourine2 = new float[10] { 0.2f, 0.2f, 0, 0, 0, 0f, 0f, 2, 2, 2 };
        Tambourine3 = new float[10] { 0.3f, 0.3f, 0, 0, 0, 0f, 0f, 3, 3, 3 };

        Collar1 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 5f, 5f, 1, 1, 1 };
        Collar2 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 7f, 7f, 1, 1, 1 };
        Collar3 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 8f, 8f, 1, 1, 1 };

        HorseShoe1 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 5f, 5f, 1, 1, 1 };
        HorseShoe2 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 7f, 7f, 1, 1, 1 };
        HorseShoe3 = new float[10] { 0.1f, 0.1f, 1, 1, 1, 8f, 8f, 1, 1, 1 };
    }
    void Start()
    {

        StatCtrl = GetComponent<StatController>();
        StatCtrl.SetMaxHp();
    }

    void Update()
    {
        TimeForHealSimple();
    }

    public void CheckRelics()
    {
        PlusHealPercent = 0;
        MinusKdPercent = 0;
        PlusSimpleDamage = 0;
        PlusGoodDamage = 0;
        PlusTrickyDamage = 0;
        PlusGoodChance = 0;
        PlusTrickyChance = 0;
        MinusSimpleDamage = 0;
        MinusGoodDamage = 0;
        MinusTrickyDamage = 0;

        for (int i = 42; i <= 44; i++)
        {
            if (AllData.AllSlotsItems[i].Type == "Relic")
            {
                switch (AllData.AllSlotsItems[i].Description1)
                {
                    case "Tambourine":
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 1)
                        {
                            PlusHealPercent += Tambourine1[0];
                            MinusKdPercent += Tambourine1[1];
                            PlusSimpleDamage += Convert.ToInt32(Tambourine1[2]);
                            PlusGoodDamage += Convert.ToInt32(Tambourine1[3]);
                            PlusTrickyDamage += Convert.ToInt32(Tambourine1[4]);
                            PlusGoodChance += Tambourine1[5];
                            PlusTrickyChance += Tambourine1[6];
                            MinusSimpleDamage += Convert.ToInt32(Tambourine1[7]);
                            MinusGoodDamage += Convert.ToInt32(Tambourine1[8]);
                            MinusTrickyDamage += Convert.ToInt32(Tambourine1[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 2)
                        {
                            PlusHealPercent += Tambourine2[0];
                            MinusKdPercent += Tambourine2[1];
                            PlusSimpleDamage += Convert.ToInt32(Tambourine2[2]);
                            PlusGoodDamage += Convert.ToInt32(Tambourine2[3]);
                            PlusTrickyDamage += Convert.ToInt32(Tambourine2[4]);
                            PlusGoodChance += Tambourine2[5];
                            PlusTrickyChance += Tambourine2[6];
                            MinusSimpleDamage += Convert.ToInt32(Tambourine2[7]);
                            MinusGoodDamage += Convert.ToInt32(Tambourine2[8]);
                            MinusTrickyDamage += Convert.ToInt32(Tambourine2[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 3)
                        {
                            PlusHealPercent += Tambourine3[0];
                            MinusKdPercent += Tambourine3[1];
                            PlusSimpleDamage += Convert.ToInt32(Tambourine3[2]);
                            PlusGoodDamage += Convert.ToInt32(Tambourine3[3]);
                            PlusTrickyDamage += Convert.ToInt32(Tambourine3[4]);
                            PlusGoodChance += Tambourine3[5];
                            PlusTrickyChance += Tambourine3[6];
                            MinusSimpleDamage += Convert.ToInt32(Tambourine3[7]);
                            MinusGoodDamage += Convert.ToInt32(Tambourine3[8]);
                            MinusTrickyDamage += Convert.ToInt32(Tambourine3[9]);
                        }
                        break;
                    case "Collar":
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 1)
                        {
                            PlusHealPercent += Collar1[0];
                            MinusKdPercent += Collar1[1];
                            PlusSimpleDamage += Convert.ToInt32(Collar1[2]);
                            PlusGoodDamage += Convert.ToInt32(Collar1[3]);
                            PlusTrickyDamage += Convert.ToInt32(Collar1[4]);
                            PlusGoodChance += Collar1[5];
                            PlusTrickyChance += Collar1[6];
                            MinusSimpleDamage += Convert.ToInt32(Collar1[7]);
                            MinusGoodDamage += Convert.ToInt32(Collar1[8]);
                            MinusTrickyDamage += Convert.ToInt32(Collar1[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 2)
                        {
                            PlusHealPercent += Collar2[0];
                            MinusKdPercent += Collar2[1];
                            PlusSimpleDamage += Convert.ToInt32(Collar2[2]);
                            PlusGoodDamage += Convert.ToInt32(Collar2[3]);
                            PlusTrickyDamage += Convert.ToInt32(Collar2[4]);
                            PlusGoodChance += Collar2[5];
                            PlusTrickyChance += Collar2[6];
                            MinusSimpleDamage += Convert.ToInt32(Collar2[7]);
                            MinusGoodDamage += Convert.ToInt32(Collar2[8]);
                            MinusTrickyDamage += Convert.ToInt32(Collar2[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 3)
                        {
                            PlusHealPercent += Collar3[0];
                            MinusKdPercent += Collar3[1];
                            PlusSimpleDamage += Convert.ToInt32(Collar3[2]);
                            PlusGoodDamage += Convert.ToInt32(Collar3[3]);
                            PlusTrickyDamage += Convert.ToInt32(Collar3[4]);
                            PlusGoodChance += Collar3[5];
                            PlusTrickyChance += Collar3[6];
                            MinusSimpleDamage += Convert.ToInt32(Collar3[7]);
                            MinusGoodDamage += Convert.ToInt32(Collar3[8]);
                            MinusTrickyDamage += Convert.ToInt32(Collar3[9]);
                        }
                        break;
                    case "Horseshoe":
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 1)
                        {
                            PlusHealPercent += HorseShoe1[0];
                            MinusKdPercent += HorseShoe1[1];
                            PlusSimpleDamage += Convert.ToInt32(HorseShoe1[2]);
                            PlusGoodDamage += Convert.ToInt32(HorseShoe1[3]);
                            PlusTrickyDamage += Convert.ToInt32(HorseShoe1[4]);
                            PlusGoodChance += HorseShoe1[5];
                            PlusTrickyChance += HorseShoe1[6];
                            MinusSimpleDamage += Convert.ToInt32(HorseShoe1[7]);
                            MinusGoodDamage += Convert.ToInt32(HorseShoe1[8]);
                            MinusTrickyDamage += Convert.ToInt32(HorseShoe1[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 2)
                        {
                            PlusHealPercent += HorseShoe2[0];
                            MinusKdPercent += HorseShoe2[1];
                            PlusSimpleDamage += Convert.ToInt32(HorseShoe2[2]);
                            PlusGoodDamage += Convert.ToInt32(HorseShoe2[3]);
                            PlusTrickyDamage += Convert.ToInt32(HorseShoe2[4]);
                            PlusGoodChance += HorseShoe2[5];
                            PlusTrickyChance += HorseShoe2[6];
                            MinusSimpleDamage += Convert.ToInt32(HorseShoe2[7]);
                            MinusGoodDamage += Convert.ToInt32(HorseShoe2[8]);
                            MinusTrickyDamage += Convert.ToInt32(HorseShoe2[9]);
                        }
                        if (Convert.ToInt32(AllData.AllSlotsItems[i].Description2) == 3)
                        {
                            PlusHealPercent += HorseShoe3[0];
                            MinusKdPercent += HorseShoe3[1];
                            PlusSimpleDamage += Convert.ToInt32(HorseShoe3[2]);
                            PlusGoodDamage += Convert.ToInt32(HorseShoe3[3]);
                            PlusTrickyDamage += Convert.ToInt32(HorseShoe3[4]);
                            PlusGoodChance += HorseShoe3[5];
                            PlusTrickyChance += HorseShoe3[6];
                            MinusSimpleDamage += Convert.ToInt32(HorseShoe3[7]);
                            MinusGoodDamage += Convert.ToInt32(HorseShoe3[8]);
                            MinusTrickyDamage += Convert.ToInt32(HorseShoe3[9]);
                        }
                        break;
                }
            }
        }
    }

    public void ActivateAbility(string name)
    {
        switch (name)
        {
            case "DefaultHeal":
                int HealValue = 0;
                if (AllData.AllSlotsItems[41].Type == "Healing")
                {
                    if (AllData.AllSlotsItems[41].Description1 == "Soup")
                    {
                        HealValue = SoupHealValue;
                    }
                    if (AllData.AllSlotsItems[41].Description1 == "Jam")
                    {
                        HealValue = JamHealValue;
                    }
                    if (AllData.AllSlotsItems[41].Description1 == "Bananas")
                    {
                        HealValue = BananasHealValue;
                    }
                    if (AllData.AllSlotsItems[41].Description1 == "Berries")
                    {
                        HealValue = BerriesHealValue;
                    }
                }
                ActivateRengenerating(HealValue + (int)(HealValue * PlusHealPercent));
                break;
        }
    }    

    public float GetCooldown(string Name)
    {
        float Prograess = 0;


        switch (Name)
        {
            case "DefaultHeal":

                Prograess = 1 - HealBeforeRestart / (HealRestartTime - (HealRestartTime * MinusKdPercent));
                break;
        }

        if (Prograess >= 1) Prograess = 0;



        return Prograess;
    }

    public void DisableAbility(string name)
    {

    }

    public void ActivateRengenerating(int regenCount)
    {
        if (HealIsReady && StatCtrl.Hp < StatCtrl.MaxHp)
        {

            AllData.AllSlotsItems[41].Quantity -= 1;
            UiCtrl.UpdateHealBtn();

            AllData.StaticFieldsCleanup();
            AllData.CleanupEmptySlots();
            AllData.Refresh();

            StatCtrl.AddHp((int)(regenCount + (regenCount * PlusHealPercent)));
            HealIsReady = false;
            HealTimeBeforeEnd = 0;

            InventoryItems.SendChangeInventory(AllData.AllSlotsItems);

            if (gameObject.tag == "Player") AudioUiCtrl.EatPlay();
        }
    }

    private void TimeForHealSimple()
    {
        if (!HealIsReady)
        {
            HealBeforeRestart += Time.deltaTime;
            if (HealBeforeRestart >= (HealRestartTime - (HealRestartTime * MinusKdPercent)))
            {
                HealIsReady = true;
                HealBeforeRestart = 0;
            }
        }
    }
}
