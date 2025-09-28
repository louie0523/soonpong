using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SoonPung : MonoBehaviour
{
    public static SoonPung instance;

    public List<GameObject> Dotty = new List<GameObject>();
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.instance.gotChaDangay == 0)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            DottySoonPung();
        } else if(Input.GetKeyDown(KeyCode.Space) && GameManager.instance.gotChaDangay == 0 )
        {
            DottySoonPung();
        }

        int count = GameManager.instance.currentDotty.Count;
        if (count >= 100)
        {
            // 100개일 때 0.99, 300개일 때 0.5가 되도록 보간
            float t = Mathf.InverseLerp(100, 300, count);
            float scale = Mathf.Lerp(0.99f, 0.5f, t);

            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            // 100개 미만일 때는 기본 크기
            transform.localScale = Vector3.one;
        }
    }

    public void DottySoonPung()
    {

        int DottyNum = 0;
        if(GameManager.instance.Gpoint >= 1)
        {


                
            int r = Random.Range(0, GameManager.instance.Gpoint + 1);
    
                DottyNum = r;

    

 
            
        }

        animator.SetTrigger("soon");
        Dotty dt = Instantiate(Dotty[DottyNum], SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
        int rand = Random.Range(1, 3);
        if(GameManager.instance.gotChaDangay == 0)
        SfxManager.instance.PlaySfx("호잇짜" + rand);

        GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);

        GameManager.instance.currentDotty.Add(dt);
        GameManager.instance.currentGetDotty++;



        GameManager.instance.LvCheck();

        if (GameManager.instance.DoubleRand <= 0)
            return;

        int DottyNum2 = 0;
        if (GameManager.instance.Gpoint >= 1)
        {
            int r = Random.Range(0, GameManager.instance.Gpoint + 1);

            DottyNum2 =  r;
        }

        int dr = Random.Range(1, 101);
        if(dr < GameManager.instance.DoubleRand)
        {
            animator.SetTrigger("soon");
            Dotty dt2 = Instantiate(Dotty[DottyNum2], SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
            if (GameManager.instance.gotChaDangay == 0)
                SfxManager.instance.PlaySfx("짜잇호1");
            GameManager.instance.currentDotty.Add(dt2);
            GameManager.instance.currentGetDotty++;


            GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);

        }


        GameManager.instance.LvCheck();

    }
}
