using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Image image;
    private Color color;
    private bool isActive;
    private float timer;
    void Start()
    {
        color = image.color;
    }

    void Update()
    {
        if (timer < 0.2f || timer > 1)
        {
            isActive = !isActive;
            if (isActive == true)
                timer = 0.3f;
            else
                timer = 1f;
        }

        timer = timer + Time.deltaTime * ((isActive == true) ? 1f : -1f) / 2;
        
        image.color = new Color(color.r,color.g, color.b , timer);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
