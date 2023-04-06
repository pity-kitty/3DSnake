using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeController : MonoBehaviour
{
    [SerializeField] List<Transform> Tails;
    [Range(0, 3)]
    [SerializeField] float BonesDistance;
    [SerializeField] GameObject BonePrefab;
    [Range(0, 4)]
    [SerializeField] float Speed;
    private float RotationMultiplier = 6.0f;
    private Transform _transform;

    public UnityEvent OnEat;

    void Start()
    {
        _transform = GetComponent<Transform>();        
    }
    void Update()
    {
        MoveSnake(_transform.position + _transform.forward * Speed);

        float angle = Input.GetAxis("Horizontal") * RotationMultiplier;
        _transform.Rotate(0, angle, 0);
    }

    void MoveSnake(Vector3 newPosition)
    {
        float sqrDistance = BonesDistance * BonesDistance;
        Vector3 previousPosition = _transform.position;

        foreach(Transform bone in Tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude > sqrDistance)
            {
                Vector3 temp = bone.position;
                bone.position = previousPosition;
                previousPosition = temp;
            }
            else
            {
                break;
            }
        }

        _transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);

            var bone = Instantiate(BonePrefab, _transform.position, BonePrefab.gameObject.transform.rotation);
            Tails.Add(bone.transform);

            if (OnEat != null)
            {
                OnEat.Invoke();
            }
        }
    }
}
