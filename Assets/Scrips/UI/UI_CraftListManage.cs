using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftListManage : MonoBehaviour
{
    UI_CraftList CraftList => GetComponentInChildren<UI_CraftList>();
    private void Start()
    {
        CraftList.SetUpCraftList();
    }
}
