using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public float SCREEN_LEFT;
    public float SCREEN_RIGHT;
    public float SCREEN_BOTTOM;
    public float SCREEN_TOP;
    public float SCREEN_WIDTH;
    public float SCREEN_HEIGHT;
    public float WIDTH_HEIGHT_RATIO;

    void Awake()
    {
        Vector3 posLeftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, GameConstants.Z_DEPTH_BASE));
        Vector3 posRightUpper = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, GameConstants.Z_DEPTH_BASE));
        SCREEN_LEFT = posLeftBottom.x;
        SCREEN_RIGHT = posRightUpper.x;
        SCREEN_BOTTOM = posLeftBottom.y;
        SCREEN_TOP = posRightUpper.y;
        SCREEN_WIDTH = SCREEN_RIGHT - SCREEN_LEFT;
        SCREEN_HEIGHT = SCREEN_TOP - SCREEN_BOTTOM;
        WIDTH_HEIGHT_RATIO = SCREEN_WIDTH / SCREEN_HEIGHT;
    }

    /// <summary>
    /// Viewport座標を与えて、World座標系のVector3座標を返します。
    /// Z座標にはデフォルト値を用います。
    /// </summary>
    /// <returns>The position.</returns>
    /// <param name="viewportX">Viewport x.</param>
    /// <param name="viewportY">Viewport y.</param>
    public Vector3 WPos(float viewportX, float viewportY)
    {
        return WPos(viewportX, viewportY, GameConstants.Z_DEPTH_BASE);
    }

    /// <summary>
    /// Viewport座標を与えて、World座標系のVector3座標を返します。
    /// Z座標には指定した値を用います。
    /// </summary>
    /// <returns>The position.</returns>
    /// <param name="viewportX">Viewport x.</param>
    /// <param name="viewportY">Viewport y.</param>
    /// <param name="zPos"Z position.</param>
    public Vector3 WPos(float viewportX, float viewportY, float zPos)
    {
        return new Vector3(
            SCREEN_LEFT + SCREEN_WIDTH * viewportX,
            SCREEN_BOTTOM + SCREEN_HEIGHT * viewportY,
            zPos
        );
    }

    /// <summary>
    /// Viewportスケールを与えて、World座標系のVector3スケールを返します。
    /// Zスケールにはデフォルト値を用います。
    /// </summary>
    /// <returns>The scale.</returns>
    /// <param name="viewportX">Viewport x.</param>
    /// <param name="viewportY">Viewport y.</param>
    public Vector3 WScale(float viewportX, float viewportY)
    {
        return WScale(viewportX, viewportY, GameConstants.Z_DEPTH_BASE);
    }

    /// <summary>
    /// Viewportスケールを与えて、World座標系のVector3スケールを返します。
    /// Zスケールには指定した値を用います。
    /// </summary>
    /// <returns>The scale.</returns>
    /// <param name="viewportX">Viewport x.</param>
    /// <param name="viewportY">Viewport y.</param>
    /// <param name="zScale">Z scale.</param>
    public Vector3 WScale(float viewportX, float viewportY, float zScale)
    {
        return new Vector3(
            SCREEN_WIDTH * viewportX,
            SCREEN_HEIGHT * viewportY,
            zScale
        );
    }

}
