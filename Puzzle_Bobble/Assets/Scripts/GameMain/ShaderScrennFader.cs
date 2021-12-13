using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScrennFader : MonoBehaviour
{
    [SerializeField] private Material m_Material;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, m_Material);
    }

    // マテリアル（シェーダー）のプロパティを変更
    public void SetMatrialFloat(string paramName, float value)
    {
        m_Material.SetFloat(paramName, value);
    }
}
