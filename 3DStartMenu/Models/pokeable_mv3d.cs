using EffectLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml;

namespace WFTools3D.Models
{
    public class effect_host : Grid
    {
        public void set_effect(Effect e)
        {
            this.Effect = e;

        }


        public void remove_effect()
        {
            this.Effect = null;
        }
    }
    public class pokeable_mv3d : ModelVisual3D
    {


        TranslateTransform3D tt3d;
        Viewport2DVisual3D interactive_vp2dv3d;

        private Grid container_grid;


        public effect_host magnify_host;
        public effect_host invert_host;
        public effect_host swirl_host;
        public effect_host brick_host;
        public effect_host ripple_host;
        public effect_host color_key_host;


        private Point mouse_position;


        public void set_control(UIElement uie)
        {
            container_grid.Children.Clear();
            container_grid.Children.Add(uie);
        }


        ////public Effect get_effect()
        ////{
        ////   if (container_grid.Effect != null) {
        ////      return container_grid.Effect;
        ////   }
        ////   else {
        ////      return null;
        ////   }

        ////}


        //public void set_effect(Effect e)
        //{
        //   if (container_grid.Effect != null)
        //   {
        //      if (container_grid.Effect.GetType() == e.GetType())
        //      {
        //         container_grid.Effect = null; // if they're the same type, just remove the effect (a toggle)
        //      }
        //      else
        //      {
        //         container_grid.Effect = e;
        //      }
        //   }
        //   else
        //   {
        //      container_grid.Effect = e;
        //   }
        //}

        //public void remove_effect()
        //{
        //   if (container_grid.Effect != null)
        //   {
        //      container_grid.Effect = null;
        //   }
        //}







        public pokeable_mv3d()
        {
            mouse_position = new Point(0.5, 0.5);


            tt3d = new TranslateTransform3D();
            interactive_vp2dv3d = new Viewport2DVisual3D();

            this.Children.Add(interactive_vp2dv3d);
            MeshGeometry3D mg3d = make_plane_meshgeometry3d();


            container_grid = new Grid();
            container_grid.MouseMove += new System.Windows.Input.MouseEventHandler(container_grid_MouseMove);



            magnify_host = new effect_host();
            swirl_host = new effect_host();
            brick_host = new effect_host();
            ripple_host = new effect_host();
            color_key_host = new effect_host();
            invert_host = new effect_host();


            magnify_host.Children.Add(swirl_host);
            swirl_host.Children.Add(brick_host);
            brick_host.Children.Add(ripple_host);
            ripple_host.Children.Add(invert_host);
            invert_host.Children.Add(color_key_host);
            // color_key_host.Children.Add(invert_host);






            //       FileStream fs = File.Open("c:\\border.xaml", FileMode.Open);

            string my_xaml = "		<Grid 	xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" 	xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Margin=\"150,90,150,145\" SnapsToDevicePixels=\"True\" Width=\"1000\" Height=\"500\" > 			<Border Margin=\"0,0,0,0\" BorderThickness=\"10,10,10,10\" Opacity=\"0.675\"> 				<Border.BorderBrush> 					<LinearGradientBrush EndPoint=\"0.982,0.972\" StartPoint=\"0.01,0.025\"> 						<GradientStop Color=\"#4CFFFFFF\" Offset=\"0\"/> 						<GradientStop Color=\"#44FFFFFF\" Offset=\"1\"/> 						<GradientStop Color=\"#FF545454\" Offset=\"0.174\"/> 						<GradientStop Color=\"#ED434343\" Offset=\"0.688\"/> 						<GradientStop Color=\"#FF2A2A2A\" Offset=\"0.808\"/> 						<GradientStop Color=\"#F2999999\" Offset=\"0.397\"/> 						<GradientStop Color=\"#80494949\" Offset=\"0.571\"/> 					</LinearGradientBrush> 				</Border.BorderBrush> 			</Border> 			<Border Width=\"Auto\" Height=\"Auto\" BorderThickness=\"1,1,1,1\"  BorderBrush=\"#59FFFFFF\"/> 			<Border Width=\"Auto\" Height=\"Auto\" BorderThickness=\"1,1,1,1\" BorderBrush=\"#59FFFFFF\" Margin=\"10,10,10,10\" Name=\"inner_border\"> 								<!--Insert Content Here-->  			</Border> 		</Grid>";
            //   string my_xaml = File.Open("c:\\border.xaml", FileMode.Open);




            StringReader strReader = new StringReader(my_xaml);
            XmlTextReader xmlReader = new XmlTextReader(strReader);

            object obj = XamlReader.Load(xmlReader);

            Grid border_grid = ((Grid)obj);


            Image i = new Image();
            i.Source = make_trasnparent_grid();
            i.HorizontalAlignment = HorizontalAlignment.Stretch;
            i.VerticalAlignment = VerticalAlignment.Stretch;



            container_grid.Children.Add(i);


            color_key_host.Children.Add(container_grid);




            Viewbox vb = new Viewbox();
            vb.Stretch = Stretch.Fill;
            vb.Child = magnify_host;



            (border_grid.FindName("inner_border") as Border).Child = vb;

            interactive_vp2dv3d.Visual = border_grid;



            DiffuseMaterial dm = new DiffuseMaterial();
            Viewport2DVisual3D.SetIsVisualHostMaterial(dm, true);

            interactive_vp2dv3d.Geometry = mg3d;
            interactive_vp2dv3d.Material = dm;


            RenderOptions.SetCachingHint(interactive_vp2dv3d, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(interactive_vp2dv3d, 0.001);
            RenderOptions.SetCacheInvalidationThresholdMaximum(interactive_vp2dv3d, 1000);

        }





        static MeshGeometry3D make_plane_meshgeometry3d()
        {
            MeshGeometry3D mg3d = new MeshGeometry3D();

            mg3d.Positions.Add(new Point3D(-1.5, -.75, 1));
            mg3d.Positions.Add(new Point3D(1.5, -.75, 1));
            mg3d.Positions.Add(new Point3D(1.5, 0.75, 1));
            mg3d.Positions.Add(new Point3D(-1.5, 0.75, 1));

            mg3d.Normals.Add(new Vector3D(0, 0, 1));
            mg3d.Normals.Add(new Vector3D(0, 0, 1));
            mg3d.Normals.Add(new Vector3D(0, 0, 1));
            mg3d.Normals.Add(new Vector3D(0, 0, 1));

            mg3d.TextureCoordinates.Add(new Point(0, 1));
            mg3d.TextureCoordinates.Add(new Point(1, 1));
            mg3d.TextureCoordinates.Add(new Point(1, 0));
            mg3d.TextureCoordinates.Add(new Point(0, 0));

            mg3d.TriangleIndices.Add(0);
            mg3d.TriangleIndices.Add(1);
            mg3d.TriangleIndices.Add(2);

            mg3d.TriangleIndices.Add(2);
            mg3d.TriangleIndices.Add(3);
            mg3d.TriangleIndices.Add(0);

            return mg3d;
        }







        static private RenderTargetBitmap make_trasnparent_grid()
        {
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext ctx = dv.RenderOpen())
            {
                //LinearGradientBrush lgb = new LinearGradientBrush();
                //lgb.GradientStops.Add(new GradientStop(Colors.Tomato, 0));
                //lgb.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                //lgb.StartPoint = new Point(0, 0);
                //lgb.EndPoint = new Point(0, 1);
                //lgb.Freeze();

                //ctx.DrawRectangle(lgb, null, new Rect(0, 0, 1024, 1024));
                Pen whitePen = new Pen();
                whitePen.Brush = Brushes.DarkGray;
                whitePen.Thickness = 6;
                whitePen.Freeze();
                // Horizontal Lines

                const int line_count = 20;

                for (int n = 0; n < line_count; n++)
                {
                    ctx.DrawLine(whitePen, new Point(0, (1.0 / (line_count - 1)) * 1024 * n), new Point(1024, (1.0 / (line_count - 1)) * 1024 * n));
                }

                // Vertical Lines
                for (int n = 0; n < line_count; n++)
                {      // 21 -> 20
                    ctx.DrawLine(whitePen, new Point((1.0 / (line_count - 1)) * 1024 * n, 0), new Point((1.0 / (line_count - 1)) * 1024 * n, 1024));
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(1024, 1024, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            //    ImageBrush ib = new ImageBrush(rtb);
            return rtb;
        }







        void container_grid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point new_center = new Point();
            new_center.X = e.GetPosition(container_grid).X;
            new_center.Y = e.GetPosition(container_grid).Y;

            new_center.X /= container_grid.ActualWidth;
            new_center.Y /= container_grid.ActualHeight;


            mouse_position = new_center;
        }







        double x_axis_angular_speed = 0;
        double y_axis_angular_speed = 0;
        double linear_speed = 0;

        double x_axis_rotational_position = 0;
        double y_axis_rotational_position = 0;
        double linear_position = 0;


        double angular_elasticity_coefficient = 15;
        double linear_elasticity_coefficient = 5;


        double angular_damping_coefficient = 20;
        double linear_damping_coefficient = 50;

        double universal_moment_of_inertia = 5;
        double mass = 4;

        double delta_t = 1.0 / 60.0;


        // p ranges from 0 to 1 on x and y
        public void poke(Point p)
        {

            //Console.WriteLine("orthogonality: " + orthogonality.ToString());

            //          status_textblock.Text = "poke(" + p.ToString() + ")";


            double delta_x = p.X - 0.5;
            double delta_y = p.Y - 0.5;

            y_axis_angular_speed += 30 * delta_x;// *orthogonality;
            x_axis_angular_speed += 30 * delta_y;// *orthogonality;

            linear_speed -= 0.1 * ((1 - delta_x) + (1 - delta_y));

            //Matrix3D old_transform = this.Transform.Value;

            //            RotateTransform3D rt3d = new RotateTransform3D( new AxisAngleRotation3D(new Vector3D(1, 0, 0), x_axis_angular_speed)).Value;
            //          AxisAngleRotation3D new_x = new AxisAngleRotation3D(new Vector3D(1, 0, 0), x_axis_angular_speed);


            //       this.Transform = new MatrixTransform3D( orientation_matrix3d * tt3d.Value );
        }





        double d = 0;




        public void update()
        {
            if (ripple_host.Effect != null)
            {
                ((RippleEffect)(ripple_host.Effect)).Center = mouse_position;
                ((RippleEffect)(ripple_host.Effect)).Phase -= 0.1;
            }

            if (magnify_host.Effect != null)
            {
                ((SmoothMagnifyEffect)(magnify_host.Effect)).Center = mouse_position;
            }

            if (swirl_host.Effect != null)
            {
                ((SwirlEffect)(swirl_host.Effect)).SwirlStrength = 1.5 * Math.Sin(d * 2);
            }


            if (brick_host.Effect != null)
            {
                ((BrickMasonEffect)(brick_host.Effect)).BrickCounts = new Size(50 * (Math.Sin(d / 2) + 1.2), 50 * (Math.Sin(d / 2) + 1.2));
            }

            d += 0.05;

            update_physics();
        }







        public void update_physics()
        {
            // bobbing up and down
            tt3d.OffsetY = Math.Sin(d) / 90;
            tt3d.OffsetX = Math.Cos(d / 3) / 80;



            // update forces
            // F = m a = -kx -bv
            double x_axis_force = (x_axis_rotational_position * angular_elasticity_coefficient) + (x_axis_angular_speed * angular_damping_coefficient);
            double y_axis_force = (y_axis_rotational_position * angular_elasticity_coefficient) + (y_axis_angular_speed * angular_damping_coefficient);
            double linear_force = (linear_position * linear_elasticity_coefficient) + (linear_speed * linear_damping_coefficient);


            // update velocity and position
            double x_axis_angular_acceleration = x_axis_force / universal_moment_of_inertia;
            double y_axis_angular_acceleration = y_axis_force / universal_moment_of_inertia;
            double linear_acceleration = linear_force / mass;


            x_axis_angular_speed -= x_axis_angular_acceleration * delta_t;
            y_axis_angular_speed -= y_axis_angular_acceleration * delta_t;
            linear_speed -= linear_acceleration * delta_t;


            //   next_position = position + velocity * delta_t + (.5 * acceleration * delta_t * delta_t);


            // angular position and speed
            x_axis_rotational_position += x_axis_angular_speed;
            y_axis_rotational_position += y_axis_angular_speed;
            linear_position += linear_speed;

            // "drag"
            //          x_axis_angular_speed *= 0.9;
            //          y_axis_angular_speed *= 0.9;

            Matrix3D matrix_delta_x = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), x_axis_rotational_position)).Value;
            Matrix3D matrix_delta_y = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y_axis_rotational_position)).Value;
            Matrix3D matrix_delta_position = new TranslateTransform3D(0, 0, linear_position).Value;

            // orientation_matrix3d = matrix_delta_x * matrix_delta_y * tt3d.Value;

            this.Transform = new MatrixTransform3D(matrix_delta_x * matrix_delta_y * matrix_delta_position * tt3d.Value);

        }

    }
}
