using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private const int TARGET_FRAME_RATE = 60;
    private readonly Vector2 CAMERA_RENDER_SIZE = new(1920, 1080);

    protected override void OnCreated()
    {
        base.OnCreated();

        Application.targetFrameRate = TARGET_FRAME_RATE;

        OnReset();
    }

    protected override void OnReset()
    {
        SetResolution();

        foreach (var canvas in FindObjectsOfType<CanvasScaler>(true))
            canvas.referenceResolution = CAMERA_RENDER_SIZE;
    }

    private void SetResolution()
    {
        int setWidth = Mathf.CeilToInt(CAMERA_RENDER_SIZE.x);
        int setHeight = Mathf.CeilToInt(CAMERA_RENDER_SIZE.y);

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)((float)deviceHeight / deviceWidth * setWidth), true);

        float screenMultiplier = (float)setWidth / setHeight;
        float deviceMultiplier = (float)deviceWidth / deviceHeight;

        if (screenMultiplier < deviceMultiplier)
        {
            float newWidth = screenMultiplier / deviceMultiplier;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = deviceMultiplier / screenMultiplier;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}