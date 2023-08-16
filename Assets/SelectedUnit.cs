using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedUnit : MonoBehaviour
{

    public Player unit;

    private void Start()
    {
        unit = null;
    }


    public static SelectedUnit instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }
}
