using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader {

    public static IEnumerator LoadScene(string sceneNameToLoad)
    {

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name != sceneNameToLoad)
        {
            
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
                yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneNameToLoad));

        }
    }

    public static IEnumerator UnloadScene(string sceneToUnload)
    {
        Scene scene = SceneManager.GetSceneByName(sceneToUnload);
        AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(scene);
        while (!loadOperation.isDone)
            yield return null;

    }

}
