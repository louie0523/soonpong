using UnityEngine;
using UnityEngine.EventSystems;

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
        SfxManager.instance.PlaySfx("호잇짜" + rand);

        GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);

        GameManager.instance.currentDotty.Add(dt);

        if (GameManager.instance.DoubleRand <= 0)
            return;

        int dr = Random.Range(1, 101);
        if(dr < GameManager.instance.DoubleRand)
        {
            animator.SetTrigger("soon");
            Dotty dt2 = Instantiate(Dotty, SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
            SfxManager.instance.PlaySfx("짜잇호1");
            GameManager.instance.currentDotty.Add(dt2);


            GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);
        }
    }
}
