using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Hero : MonoBehaviour {

    public Animation RedEyes;
    public Animation Baseball;
    public Animation DopplegangerBaseball;
    public Animation BlackScreenHide;

    private float DamageTimeOrigin = 1f;
    private float DamageTime = 0;
    private bool CanDamage = true;

    public GameObject Doppelganger;
    private float DoppelgangerPosZ;

    public RawImage BlackEndScreen;
    public RawImage RedAngry;
    public float AngryAlpha = 0;
    public int BreakThingsValue = 0;

    public bool IsEnd = false;
    public bool DoppelGangerMoved = false;

    public AudioSource Music;
    public AudioSource HeartMonitor;
    private float MusicVolumeMax = 1f;
    private bool MusicOn = false;
    private bool MusicOff = false;
    public AudioSource Hit;
    public AudioSource HitBody;
    public AudioSource Break;
    public VoiceSoundScript Voice;
    public AudioSource Blat;
    public AudioSource MirrorBreak;

    void Start()
    {
        DoppelgangerPosZ = Doppelganger.transform.position.z;
        BlackEndScreenHide();
    }

    public void BlackEndScreenHide()
    {
        Hit.Play();
        MirrorBreak.Play();
        Blat.Play();
        HeartMonitor.Play();
        StartCoroutine(StartBlackScreenHide(2f));
    }

    private IEnumerator StartBlackScreenHide(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BlackScreenHide.Play();
        GetComponent<FirstPersonController>().CanWalk = true;
        MusicOn = true;
    }

    void Update()
    {
        if (DoppelGangerMoved)
        {
            DoppelgangerPosZ += Time.deltaTime*4;
            Doppelganger.transform.position = new Vector3(Doppelganger.transform.position.x, Doppelganger.transform.position.y, DoppelgangerPosZ);
        }
        if (Doppelganger.transform.position.z >= -8f && DoppelGangerMoved)
        {
            DoppelGangerMoved = false;
            GetComponent<Animation>().Play();
        }

        if (DamageTime <= 0)
        {
            CanDamage = true;
            DamageTime = 0;
        }
        else
        {
            DamageTime -= Time.deltaTime;
            CanDamage = false;
        }

        if (CanDamage && !IsEnd)
        {
            RaycastHit[] _hitArray;
            Ray _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            _hitArray = Physics.RaycastAll(_ray, 2);
            if (_hitArray.Length > 0)
            {
                foreach (RaycastHit _hit in _hitArray)
                {
                    if (_hit.transform.tag == "thing")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            DamageTime = DamageTimeOrigin;
                            _hit.transform.GetComponent<Thing>().GetDamage(_hit.transform.name);
                            RedEyes.Play();
                            Baseball.Play();
                            if (_hit.transform.name == "Mirror")
                            {
                                Hit.Play();
                                Doppelganger.transform.position = new Vector3(gameObject.transform.localPosition.x, 2.35f, -10f);
                            }
                            else
                            {
                                Hit.Play();
                                Voice.RandomVoicePlay();
                            }
                        }
                    }
                }
            }
        }

        if (MusicOn && !MusicOff)
        {
            if (Music.volume <= MusicVolumeMax)
            {
                Music.volume += Time.deltaTime * 0.3f;
            }
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public void ChangeAngry(float _addAngryAlphaValue)
    {
        AngryAlpha += _addAngryAlphaValue;
        if (AngryAlpha >= 1f)
        {
            AngryAlpha = 1f;
        }
        RedAngry.color = new Color(0.26f, 0, 0, AngryAlpha);
    }

    public void BreakMirror() {
        GetComponent<FirstPersonController>().CanWalk = false;
        Music.mute = true;
        StartCoroutine(DoppelgangerMove(1f));
    }

    private IEnumerator DoppelgangerMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DoppelGangerMoved = true;
    }

    public void DoppelgangerBaseballHit()
    {
        DopplegangerBaseball.Play();
        HitBody.Stop();
        HitBody.Play();
        ChangeAngry(0.3f);
    }
    public void DoppelgangerBaseballHitSound() {

    }

    public void BlackEndScreenShow()
    {
        HitBody.Stop();
        HitBody.Play();
        BlackEndScreen.color = new Color(0, 0, 0, 1);
        StartCoroutine(EndGame(2f));
    }

    private IEnumerator EndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }
}
