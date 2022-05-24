using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;

public class UnityShader : IRShader
{
    public UnityShader(EngineRunner engineRunner)
    {
        EngineRunner = engineRunner;
    }

    public RShader UnlitClip => new RShader(EngineRunner.UnlitClip);

    public RShader PBRClip => new RShader(EngineRunner.PBRClip);

    public RShader PBR => new RShader(EngineRunner.PBR);

    public RShader Unlit => new RShader(EngineRunner.Unlit);

    public EngineRunner EngineRunner { get; }
}
