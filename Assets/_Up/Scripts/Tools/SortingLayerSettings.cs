using UnityEngine;

namespace Assets._Scripts.Tools
{
    [RequireComponent(typeof(Renderer))]
    public class SortingLayerSettings : MonoBehaviour
    {
        public string Layer;
        public int OrderInLayer;

        public void Awake()
        {
            var renderer = GetComponent<Renderer>();
            renderer.sortingLayerName = Layer;
            renderer.sortingOrder = OrderInLayer;
        }
    }
}

