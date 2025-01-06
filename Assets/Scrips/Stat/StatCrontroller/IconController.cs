using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    SpriteRenderer sr;
    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
    public void OpenIcon() => sr.enabled = true;
    public void CloseIcon() => sr.enabled = false;
}
