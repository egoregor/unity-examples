using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BossDangerHead : MonoBehaviour {

    public ThirdPersonCharacter Hero;
    public StatController Enemy;

    public int Damage;
    int Speed = 1;
    bool StopHead = false;
    ParticleSystem ExplosionEffect;

    // Use this for initialization
    void Start () {
        ExplosionEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        StartCoroutine(HeadHide(3f));
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(StopHead);
        if (!StopHead)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Hero.transform.position, step);
        }
        if (Enemy.IsDead())
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!StopHead)
            {
                Hero.GetHitWOAnim(Damage);
                StopHead = true;
                ExplosionEffect.Play();
                Destroy(gameObject, 1f);
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!StopHead)
            {
                Hero.GetHitWOAnim(Damage);
                StopHead = true;
                ExplosionEffect.Play();
                Destroy(gameObject, 1f);
            }
        }
    }

    private IEnumerator HeadHide(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ExplosionEffect.Play();
        StopHead = true;
        Destroy(gameObject, 2f);
    }
}
