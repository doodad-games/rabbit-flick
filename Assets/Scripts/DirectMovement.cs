using UnityEngine;

public class DirectMovement : Movement
{
    public float DistancePerSecond;

    public void Update()
    {
        if (Target == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            Target.position,
            DistancePerSecond * Time.deltaTime
        );
    }
}