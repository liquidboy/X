using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace X.Engine
{
    using SharpDX.Direct3D12;
    using System.Diagnostics;
    using System.Threading;
    using Windows.UI.Core;

    public class D3D12Pipeline : IDisposable
    {
        [ComImport, Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ICoreWindowInterop
        {
            IntPtr WindowHandle
            {
                get;
            }
            bool MessageHandled
            {
                set;
            }
        }


        private const int SwapBufferCount = 2;
        private int width;
        private int height;
        private Device device;
        private CommandAllocator commandListAllocator;
        private CommandQueue commandQueue;
        private SwapChain swapChain;
        private DescriptorHeap descriptorHeap;
        private GraphicsCommandList commandList;
        private Resource renderTarget;
        private Rectangle scissorRectangle;
        private ViewportF viewPort;
        private AutoResetEvent eventHandle;
        private Fence fence;
        private long currentFence;
        private int indexLastSwapBuf;
        private readonly Stopwatch clock;

        public D3D12Pipeline() {
            clock = Stopwatch.StartNew();
        }

        public void InitPipeline(CoreWindow window, int w, int h) {

            width = w;
            height = h;

            dynamic corewin = window;
            var interop = (ICoreWindowInterop)corewin;
            var handle = interop.WindowHandle;

            

            LoadPipeline(handle);
            LoadAssets();
        }


        private void LoadPipeline(IntPtr handleToWindow)
        {
            // create swap chain descriptor
            var swapChainDescription = new SwapChainDescription()
            {
                BufferCount = SwapBufferCount,
                ModeDescription = new ModeDescription(Format.R8G8B8A8_UNorm),
                Usage = Usage.RenderTargetOutput,
                OutputHandle = handleToWindow,
                SwapEffect = SwapEffect.FlipDiscard,
                SampleDescription = new SampleDescription(1, 0),
                IsWindowed = true
            };

            // create the device
            try
            {
                device = CreateDeviceWithSwapChain(DriverType.Hardware, FeatureLevel.Level_11_0, swapChainDescription, out swapChain, out commandQueue);
            }
            catch (SharpDXException)
            {
                device = CreateDeviceWithSwapChain(DriverType.Warp, FeatureLevel.Level_11_0, swapChainDescription, out swapChain, out commandQueue);
            }

            // create command queue and allocator objects
            commandListAllocator = device.CreateCommandAllocator(CommandListType.Direct);
        }

        private void LoadAssets()
        {
            // Create the descriptor heap for the render target view
            descriptorHeap = device.CreateDescriptorHeap(new DescriptorHeapDescription()
            {
                Type = DescriptorHeapType.RenderTargetView,
                DescriptorCount = 1
            });

            // Create the main command list
            commandList = device.CreateCommandList(CommandListType.Direct, commandListAllocator, null);

            // Get the backbuffer and creates the render target view
            renderTarget = swapChain.GetBackBuffer<Resource>(0);
            device.CreateRenderTargetView(renderTarget, null, descriptorHeap.CPUDescriptorHandleForHeapStart);

            // Create the viewport
            viewPort = new ViewportF(0, 0, width, height);

            // Create the scissor
            scissorRectangle = new Rectangle(0, 0, width, height);

            // Create a fence to wait for next frame
            fence = device.CreateFence(0, FenceFlags.None);
            currentFence = 1;

            // Close command list
            commandList.Close();

            // Create an event handle use for VTBL
            eventHandle = new AutoResetEvent(false);

            // Wait the command list to complete
            WaitForPrevFrame();
        }

        private static Device CreateDeviceWithSwapChain(DriverType driverType, FeatureLevel level,
           SwapChainDescription swapChainDescription,
           out SwapChain swapChain, out CommandQueue queue)
        {
#if DEBUG
            // Enable the D3D12 debug layer.
            // DebugInterface.Get().EnableDebugLayer();
#endif
            using (var factory = new Factory4())
            {
                var adapter = driverType == DriverType.Hardware ? null : factory.GetWarpAdapter();
                var device = new Device(adapter, level);
                queue = device.CreateCommandQueue(new CommandQueueDescription(CommandListType.Direct));

                swapChain = new SwapChain(factory, queue, swapChainDescription);
                return device;
            }
        }

        private void WaitForPrevFrame()
        {
            // WAITING FOR THE FRAME TO COMPLETE BEFORE CONTINUING IS NOT BEST PRACTICE.
            // This is code implemented as such for simplicity.
            long localFence = currentFence;
            commandQueue.Signal(fence, localFence);
            currentFence++;

            if (fence.CompletedValue < localFence)
            {
                fence.SetEventOnCompletion(localFence, eventHandle.GetSafeWaitHandle().DangerousGetHandle());
                eventHandle.WaitOne();
            }
        }

        private void PopulateCommandLists()
        {
            commandListAllocator.Reset();

            commandList.Reset(commandListAllocator, null);

            // setup viewport and scissors
            commandList.SetViewport(viewPort);
            commandList.SetScissorRectangles(scissorRectangle);

            // Use barrier to notify that we are using the RenderTarget to clear it
            commandList.ResourceBarrierTransition(renderTarget, ResourceStates.Present, ResourceStates.RenderTarget);

            // Clear the RenderTarget
            var time = clock.Elapsed.TotalSeconds;
            commandList.ClearRenderTargetView(descriptorHeap.CPUDescriptorHandleForHeapStart, new Color4((float)Math.Sin(time) * 0.25f + 0.5f, (float)Math.Sin(time * 0.5f) * 0.4f + 0.6f, 0.4f, 1.0f), 0, null);

            // Use barrier to notify that we are going to present the RenderTarget
            commandList.ResourceBarrierTransition(renderTarget, ResourceStates.RenderTarget, ResourceStates.Present);

            // Execute the command
            commandList.Close();
        }

        public void Update()
        {
        }

        public void Render()
        {
            // record all the commands we need to render the scene into the command list
            PopulateCommandLists();

            // execute the command list
            commandQueue.ExecuteCommandList(commandList);

            // swap the back and front buffers
            swapChain.Present(1, 0);
            indexLastSwapBuf = (indexLastSwapBuf + 1) % SwapBufferCount;
            Utilities.Dispose(ref renderTarget);
            renderTarget = swapChain.GetBackBuffer<Resource>(indexLastSwapBuf);
            device.CreateRenderTargetView(renderTarget, null, descriptorHeap.CPUDescriptorHandleForHeapStart);

            // wait and reset EVERYTHING
            WaitForPrevFrame();
        }



        public void Dispose()
        {
            clock.Stop();

            // wait for the GPU to be done with all resources
            WaitForPrevFrame();

            swapChain.SetFullscreenState(false, null);

            //eventHandle.Close();
            eventHandle.Dispose();

            // asset objects
            Utilities.Dispose(ref commandList);

            // pipeline objects
            Utilities.Dispose(ref descriptorHeap);
            Utilities.Dispose(ref renderTarget);
            Utilities.Dispose(ref commandListAllocator);
            Utilities.Dispose(ref commandQueue);
            Utilities.Dispose(ref device);
            Utilities.Dispose(ref swapChain);
        }
    }
}
