using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{

    //public KeyCode Key => KeyCode.S;
    public float Damage => 10f;
    public void Attack()
    {
        Debug.Log("탕!");
    }
    public void PlayAnimaion()
    {
        Debug.Log("총 애니메이션 재생");
    }
   
}
