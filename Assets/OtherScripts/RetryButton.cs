using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Click() {
        SceneManager.LoadScene(0);
    }
}
