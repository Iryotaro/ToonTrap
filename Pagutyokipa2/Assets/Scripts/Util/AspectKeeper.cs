using UnityEngine;

namespace Ryocatusn.Util
{
    public class AspectKeeper : MonoBehaviour
    {
        [SerializeField]
        private Camera targetCamera;
        [SerializeField]
        private Vector2 aspect;

        private void Update()
        {
            targetCamera.rect = GetViewRectByAspect(aspect);
        }
        private Rect GetViewRectByAspect(Vector2 aspect)
        {
            float screenY = Screen.height / (float)Screen.width;
            float aspectY = aspect.y / aspect.x;
            float height = aspectY / screenY;

            Rect rect = new Rect(0, 0, 1, 1);

            if (height < 1)
            {
                rect.height = height;
            }
            else
            {
                rect.width = 1 / height;
            }

            rect.x = (1 - rect.width) / 2;
            rect.y = (1 - rect.height) / 2;

            return rect;
        }
    }
}
