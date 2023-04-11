using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Scene
    {

        public double Height { get; }  //aranżowanie sceny dla piłek

        public double Width { get; }
       
        public Scene(double w, double h)
        {
            Width = w;
            Height = h;
        }
    }
}
