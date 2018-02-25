using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WFTools3D;
using WFTools3D.Basics;
using WFTools3D.Models;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        pokeable_mv3d pokeable_modelvisual_3d = new pokeable_mv3d();
        TextBox tb = new TextBox();
        List<Point3D> lst = VogalSpiral.GetSpiralPoints(144);
        private string[] icons;
        public MainWindow()
        {
            InitializeComponent();
            //var distributor = new SpherePointDistribution();
            //List<Point3D> lst = VogalSpiral.GetSpiralPoints(144); //distributor.GetRandomPoints(56);
            //foreach(var f in icons)
            //{
            //    System.Drawing.Image dummy = System.Drawing.Image.FromFile(f);
            //    dummy.Save(f+".bmp", ImageFormat.Bmp);
            //}
            icons = Directory.GetFiles(@".\icons");
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.Width = 300;
            tb.Height = 300;
            tb.AcceptsReturn = true;
            tb.FontSize = 50;

            //    tb.TextWrapping = TextWrapping.Wrap;
            tb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tb.Background = new LinearGradientBrush(Colors.DarkGray, Colors.SteelBlue, 45);
            tb.SpellCheck.IsEnabled = true;
            pokeable_modelvisual_3d.set_control(tb);
            scene.Background = new ImageBrush(new BitmapImage(new Uri(new FileInfo(@".\space.jpg").FullName)));
            //scene.Models.Add(new AxisModel(20));
            
            Sphere windows = new Sphere(32);
            //sun.DiffuseMaterial.Brush = Brushes.Goldenrod;
            windows.DiffuseMaterial.Brush = new ImageBrush(new BitmapImage(new Uri(new FileInfo(@".\win.jpg").FullName)));
            scene.Models.Add(windows);
            for (int i = 0; i < 144; i++)
            {
                //Sphere earth = CreateEarth(i * 1.5, i * 1.5, 0);
                Disk point = CreatePoint(lst[i].X, lst[i].Y, lst[i].Z);
                 
                //var moon = CreateMoon(2, 0, 0);
                //earth.Children.Add(moon);
            }

            // moon camera
            //scene.ActivateCamera(2);
            //scene.Camera.Position = new Point3D(9, 0, 0.1);
            //scene.Camera.LookDirection = Math3D.UnitY;
            //scene.Camera.UpDirection = Math3D.UnitZ;
            //scene.Camera.Rotate(Math3D.UnitZ, -30);
            //scene.Camera.ChangeRoll(-12);
            //scene.Camera.Speed = 8;

            //scene.ActivateCamera(1);
            //scene.Camera.Position = new Point3D(0, 4, 0);
            //scene.Camera.LookDirection = Math3D.UnitX;
            //scene.Camera.UpDirection = Math3D.UnitZ;
            //scene.Camera.ChangeRoll(25);
            //scene.Camera.Speed = 8;

            scene.ActivateCamera(0);
            scene.Camera.Position = new Point3D(25, -15, 8);
            scene.Camera.LookAtOrigin();
            scene.Camera.Move(scene.Camera.LookDirection, -20);
            scene.Camera.Rotate(Math3D.UnitZ, 10);
            scene.Camera.Rotate(scene.Camera.RightDirection, -20);



            //scene.ToggleHelperModels();
            scene.TimerTicked += TimerTicked;
            scene.StartTimer();
            scene.MouseLeftButtonDown += Scene_MouseLeftButtonDown;

        }

        private void Scene_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mouseposition = e.GetPosition(scene);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);

            VisualTreeHelper.HitTest(scene, null, HTResult, pointparams);

        }
        Point3D old;
        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            if (rayResult != null)
            {
                if (rayResult.VisualHit is Disk)
                {
                    MessageBox.Show(((Disk)(rayResult.VisualHit)).AppName);
                }

                if (scene.Models[0] == rayResult.VisualHit)
                {
                    if (!((Sphere)scene.Models[0]).Children.Contains(pokeable_modelvisual_3d))
                    {
                        //System.Diagnostics.Process.Start("Notepad.exe");
                        scene.ToggleTimer();
                        //scene.Models.Add(new TextVisual3D() {Text="Hello",  Position= new Point3D (0,0,3) });
                        old = scene.Camera.Position;
                        scene.Camera.Position = new Point3D(15, -6, 12);
                        scene.Camera.Rotate(new Vector3D(2, 1, 1), -70);
                        ((Sphere)scene.Models[0]).Children.Add(pokeable_modelvisual_3d);
                        tb.Focus();
                        tb.TextChanged += Tb_TextChanged;
                        //tb.Text = angle.ToString();                        
                    }
                    else
                    {
                        scene.ToggleTimer();
                        //scene.Models.Add(new TextVisual3D() {Text="Hello",  Position= new Point3D (0,0,3) });
                        //scene.Camera.Position = new Point3D(25, -15, 8);
                        //scene.Camera.Rotate(new Vector3D(2, 1, 1), 100);
                        ((Sphere)scene.Models[0]).Children.Remove(pokeable_modelvisual_3d);
                        scene.Camera.Position = old;
                       
                    }

                }

                //if (cross == rayResult.ModelHit)
                //{
                //    this.Close();
                //}

                //if (pyramid == rayResult.ModelHit)
                //{

                //    DragMove();
                //}


            }


            return HitTestResultBehavior.Stop;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((TextBox)sender).Text;
            if (txt.Length > 2)
            {
                // remove all the icons that doesn't have the name
                var apps = scene.Models.Where(x => x is Disk).ToArray();
                List<Disk> filtered = new List<Disk>();
                for(int i=0 ; i < apps.Length;i++)
                {
                    if (((Disk)apps[i]).AppName.Contains(txt))
                    {
                        filtered.Add(((Disk)apps[i]));
                    }
                    else
                    {
                        scene.Models.Remove(apps[i]);
                    }
                }

                // now add the removed ones to the early vogal points
                for (int i = 0; i < filtered.Count; i++)
                {
                    filtered[i].Position = lst[i];
                }

            }
        }

        private static Sphere CreateSubPoint(double x, double y, double z)
        {
            Sphere sub = new Sphere { Radius = 0.3, Position = new Point3D(x, y, z) };
            sub.DiffuseMaterial.Brush = Brushes.NavajoWhite;
            return sub;
        }
        
        Random r = new Random();
        private Disk CreatePoint(double x,double y ,double z)
        {
            int i = r.Next(1, 55);
            Disk point = new Disk(24) { Radius = 0.5, Position = new Point3D(x, y, z) };
            point.AppName = icons[i];
            //point.Rotation2 = Math3D.RotationY(90);
            //            ImageBrush b = new ImageBrush(new BitmapImage(new Uri(@"C:\Users\hakhedre\Documents\Visual Studio 2015\Projects\WFTools3D\WFTools3D\Demo\bin\Debug\icons\facebook.png")));
            ImageBrush b = new ImageBrush(new BitmapImage(new Uri(new FileInfo(icons[i]).FullName)));
            //b.TileMode = TileMode.Tile;
            //b.Stretch = Stretch.None;
            //b.AlignmentX = AlignmentX.Left;
            //b.AlignmentY = AlignmentY.Top;
            //b.ViewportUnits = BrushMappingMode.Absolute;
            //b.Viewport = new Rect(0, 0, 0.1, 0.1);
            //b.Viewbox = new Rect(0, 0, 1, 1);
            point.DiffuseMaterial.Brush = b;

            scene.Models.Add(point);
            return point;
        }

        void TimerTicked(object sender, EventArgs e)
        {
            angle += 2;
            for (int i = 0; i < 145 ; i++)
            {
                Object3D point = scene.Models[i] as Object3D;
                if (point is Sphere)
                {
                   point.Rotation1 = Math3D.RotationZ(angle);
                }
                else
                {
                    //point.Rotation1 = Math3D.RotationX(angle);
                }
                point.Rotation3 = Math3D.RotationZ(angle * 0.1);
            }
        }
        double angle;

    }
}
