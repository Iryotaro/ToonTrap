using UnityEngine;
using Ryocatusn.Janken;

namespace Ryocatusn.Characters
{
    public class Tunnel : MonoBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private LocomotiveData data;
        [SerializeField]
        private Railway railway;

        [SerializeField]
        private Locomotive locomotive;

        private void Start()
        {
            CreateLocomotive();
        }

        private Locomotive CreateLocomotive()
        {
            Locomotive newLocomotive = Instantiate(locomotive);
            newLocomotive.SetUp(shape, railway, data);
            return newLocomotive;
        }

        public Hand.Shape GetShape()
        {
            return shape;
        }
        public void UpdateView(Hand.Shape shape)
        {

        }
    }
}
