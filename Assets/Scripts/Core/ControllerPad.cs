using UnityEngine;
using System.Collections;

public class ControllerPad
{
    protected button[] Buttons = new button[12];
    protected float[] Pad = new float[8];
    
    public bool GetButton(button_pad btn)
    {
        return Buttons[(int)btn].state;
    }
    
    public void SetButton(button_pad btn, bool val)
    {
        Buttons[(int)btn].state = val;
    }

    public float GetPad(direcction_pad btn)
    {
        return Pad[(int)btn];
    }

    public void SetPad(direcction_pad btn, float val)
    {
        Pad[(int)btn] = val;
    }
}

public enum direcction_pad
{
    Up,
    Right,
    Down,
    Left,
    Up_2,
    Right_2,
    Down_2,
    Left_2,
};

public enum button_pad
{
    Triangle,
    Circle,
    Cross,
    Square,
    L1,
    L2,
    L3,
    R1,
    R2,
    R3
};

public struct button
{
    public bool state {get; set;}
}