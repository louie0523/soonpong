using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("회전 설정")]
    public GameObject target;     // 회전시킬 오브젝트
    public int vec = 1;           // 0=x, 1=y, 2=z
    public float time = 2f;       // 360도 한 바퀴 도는 시간
    public bool isRotating = true; // 회전 On/Off

    private Vector3 axis;

    void Start()
    {
        if (target == null)
            target = this.gameObject; // 기본은 자기 자신

        // 축 설정
        switch (vec)
        {
            case 0: axis = Vector3.right; break;
            case 1: axis = Vector3.up; break;
            case 2: axis = Vector3.forward; break;
            default: axis = Vector3.up; break;
        }
    }

    void Update()
    {
        if (isRotating && time > 0f)
        {
            float anglePerFrame = 360f / time * Time.deltaTime;
            target.transform.Rotate(axis, anglePerFrame, Space.Self);
        }
    }
}
