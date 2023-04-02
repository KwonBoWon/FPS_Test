using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // ���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon;

    // 
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // ���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    // ���� ��ü ������, ���� ��ü�� ������ ���� ����
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    // ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunsDictinary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handsDictinary = new Dictionary<string, Hand>();

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunsDictinary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handsDictinary.Add(hands[i].handName, hands[i]);
        }

    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // hand
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // gun
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }


        }
    }
   


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivte = false;
                break;
            case "HAND":
                HandController.isActivate = false;

                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunsDictinary[_name]);
        }
        else if (_type == "HAND") 
        {
            theHandController.HandChange(handsDictinary[_name]);
        }
    }


}
