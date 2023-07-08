using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData itemData;

    [SerializeField] 
    private float getRangeRadius;
    private bool isActive = true;

    public abstract void ActiveItem();

    private void Update()
    {
        var hit = Physics.OverlapSphere(transform.position, getRangeRadius, LayerMask.GetMask("Player"));

        if (hit.Length > 0 && isActive)
        {
            ActiveItem();
            isActive = false;
            gameObject.SetActive(isActive);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, getRangeRadius);
    }
}

public enum ItemType
{
    Skill,
    Armour,
    Weapon,
    Item,
}
