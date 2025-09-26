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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            DottySoonPung();
        } else if(Input.GetKeyDown(KeyCode.Space))
        {
            DottySoonPung();
        }
    }

    public void DottySoonPung()
    {

        int DottyNum = 0;
        if(GameManager.instance.SDottyCount >= GameManager.instance.MaxSDottyCount && GameManager.instance.SpecialDotty)
        {
            int Dcount = 0;
            while(DottyNum == 0)
            {
                
                int r = Random.Range(1, Dotty.Count);
                if(r != 0 && GameManager.instance.DottyUnLock[r])
                {
                    DottyNum = r;
                    GameManager.instance.SDottyCount = 0;
                    break;
                }
                Dcount++;

                if(Dcount >= 100)
                {
                    break;
                }
            }
        }

        animator.SetTrigger("soon");
        Dotty dt = Instantiate(Dotty[DottyNum], SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
        int rand = Random.Range(1, 3);
        SfxManager.instance.PlaySfx("호잇짜" + rand);

        GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);

        GameManager.instance.currentDotty.Add(dt);

        if(DottyNum == 0)
            GameManager.instance.SDottyCount++;

        if (GameManager.instance.DoubleRand <= 0)
            return;

        int DottyNum2 = 0;
        if (GameManager.instance.SDottyCount >= GameManager.instance.MaxSDottyCount && GameManager.instance.SpecialDotty)
        {
            int Dcount = 0;
            while (DottyNum2 == 0)
            {

                int r = Random.Range(1, Dotty.Count);
                if (r != 0 && GameManager.instance.DottyUnLock[r])
                {
                    DottyNum2 = r;
                    GameManager.instance.SDottyCount = 0;
                    break;
                }
                Dcount++;

                if (Dcount >= 100)
                {
                    break;
                }
            }
        }

        int dr = Random.Range(1, 101);
        if(dr < GameManager.instance.DoubleRand)
        {
            animator.SetTrigger("soon");
            Dotty dt2 = Instantiate(Dotty[DottyNum2], SonnPungPoint.position, Quaternion.identity, transform).GetComponent<Dotty>();
            SfxManager.instance.PlaySfx("짜잇호1");
            GameManager.instance.currentDotty.Add(dt2);


            GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);

            if (DottyNum == 0)
                GameManager.instance.SDottyCount++;
        }
    }
}
