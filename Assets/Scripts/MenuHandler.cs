using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void OnPlayButtonHandler()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitButtonHandler() 
    {
        Application.Quit();
    }
}
