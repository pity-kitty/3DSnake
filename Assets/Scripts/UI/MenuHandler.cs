using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [Header("Menu buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private ButtonAnimation playButtonAnimation;
    [SerializeField] private Button exitButton;
    [SerializeField] private ButtonAnimation exitButtonAnimation;
    
    [Header("Loading")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progress;

    private PlayerInput playerInput;
    private AsyncOperation sceneLoading;

    private IEnumerator Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.SnakeInput.Escape.performed += ExitGame;
        playButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        yield return new WaitForSeconds(1f);
        yield return playButtonAnimation.Animate();
        yield return exitButtonAnimation.Animate();
    }

    private void OnDestroy()
    {
        playerInput.SnakeInput.Escape.performed -= ExitGame;
        playerInput.Disable();
        playButton.onClick.RemoveListener(StartGame);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void StartGame()
    {
        loadingScreen.SetActive(true);
        sceneLoading = SceneManager.LoadSceneAsync(1);
        StartCoroutine(SetProgress());
    }

    private IEnumerator SetProgress()
    {
        while (!sceneLoading.isDone)
        {
            progress.fillAmount = sceneLoading.progress;
            yield return null;
        }
    }

    private void ExitGame() => Application.Quit();

    private void ExitGame(InputAction.CallbackContext callbackContext) => ExitGame();
}
