using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private ButtonAnimation playButtonAnimation;
    [SerializeField] private Button exitButton;
    [SerializeField] private ButtonAnimation exitButtonAnimation;

    private IEnumerator Start()
    {
        playButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        yield return new WaitForSeconds(1f);
        yield return playButtonAnimation.Animate();
        yield return exitButtonAnimation.Animate();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(StartGame);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ExitGame() 
    {
        Application.Quit();
    }
}
