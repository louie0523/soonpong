using UnityEngine;
using DG.Tweening;

public class HeartbeatEffect : MonoBehaviour
{
    [Header("Heartbeat Settings")]
    public Vector3 minScale = new Vector3(0.9f, 0.9f, 0.9f);  // 최소 스케일
    public Vector3 maxScale = new Vector3(1.1f, 1.1f, 1.1f);  // 최대 스케일
    public float duration = 0.1f;  // 한 번 커졌다 작아지는 시간

    private Tween heartbeatTween;
    private bool _isActive = false;

    void Awake()
    {
        if (transform.localScale == Vector3.zero)
            transform.localScale = minScale;
    }

    public void EnableHeartbeat()
    {
        if (_isActive) return;
        _isActive = true;

        transform.localScale = minScale;

        heartbeatTween = transform.DOScale(maxScale, duration)
            .SetLoops(-1, LoopType.Yoyo)      // 최소 ↔ 최대 반복
            .SetEase(Ease.InOutBack);         // 팍팍 튀면서 자연스럽게
    }

    public void DisableHeartbeat()
    {
        if (!_isActive) return;
        _isActive = false;

        if (heartbeatTween != null && heartbeatTween.IsActive())
        {
            heartbeatTween.Kill();
            heartbeatTween = null;
        }

    }
}
