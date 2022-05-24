using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;
using RNumerics;

public class UnityText : IRText
{
    public void Add(string id, string v, Matrix p)
    {
    }

    public void Add(string id, char c, Matrix p, Colorf color, RFont rFont, RhuEngine.Linker.FontStyle fontStyle, Vector2f textCut)
    {
    }

    public Vector2f Size(RFont rFont, char c, RhuEngine.Linker.FontStyle fontStyle)
    {
        ((Font)rFont.Instances).GetCharacterInfo(c, out var info);
        return new Vector2f(info.glyphWidth, info.glyphHeight);
    }
}
