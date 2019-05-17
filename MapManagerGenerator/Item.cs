using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapManagerGenerator
{
    class Item
    {
        private List<ColorRectangle> rects;
 
        public Item()
        {
            rects = new List<ColorRectangle>();
        }
        public String ToString( )
        {
            String output = "";
            output += "ITEMS \r\n";

            foreach (ColorRectangle r in rects)
            {
                output += r.tag.ToString() + "-" + r.X.ToString() + "-"
                    + r.Y.ToString() + "-" + r.W.ToString() + "-" + r.H.ToString() + "-" + r.item.ToString() + "   \r\n";
            }
            return output;
        }

        public void Remove(ColorRectangle rect)
        {
            this.rects.Remove(rect);
        }
        public void Add(ColorRectangle rect)
        {

            rects.Add(rect);


        }
    }
}
