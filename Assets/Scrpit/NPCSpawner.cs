using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public static NPCSpawner Instance;

    public GameObject NpcPrafab;

    public float timer;
    public float Stime = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {


        timer += Time.deltaTime;
        if (Stime <= timer)
        {
            timer = 0;
            Stime = Random.Range(0.75f, 2.3f);
            GameObject npc = Instantiate(NpcPrafab, transform.position, Quaternion.identity, transform);
        }

        

        
    }
}
