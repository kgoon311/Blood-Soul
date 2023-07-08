using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        SetInst();
    }

    public void InventoryUpdate(Item item) => inventory.GetNewItem(item);
    public void ItemUISwap(int index) => inventory.ItemSwap(index);
}
