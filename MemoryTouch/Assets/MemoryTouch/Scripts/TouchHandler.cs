using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 画面タッチのリスナの登録を受け付け、タッチイベントが起きた時にリスナを呼び出します。
/// </summary>
public class TouchHandler : MonoBehaviour
{

    private StageManager stageManager;

    private List<PanelTouchListener> panelTouchDownListeners = new List<PanelTouchListener>();
    private List<PanelTouchListener> panelTouchingListeners = new List<PanelTouchListener>();
    private List<PanelTouchListener> panelTouchUpListeners = new List<PanelTouchListener>();
    private List<TouchListener> touchDownListeners = new List<TouchListener>();
    private List<TouchListener> touchingListeners = new List<TouchListener>();
    private List<TouchListener> touchUpListeners = new List<TouchListener>();

    private bool isTouching = false;
    private Vector3 touchOrigin;
    private Vector3 touchOriginViewport;

    void Start()
    {
        stageManager = GetComponent<StageManager>();
        Logger.Info("StageManager component is found.", this);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnTouchUp();
        }

        if (isTouching)
        {
            OnTouching();
        }
    }

    private void OnTouchDown()
    {
        Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // touch down
        foreach (TouchListener listener in touchDownListeners)
        {
            listener.OnTouchDown(new TouchInfo(Input.mousePosition, touchPos, Input.mousePosition, touchPos, Time.deltaTime));
        }
        // panel touch down
        Panel panel = JudgeTouchedPanel(touchPos);
        if (panel != null)
        {
            foreach (PanelTouchListener listener in panelTouchDownListeners)
            {
                listener.OnPanelTouchDown(new PanelTouchInfo(Input.mousePosition, touchPos, Input.mousePosition, touchPos, Time.deltaTime, panel));
            }
        }
        isTouching = true;
        touchOrigin = Input.mousePosition;
        touchOriginViewport = touchPos;
    }

    private void OnTouching()
    {
        Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // touching
        foreach (TouchListener listener in touchingListeners)
        {
            listener.OnTouching(new TouchInfo(Input.mousePosition, touchPos, touchOrigin, touchOriginViewport, Time.deltaTime));
        }
        // panel touching
        Panel panel = JudgeTouchedPanel(touchPos);
        if (panel != null)
        {
            foreach (PanelTouchListener listener in panelTouchingListeners)
            {
                listener.OnPanelTouching(new PanelTouchInfo(Input.mousePosition, touchPos, touchOrigin, touchOriginViewport, Time.deltaTime, panel));
            }
        }
    }

    private void OnTouchUp()
    {
        Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // touch up
        foreach (TouchListener listener in touchUpListeners)
        {
            listener.OnTouchUp(new TouchInfo(Input.mousePosition, touchPos, touchOrigin, touchOriginViewport, Time.deltaTime));
        }
        // panel touch up
        Panel panel = JudgeTouchedPanel(touchPos);
        if (panel != null)
        {
            foreach (PanelTouchListener listener in panelTouchUpListeners)
            {
                listener.OnPanelTouchUp(new PanelTouchInfo(Input.mousePosition, touchPos, touchOrigin, touchOriginViewport, Time.deltaTime, panel));
            }
        }
        isTouching = false;
    }

    private Panel JudgeTouchedPanel(Vector3 viewportPoint)
    {
        foreach (Panel panel in stageManager.panels)
        {
            if (panel.Contains(viewportPoint))
            {
                return panel;
            }
        }
        return null;
    }

    public void AddPanelTouchDownListener(PanelTouchListener listener)
    {
        panelTouchDownListeners.Add(listener);
    }

    public void AddPanelTouchingListener(PanelTouchListener listener)
    {
        panelTouchingListeners.Add(listener);
    }

    public void AddPanelTouchUpListener(PanelTouchListener listener)
    {
        panelTouchUpListeners.Add(listener);
    }

    public void AddTouchDownListener(TouchListener listener)
    {
        touchDownListeners.Add(listener);
    }

    public void AddTouchingListener(TouchListener listener)
    {
        touchingListeners.Add(listener);
    }

    public void AddTouchUpListener(TouchListener listener)
    {
        touchUpListeners.Add(listener);
    }

}

/// <summary>
/// Panel.
/// </summary>
[System.Serializable]
public class Panel
{
    public int indexX;
    public int indexY;
    public float leftX;
    public float bottomY;
    public float width;
    public float height;

    public GameObject panelOjcect;

    public Panel(GameObject panelOjcect, int indexX, int indexY, float leftX, float bottomY, float width, float height)
    {
        this.panelOjcect = panelOjcect;
        this.indexX = indexX;
        this.indexY = indexY;
        this.leftX = leftX;
        this.bottomY = bottomY;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// 指定したviewport座標がパネル範囲内に含まれる場合、true
    /// </summary>
    /// <param name="viewportPoint">Viewport point.</param>
    public bool Contains(Vector3 viewportPoint)
    {
        if (!RangeUtil.IsBetween(viewportPoint.x, leftX, leftX + width))
            return false;
        if (!RangeUtil.IsBetween(viewportPoint.y, bottomY, bottomY + height))
            return false;
        return true;
    }

    /// <summary>
    /// 他のパネルとの「距離」を返す
    /// </summary>
    /// <returns>The distance from.</returns>
    /// <param name="other">Other.</param>
    public int GetDistanceFrom(Panel other)
    {
        return System.Math.Abs(this.indexX - other.indexX) + System.Math.Abs(this.indexY - other.indexY);
    }

    /// <summary>
    /// 広義で隣り合ったパネルかどうかを判定
    /// </summary>
    /// <returns><c>true</c> if this instance is neighbor the specified other; otherwise, <c>false</c>.</returns>
    /// <param name="other">Other.</param>
    public bool IsNeighbor(Panel other)
    {
        return System.Math.Abs(this.indexX - other.indexX) <= 1 && System.Math.Abs(this.indexY - other.indexY) <= 1;
    }

    /// <summary>
    /// パネル座標を示す文字列を返す
    /// </summary>
    /// <returns>The coordinates string.</returns>
    public string ToCoordinatesString()
    {
        return string.Format("[{0},{1}]", this.indexX.ToString(), this.indexY.ToString());
    }

    /// <summary>
    /// ピカッと点灯する
    /// </summary>
    /// <param name="flushInfo">Flush info.</param>
    public void Flush(FlushInfo flushInfo)
    {
        this.panelOjcect.SendMessage("Flush", flushInfo);
    }

    /// <summary>
    /// チカチカ点滅する
    /// </summary>
    /// <param name="flickInfo">Flick info.</param>
    public void Flick(FlickInfo flickInfo)
    {
        this.panelOjcect.SendMessage("Flick", flickInfo);
    }

    /// <summary>
    /// パネル色を変更する
    /// </summary>
    /// <param name="color">Color.</param>
    public void ChangeOriginalColor(Color color)
    {
        this.panelOjcect.SendMessage("ChangeOriginalColor", color);
    }
}

/// <summary>
/// パネルのタッチ情報
/// </summary>
[System.Serializable]
public class PanelTouchInfo : TouchInfo
{
    public Panel touchPanel;

    public PanelTouchInfo(Vector3 touchScreenPoint, Vector3 touchViewportPoint, Vector3 originScreenPoint, Vector3 originViewportPoint, float deltaTime, Panel touchPanel)
        : base(touchScreenPoint, touchViewportPoint, originScreenPoint, originViewportPoint, deltaTime)
    {
        this.touchPanel = touchPanel;
    }
}

/// <summary>
/// タッチ情報
/// </summary>
[System.Serializable]
public class TouchInfo
{
    public Vector3 touchScreenPoint;
    public Vector3 touchViewportPoint;
    public Vector3 originScreenPoint;
    public Vector3 originViewportPoint;
    public float deltaTime;

    public TouchInfo(Vector3 touchScreenPoint, Vector3 touchViewportPoint, Vector3 originScreenPoint, Vector3 originViewportPoint, float deltaTime)
    {
        this.touchScreenPoint = touchScreenPoint;
        this.touchViewportPoint = touchViewportPoint;
        this.originScreenPoint = originScreenPoint;
        this.originViewportPoint = originViewportPoint;
        this.deltaTime = deltaTime;
    }
}

/// <summary>
/// Panel touch listener.
/// </summary>
public interface PanelTouchListener
{
    void OnPanelTouchDown(PanelTouchInfo touchInfo);

    void OnPanelTouching(PanelTouchInfo touchInfo);

    void OnPanelTouchUp(PanelTouchInfo touchInfo);
}

/// <summary>
/// Touch listener.
/// </summary>
public interface TouchListener
{
    void OnTouchDown(TouchInfo touchInfo);

    void OnTouching(TouchInfo touchInfo);

    void OnTouchUp(TouchInfo touchInfo);
}


