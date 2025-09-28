using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HosSoonpung : MonoBehaviour
{
    public static HosSoonpung instance;

    public List<Dotty> Dotty = new List<Dotty>();
    public Transform SonnPungPoint;


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



    public void HosDottySoonPung(int max)
    {

        if (GameManager.instance.CurrentHosDottyCount >= GameManager.instance.MaxCurrentDootyCoun)
            return;

        for (int i = 0; i < max; i++) {


            int DottyNum = 0;
            if (GameManager.instance.Gpoint >= 1)
            {



                int r = Random.Range(0, GameManager.instance.Gpoint + 1);

                DottyNum = r;





            }


            Dotty dt = Dotty[DottyNum];
            int rand = Random.Range(1, 3);


            GameManager.instance.AddMoeny((int)(GameManager.instance.SoonPungMoney / 15f));

            GameManager.instance.currentDotty.Add(dt);
            GameManager.instance.CurrentHosDottyCount++;
            GameManager.instance.currentGetDotty++;



            GameManager.instance.LvCheck();

            if(GameManager.instance.CurrentHosDottyCount >= GameManager.instance.MaxCurrentDootyCoun)
                return;

            if (GameManager.instance.DoubleRand <= 0)
                continue;

            int DottyNum2 = 0;
            if (GameManager.instance.Gpoint >= 1)
            {
                int r = Random.Range(0, GameManager.instance.Gpoint + 1);

                DottyNum2 = r;
            }

            int dr = Random.Range(1, 101);
            if(GameManager.instance.hospitalLv < 5)
            {
                if (dr < GameManager.instance.DoubleRand)
                {

                    Dotty dt2 = Dotty[DottyNum];
                    GameManager.instance.currentDotty.Add(dt2);
                    GameManager.instance.CurrentHosDottyCount++;


                    GameManager.instance.AddMoeny(GameManager.instance.SoonPungMoney);
                    GameManager.instance.currentGetDotty++;

                }
            } else
            {
                if (dr < GameManager.instance.DoubleRand + 10)
                {

                    Dotty dt2 = Dotty[DottyNum];
                    GameManager.instance.currentDotty.Add(dt2);
                    GameManager.instance.CurrentHosDottyCount++;


                    GameManager.instance.AddMoeny((int)(GameManager.instance.SoonPungMoney / 25f));
                    GameManager.instance.currentGetDotty++;

                }
            }




            GameManager.instance.LvCheck();


            if (GameManager.instance.CurrentHosDottyCount >= GameManager.instance.MaxCurrentDootyCoun)
                return;
        }

      

    }
}
