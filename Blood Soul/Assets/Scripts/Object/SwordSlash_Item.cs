using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash_Item : Item
{
    public override void ActiveItem()
    {
        Player.Inst.GetSkill();
        UIManager.Inst.InventoryUpdate(this);
    }
}
