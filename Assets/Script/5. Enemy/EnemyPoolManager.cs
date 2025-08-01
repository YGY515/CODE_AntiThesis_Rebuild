using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;
    public bool EnemyCanBeDamaged = false;

    public GameObject[] Prefabs;
    public List<GameObject>[] Pool;


    void OnEnable()
    {

        Instance = this;
        Pool = new List<GameObject>[Prefabs.Length];
        
        int count = BossPhaseManager.Instance.currentPhase switch
        {
            1 => 3, 
            2 => 5, 
            3 => 7 
        };

        for (int index = 0; index < Pool.Length; index++)
        {
            Pool[index] = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(Prefabs[index], transform);
                obj.SetActive(false);
                Pool[index].Add(obj);
            }
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in Pool[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                //Debug.Log($"[Pool] 활성화 시도: {item.name}, 상태: {item.activeSelf}");
                return select; 
            }
        }
        
        select = Instantiate(Prefabs[index], transform);
        Pool[index].Add(select);
        select.SetActive(true);     // 새 인스턴스 생성시 보이게
        //Debug.Log($"[Pool] 새 인스턴스 생성: {select.name}, 상태: {select.activeSelf}");
        return select;
    }
}
