using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private bool isChangingScene = false;
    public void ChangeToMain()
    {
        if (isChangingScene) return;
        StartCoroutine(WaitThenChange());
    }

    private IEnumerator WaitThenChange()
    {
        isChangingScene = true;
        yield return Manager.instance.StartCoroutine(Manager.instance.ChangeScene("RealHealthyPatientCare", "TitleScreen"));
        isChangingScene = false;
    }
}