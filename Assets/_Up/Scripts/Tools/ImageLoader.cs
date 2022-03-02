using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Tools
{
    [ExecuteInEditMode]
    public class ImageLoader : MonoBehaviour
    {
        public TextAsset Asset; //filename.bytes !!!

        // private Texture2D texture;
        private Sprite sprite;

        private TextAsset loadedAsset;
        public Vector2 Pivot=new Vector2(0.5f,0.5f);

        private void Awake()
        {
            TryToLoadAsset();
        }

        private void TryToLoadAsset()
        {
            if (loadedAsset != null) { Destroy(); }
            if (Asset == null) { return; }
            sprite = TryToLoadAsset(Asset, Pivot);

            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else if (GetComponent<Image>() != null)
            {
                GetComponent<Image>().sprite = sprite;
            }


            loadedAsset = Asset;
        }

        public static Sprite TryToLoadAsset(TextAsset asset,Vector2 pivot)
        {
            if (asset == null) { return null; }

            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false)
            {
                hideFlags = HideFlags.DontSave
            };
            texture.LoadImage(asset.bytes);

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (GetComponent<SpriteRenderer>() != null && GetComponent<SpriteRenderer>().sprite)
            {
                DestroyImmediate(GetComponent<SpriteRenderer>().sprite);
            }
            TryToLoadAsset();
        }
#endif

        private void Destroy()
        {
#if UNITY_EDITOR
            DestroyImmediate(sprite);
            //DestroyImmediate(texture);
#else
        Destroy(sprite);
        //Destroy(texture);
#endif
        }
    }
}