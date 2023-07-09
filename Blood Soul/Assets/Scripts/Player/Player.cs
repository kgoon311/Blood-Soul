using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player>
{
    public PlayerStats playerStats;
    private PlayerController playerController;
    [SerializeField] private GaugeBar playerHpGauge;
    [SerializeField] private GaugeBar playerMpGauge;
    [SerializeField] private GaugeBar playerStaminaGauge;
    [SerializeField] private GameObject helmat;
    [SerializeField] private float staminaPlayBackValue;
    private float maxPlayerHp;
    private float maxPlayerMp;
    private float maxPlayerStamina;
    private bool isDie = false;

    public Potion[] potions;
    public int CurItemIndex { get; set; } = 0;
    #region Gauge

    public float HP
    {
        get => playerStats.hp;
        set
        {
            if (value <= 0)
            {
                value = 0; 
                Die(); 
            }
            else if (value > maxPlayerHp) value = maxPlayerHp;

            if (value > playerStats.hp) playerHpGauge.SetGaugeValue(value / maxPlayerHp, 0.1f);
            else playerHpGauge.SetGaugeValue(value / maxPlayerHp, 1.25f);

            playerStats.hp = value;
        }
    }
    public float MP
    {
        get => playerStats.mp;
        set
        {
            if (value < 0) value = 0;
            else if (value > maxPlayerMp) value = maxPlayerMp;

            if (value > playerStats.mp) playerMpGauge.SetGaugeValue(value / maxPlayerMp, 0.1f);
            else playerMpGauge.SetGaugeValue(value / maxPlayerMp, 1.25f);

            playerStats.mp = value;
        }
    }
    public float Stamina
    {
        get => playerStats.stamina;
        set
        {
            if (value < 0) value = 0;
            else if (value > maxPlayerStamina) value = maxPlayerStamina;

            if (value > playerStats.stamina) playerStaminaGauge.SetGaugeValue(value / maxPlayerStamina, 0.1f);
            else playerStaminaGauge.SetGaugeValue(value / maxPlayerStamina, 1.25f);

            playerStats.stamina = value;
        }
    }

    #endregion

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        SetInst();
        PlayerInit();
    }

    private void PlayerInit()
    {
        playerStats = new PlayerStats();
        maxPlayerHp = playerStats.hp;
        maxPlayerMp = playerStats.mp;
        maxPlayerStamina = playerStats.stamina;
    }

    private void Update()
    {
        PlayBack_Stamina();
    }

    private void Die()
    {
        if (!isDie)
        {
            isDie = true;
            playerController.PlayerDie_Animation();
            UIManager.Inst.FadeOut(4, false);
        }
    }
    public void GetDamage(float damage, bool knockback = false)
    {
        if (playerController.isInvis) return;

        if(damage > 0)
        {
            HP -= damage;
        }

        if (knockback) playerController.PlayerKnockBack_Animation();
    }
    private void PlayBack_Stamina()
    {
        if(Stamina < maxPlayerStamina && playerStaminaGauge.isDone)
        {
            var value = staminaPlayBackValue * Time.deltaTime;
            Stamina += value;
        }
    }
    public void GetSkill() => playerController.isExistSkill = true;
    public void GetHelmat() => helmat.SetActive(true);
}

[System.Serializable]
public class PlayerStats
{
    public float hp = 0;
    public float mp = 0;
    public float stamina = 0;
    public float moveSpeed = 0;

    public PlayerStats()
    {
        hp = 200;
        mp = 200;
        stamina = 100;
        moveSpeed = 10;
    }
}
