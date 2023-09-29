using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攝影機適配器
/// </summary>
public class CameraScript : MonoBehaviour
{
    /// <summary>
    /// 使用此圖的大小來適配攝影機的 orthographicSize
    /// </summary>
    [SerializeField] SpriteRenderer _background;
    private float _halfBackgroundBoundSizeY;

    // Start is called before the first frame update
    void Start()
    {
        _halfBackgroundBoundSizeY = _background.bounds.size.y / 2;

        _fitFullHeight();
    }

    /// <summary>
    /// 水平和垂直適配
    /// </summary>
    private void _autoFit()
    {
        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float TargetRatio = _background.bounds.size.x / _background.bounds.size.y;

        if (ScreenRatio >= TargetRatio)
        {
            _fitFullHeight();
        }
        else
        {
            float DifferenceInSize = TargetRatio / ScreenRatio;

            Camera.main.orthographicSize = _halfBackgroundBoundSizeY * DifferenceInSize;
        }
    }

    /// <summary>
    /// 垂直適配，寬度可能會被切到，或多出來
    /// </summary>
    private void _fitFullHeight()
    {
        Camera.main.orthographicSize = _halfBackgroundBoundSizeY;
    }

    /// <summary>
    /// 寬度適配，高度可能會被切到，或多出來
    /// </summary>
    private void _fitFullWidth()
    {
        float OrthoSize = _background.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = OrthoSize;
    }
}