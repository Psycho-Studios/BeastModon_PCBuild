using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    public GameObject shoot;
    private Transform camAddress;
    public float endTime, beginning, howManyHalfSeconds;
    private float startTime;
    private bool beGone;

    void Start()
    {
        beGone = false;
        camAddress = shoot.GetComponent<Transform>();
    }

    void Update()
    {
        startTime = Time.time;
        if(beGone == true)
        {
            if((startTime - endTime) >= (endTime - beginning))
            {
                shoot.transform.position = camAddress.transform.position;
                beGone = false;
                beginning = 500000;
                endTime = 500000;
            }
        }
        if(startTime >= 0 && (beGone == false))
        {

            for (int i = 0; i < howManyHalfSeconds; i++)
            {
                StartCoroutine(ScreenShake());
                shoot.transform.position = camAddress.position;
            }
            beGone = true;

        }
        
    }

    IEnumerator ScreenShake()
    {
        yield return new WaitForSeconds(0.3f);
        shoot.transform.position = new Vector3(-24, -0.2f, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-22, 0, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-23, -0.2f, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-25, 0.1f, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-22, -0.4f, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-21, 0, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-23.5f, -0.2f, -10.46f);
        yield return new WaitForSeconds(0.045f);
        shoot.transform.position = new Vector3(-24, -0.2f, -10.46f);
        yield return new WaitForSeconds(0.03f);
        shoot.transform.position = new Vector3(-22, 0, -10.46f);
        yield return new WaitForSeconds(0.045f);
    }
}
