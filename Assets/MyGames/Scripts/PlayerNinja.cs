using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using System;

public class PlayerNinja : MonoBehaviour
{
    bool lowSpeed;
    [SerializeField] float shootForce;
    [SerializeField] GameObject bulletPrefab;
    public TimeManager timeManager;
    [SerializeField] Transform shootPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //LowSpeedEffect();
            timeManager.DoSlowMotion();
        }

        if(Input.GetMouseButtonUp(1))
        {
            CMDebug.TextPopupMouse("Disparando");
            //Shoot();
        }
    }

    private void Shoot()
    {
        GameObject lastBullet = Instantiate(bulletPrefab, shootPosition.position, Quaternion.identity);
        lastBullet.GetComponent<Rigidbody2D>().AddForce(-shootPosition.up * shootForce, ForceMode2D.Impulse);
    }

    /*
    void LowSpeedEffect()
    {
        LowSpeed();
        Invoke(nameof(NormalSpeed), 1f);
    }

    void LowSpeed()
    {
        Debug.Log("low");
        Time.timeScale = 0.1f;
    }

    void NormalSpeed()
    {
        Debug.Log("normal");
        Time.timeScale = 1f;
    }
    */
}
