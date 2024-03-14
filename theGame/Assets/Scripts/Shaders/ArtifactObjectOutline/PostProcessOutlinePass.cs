using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessOutlinePass : ScriptableRenderPass
{
    private Material outlineMaterial;
    private RenderTargetIdentifier source;
    private RenderTargetHandle destination;
    private PostProcessOutline settings;

    public PostProcessOutlinePass()
    {
        // Set the render pass event
        renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        // Load your shader and create the material
        outlineMaterial = CoreUtils.CreateEngineMaterial("Hidden/Outline");
    }

    public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination, PostProcessOutline settings)
    {
        this.source = source;
        this.destination = destination;
        this.settings = settings;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (outlineMaterial == null) return;

        // Apply the effect parameters to the material
        outlineMaterial.SetFloat("_Thickness", settings.thickness.value);
        outlineMaterial.SetFloat("_MinDepth", settings.depthMin.value);
        outlineMaterial.SetFloat("_MaxDepth", settings.depthMax.value);

        // Execute the blit command
        CommandBuffer cmd = CommandBufferPool.Get("PostProcessOutline");
        cmd.Blit(source, destination.Identifier(), outlineMaterial);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
