using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    [SerializeField] private List<int> Modifies;
    public int GetValue()
    {
        int currentValue = baseValue;
        for (int i = 0; i < Modifies.Count; i++)
        {
            currentValue += Modifies[i];
        }
        return currentValue;
    }

    public void setBaseValue(int baseValue)
    {
           this.baseValue = baseValue;
    }

    public void addModifies(int modify)
    {
        Modifies.Add(modify);
    }
    public void removeModifies(int modify)
    {
        Modifies.Remove(modify);
    }
}
