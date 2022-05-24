using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;

public class UnityFont : IRFont
{
    public Font Main { get; private set; } = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

    public RFont Default => new RFont(Main);
}
