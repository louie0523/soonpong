using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public Animator ani;
    public GameObject Handobj;
    float y = 0;
    public bool People = true;
    public float Amtimer;
    public bool Job = false;

    private void Start()
    {
        if(People)
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (People)
        {
            if (Handobj == null)
            {
                transform.position += Vector3.left * 0.5f * Time.deltaTime;
                if(transform.localPosition.x <= -20)
                {
                    if(Handobj != null)
                    {
                        GameManager.instance.currentGetsPeopls--;
                    }
                    Destroy(gameObject);
                }
            }

 

            else

                transform.position = Handobj.transform.position + new Vector3(y, 0, 0);

        } else
        {
            if (!GameManager.instance.HospAct)
                return;

            transform.position += Vector3.left * 3f * Time.deltaTime;
            if(transform.localPosition.x < -18f)
            {
                Amtimer += Time.deltaTime;
                if(Amtimer >= GameManager.instance.GetHosPeopleTime)
                {
                    Amtimer = 0;
                    transform.localPosition = new Vector3(25f, transform.localPosition.y, transform.localPosition.z);
                }
            }
        } 




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Hand") && Handobj == null && People && GameManager.instance.currentGetsPeopls < GameManager.instance.GetHosPeopleCoun)
        {
            if (GameManager.instance.currentMap == 1)
                SfxManager.instance.PlaySfx("ºñ¸í");
            GameManager.instance.currentGetsPeopls++;
            ani.SetTrigger("Oh");
            Handobj = collision.transform.GetChild(0).gameObject;
            y = 0;
            transform.parent = Handobj.transform;
        }

        if (collision.CompareTag("Hd") && Handobj != null && People && !Job)
        {
            Job = true;
            GameManager.instance.HosPeople++;
            GameManager.instance.currentGetsPeopls--;
            Destroy(gameObject);
        }
    }



    IEnumerator DestoryPeople(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
