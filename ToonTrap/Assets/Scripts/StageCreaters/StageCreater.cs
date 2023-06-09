using Ryocatusn.Games;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UniRx;
using Ryocatusn.Games.Stages;

namespace Ryocatusn.StageCreaters 
{
    public class StageCreater : MonoBehaviour
    {
        [SerializeField]
        private Area[] areas;
        [SerializeField]
        private Area firstArea;

        private List<Area> enableAreas = new List<Area>();

        [Inject]
        private StageDataCreater stageDataCreater;
        [Inject]
        private DiContainer diContainer;
        [Inject]
        private GameManager gameManager;
        [Inject]
        private StageApplicationService stageApplicationService;

        private void Start()
        {
            StageData stageData = stageDataCreater.Create(areas, firstArea, 3);

            gameManager.SetStageEvent
                .Subscribe(x =>
                {
                    string stageName = stageApplicationService.Get(x.id).name.value;
                    Area[] areas = CreateAreas(firstArea, stageData, stageName);
                })
                .AddTo(this);
        }

        private Area[] CreateAreas(Area firstArea, StageData stageData, string stageName)
        {
            List<Area> newAreas = Instantiate();
            ChangePositions(newAreas);
            SetDeleteAreasHandler(newAreas);
            return newAreas.ToArray();

            List<Area> Instantiate()
            {
                List<Area> areas = new List<Area>();
                foreach (Area area in stageData.areas)
                {
                    Area newArea = diContainer.InstantiatePrefab(area).GetComponent<Area>();
                    SceneManager.MoveGameObjectToScene(newArea.gameObject, SceneManager.GetSceneByName(stageName));
                    areas.Add(newArea);
                    enableAreas.Add(newArea);
                }

                areas.Insert(0, firstArea);

                return areas;
            }
            void ChangePositions(List<Area> areas)
            {
                for (int i = 0; i < areas.Count - 1; i++)
                {
                    Area prevArea = areas[i];
                    Area nextArea = areas[i + 1];

                    nextArea.ChangePosition(prevArea.endJoint.GetPositoin());
                }
            }
            void SetDeleteAreasHandler(List<Area> areas)
            {
                foreach (Area area in areas)
                {
                    area.EndEvent
                    .Subscribe(x =>
                    {
                        DeleteArea(x);
                        CreateAreas(enableAreas[enableAreas.Count - 1], stageData, stageName);
                    })
                    .AddTo(this);
                }
            }
        }
        private void DeleteArea(Area area)
        {
            enableAreas.Remove(area);
            Destroy(area);
        }
    }
}
