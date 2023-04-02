using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{

    // ÇÊ¿äÇÑ ÄÄÆ÷³ÍÆ®
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // ÇÊ¿äÇÏ¸é HUD È£­Œ, ÇÊ¿ä¾øÀ¸¸é HUD ºñÈ°¼ºÈ­
    [SerializeField]
    private GameObject go_BulletHUD;
    // ÃÑ¾Ë °³¼ö ÅØ½ºÆ® ¹Ý¿µ
    [SerializeField]
    private Text[] text_Bullet;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }


}
