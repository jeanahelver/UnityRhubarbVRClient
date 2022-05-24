using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;
using System.IO;
using UnityEngine.XR;
using RNumerics;



public class EngineRunner : MonoBehaviour
{
    public static EngineRunner _;


    private static Vector2f LastMousePos;
    public static Vector2f MouseDelta;

    public GameObject UserRoot;

    public GameObject UserHead;

    public GameObject Root;

    public GameObject LeftController;

    public GameObject RightController;

    public Shader UnlitClip;

    public Shader Unlit;

    public Shader PBR;

    public Shader PBRClip;

    public InputDevice left;

    public InputDevice right;

    public bool isHardwarePresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                var inputDevices = new List<UnityEngine.XR.InputDevice>();
                UnityEngine.XR.InputDevices.GetDevices(inputDevices);

                foreach (var device in inputDevices)
                {
                    Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
                    if ((device.characteristics & InputDeviceCharacteristics.Controller) != InputDeviceCharacteristics.None)
                    {
                        if ((device.characteristics & InputDeviceCharacteristics.Right) != InputDeviceCharacteristics.None)
                        {
                            right = device;
                        }
                        if ((device.characteristics & InputDeviceCharacteristics.Left) != InputDeviceCharacteristics.None)
                        {
                            left = device;
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }

    public Engine engine;
    public OutputCapture cap;
    public UnityEngineLink link;

    void Start()
    {
        _ = this;
        var isVR = isHardwarePresent();
        if (!isVR)
        {
            Debug.Log("Starting RuhbarbVR ScreenMode");
            Application.targetFrameRate = 60;
        }
        else
        {
            Debug.Log("Starting RuhbarbVR VRMode");
        }
        Debug.Log("Graphics Memory Size: " + SystemInfo.graphicsMemorySize);
        cap = new OutputCapture();
        link = new UnityEngineLink(this);
        var args = Environment.GetCommandLineArgs();
        var appPath = Path.GetDirectoryName(Application.dataPath);
        engine = new Engine(link, args, cap, appPath);
        engine.Init(false);
    }

    public class TempMesh
    {
        public bool UsedThisFrame = true;

        public GameObject gameObject;

        public MeshFilter meshfilter;

        public MeshRenderer meshRenderer;

        public Material Parentmaterial;

        public Material material;

        public void Reload(Mesh mesh, Material target, Matrix p, Colorf tint)
        {
            UsedThisFrame = true;
            if (Parentmaterial == target)
            {
                try
                {
                    material.color = new Color(tint.r, tint.g, tint.b, tint.a);
                }
                catch { }
            }
            else
            {
                Parentmaterial = target;
                UnityEngine.Object.Destroy(material);
                material = new Material(target);
                try
                {
                    material.color = new Color(tint.r, tint.g, tint.b, tint.a);
                }
                catch { }
            }

            meshRenderer.material = material;
            meshfilter.mesh = mesh;
            var pos = p.Translation;
            var rot = p.Rotation;
            var scale = p.Scale;
            gameObject.transform.localPosition = new Vector3(float.IsNaN(pos.x) ? 0 : pos.x, float.IsNaN(pos.y) ? 0 : pos.y, float.IsNaN(pos.z) ? 0 : pos.z);
            gameObject.transform.localRotation = new Quaternion(float.IsNaN(rot.x) ? 0 : rot.x, float.IsNaN(rot.y) ? 0 : rot.y, float.IsNaN(rot.z) ? 0 : rot.z, float.IsNaN(rot.w) ? 0 : rot.w);
            gameObject.transform.localScale = new Vector3(float.IsNaN(scale.x) ? 0 : scale.x, float.IsNaN(scale.y) ? 0 : scale.y, float.IsNaN(scale.z) ? 0 : scale.z);
        }

        public TempMesh(string id, Mesh mesh, Material target, Matrix p, Colorf tint)
        {
            Parentmaterial = target;
            material = new Material(target);
            try
            {
                material.color = new Color(tint.r, tint.g, tint.b, tint.a);
            }
            catch { }
            gameObject = new GameObject("TempMesh" + id);
            gameObject.transform.parent = EngineRunner._.Root.transform;
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            meshfilter = gameObject.AddComponent<MeshFilter>();
            meshfilter.mesh = mesh;
            var pos = p.Translation;
            var rot = p.Rotation;
            var scale = p.Scale;
            gameObject.transform.localPosition = new Vector3(float.IsNaN(pos.x) ? 0 : pos.x, float.IsNaN(pos.y) ? 0 : pos.y, float.IsNaN(pos.z) ? 0 : pos.z);
            gameObject.transform.localRotation = new Quaternion(float.IsNaN(rot.x) ? 0 : rot.x, float.IsNaN(rot.y) ? 0 : rot.y, float.IsNaN(rot.z) ? 0 : rot.z, float.IsNaN(rot.w) ? 0 : rot.w);
            gameObject.transform.localScale = new Vector3(float.IsNaN(scale.x) ? 0 : scale.x, float.IsNaN(scale.y) ? 0 : scale.y, float.IsNaN(scale.z) ? 0 : scale.z);
        }

        public void Remove()
        {
            UnityEngine.Object.Destroy(gameObject);
            UnityEngine.Object.Destroy(material);
        }
    }

    public Dictionary<string, TempMesh> tempmeshes = new();

    public void Draw(string id, Mesh mesh, Material target, Matrix p, Colorf tint)
    {
        if (!tempmeshes.ContainsKey(id))
        {
            tempmeshes.Add(id, new TempMesh(id, mesh, target, p, tint));
        }
        else
        {
            tempmeshes[id].Reload(mesh, target, p, tint);
        }
    }

    void Update()
    {
        var cpos = Input.compositionCursorPos;
        var rhubarbcpos = new Vector2f(cpos.x, cpos.y);
        MouseDelta = LastMousePos - rhubarbcpos;
        LastMousePos = rhubarbcpos;

        foreach (var item in tempmeshes)
        {
            item.Value.UsedThisFrame = false;
        }
        engine.Step();
        var removethisframe = new List<string>();
        foreach (var item in tempmeshes)
        {
            if (!item.Value.UsedThisFrame)
            {
                removethisframe.Add(item.Key);
                item.Value.Remove();
            }
        }
        foreach (var item in removethisframe)
        {
            tempmeshes.Remove(item);
        }
    }
}
