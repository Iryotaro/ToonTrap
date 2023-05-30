using Ryocatusn.StageCreaters;
using Ryocatusn.TileTransforms;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StageCreater : MonoBehaviour
{
    [SerializeField]
    private Area[] areas;
    [SerializeField]
    private Area firstArea;

    [Inject]
    private StageDataCreater stageDataCreater;
    [Inject]
    private AreaService areaService;

    private void Start()
    {
        StageData stageData = stageDataCreater.Create(areas, firstArea, 10);
        Create(stageData);
    }

    private void Create(StageData stageData)
    {
        List<Area> newAreas = new List<Area>();
        foreach (Area area in stageData.areas)
        {
            Area newArea = Instantiate(area);
            newAreas.Add(newArea);
        }

        for (int i = 0; i < newAreas.Count - 1; i++) 
        {
            Area prevArea = newAreas[i];
            Area nextArea = newAreas[i + 1];

            TilePosition tilePosition = areaService.GetNewNextAreaStartPosition(prevArea, nextArea);
            nextArea.ChangePosition(tilePosition);
        }
    }
}
