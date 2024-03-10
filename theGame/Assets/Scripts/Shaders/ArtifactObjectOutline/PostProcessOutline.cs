using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline
    ("CoolPostProcessYee/PostProcessOutlineRenderer", typeof(UniversalRenderPipeline))]
public class PostProcessOutline : VolumeComponent, IPostProcessComponent 
{
    public FloatParameter thickness = new FloatParameter(1f);
    public FloatParameter depthMin = new FloatParameter(0f);
    public FloatParameter depthMax = new FloatParameter(1f);
    public bool IsActive() => true;
    public bool IsTileCompatible() => true;
}

/*
public class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Hidden/Outline"));
        sheet.properties.SetFloat("_Thickness", settings.thickness);
        sheet.properties.SetFloat("_MinDepth", settings.depthMin);
        sheet.properties.SetFloat("_MaxDepth", settings.depthMax);

        // using the property sheet aka the shader
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
 */