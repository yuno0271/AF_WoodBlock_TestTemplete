using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false)
            Debug.unityLogger.logEnabled = false;
    }

    public void LoadScene(string name)
    {
        AudioManager.Instance.PlayClickClip();
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
