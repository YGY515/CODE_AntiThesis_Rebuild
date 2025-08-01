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
    public int weapon_index = 0;

    
    void Start()
    {
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
        weapon_index = (weapon_index + 1) % weaponObjects.Length;
        currentWeapon = weaponObjects[weapon_index].GetComponent<IWeapon>();

    }
}
