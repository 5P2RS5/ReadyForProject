using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TrainStop : MonoBehaviour
{
    [SerializeField] GameObject[] trains; // 기차 오브젝트들
    [SerializeField] GameObject railWay; // 기차 레일 오브젝트

    public float trainOperatingTime; // 기차 작동 시간
    private float speed; // 기차 속도
    private float length; // 레일의 길이
    private float currentTime; // 현재 경과 시간
    
    private void Start()
    {
        length = railWay.GetComponent<CinemachinePath>().PathLength; // 레일(Dolly track)의 길이 가져오기
        speed = trains[0].GetComponent<CinemachineDollyCart>().m_Speed; // 기차(Cart) 속도 가져오기
        trainOperatingTime = (length - trains[0].GetComponent<CinemachineDollyCart>().m_Position) / speed; // 시간 = 거리 / 시간
        currentTime = 0f;
    }
    
    private void Update()
    {
        if (currentTime >= trainOperatingTime)
        {
            ScriptOff();
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void ScriptOff() // 기차 멈추게 하기
    {
        for (int i = 0; i < trains.Length; i++)
        {
            trains[i].GetComponent<CinemachineDollyCart>().enabled = false;
        }
    }
}
