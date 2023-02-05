using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class NinjaMovement : Movement
    {
        const float INTERMEDIATE_TARGET_DISTANCE = 2f;
        const float INTERMEDIATE_TARGET_PERPENDICULAR_VARIANCE = 4f;


        public float DistancePerSecond;

        readonly List<Vector3> _targets = new();
        Vector3 CurNinjaTarget => _targets.Count == 0 ? Target : _targets[0];

        public void Awake() =>
            OnTargetChanged += CalculateTargets;

        public void Update()
        {
            if (!HasTarget)
                return;

            var curTarget = CurNinjaTarget;

            var newPos = Vector3.MoveTowards(
                transform.position,
                curTarget,
                DistancePerSecond * Time.deltaTime
            );
            transform.position = newPos;

            transform.LookAt(curTarget);

            if (_targets.Count == 0)
                return;

            var targetPositionDelta = curTarget - newPos;
            if (Vector3.SqrMagnitude(targetPositionDelta) <= Bunny.EAT_DISTANCE_THRESHOLD_SQ)
                _targets.RemoveAt(0);
        }

        void CalculateTargets()
        {
            _targets.Clear();

            if (!HasTarget)
                return;

            var primaryTarget = Target;
            var curPos = transform.position;
            var delta = primaryTarget - curPos;
            var remainingDistance = delta.magnitude;

            if (remainingDistance < INTERMEDIATE_TARGET_DISTANCE * 1.5f)
                return;

            var dir = delta.normalized;
            var perpendicularDir = Vector3.Cross(dir, Vector3.up);
            while (remainingDistance >= INTERMEDIATE_TARGET_DISTANCE * 1.5f)
            {
                curPos += dir * INTERMEDIATE_TARGET_DISTANCE;
                remainingDistance -= INTERMEDIATE_TARGET_DISTANCE;

                var offsetPos = curPos +
                    perpendicularDir * Random.Range(-INTERMEDIATE_TARGET_PERPENDICULAR_VARIANCE, INTERMEDIATE_TARGET_PERPENDICULAR_VARIANCE);

                _targets.Add(offsetPos);
            }
        }
    }
}