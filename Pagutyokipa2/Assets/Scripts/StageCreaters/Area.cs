using Ryocatusn.TileTransforms;
using UnityEngine;

namespace Ryocatusn.StageCreaters
{
    public class Area : MonoBehaviour
    {
        public AreaJoint startJoint;
        public AreaJoint endJoint;

        public void ChangePosition(TilePosition newStartPosition)
        {
            Vector2 nowPosition = startJoint.GetPositoin().GetWorldPosition();
            Vector2 newPosition = newStartPosition.GetWorldPosition();
            Vector2 differencePosition = newPosition - nowPosition;

            transform.position = (Vector2)transform.position + differencePosition;
        }
    }
}
