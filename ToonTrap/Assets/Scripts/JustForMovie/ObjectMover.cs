using Cysharp.Threading.Tasks;
using Ryocatusn.TileTransforms;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField]
        private MoveTask[] tasks;
        [SerializeField]
        private float speed;
        [SerializeField]
        private bool loop;

        public (bool isMoving, TileDirection.Direction direction) moveDirection { get; private set; } = (false, default);

        private void Start()
        {
            Move(tasks);
        }

        private void Move(MoveTask[] tasks, int index = 0)
        {
            MoveTask task = tasks[index];
            moveDirection = (true, task.direction);

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + GetMoveVector(task);
            float startTime = Time.fixedTime;

            IDisposable disposable = null;
            disposable = this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    float time = (Time.fixedTime - startTime) / Vector2.Distance(startPosition, endPosition) * speed;
                    transform.position = Vector2.Lerp(startPosition, endPosition, time);

                    if (time > 1) Next();
                });

            void Next()
            {
                disposable.Dispose();

                if (index + 1 <= tasks.Count() - 1) Move(tasks, index + 1);
                else if (loop) Move(tasks);
                else moveDirection = (false, default);
            }
        }

        private Vector2 GetMoveVector(MoveTask task)
        {
            return task.direction switch
            {
                TileDirection.Direction.Up => new Vector2(0, task.move),
                TileDirection.Direction.Down => new Vector2(0, -task.move),
                TileDirection.Direction.Left => new Vector2(-task.move, 0),
                TileDirection.Direction.Right => new Vector2(task.move, 0),
                _ => new Vector2(0, 0)
            };
        }

        [Serializable]
        public class MoveTask
        {
            [Min(0)]
            public float move;
            public TileDirection.Direction direction;
        }
    }
}
