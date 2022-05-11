using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public static bool isWater = false; // 물속인지 아닌지 여부, 정적이므로 이걸 이용해서 플레이어의 행동 제약을 준다.
    
    [SerializeField] float waterDrag; // 물에서 중력 저항, 천천히 가라앉기
    float originDrag; // 원래 바깥 저항값

    [SerializeField] Color waterColor;
    [SerializeField] float waterFogDenstity; // 물 탁함 정도

    [SerializeField] Color originColor; // 원래 바깥 색깔
    [SerializeField] float originFogDensity; // 원래 탁함 정도

    // Color originNightColor;
    // float originNightFogDensity;
    
    
    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor; // RenderSettings.fogColor : 안개(fog)의 색상을 나타냅니다.
        originFogDensity = RenderSettings.fogDensity; // RenderSettings.fogDensity : exponential 안개(fog)의 밀도(탁함)를 나타냅니다.

        originDrag = 0; // 플레이어의 리지드바디 값을 기본값으로
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    void GetWater(Collider _player)
    {
        isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag; // 리지드 바디의 저항을 waterDrag로 바꿔 저항을 높인다.

        RenderSettings.fogColor = waterColor;
        RenderSettings.fogDensity = waterFogDenstity;
    }
    
    void GetOutWater(Collider _player)
    {
        if (isWater)
        {
            _player.transform.GetComponent<Rigidbody>().drag = originDrag; // 리지드 바디의 저항을 originDrag로 바꿔 저항을 낮춘다.
            RenderSettings.fogColor = originColor;
            RenderSettings.fogDensity = originFogDensity;
        }
    }
}
