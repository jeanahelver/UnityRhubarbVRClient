using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;
using RNumerics;
using TMPro;
public class UnityText : IRText
{
    public EngineRunner EngineRunner;
    public UnityText(EngineRunner engineRunner)
    {
        EngineRunner = engineRunner;
    }

    public void Add(string id, string v, Matrix p)
    {
        EngineRunner.AddText(id, v, p);
    }

    public void Add(string id, char c, Matrix p, Colorf color, RFont rFont, RhuEngine.Linker.FontStyle fontStyle, Vector2f textCut)
    {
        EngineRunner.AddChar(id, c, p,(Font)rFont.Instances,fontStyle,textCut);
    }

    public Vector2f Size(RFont rFont, char c, RhuEngine.Linker.FontStyle fontStyle)
    {
        return EngineRunner.RunonMainThread(() =>
        {
            ((Font)rFont.Instances).GetCharacterInfo(c, out var info);
            return new Vector2f(100f / info.glyphWidth, 100f / info.glyphHeight);
        });
    }
}
