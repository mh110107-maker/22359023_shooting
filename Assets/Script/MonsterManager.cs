using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject prefabsMonster;

    float nowTime = 0;
    float minTime = 1f;
    float maxTime = 5f;


    public float createTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        nowTime = nowTime + Time.deltaTime;

        if(nowTime > createTime)
        {
            GameObject monster = Instantiate(prefabsMonster);
            monster.transform.position = transform.position;

            createTime = Random.Range(minTime, maxTime);

            nowTime = 0;
        }
        
    }
}
