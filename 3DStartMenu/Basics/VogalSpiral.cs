using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace WFTools3D.Basics
{

    public class VogalSpiral
    {
//        from pyx import*
//from ent import *
//from math import*

//n = 144
//ca = canvas.canvas()

//phi = (1 + sqrt(5)) / 2.0
//fibs = [0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040]

//        text.defaulttexrunner.set(mode="latex")
//text.defaulttexrunner.preamble("\\usepackage{palatino}")
//for j in range(n):   
//    i = j + 1 
//    r = sqrt(i)
//    theta = i* 2 * pi  / (phi* phi)
//    x = cos(theta)*r
//    y = sin(theta) * r      
//    if i in fibs:
//        ca.fill(path.circle(x,y,0.6), [color.rgb(0.6, 0.6, 1.0)])
//    else:
//        ca.fill(path.circle(x,y,0.6), [color.rgb(0.6, 0.6, 0.6)])
//        ca.stroke(path.circle(x,y,0.6))
    
//    ca.text(x,y,"\\Large "+str(i), [text.halign.boxcenter, text.valign.middle])
    
           
//d = document.document(pages = [document.page(ca, paperformat = document.paperformat.A4, fittosize = 1)])
//d.writePSfile("vogel_labeled.ps")

         public static List<Point3D> GetSpiralPoints(int size)
        {
            var result = new List<Point3D>();

            double phi = (1 + Math.Sqrt(5)) / 2.0;
            int[] fibs = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040 };

            Point3D p = new Point3D();
            for(int i=5;i<size+5;i++)
            {
                int j = i + 1;
                double r = Math.Sqrt(i);
                double theta = j * 2 * Math.PI / (phi * phi);
                p.X = Math.Cos(theta) * r;
                p.Y = Math.Sin(theta) * r;
                p.Z = 0;
                result.Add(p);
            }


            return result;

        }
    }
}
