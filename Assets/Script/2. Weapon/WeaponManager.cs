using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponObjects;
    private IWeapon currentWeapon;
    private int weapon_index = 0;

    /*
    public WeaponManager()
    {
        weapons = new IWeapon[]
        {
            new Baton(),
            new Gun(),

        };
        currentWeapon = weapons[0];
    }
    */
    void Start()
    {
        // WeaponManager의 인스펙터에 무기 할당하는 것으로 수정
        //weaponObjects = GameObject.FindGameObjectsWithTag("Weapon");
        //weaponObjects[weapon_index].SetActive(true);

        currentWeapon = weaponObjects[weapon_index].GetComponent<IWeapon>();

        if (currentWeapon == null)
            Debug.LogError($"{weaponObjects[weapon_index].name} 오브젝트에 IWeapon 컴포넌트가 없음");

        weaponObjects[weapon_index].SetActive(false);

    }


    public void UseWeapon()
    {
        
        if (currentWeapon != null)
            currentWeapon.Attack();
        else
            Debug.LogError("currentWeapon에 아직 할당되지 않음");
    }

    public void ChangeWeapon()
    {
        Debug.Log("무기 변경");

        /*
        if (weapon_index % 2 == 1)
        {
            weapon_index = 0;
        }
        else
        {
            weapon_index = 1;
        }
        currentWeapon = weapons[weapon_index];
        */

        weapon_index = (weapon_index + 1) % weaponObjects.Length;
        currentWeapon = weaponObjects[weapon_index].GetComponent<IWeapon>();
    

    }
}
