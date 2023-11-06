using Ryocatusn.TileTransforms;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class TunnelRepeatTrigger : MonoBehaviour
    {
        [SerializeField]
        private TunnelAndDirection[] tunnels;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnTriggerExitPlayerEvent
                .Subscribe(x => RepeatAndStopLocomotives(x))
                .AddTo(this);
        }
        private void RepeatAndStopLocomotives(TileDirection.Direction direction)
        {
            TunnelAndDirection[] targets = tunnels.Where(x => x.direction.Equals(direction)).ToArray();

            foreach (TunnelAndDirection target in targets)
            {
                if (target.startOrFinish == TunnelAndDirection.StartOrStop.Start) target.tunnel.Repeat();
                else target.tunnel.StopRepeat();
            }
        }


        [Serializable]
        private class TunnelAndDirection
        {
            public Tunnel tunnel;
            public StartOrStop startOrFinish;
            public TileDirection.Direction direction;

            public enum StartOrStop
            {
                Start,
                Stop
            }

        }
    }
}
