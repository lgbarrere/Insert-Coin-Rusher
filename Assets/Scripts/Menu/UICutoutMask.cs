using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UICutoutMask : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
