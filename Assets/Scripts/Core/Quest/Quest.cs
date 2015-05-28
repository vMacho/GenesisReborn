using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public int code;
    public string text, description;
    bool done;

    public Quest()
    {
        done = false;
    }

    public void SetDone() { done = true; }

    public bool IsDone(){ return done; }
}
