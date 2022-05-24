using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;

public class UnityTime : IRTime
{
    public float Elapsedf => Time.deltaTime;
}
