using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    int Damage { get; }
    void Attack();
}
