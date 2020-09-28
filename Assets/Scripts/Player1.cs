using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Character
{

    public void SetHealth(int x)
    {
        Health = x;
        pb.BarValue = x;
    }
}
