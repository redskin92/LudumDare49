using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Pixelate : ScriptableRendererFeature
{
    [System.Serializable]
    public class PixelateSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material pixelateMaterial = null;

        [Range(1, 50)]
        public int downSample = 1;
    }

    public PixelateSettings settings = new PixelateSettings();
    
    class CustomRenderPass : ScriptableRenderPass
    {
        public Material pixelateMaterial;

        public bool copyToFramebuffer;

        public int downSample = 2;

        private string profilerTag;
        private int tmpId1;
        private RenderTargetIdentifier tmpRT1;
        
        private RenderTargetIdentifier source { get; set; }

        public CustomRenderPass(string profilerTag)
        {
            this.profilerTag = profilerTag;
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            var width = cameraTextureDescriptor.width / downSample;
            var height = cameraTextureDescriptor.height / downSample;
            tmpId1 = Shader.PropertyToID("tmpPixelateRT1");
            
            cmd.GetTemporaryRT(tmpId1, width, height, 0, FilterMode.Point, RenderTextureFormat.ARGB32);
            
            tmpRT1 = new RenderTargetIdentifier(tmpId1);
            
            ConfigureTarget(tmpRT1);
        }
        
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(profilerTag);

            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            
            cmd.Blit(source, tmpRT1, pixelateMaterial);
            
            cmd.Blit(tmpRT1, source, pixelateMaterial);
            
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass("Pixelate");
        m_ScriptablePass.downSample = settings.downSample;
        m_ScriptablePass.pixelateMaterial = settings.pixelateMaterial;
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTarget;
        m_ScriptablePass.Setup(src);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


