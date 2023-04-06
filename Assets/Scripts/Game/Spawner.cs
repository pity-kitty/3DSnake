using UnityEngine;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject foodPrefab;
        
        [Header("Constraints")] 
        [SerializeField] private float xConstraint = 17f;
        [SerializeField] private float zBottomConstraint = 2f;
        [SerializeField] private float zTopConstraint = 37f;
        [SerializeField] private float yPosition = 0.7f;

        public void SpawnFood()
        {
            var spawnPosition = GenerateRandomPosition();
            Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        }
        
        private Vector3 GenerateRandomPosition()
        {
            var x = Random.Range(xConstraint, -xConstraint);
            var z = Random.Range(zBottomConstraint, zTopConstraint);
            return new Vector3(x, yPosition, z);
        }
    }
}