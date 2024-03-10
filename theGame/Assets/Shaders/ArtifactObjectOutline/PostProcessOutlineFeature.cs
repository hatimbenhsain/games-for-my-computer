using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessOutlineFeature : ScriptableRendererFeature
{
    class PostProcessOutlinePass : ScriptableRenderPass
    {
        private Material outlineMaterial = null;
        private RenderTargetIdentifier source { get; set; }
        private RenderTargetHandle destination { get; set; }
        private PostProcessOutline settings;

        public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination, PostProcessOutline settings)
        {
            this.source = source;
            this.destination = destination;
            this.settings = settings;

            if (outlineMaterial == null)
                outlineMaterial = CoreUtils.CreateEngineMaterial("HighlightOutline");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings == null || outlineMaterial == null)
                return;

            // Apply the effect parameters to the material
            outlineMaterial.SetFloat("_Thickness", settings.thickness.value);
            outlineMaterial.SetFloat("_MinDepth", settings.depthMin.value);
            outlineMaterial.SetFloat("_MaxDepth", settings.depthMax.value);

            CommandBuffer cmd = CommandBufferPool.Get("PostProcessOutline");

            Blit(cmd, source, destination.Identifier(), outlineMaterial);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    PostProcessOutlinePass m_ScriptablePass;
    public override void Create()
    {
        m_ScriptablePass = new PostProcessOutlinePass();
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isSceneViewCamera)
            return;

        var volumeStack = VolumeManager.instance.stack;
        PostProcessOutline settings = volumeStack.GetComponent<PostProcessOutline>();

        // Check if the post-process outline component is active
        if (settings == null || !settings.IsActive())
            return;

        m_ScriptablePass.Setup(renderer.cameraColorTarget, RenderTargetHandle.CameraTarget, settings);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
