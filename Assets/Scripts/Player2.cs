using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Character
{
   public void SetHealth(int x)
    {
        Health = x;
        pb.BarValue = x;
    }
}
