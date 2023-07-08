using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Item Data", menuName ="Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Image itemUI;
    public Sprite itemSprite;
}
