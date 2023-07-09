using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Potion
{
    public override void Use()
    {
        Player.Inst.HP += 40f;
    }
}
