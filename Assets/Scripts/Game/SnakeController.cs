using System.Collections.Generic;
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
    
    private float RotationMultiplier = 6.0f;
    private Transform snakeTransform;

    public UnityEvent OnEat;

    void Start()
    {
        snakeTransform = GetComponent<Transform>();        
    }
    void Update()
    {
        MoveSnake(snakeTransform.position + snakeTransform.forward * speed);

        float angle = Input.GetAxis("Horizontal") * RotationMultiplier;
        snakeTransform.Rotate(0, angle, 0);
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

            var bone = Instantiate(bonePrefab, snakeTransform.position, bonePrefab.gameObject.transform.rotation);
            tails.Add(bone.transform);

            if (OnEat != null)
            {
                OnEat.Invoke();
            }

            return;
        }

        if (((1<<collision.gameObject.layer) & obstacleLayer) != 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
