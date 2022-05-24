using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhuEngine;
using RhuEngine.Linker;
using System;
using RNumerics;
using UnityEngine.Experimental.Rendering;

public class UnityTexture2D : IRTexture2D
{
    public RTexture2D White => new(Texture2D.whiteTexture);

    public TexAddress GetAddressMode(object target)
    {
        switch (((Texture2D)target).wrapMode)
        {
            case TextureWrapMode.Repeat:
                return TexAddress.Wrap;
            case TextureWrapMode.Clamp:
                return TexAddress.Clamp;
            case TextureWrapMode.Mirror:
                return TexAddress.Mirror;
            case TextureWrapMode.MirrorOnce:
                return TexAddress.Mirror;
            default:
                break;
        }
        return TexAddress.Wrap;
    }

    public int GetAnisoptropy(object target)
    {
        return ((Texture2D)target).anisoLevel;
    }

    public int GetHeight(object target)
    {
        return ((Texture2D)target).height;
    }

    public TexSample GetSampleMode(object target)
    {
        switch (((Texture2D)target).filterMode)
        {
            case FilterMode.Point:
                return TexSample.Point;
            case FilterMode.Bilinear:
                return TexSample.Anisotropic;
            case FilterMode.Trilinear:
                return TexSample.Anisotropic;
            default:
                break;
        }
        return TexSample.Linear;
    }

    public int GetWidth(object target)
    {
        return ((Texture2D)target).width;
    }

    public object Make(TexType dynamic, TexFormat rgba32Linear)
    {
        var unityFormat = TextureFormat.RGBAFloat;
        var linear = false;
        switch (rgba32Linear)
        {
            case TexFormat.None:
                break;
            case TexFormat.Rgba32:
                unityFormat = TextureFormat.RGBA32;
                break;
            case TexFormat.Rgba32Linear:
                unityFormat = TextureFormat.RGBA32;
                linear = true;
                break;
            case TexFormat.Bgra32:
                unityFormat = TextureFormat.BGRA32;
                break;
            case TexFormat.Bgra32Linear:
                unityFormat = TextureFormat.BGRA32;
                linear = true;
                break;
            case TexFormat.Rg11b10:
                unityFormat = TextureFormat.RGBA32;
                linear = true;
                break;
            case TexFormat.Rgb10a2:
                unityFormat = TextureFormat.RGBA32;
                linear = true;
                break;
            case TexFormat.Rgba64:
                unityFormat = TextureFormat.RGBA64;
                break;
            case TexFormat.R8:
                unityFormat = TextureFormat.R8;
                break;
            case TexFormat.R16:
                unityFormat = TextureFormat.R16;
                break;
            case TexFormat.DepthStencil:
                unityFormat = TextureFormat.R8;
                break;
            case TexFormat.Depth16:
                unityFormat = TextureFormat.R16;
                break;
            default:
                break;
        }
        return new Texture2D(2, 2, unityFormat, true, linear);
    }

    public object MakeFromColors(Colorb[] colors, int width, int height, bool srgb)
    {
        var tex = new Texture2D(width, height, TextureFormat.RGBA32,true,srgb);
        tex.SetPixelData(colors, 0);
        return tex;
    }

    public object MakeFromMemory(byte[] data)
    {
        var tex = new Texture2D(4, 4);
        tex.LoadImage(data);
        return tex;
    }

    public void SetAddressMode(object target, TexAddress value)
    {
        ((Texture2D)target).wrapMode = value switch
        {
            TexAddress.Wrap => TextureWrapMode.Repeat,
            TexAddress.Clamp => TextureWrapMode.Clamp,
            TexAddress.Mirror => TextureWrapMode.Mirror,
            _ => TextureWrapMode.Mirror,
        };
    }

    public void SetAnisoptropy(object target, int value)
    {
        ((Texture2D)target).anisoLevel = value;
    }

    public void SetColors(object tex, int width, int height, byte[] rgbaData)
    {
        ((Texture2D)tex).Reinitialize(width, height);
        ((Texture2D)tex).SetPixelData(rgbaData, 0);
    }

    public void SetSampleMode(object target, TexSample value)
    {
        ((Texture2D)target).filterMode = value switch
        {
            TexSample.Linear => FilterMode.Bilinear,
            TexSample.Point => FilterMode.Point,
            TexSample.Anisotropic => FilterMode.Trilinear,
            _ => FilterMode.Bilinear,
        };
    }

    public void SetSize(object tex, int width, int height)
    {
        ((Texture2D)tex).Reinitialize(width, height);
    }
}
