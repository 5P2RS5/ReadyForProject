using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitude = 1f; // 진폭
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying objects!");
            Destroy(this);
        }
    }

    void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float _x) // 파도의 진폭
    {
        return amplitude * Mathf.Sin(_x / length + offset);
    }
}
