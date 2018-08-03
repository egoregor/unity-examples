using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatController : MonoBehaviour
{

    private HealHelper HpHelper;
    private AbilitiesController AbilitiesCtrl;


    public EnemiesHpController EnemiesHp;

    public int Hp = 100;
    public int MaxHp = 100;
    public int Exp = 0;
    public int Lvl = 1;
    public int PreviousExpLvl = 0;
    public int NextExpLvl = 100;
    private float RegenerationTick = 0;
    private int[] ExpList;

    public UnityEngine.UI.Image ExpBar;
    public TMPro.TextMeshProUGUI LvlText;

    public int ExpForResourceSuccess = 2;
    public int ExpForEnemyHit = 2;

    // Use this for initialization
    void Awake()
    {
        AbilitiesCtrl = GetComponent<AbilitiesController>();
        if (gameObject.tag == "Player")
        {
            HpHelper = GetComponent<HealHelper>();
            ExpList = new int[11] { 0, 100, 250, 450, 800, 1250, 1700, 2300, 2900, 3500, 9999999 };

        }
        if (gameObject.tag == "Player")
        {
            if (Connection.IsServer)
            {
                PlayerInfo.RequestHP();
            }
            else
            {
                if (PlayerPrefs.HasKey("PlayerHp"))
                {
                    Hp = PlayerPrefs.GetInt("PlayerHp");
                }
                else
                {
                    Hp = 100;
                }
            }
        }
    }
    void Start()
    {
        if (gameObject.tag == "Player")
        {
            AnalyticLogic.AnalyticLevel(1);
            if (Connection.IsServer)
            {
                PlayerInfo.RequestExp();
                
            }
            else
            {
                SetExp(80);
            }
        }
    }

    void OnEnable()
    {
        PlayerInfo.OnExpUpdate += UpdateExp;
        PlayerInfo.OnHpUpdate += UpdateHp;
    }

    void OnDisable()
    {
        PlayerInfo.OnExpUpdate -= UpdateExp;
        PlayerInfo.OnHpUpdate -= UpdateHp;
    }
    public void UpdateExp()
    {
        if (gameObject.tag == "Player")
        {
            if (Connection.IsServer)
            {
                SetExp(PlayerInfo.exp);
            }
            else
            {
                SetExp(6300);
            }
        }
    }
    public void UpdateHp()
    {
        if (gameObject.tag == "Player")
        {
            if (Connection.IsServer)
            {
                Hp = PlayerInfo.hp;
            }
            else
            {
                if (PlayerPrefs.HasKey("PlayerHp"))
                {
                    Hp = PlayerPrefs.GetInt("PlayerHp");
                }
                else
                {
                    Hp = 100;
                }
            }
        }
        UpdateHpHelper();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateHpHelper()
    {
        if (gameObject.tag == "Player")
        {
            HpHelper.CurrentHp = Hp;
            HpHelper.HpHandler.SetMaxHp(MaxHp);
        }
    }

    public void LossHp(int num)
    {


        Hp -= num;
        if (Hp <= 0) Hp = 0;



        UpdateHpHelper();
    }
    public void AddHp(int num)
    {
        Hp += num;
        if (Hp >= MaxHp) Hp = MaxHp;
        if (Connection.IsServer)
        {
            PlayerInfo.ChangeHP(Hp);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerHp", Hp);
        }

        UpdateHpHelper();
    }
    public int GetHp()
    {
        return Hp;
    }
    public int GetMaxHp()
    {
        return MaxHp;
    }
    public bool IsDead()
    {
        return Hp <= 0;
    }

    public int GetLvl()
    {
        return Lvl;
    }
    public void SetMaxHp()
    {
        if (gameObject.tag == "Player")
        {
            MaxHp = 100;
            UpdateHpHelper();
        }
    }
    public void SetLvl(int newLvl)
    {
        if (newLvl != 0) { Lvl = newLvl; }
        else { Lvl = 1; }
        AnalyticLogic.AnalyticLevel(Lvl);

        PreviousExpLvl = ExpList[Lvl - 1];
        NextExpLvl = ExpList[Lvl];
    }

    public void AddExp(int expNum)
    {
        if (gameObject.tag == "Player")
        {

            Exp += expNum;
            if (Exp > ExpList[9])
            {
                Exp = ExpList[9];
            }
            for (int i = 0; i < ExpList.Length; i++)
            {
                if (Exp < ExpList[i])
                {
                    if (Lvl != i) SetLvl(i);
                    break;
                }
            }


            float TempA, TempB, TempC;

            TempA = Exp;
            TempB = PreviousExpLvl;
            TempC = NextExpLvl;


            float expPrograess = (TempA - TempB) / (TempC - TempB);


            ExpBar.material.SetFloat("_CurrentHp", expPrograess);
            LvlText.text = "lvl. " + Lvl.ToString();
            PlayerInfo.SendChangeExp(Exp);
        }
    }
    public void SetExp(int expValue)
    {
        Exp = expValue;
        for (int i = 0; i < ExpList.Length; i++)
        {
            if (Exp < ExpList[i])
            {
                if (Lvl != i) SetLvl(i);
                break;
            }
        }

        float TempA, TempB, TempC;

        TempA = Exp;
        TempB = PreviousExpLvl;
        TempC = NextExpLvl;


        float expPrograess = (TempA - TempB) / (TempC - TempB);


        ExpBar.material.SetFloat("_CurrentHp", expPrograess);
        LvlText.text = "lvl. " + Lvl.ToString();
    }

    public void RegenerationInRelax(int regenHp)
    {
        RegenerationTick += Time.deltaTime;
        if (RegenerationTick >= 0.05f)
        {
            AddHp(regenHp / 20);
            RegenerationTick = 0;
        }
    }
}
