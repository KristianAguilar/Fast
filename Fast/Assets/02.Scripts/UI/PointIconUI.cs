using Services;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PointIconUI : MonoBehaviour
{
    private Image _pointIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (_pointIcon == null)
            _pointIcon = GetComponent<Image>();

        _pointIcon.sprite = UIConfigService.Instance.pointsIcon;
    }

}
