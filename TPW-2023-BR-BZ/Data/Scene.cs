using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Scene
    {

        public double Height { get; }  //aranżowanie sceny dla piłek

        public double Length { get; }

        public Scene(double l, double h)
        {
            Length = l;
            Height = h;
        }
    }
}
