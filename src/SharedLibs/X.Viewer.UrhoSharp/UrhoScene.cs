using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Resources;

namespace X.Viewer.UrhoSharp
{
    public partial class UrhoScene : Application
    {

        bool movementsEnabled;
        Scene scene;
        Node plotNode;
        Camera camera;
        Octree octree;
        List<Bar> bars;

        public Bar SelectedBar { get; private set; }

        public IEnumerable<Bar> Bars => bars;

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
        }

        async void CreateScene()
        {
            Input.SubscribeToTouchEnd(OnTouched);

            var cache = ResourceCache;
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            plotNode = scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();
            var plane = baseNode.CreateComponent<StaticModel>();
            plane.Model = ResourceCache.GetModel("Models/Plane.mdl");

            var cameraNode = scene.CreateChild("camera");
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(10, 15, 10) / 1.75f;
            cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

            Node lightNode = cameraNode.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;

            int size = 3;
            baseNode.Scale = new Vector3(size * 1.5f, 1, size * 1.5f);
            bars = new List<Bar>(size * size);
            for (var i = 0f; i < size * 1.5f; i += 1.5f)
            {
                for (var j = 0f; j < size * 1.5f; j += 1.5f)
                {
                    var boxNode = plotNode.CreateChild();
                    boxNode.Position = new Vector3(size / 2f - i, 0, size / 2f - j);
                    var box = new Bar(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                    boxNode.AddComponent(box);
                    box.SetValueWithAnimation((Math.Abs(i) + Math.Abs(j) + 1) / 2f);
                    bars.Add(box);
                }
            }
            SelectedBar = bars.First();
            SelectedBar.Select();
            await plotNode.RunActionsAsync(new EaseBackOut(new RotateBy(2f, 0, 360, 0)));
            movementsEnabled = true;
        }

        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry);
            if (results != null && results.Any())
            {
                var bar = results[0].Node?.Parent?.GetComponent<Bar>();
                if (SelectedBar != bar)
                {
                    SelectedBar?.Deselect();
                    SelectedBar = bar;
                    SelectedBar?.Select();
                }
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            if (Input.NumTouches >= 1 && movementsEnabled)
            {
                var touch = Input.GetTouch(0);
                plotNode.Rotate(new Quaternion(0, -touch.Delta.X, 0), TransformSpace.Local);
            }
            base.OnUpdate(timeStep);
        }

        public void Rotate(float toValue)
        {
            plotNode.Rotate(new Quaternion(0, toValue, 0), TransformSpace.Local);
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }
    }
}
