using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image gauge;
    [SerializeField] private Image gaugeValue;
    private Coroutine SetGaugeCor;
    public bool isDone { get; private set; } = true;

    public void SetGaugeValue(float value, float time)
    {
        isDone = false;
        gauge.fillAmount = value;

        if (SetGaugeCor != null) StopCoroutine(SetGaugeCor);
        SetGaugeCor = StartCoroutine(SetValueToDeltaTime(value, time));
    }

    private IEnumerator SetValueToDeltaTime(float targetValue, float time)
    {
        float gaugeValue = this.gaugeValue.fillAmount;
        float curTime = 0;
        float percent = 0;

        while (percent < 1)
        {
            curTime += Time.deltaTime;
            percent = curTime / time;

            this.gaugeValue.fillAmount = Mathf.Lerp(gaugeValue, targetValue, percent);
            yield return null; 
        }

        isDone = true;
        yield break;
    }

    public float GetGaugeValue()
    {
        return gauge.fillAmount;
    }
}
