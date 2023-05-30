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
            StageData stageData = stageDataCreater.Create(areas, firstArea, 10);

            gameManager.SetStageEvent
                .Subscribe(x =>
                {
                    string stageName = stageApplicationService.Get(x.id).name.value;
                    Create(stageData, stageName);
                });
        }

        private void Create(StageData stageData, string stageName)
        {
            List<Area> newAreas = new List<Area>();
            foreach (Area area in stageData.areas)
            {
                Area newArea = diContainer.InstantiatePrefab(area).GetComponent<Area>();
                SceneManager.MoveGameObjectToScene(newArea.gameObject, SceneManager.GetSceneByName(stageName));
                newAreas.Add(newArea);
            }

            newAreas.Insert(0, firstArea);

            for (int i = 0; i < newAreas.Count - 1; i++)
            {
                Area prevArea = newAreas[i];
                Area nextArea = newAreas[i + 1];

                nextArea.ChangePosition(prevArea.endJoint.GetPositoin());
            }
        }
    }
}
