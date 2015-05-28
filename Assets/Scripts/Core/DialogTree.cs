using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogTree
{
    public bool Repeteable;
    public List<Dialog> dialog = new List<Dialog>();
    public Quest quest;
}

[System.Serializable]
public struct Dialog
{
    public string Text;
    public float Time;
    public bool Said;

    public void SetSaid() { Said = true; }

    public Dialog(string t, float ti, bool s)
    {
        Text = t;
        Time = ti;
        Said = s;
    }
}
