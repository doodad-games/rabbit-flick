using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bunny : MonoBehaviour, IPointerClickHandler
{
    const float CARROT_DISTANCE_THRESHOLD_SQ = 0.001f;
    const float EAT_DURATION = 2f;
    [SerializeField] int health = 1;

    public static readonly HashSet<Bunny> All = new();


    Movement _movement;
    Carrot _targetCarrot;
    bool _isEating;

    public void OnEnable()
    {
        All.Add(this);

        if (_movement == null)
            _movement = GetComponent<Movement>();
    }

    public void OnDisable() =>
        All.Remove(this);

    public void Update()
    {
        if (_isEating)
            return;

        if (_targetCarrot == null)
            FindTargetCarrot();

        if (_targetCarrot != null)
            TryToEatCarrot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (health--<=0) Destroy(gameObject);
    }

    void FindTargetCarrot()
    {
        var curPos = transform.position;

        var minimumDistanceSq = float.MaxValue;
        _targetCarrot = null;

        foreach (var carrot in Carrot.All)
        {
            var distSq = (curPos - carrot.transform.position).sqrMagnitude;
            if (distSq >= minimumDistanceSq)
                continue;

            minimumDistanceSq = distSq;
            _targetCarrot = carrot;
        }

        if (_movement != null)
            _movement.Target = _targetCarrot?.transform;
    }

    void TryToEatCarrot()
    {
        var carrotPositionDelta = _targetCarrot.transform.position - transform.position;
        if (Vector3.SqrMagnitude(carrotPositionDelta) > CARROT_DISTANCE_THRESHOLD_SQ)
            return;

        StartCoroutine(EatCarrot());

        return;


        IEnumerator EatCarrot()
        {
            Destroy(_targetCarrot.gameObject);
            _movement.enabled = false;
            _isEating = true;

            yield return new WaitForSeconds(EAT_DURATION);

            _isEating = false;
            _movement.enabled = true;
        }
    }
}
