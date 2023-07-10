using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : Potion
{
    public override void Use()
    {
        Player.Inst.MP += 40f;
    }
}
