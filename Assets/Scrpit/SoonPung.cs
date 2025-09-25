using UnityEngine;
using UnityEngine.EventSystems; // UI üũ�Ϸ��� �ʿ�

public class SoonPung : MonoBehaviour
{
    public static SoonPung instance;

    public GameObject Dotty;
    public Transform SonnPungPoint;

    public GameObject PlayerObj;
    public Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = PlayerObj.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // UI ���� ���� ���� �� ��
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            DottySoonPung();
        }
    }

    public void DottySoonPung()
    {
        animator.SetTrigger("soon");
        Dotty dt = Instantiate(Dotty, SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
        int rand = Random.Range(1, 3);
        SfxManager.instance.PlaySfx("ȣ��¥" + rand);

        GameManager.instance.currentDotty.Add(dt);
    }
}
