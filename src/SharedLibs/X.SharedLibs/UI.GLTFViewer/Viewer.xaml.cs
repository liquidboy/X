using SceneLoaderComponent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Composition.Scenes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.UI.GLTFViewer
{
    public sealed partial class Viewer : UserControl
    {
        Compositor compositor;
        SceneVisual sceneVisual;

        public Viewer()
        {
            this.InitializeComponent();
        }

        private void InitializeGltf() {
            compositor = ElementCompositionPreview.GetElementVisual(grdPlayer).Compositor;

            var root = compositor.CreateContainerVisual();

            root.Size = new System.Numerics.Vector2(1000, 1000);
            ElementCompositionPreview.SetElementChildVisual(grdPlayer, root);

            sceneVisual = SceneVisual.Create(compositor);
            root.Children.InsertAtTop(sceneVisual);

            sceneVisual.Offset = new System.Numerics.Vector3(200, 150, 0);
            sceneVisual.RotationAxis = new System.Numerics.Vector3(0, 1, 0);
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGltf();

            //var sceneNode = await LoadGLTF(new Uri("ms-appx:///Assets/DamagedHelmet.gltf"));
            //sceneVisual.Root = sceneNode;
        }

        async Task<SceneNode> LoadGLTF(Uri uri)
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IBuffer buffer = await FileIO.ReadBufferAsync(storageFile);

            SceneLoader loader = new SceneLoader();
            return loader.Load(buffer, compositor);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //var sceneNode = await LoadGLTF(new Uri("ms-appx:///Assets/DamagedHelmet.gltf"));
            //sceneVisual.Root = sceneNode;

            // ---------

            //var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();

            //rotationAnimation.InsertKeyFrame(0f, 0.0f, compositor.CreateLinearEasingFunction());
            //rotationAnimation.InsertKeyFrame(0.5f, 360.0f, compositor.CreateLinearEasingFunction());
            //rotationAnimation.InsertKeyFrame(1f, 0.0f, compositor.CreateLinearEasingFunction());

            //rotationAnimation.Duration = TimeSpan.FromSeconds(8);
            //rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            //sceneVisual.StartAnimation("RotationAngleInDegrees", rotationAnimation);


            sceneVisual.RotationAngleInDegrees += 4f;
        }
    }
}
