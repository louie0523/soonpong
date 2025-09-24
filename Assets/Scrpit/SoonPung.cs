using UnityEngine;

public class SoonPung : MonoBehaviour
{

    public GameObject Dotty;
    public Transform SonnPungPoint;

    public GameObject PlayerObj;
    public Animator animator;


    private void Start()
    {
        animator = PlayerObj.GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            DottySoonPung();
        }
    }


    public void DottySoonPung()
    {
        animator.SetTrigger("soon");
        Instantiate(Dotty, SonnPungPoint.position, Quaternion.identity, transform);
        int rand = Random.Range(1, 3);
        SfxManager.instance.PlaySfx("È£ÀÕÂ¥" + rand);
    }
}
