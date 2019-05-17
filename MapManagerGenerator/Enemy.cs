using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapManagerGenerator
{
    class Enemy
    {
        private List<ColorRectangle> rects;
        public Enemy()
        {
            rects = new List<ColorRectangle>();
        }
        public void Add(ColorRectangle rect)
        {
            this.rects.Add(rect);
        }
        public void Remove(ColorRectangle rect)
        {
            this.rects.Remove(rect);
        }
        public String ToString()   
        { 
            string output = "";
            output += "ENEMIES \r\n";
            foreach (ColorRectangle r in rects)
            {
                output += r.tag.ToString() + "-" + r.X.ToString() + "-"
                    + r.Y.ToString() + "-" + r.W.ToString() + "-" + r.H.ToString() + "\r\n";
            }
            return output;
        }
    }
}
