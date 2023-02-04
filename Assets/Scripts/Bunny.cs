using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Bunny : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum State
    {
        LookingForFood,
        Eating,
        Leaving,
    }


    const float DISTANCE_THRESHOLD_SQ = 0.001f;
    const float RANDOM_CARROT_CHANCE = 0.5f;
    const float EAT_DURATION = 2f;


    public static event Action OnHoveredBunnyChanged;

    public static readonly HashSet<Bunny> All = new();
    public static Bunny CurrentlyHovered { get; private set; }

    static readonly int s_destroyed = Animator.StringToHash("Destroyed");
    static readonly int s_eating = Animator.StringToHash("Eating");

    static void SetHoveredBunny(Bunny bunny)
    {
        CurrentlyHovered = bunny;
        OnHoveredBunnyChanged?.Invoke();
    }


    public event Action OnReachedLowLife;
    public event Action OnDestroyed;

    [SerializeField] Animator _animator;
    [SerializeField] int _health = 1;
    [SerializeField] int _lowHealth = 1;

    Movement _movement;
    Carrot _targetCarrot;
    State _state;

    public void OnEnable()
    {
        All.Add(this);

        GameManager.OnLoseGame += StartLeaving;

        if (_movement == null)
            _movement = GetComponent<Movement>();

        if (GameManager.I.CurState == GameManager.State.Menu)
            StartLeaving();
        else _state = State.LookingForFood;
    }

    public void OnDisable()
    {
        All.Remove(this);

        GameManager.OnLoseGame -= StartLeaving;

        if (CurrentlyHovered == this)
            SetHoveredBunny(null);
    }

    public void Update()
    {
        if (_state == State.LookingForFood)
        {
            if (_targetCarrot == null)
                FindTargetCarrot();

            if (_targetCarrot != null)
                TryToEatCarrot();
        }
        else if (_state == State.Leaving)
            SelfDestructIfFinishedLeaving();
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

        if (_health <= 0)
        {
            _animator.SetTrigger(s_destroyed);
            OnDestroyed?.Invoke();
            Destroy(_movement);
            Destroy(this);
        }
        else if (_health <= _lowHealth)
            OnReachedLowLife?.Invoke();
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
            _movement.SetTarget(_targetCarrot?.transform);
    }

    void TryToEatCarrot()
    {
        var carrotPositionDelta = _targetCarrot.transform.position - transform.position;
        if (Vector3.SqrMagnitude(carrotPositionDelta) > DISTANCE_THRESHOLD_SQ)
            return;

        StartCoroutine(EatCarrot());

        return;


        IEnumerator EatCarrot()
        {
            _animator.SetBool(s_eating, true);

            Destroy(_targetCarrot.gameObject);
            _movement.enabled = false;
            _state = State.Eating;

            yield return new WaitForSeconds(EAT_DURATION);

            _movement.enabled = true;
            _animator.SetBool(s_eating, false);

            if (_state == State.Leaving)
                yield break;

            _state = State.LookingForFood;
        }
    }

    void StartLeaving()
    {
        if (_state == State.Leaving)
            return;

        _state = State.Leaving;
        _movement.SetTarget(BunnySpawnZone.I.PickPointInSpawnZone());
    }

    void SelfDestructIfFinishedLeaving()
    {
        var targetDelta = _movement.Target - transform.position;
        if (Vector3.SqrMagnitude(targetDelta) > DISTANCE_THRESHOLD_SQ)
            return;

        Destroy(gameObject);
    }
}
