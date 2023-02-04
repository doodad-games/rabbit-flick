using UnityEngine;

public class DirectMovement : Movement
{
    public float DistancePerSecond;

    public void Update()
    {
        if (!HasTarget)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            Target,
            DistancePerSecond * Time.deltaTime
        );

        transform.LookAt(Target);
    }
}