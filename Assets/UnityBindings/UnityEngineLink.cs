using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;
using RhuEngine.Physics;

public class UnityEngineLink : IEngineLink
{
    public UnityEngineLink(EngineRunner engineRunner)
    {
        EngineRunner = engineRunner;
    }

    public bool SpawnPlayer => true;

    public bool CanRender => true;

    public bool CanAudio => false;

    public bool CanInput => true;

    public string BackendID => "Unity 2021";

    public EngineRunner EngineRunner { get; }

    public void BindEngine(RhuEngine.Engine engine)
    {
        RLog.Instance = new UnityLoger();
    }

    public void LoadStatics()
    {
        RTexture2D.Instance = new UnityTexture2D();
        RMaterial.Instance = new UnityMaterial();
        RShader.Instance = new UnityShader(EngineRunner);
        RText.Instance = new UnityText();
        RMesh.Instance = new UnityMesh(EngineRunner);
        RRenderer.Instance = new UnityRenderer(EngineRunner);
        RFont.Instance = new UnityFont();
        RTime.Instance = new UnityTime();
        RInput.Instance = new UnityInput(EngineRunner);
        //Use bypass to load libs
        new RBullet.BulletPhsyicsLink(true).RegisterPhysics();
    }

    public void Start()
    {

    }
}
