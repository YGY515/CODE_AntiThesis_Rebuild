using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    //KeyCode Key { get; }
    float Damage { get; }
    void Attack();
    void PlayAnimaion();
}


// 진압봉과 총
/*
public class Baton : IWeapon
{


    //public KeyCode Key => KeyCode.A;
    public float Damage => 5f;


    public void Attack()
    {
        Debug.Log("휘두르기");
    }

    public void PlayAnimaion()
    {
        Debug.Log("진압봉 애니메이션 재생");
    }
}

public class Gun : IWeapon, 
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
*/