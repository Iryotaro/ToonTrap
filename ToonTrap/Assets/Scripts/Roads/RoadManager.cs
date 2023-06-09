using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Ryocatusn
{
    public class RoadManager
    {
        public static RoadManager instance = new RoadManager();

        private RoadManager() { }

        private List<Road> roads = new List<Road>();

        public void Save(Road road)
        {
            roads.Add(road);
        }
        public Tilemap[] GetTilemaps()
        {
            return roads.Select(x => x.tilemap).ToArray();
        }
        public void Delete(Road road)
        {
            roads.Remove(road);
        }
    }
}
