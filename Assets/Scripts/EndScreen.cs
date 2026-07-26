using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private List<String> WordsOfEncouragement = new List<String>();

    [Header("Text Fields that can be changes")] 
    [SerializeField] private TMP_Text EncouragingText;
    [SerializeField] private TMP_Text TimeText;
    
    void Start()
    {
        int a = Random.Range(0, WordsOfEncouragement.Count);
        EncouragingText.text = WordsOfEncouragement[a];
       
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            PauseManager.Unpause();
            Manager.instance.StartCoroutine(Manager.instance.ChangeScene("RealHealthyPatientCare", "EndScreen"));
        } else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PauseManager.Unpause();
            Manager.instance.StartCoroutine(Manager.instance.ChangeScene("TitleScreen", "EndScreen"));
        }
    }
}
