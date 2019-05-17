using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapManagerGenerator
{
    class GridInfo
    {
        private Dictionary<int, Dictionary<int, List<ColorRectangle>>> infos;
        private List<Tuple<int, int>> occupiedIndexes;
        static int count = 0;
        private int gridSize;
        public GridInfo(int size)
        {
            infos = new Dictionary<int, Dictionary<int, List<ColorRectangle>>>();
            occupiedIndexes = new List<Tuple<int, int>>();
            this.gridSize = size;
        }
        static int incCount()
        {
            count++;
            return count;
        }
        public void Add(ColorRectangle obj)
        {
            

            int gridX = obj.X / gridSize;
            int gridY = obj.Y / gridSize;
            Tuple<int, int> tuple = new Tuple<int, int>(gridX, gridY);

            //Tìm xem đã map key chưa
            bool matchedX = false;
            bool matched = false;
            foreach (Tuple<int, int> t in occupiedIndexes)
            {
                if (t.Item1 == tuple.Item1)
                {
                    if (t.Item2 == tuple.Item2)
                        matched = true;
                    matchedX = true;
                }
            }
            if (!infos.ContainsKey(tuple.Item1))
            {
               
                infos.Add(tuple.Item1, new Dictionary<int, List<ColorRectangle>>());

            }
            if ( !infos[tuple.Item1].ContainsKey(tuple.Item2))
            {
               
                 
                infos[tuple.Item1].Add(tuple.Item2, new List<ColorRectangle>());
                occupiedIndexes.Add(tuple);
            }
            
            infos[tuple.Item1][tuple.Item2].Add(obj);
            System.Diagnostics.Debug.WriteLine(tuple.Item1 + "   -   " + tuple.Item2);

        }
        public string ToString()
        {
            string output = "";
            output += "GRID \r\n";

            foreach (Tuple<int, int> index in occupiedIndexes)
            {
                System.Diagnostics.Debug.WriteLine(index.ToString());
                output += index.Item1.ToString() + "-" + index.Item2.ToString();
                foreach (ColorRectangle rectangle in infos[index.Item1][index.Item2])
                {
                    output += "-" + rectangle.tag.ToString();
                }
                output += "\r\n";
            }
            return output;
        }
        public void Remove(ColorRectangle rect)
        {
            foreach (Tuple<int, int> index in occupiedIndexes.ToList<Tuple<int,int>>())
            {
                List<ColorRectangle> rectangles = infos[index.Item1][index.Item2];
                rectangles.Remove(rect);
                if (rectangles.Count == 0)
                {
                    occupiedIndexes.Remove(index);
                    infos[index.Item1].Remove(index.Item2);
                     
                }
            }
        }
    }
}
