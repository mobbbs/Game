using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{

    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float FlashDuration;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(FlashDuration);
        sr.color = currentColor;
        sr.material = originalMat;
    }
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }
    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, 0.3f);
        Invoke("CancelColorChange", seconds);
    }
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }
    public void ChillFxFor(float seconds)
    {
        InvokeRepeating("ChillColorFx", 0, 0.01f);
        Invoke("CancelColorChange", seconds);
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    public void ShockFxFor(float seconds)
    {
        InvokeRepeating("ShockColorFx", 0, 0.3f);
        Invoke("CancelColorChange", seconds);
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

}
