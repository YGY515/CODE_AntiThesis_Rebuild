using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    public static BossPhaseManager Instance;
    public int currentPhase { get; private set; } = 1;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AdvancePhase()
    {
        currentPhase++;
        //Debug.Log($"현재 페이즈: {currentPhase}단계");
    }
}
