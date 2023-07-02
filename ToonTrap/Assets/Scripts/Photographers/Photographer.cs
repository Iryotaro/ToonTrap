using UnityEngine;
using Zenject;
using UniRx;

namespace Ryocatusn.Photographers
{
    public class Photographer : MonoBehaviour
    {
        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;
        [SerializeField]
        private Camera photographerCamera;

        private IPhotographerSubject target;

        private void Start()
        {
            photographerSubjectManager.SaveSubject
                .Where(x => IsAllowedToChangeTarget(x))
                .Subscribe(x => ChangeTarget(x))
                .AddTo(this);
        }

        private bool IsAllowedToChangeTarget(IPhotographerSubject photographerSubject)
        {
            if (target == null) return true;
            if (photographerSubject.priority > target.priority) return true;
            else return false;
        }
        private void ChangeTarget(IPhotographerSubject photographerSubject)
        {
            //ノイズ発生

            target = photographerSubject;
            photographerCamera.orthographicSize = target.photographerCameraSize;
        }

        private void Update()
        {
            if (target == null) return;
            Vector3 targetPosition = target.GetPosition();
            photographerCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
    }
}
