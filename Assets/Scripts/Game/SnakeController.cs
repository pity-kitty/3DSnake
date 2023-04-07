using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] List<Transform> tails;
    [Range(0, 3)]
    [SerializeField] float bonesDistance = 0.55f;
    [SerializeField] GameObject bonePrefab;
    [Range(0, 4)]
    [SerializeField] float speed = 0.2f;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Spawner spawner;

    [Header("Start properties")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private int startTailLength = 3;

    [Space] 
    [SerializeField] private Score score;
    
    private float RotationMultiplier = 6.0f;
    private Transform snakeTransform;

    public UnityEvent OnEat;

    void Start()
    {
        snakeTransform = GetComponent<Transform>();        
        spawner.SpawnFood();
    }
    
    void FixedUpdate()
    {
        MoveSnake(snakeTransform.position + snakeTransform.forward * speed);

        float angle = Input.GetAxis("Horizontal") * RotationMultiplier;
        snakeTransform.Rotate(0, angle, 0);

        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

    void MoveSnake(Vector3 newPosition)
    {
        var sqrDistance = bonesDistance * bonesDistance;
        var previousPosition = snakeTransform.position;

        foreach(var bone in tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude > sqrDistance)
            {
                (bone.position, previousPosition) = (previousPosition, bone.position);
            }
            else break;
        }

        snakeTransform.position = newPosition;
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
            RestartGame();
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
        score.ResetPoints();
    }
}
