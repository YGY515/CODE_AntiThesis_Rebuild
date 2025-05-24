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
