using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private RectTransform armourUI;
    [SerializeField] private RectTransform skillUI;
    [SerializeField] private Image curItemUI;
    [SerializeField] private Image nextItemUI;
    [SerializeField] private Text curItemCount;
    [Space(20f)]
    [SerializeField] private GetUI getUI;
    [SerializeField] private Sprite[] items;

    public void GetNewItem(Item item)
    {
        var data = item.itemData;
        switch (data.type)
        {
            case ItemType.Skill: Instantiate(data.itemUI, skillUI); break;
            case ItemType.Armour: Instantiate(data.itemUI, armourUI); break;
        }
        getUI.text.text = data.itemName;
        getUI.icon.sprite = data.itemSprite;
        StartCoroutine(AppreanceGetUI(0.5f));
    }
    private IEnumerator AppreanceGetUI(float time)
    {
        float curTime = 0;
        float percent = 0;
        float alpha = 0;

        while (percent < 1)
        {
            curTime += Time.deltaTime; percent = curTime / time;
            alpha = Mathf.Lerp(0, 1, percent);

            var tColor = getUI.text.color;   var iColor = getUI.icon.color;
            var igColor = getUI.image.color; var gColor = getUI.getUIObj.color;
            tColor.a = alpha; iColor.a = alpha; igColor.a = alpha; gColor.a = alpha;

            getUI.text.color = tColor;
            getUI.icon.color = iColor;
            getUI.image.color = igColor;
            getUI.getUIObj.color = gColor;
            yield return null;
        }
        SoundManager.Inst.PlaySFX(SoundEffect.GetItem);
        yield return new WaitForSeconds(1f);

        curTime = 0;
        percent = 0;
        while (percent < 1)
        {
            curTime += Time.deltaTime; percent = curTime / time;
            alpha = Mathf.Lerp(1, 0, percent);

            var tColor = getUI.text.color; var iColor = getUI.icon.color;
            var igColor = getUI.image.color; var gColor = getUI.getUIObj.color;
            tColor.a = alpha; iColor.a = alpha; igColor.a = alpha; gColor.a = alpha;

            getUI.text.color = tColor;
            getUI.icon.color = iColor;
            getUI.image.color = igColor;
            getUI.getUIObj.color = gColor;
            yield return null;
        }
    }
    public void ItemSwap(int index)
    {
        var temp = curItemUI.sprite;
        curItemUI.sprite = items[index];
        nextItemUI.sprite = temp;
    }

    private void Update()
    {
        curItemCount.text = $"{Player.Inst.potions[Player.Inst.CurItemIndex].count}";
    }
}

[System.Serializable]
public struct GetUI
{
    public Image getUIObj;
    public Text text;
    public Image icon;
    public Image image;
}
