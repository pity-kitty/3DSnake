using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private List<Transform> tails;
    [Range(0, 3)]
    [SerializeField] private float bonesDistance = 0.55f;
    [SerializeField] private GameObject bonePrefab;
    [Range(4, 10)]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationMultiplier = 200.0f;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Spawner spawner;

    [Header("Start properties")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private int startTailLength = 3;

    [Space] 
    [SerializeField] private Score score;
    [SerializeField] private CanvasGroup onScreenButtons;

    private PlayerInput playerInput;
    private Transform snakeTransform;
    private bool isAlive = true;

    public UnityEvent OnEat;

    private void Start()
    {
        snakeTransform = GetComponent<Transform>();        
        spawner.SpawnFood();
        score.OnRestartPressed += RestartGame;
        ControlsSubscriptions();
        StartCoroutine(ControlMovement());
    }

    private void ControlsSubscriptions()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.SnakeInput.Escape.performed += LoadMenu;
#if !UNITY_ANDROID
        onScreenButtons.ShowCanvasGroup(false);
#endif
    }
    
    private void LoadMenu(InputAction.CallbackContext context) => SceneManager.LoadScene(0);

    private IEnumerator ControlMovement()
    {
        while (isAlive)
        {
            MoveSnake();
            RotateSnake();
            yield return null;
        }
    }

    private void MoveSnake()
    {
        var snakePosition = snakeTransform.position;
        var newPosition = snakePosition + snakeTransform.forward * speed * Time.deltaTime;
        var sqrDistance = bonesDistance * bonesDistance;
        var previousPosition = snakePosition;
        foreach(var bone in tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude > sqrDistance)
                (bone.position, previousPosition) = (previousPosition, bone.position);
            else break;
        }
        snakeTransform.position = newPosition;
    }

    private void RotateSnake()
    {
        var angle = playerInput.SnakeInput.HorizontalAxis.ReadValue<float>() * rotationMultiplier * Time.deltaTime;
        snakeTransform.Rotate(0, angle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1<<collision.gameObject.layer) & foodLayer) != 0)
        {
            Destroy(collision.gameObject);
            AddBoneToTail();
            return;
        }

        if (((1<<collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isAlive = false;
            score.ShowRestartScreen();
        }
    }

    private void AddBoneToTail()
    {
        var bone = Instantiate(bonePrefab, tails[tails.Count - 1].position, bonePrefab.gameObject.transform.rotation);
        tails.Add(bone.transform);
        spawner.SpawnFood();
        score.AddPoint();
        OnEat?.Invoke();
    }

    private void RestartGame()
    {
        for (int i = 3; i < tails.Count; i++)
            Destroy(tails[i].gameObject);
        tails.RemoveRange(startTailLength, tails.Count - 3);
        snakeTransform.position = startPosition;
        snakeTransform.rotation = Quaternion.identity;
        isAlive = true;
        StartCoroutine(ControlMovement());
    }

    private void OnDestroy()
    {
        playerInput.SnakeInput.Escape.performed -= LoadMenu;
        playerInput.Disable();
    }
}
