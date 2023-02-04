using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Bunny : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    const float CARROT_DISTANCE_THRESHOLD_SQ = 0.001f;
    const float RANDOM_CARROT_CHANCE = 0.5f;
    const float EAT_DURATION = 2f;


    public static event Action OnHoveredBunnyChanged;

    public static readonly HashSet<Bunny> All = new();
    public static Bunny CurrentlyHovered { get; private set; }

    static void SetHoveredBunny(Bunny bunny)
    {
        CurrentlyHovered = bunny;
        OnHoveredBunnyChanged?.Invoke();
    }


    [SerializeField] int _health = 1;
    [SerializeField] int _lowHealth = 1;
    [SerializeField] UnityEvent _lowHealthEvent;

    Movement _movement;
    Carrot _targetCarrot;
    bool _isEating;

    public void OnEnable()
    {
        All.Add(this);

        if (_movement == null)
            _movement = GetComponent<Movement>();
    }

    public void OnDisable()
    {
        All.Remove(this);

        if (CurrentlyHovered == this)
            SetHoveredBunny(null);
    }

    public void Update()
    {
        if (_isEating)
            return;

        if (_targetCarrot == null)
            FindTargetCarrot();

        if (_targetCarrot != null)
            TryToEatCarrot();
    }

    public void OnPointerEnter(PointerEventData eventData) =>
        SetHoveredBunny(this);

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentlyHovered == this)
            SetHoveredBunny(null);
    }

    public void TakeFlick()
    {
        _health--;
        if (_health<=0)
            Destroy(gameObject);
        else if (_health <= _lowHealth)
            _lowHealthEvent?.Invoke();
    }

    void FindTargetCarrot()
    {
        var curPos = transform.position;

        var minimumDistanceSq = float.MaxValue;
        _targetCarrot = null;

        if (Carrot.All.Count != 0)
        {
            if (Random.value <= RANDOM_CARROT_CHANCE)
                _targetCarrot = Carrot.All[Random.Range(0, Carrot.All.Count)];
            else
            {
                foreach (var carrot in Carrot.All)
                {
                    var distSq = (curPos - carrot.transform.position).sqrMagnitude;
                    if (distSq >= minimumDistanceSq)
                        continue;

                    minimumDistanceSq = distSq;
                    _targetCarrot = carrot;
                }
            }
        }

        if (_movement is not null)
        {
            _movement.Target = _targetCarrot?.transform;

            if (_targetCarrot is not null)
                transform.LookAt(_targetCarrot.transform.position);
        }
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
