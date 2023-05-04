using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ChargeShot_Player1 : MonoBehaviour
{

    private Animator animator_chargeIndicator, animator_gunfireSpawnPoint;
    private float float_timeBeforeFullyCharged, float_timeOfChargeBegin;
    private bool bool_chargeComplete;
    public GameObject gameObject_chargedShot;
    public AudioClip audioClip_chargingSound, audioClip_chargedShot;
    public  AudioSource audioSource_chargeShotSpawnPoint;
    private ProjectileControls_Player1 script_ProjectileControls_Player1;

    private void Awake()
    {
        audioSource_chargeShotSpawnPoint = gameObject.GetComponent<AudioSource>();
        animator_gunfireSpawnPoint = gameObject.GetComponent<Animator>();
        animator_chargeIndicator = GameObject.Find("Indicator_ChargedShot_Player1")
            .GetComponent<Animator>();
    }

    private void Start()
    {
        bool_chargeComplete = false;
        float_timeBeforeFullyCharged = 2.0f;
        float_timeOfChargeBegin = 500000;
    }

    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(ProjectileControls_Player1.keyCode_chargeShot)
        || Input.GetMouseButtonDown(1))
        {
            float_timeOfChargeBegin = Time.time;
            Debug.Log("Charging Shot...");
            audioSource_chargeShotSpawnPoint.PlayOneShot(audioClip_chargingSound, 0.13f);
        }
        if (Time.time - float_timeBeforeFullyCharged >= float_timeOfChargeBegin)
        {
            bool_chargeComplete = true;
            animator_chargeIndicator.SetInteger("Charge", 1);
        }

        if (Input.GetKeyUp(ProjectileControls_Player1.keyCode_chargeShot)
        || Input.GetMouseButtonUp(1))
        {
            float_timeOfChargeBegin = 500000;
            animator_chargeIndicator.SetInteger("Charge", 0);
            audioSource_chargeShotSpawnPoint.Stop();

            if (bool_chargeComplete)
            {
                audioSource_chargeShotSpawnPoint.PlayOneShot(audioClip_chargedShot, 0.18f);
                bool_chargeComplete = false;
                fireChargedShot();
            }
        }
#endif
    }

    private void fireChargedShot()
    {
        //Remove this comment...?..
        animator_gunfireSpawnPoint.SetInteger("ChargeRelease", 1);
        GameObject gameObject_chargedShot = ObjectPool.objectPool_reference.getPooled_PlayerObjects(default, "ChargedShot");
        gameObject_chargedShot.transform.position = this.gameObject.transform.position;
        gameObject_chargedShot.transform.rotation = Quaternion.identity;
        gameObject_chargedShot.SetActive(true);
        StartCoroutine(returnChargedShot(gameObject_chargedShot));
    }

    //Called within the animation to remove its ability to play.
    //This is a unique solution. One could set the state after so much time is passed.
    public void restoreIdleAnimationState()
    {
        animator_gunfireSpawnPoint.SetInteger("ChargeRelease", 0);
    }

    IEnumerator returnChargedShot(GameObject gameObject_chargedShot)
    {
        yield return new WaitForSeconds(0.6f);
        gameObject_chargedShot.SetActive(false);
    }
}