using UnityEngine;

public class HorizontalLoopManual : MonoBehaviour
{
    public float speed = 100f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 자신(스크립트가 붙은 오브젝트) 이동
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

      
        // x좌표가 6 이하이면 위치 재설정
        if (rectTransform.anchoredPosition.x <= -82f)
        {
            rectTransform.anchoredPosition = new Vector2(1630.8f - 132f, rectTransform.anchoredPosition.y);
        } 

        
    }
}
