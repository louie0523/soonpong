using UnityEngine;
using DG.Tweening;

public class CameraHeartbeat : MonoBehaviour
{
    [Header("Heartbeat Settings")]
    public float minSize = 5f;     // 기본 카메라 크기
    public float maxSize = 7.5f;   // 두근거릴 때 최대 크기
    public float duration = 0.2f;  // 커졌다 줄어드는 속도 (더 빠르게)
    public float pauseDuration = 0.3f; // 심박 사이의 휴식 시간

    [Header("Debug")]
    public bool enableHeartbeat = true;  // 디버그용 토글

    private Camera cam;
    private bool isOrthographic;
    private Tween heartbeatTween;
    private bool isHeartbeatActive = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found on " + gameObject.name);
            return;
        }
        isOrthographic = cam.orthographic;
    }

    private void Update()
    {
        // GameManager 상태에 따라 하트비트 제어
        if (GameManager.instance.gotChaDangay != 2)
        {
            // 하트비트 중지 및 기본 크기로 설정
            if (isHeartbeatActive)
            {
                StopHeartbeat();
            }
            cam.orthographicSize = minSize;
        }
        else
        {
            // 하트비트 시작
            if (enableHeartbeat && !isHeartbeatActive)
            {
                StartHeartbeat();
            }
        }
    }

    void SetCamera()
    {
        // 카메라 설정 (필요시 추가 설정)
        if (cam == null) return;

        // 기본 카메라 설정들
        cam.orthographicSize = minSize;

        // 추가 카메라 설정이 필요하다면 여기에 작성
        // 예: cam.backgroundColor = Color.black;
        // 예: cam.cullingMask = LayerMask.GetMask("Default", "UI");
    }

    void StartHeartbeat()
    {
        if (!enableHeartbeat || isHeartbeatActive) return;

        isHeartbeatActive = true;

        // 기존 트윈이 있다면 정리
        if (heartbeatTween != null && heartbeatTween.IsActive())
        {
            heartbeatTween.Kill();
        }

        if (isOrthographic)
        {
            // Orthographic 카메라 하트비트 - 팍팍 거리는 효과
            cam.orthographicSize = minSize;

            // 시퀀스로 팍팍 거리는 효과 구현
            Sequence heartSequence = DOTween.Sequence();
            heartSequence.Append(DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, maxSize, duration)
                .SetEase(Ease.OutBack, 2f)) // 팍 튀어나오는 효과
                .Append(DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, minSize, duration * 0.5f)
                .SetEase(Ease.InBack)) // 빠르게 들어가는 효과
                .AppendInterval(pauseDuration) // 잠깐 멈춤
                .SetLoops(-1);

            heartbeatTween = heartSequence;
        }
        else
        {
            // Perspective 카메라 하트비트 - 팍팍 거리는 효과
            cam.fieldOfView = minSize;

            Sequence heartSequence = DOTween.Sequence();
            heartSequence.Append(DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, maxSize, duration)
                .SetEase(Ease.OutBack, 2f)) // 팍 튀어나오는 효과
                .Append(DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, minSize, duration * 0.5f)
                .SetEase(Ease.InBack)) // 빠르게 들어가는 효과
                .AppendInterval(pauseDuration) // 잠깐 멈춤
                .SetLoops(-1);

            heartbeatTween = heartSequence;
        }
    }

    void StopHeartbeat()
    {
        isHeartbeatActive = false;

        // 트윈 정리
        if (heartbeatTween != null && heartbeatTween.IsActive())
        {
            heartbeatTween.Kill();
        }

        // 원래 크기로 복원
        if (isOrthographic)
        {
            cam.orthographicSize = minSize;
        }
        else
        {
            cam.fieldOfView = minSize;
        }
    }

    // 수동으로 하트비트 시작/중지하는 메서드들
    public void ManualStartHeartbeat()
    {
        enableHeartbeat = true;
        StartHeartbeat();
    }

    public void ManualStopHeartbeat()
    {
        enableHeartbeat = false;
        StopHeartbeat();
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 트윈 정리
        if (heartbeatTween != null && heartbeatTween.IsActive())
        {
            heartbeatTween.Kill();
        }
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 트윈 정리
        StopHeartbeat();
    }
}