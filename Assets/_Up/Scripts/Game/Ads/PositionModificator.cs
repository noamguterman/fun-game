using UnityEngine;

public class PositionModificator : MonoBehaviour
{
    [SerializeField] private bool isAdActive;
    [SerializeField] private float bannerPixelHeight;

    private RectTransform _rectTransform;
    private RectTransform _canvasTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasTransform = _rectTransform.parent.parent.GetComponent<Canvas>().GetComponent<RectTransform>();
    }

    private void Start ()
    {
        if (isAdActive)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        var screen_units_height = GetScreen.height;
        var screen_pixel_height = Screen.currentResolution.height;
        var canvas_pixel_height = _canvasTransform.rect.height;

        var unit_per_pixel = screen_units_height / screen_pixel_height;
        var unit_per_pixel_canvas = screen_units_height / canvas_pixel_height;

        var pixels_banner_height = bannerPixelHeight;//ManagerGoogle.GetNativeSize().h;

        var offset = pixels_banner_height * unit_per_pixel;
        var bottom_position = Camera.main.transform.position.y - Camera.main.orthographicSize;
        var transform_half_height = unit_per_pixel_canvas * _rectTransform.rect.height / 2;

        var target_position = _rectTransform.position;
        var offset_position_y = bottom_position + offset + transform_half_height;
        
        target_position.y = offset_position_y;
        _rectTransform.position = target_position;
    }
}