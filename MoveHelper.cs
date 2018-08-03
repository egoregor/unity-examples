using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using DG.Tweening;
using TMPro;

public class MoveHelper : MonoBehaviour
{
    bool Hit = false;


    Animator m_Animator;
    float m_ForwardAmount;
    UnityEngine.AI.NavMeshAgent m_Agent;

    public bool m_Attack;

    Vector2 smoothDeltaPosition = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    
    public ThirdPersonCharacter Hero;
    public float ExtraRotationBoost;
    public Transform HitTransform;

    public HpScript HpHandler;

    public AudioEnemyController AudioEnemyCtrl;
    
    private Hashtable CurrentWeapon = new Hashtable();

    private AttackController AttackCtrl;
    private WeaponManager WeaponCtrl;
    private StatController StatCtrl;

    public EnemyGenerator EnemyGen;
    
    void Start()
    {
        float parampam = transform.localScale.x * Random.Range(1f, 1.1f);
        transform.localScale = new Vector3(parampam, parampam, parampam);

        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Agent.updatePosition = false;

        AttackCtrl = GetComponent<AttackController>();
        StatCtrl = GetComponent<StatController>();
        WeaponCtrl = GetComponent<WeaponManager>();
    }

    public void GetHit(int Damage)
    {
        StatCtrl.EnemiesHp.GotDamage(StatCtrl, Damage);
        Hero.GetComponent<StatController>().AddExp(Hero.GetComponent<StatController>().ExpForEnemyHit);

        if (StatCtrl.GetHp() > 0)
        {
            Camera.main.DOPlayForward();
            Camera.main.DOShakePosition(0.1f, 0.05f, 1).OnComplete(RestoreCamera);

            StatCtrl.LossHp(Damage);


            if (StatCtrl.GetHp() <= 0)
            {
                Hero.ChangeFollowingEnemies(-1);
                this.gameObject.layer = 8;
                GetComponent<StatePatternEnemy>().currentState = GetComponent<StatePatternEnemy>().deadState;
            }
            else
            {
                this.GetComponent<StatePatternEnemy>().currentState = this.GetComponent<StatePatternEnemy>().knockbackState;
            }

            EnemyGen.SaveSceneEnemies();

            AudioEnemyCtrl.HitPlay();

            Hit = true;
            HitTransform.GetComponent<ParticleSystem>().Play();
        }
    }

    public void RestoreCamera()
    {
        Camera.main.transform.DOLocalMove(new Vector3(0, 0, 0), 0.05f);
    }

    public void MakeHit()
    {
        AttackCtrl.Attack(gameObject, Hero.gameObject, WeaponCtrl.GetCurrentWeapon());
    }

    void OnAnimatorMove()
    {
        transform.position = m_Agent.nextPosition;
    }
}



