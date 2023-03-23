using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunController : MonoBehaviour
{
    [SerializeField]
    private Gun CurrentGun;

    private float currentFireRate;

    

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();

    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초의 역수 60분의 1

        }
    }
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = CurrentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        Debug.Log("총알 발사됨");
    }
}
