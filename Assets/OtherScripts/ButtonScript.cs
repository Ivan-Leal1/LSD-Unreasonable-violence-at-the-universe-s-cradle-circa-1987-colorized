using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] TitleChange _titleScreen;

    public void Play() {
        _titleScreen.Stop();
        SceneManager.LoadScene(1);
    }

    public void Settings() {
        Debug.Log("Settings");
    }

    public void Exit() {
        Application.Quit();
    }
}
