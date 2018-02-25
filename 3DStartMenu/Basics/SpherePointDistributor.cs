using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace WFTools3D.Basics
{
    public class SpherePointDistribution
    {
        static Random r = new Random();
        public Point3D GetRandomPoint()
        {
            var x = r.Next(1,25)- 0.5;
            var y = r.Next(1,25) - 0.5;
            var z = r.Next(1,25) - 0.5;
            var k = Math.Sqrt(x * x + y * y + z * z);
            while (k < 1 || k > 25)
            {
                x = r.Next(1,25) - 0.5;
                y = r.Next(1,25) - 0.5;
                z = r.Next(1,25) - 0.5;
                k = Math.Sqrt(x * x + y * y + z * z);
            }
            return new Point3D( x: (x / k) * 10, y: (y / k) * 10, z: (z / k) * 10);
        }

        public List<Point3D> GetRandomPoints(int size)
        {
            List<Point3D> lst = new List<Point3D>();
            for (var i = 0; i < size; i++)
                lst.Add(GetRandomPoint());

            return lst;
        }
    }
}
