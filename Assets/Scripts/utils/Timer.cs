using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer {

    public static IEnumerator WaitATime(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

}
