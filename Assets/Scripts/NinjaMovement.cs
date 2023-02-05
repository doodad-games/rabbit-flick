using UnityEngine;

namespace DefaultNamespace
{
    public class NinjaMovement : Movement
    {
        public float DistancePerSecond; 
        Vector3[] targets;
        int targetInList;
        public void Update()
        {
            if (!HasTarget)
                return;
            
            Debug.Log($"Distance to Target {Target}");
            transform.position = Vector3.MoveTowards(
                transform.position,
                Target,
                DistancePerSecond * Time.deltaTime
            );

            transform.LookAt(Target);
        }
    }
}