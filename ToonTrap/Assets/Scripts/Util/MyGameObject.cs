using UnityEngine;

namespace Ryocatusn.Util
{
	public static class MyGameObject
	{
        public static void ChangeLayerIncludeChildren(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            int count = gameObject.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                gameObject.transform.GetChild(i).gameObject.ChangeLayerIncludeChildren(layer);
            }
        }
    }
}
