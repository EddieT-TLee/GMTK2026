using System.Collections;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private bool isChangingScene = false;
    
    public void ChangeToTutorial()
    {
        if (isChangingScene) return;
        StartCoroutine(WaitThenChange());
    }

    private IEnumerator WaitThenChange()
    {
        isChangingScene = true;
        yield return Manager.instance.StartCoroutine(Manager.instance.ChangeScene("Tutorial", "TitleScreen"));
        isChangingScene = false;
    }
}