using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmat_Item : Item
{
    public override void ActiveItem()
    {
        Player.Inst.GetHelmat();
        UIManager.Inst.InventoryUpdate(this);
    }
}
