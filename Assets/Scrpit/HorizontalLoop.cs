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
        // �ڽ�(��ũ��Ʈ�� ���� ������Ʈ) �̵�
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

      
        // x��ǥ�� 6 �����̸� ��ġ �缳��
        if (rectTransform.anchoredPosition.x <= -82f)
        {
            rectTransform.anchoredPosition = new Vector2(1630.8f - 132f, rectTransform.anchoredPosition.y);
        } 

        
    }
}
