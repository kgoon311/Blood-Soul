using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image[] fadeImage;
    [SerializeField] private Text dieMassege;
    [SerializeField] private Text clearMassege;

    private void Awake()
    {
        SetInst();
    }

    public void InventoryUpdate(Item item) => inventory.GetNewItem(item);
    public void ItemUISwap(int index) => inventory.ItemSwap(index);
    public void FadeOut(float time , bool clear)
    {
        StartCoroutine(C_FadeOut(time , clear));
    }
    private IEnumerator C_FadeOut(float time, bool clear)
    {
        yield return new WaitForSeconds(2);
        float timer = 0.001f;
        while(timer < 1f)
        {
            timer += Time.deltaTime / time;
            fadeImage[0].color = new Color(0, 0, 0, timer);
            if (clear == true)
                clearMassege.color = new Color(0, 1, 0, timer);
            else
                dieMassege.color = new Color(1, 0, 0, timer);
            yield return null; 
        }
        yield return new WaitForSeconds(1f);
        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 2;
            fadeImage[1].color = new Color(0, 0, 0, timer);
            yield return null;
        }

        if (clear == false)
            SceneManager.LoadScene("InGame");
        else
            SceneManager.LoadScene("Title");
    }
}
