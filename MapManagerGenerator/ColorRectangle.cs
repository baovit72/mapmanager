using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MapManagerGenerator
{
    class ColorRectangle
    {
        public ColorRectangle(string type,int tag,int x, int y,int width,int height, Color color,float zoom,int worldOffset, int item = -2)
        {
            this.x = x;
            this.y = y;
            this.w = width;
            this.h = height;
            this.color = color;
            this.tag = tag;
            this.item = item;
            this.type = type;
            this.zoom = zoom;
            this.offset = worldOffset;

        }
        private int offset { get; set; }
        public int X { get { return (int)(x / zoom); } }
        public int Y { get { return (int)((offset - y) / zoom - 1 ); } }
        public int W { get { return (int)(w / zoom); } }
        public int H { get { return (int)(h / zoom); } }
        public string type { get; set; }
        public float zoom { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public int tag { get; set; }
        public int item { get; set; }
        public Color color { get; set; }
        public Rectangle getRect()
        {
            return new Rectangle(x, y, w, h);
        }
    }
}
