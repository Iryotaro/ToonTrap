using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Ryocatusn.StageCreaters
{
    public class StageDataCreater
    {
        [Inject]
        private AreaService areaService;

        public StageData Create(Area[] areas, Area firstArea, int numberOfStages)
        {
            List<Area> areaData = new List<Area>();

            Area area = firstArea;

            for (int i = 0; i < numberOfStages; i++)
            {
                area = GetRandomNextArea(areas, area);
                areaData.Add(area);
            }

            areaData.Insert(0, firstArea);

            return new StageData(areaData.ToArray());
        }

        private Area GetRandomNextArea(Area[] areas, Area prevArea)
        {
            Area[] candidateAreas = areas.Where(x => areaService.IsAllowedToConnect(prevArea, x)).ToArray();

            return candidateAreas[Random.Range(0, candidateAreas.Count() - 1)];
        }
    }
}
