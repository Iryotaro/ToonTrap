using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Ryoseqs
{
    public class SequenceWaitByActive : SequenceWaitCommand
    {
        public SequenceWaitByActive(GameObject target)
        {
            handler = new TaskHandler(Finish =>
            {
                target.OnDestroyAsObservable().FirstOrDefault().Subscribe(_ => Finish());
            });
        }
        public SequenceWaitByActive(GameObject[] targets)
        {
            handler = new TaskHandler(finish =>
            {
                int destroyCcount = 0;

                foreach (GameObject target in targets)
                {
                    target.OnDestroyAsObservable()
                    .FirstOrDefault()
                    .Subscribe(_ =>
                    {
                        destroyCcount++;
                        if (destroyCcount == targets.Length) finish();
                    });
                }
            });
        }
    }
}
