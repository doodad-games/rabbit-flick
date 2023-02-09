using System;
using System.Collections;
using System.Collections.Generic;
using MyLibrary;
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

    [Serializable]
    struct Sounds
    {
        public string FlickedSound;
        public string FlickedArmourSound;
        public string FlickedAwaySound;
    }


    public const float EAT_DISTANCE_THRESHOLD_SQ = 0.001f;

    const float RANDOM_CARROT_CHANCE = 0.5f;
    const float EAT_DURATION = 2f;

    public static event Action<Bunny> OnHoveredOverBunny;
    public static event Action<Bunny> OnSomethingFlicked;

    public static readonly HashSet<Bunny> All = new();
    static readonly HashSet<Bunny> s_currentlyHovered = new();
    public static IReadOnlyCollection<Bunny> CurrentlyHovered => s_currentlyHovered;

    public static int FlickDamage = 1;
    [RuntimeInitializeOnLoadMethod]
    public static void ResetFlickDamage() =>
        FlickDamage = 1;

    static readonly int s_destroyed = Animator.StringToHash("Destroyed");
    static readonly int s_eating = Animator.StringToHash("Eating");

    public event Action OnReachedLowLife;
    public event Action OnFlicked;

    [SerializeField] Animator _animator;
    [SerializeField] int _health = 1;
    [SerializeField] int _lowHealth = 1;
    [SerializeField] bool _skipsEatingState;
    [SerializeField] Sounds _sounds;
    [SerializeField] Collider[] _colliders;

    Movement _movement;
    Carrot _targetCarrot;
    State _state;

#if UNITY_EDITOR
    [ContextMenu("Populate Colliders")]
    public void PopulateColliders()
    {
        _colliders = GetComponentsInChildren<Collider>();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

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

        s_currentlyHovered.Remove(this);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (s_currentlyHovered.Add(this))
            OnHoveredOverBunny?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData) =>
        s_currentlyHovered.Remove(this);

    public void TakeFlick()
    {
        var wasOnLowHealth = _health <= _lowHealth;

        _health -= FlickDamage;

        var isOnLowHealth = _health <= _lowHealth;
        var flickedAway = _health <= 0;

        if (flickedAway && !string.IsNullOrEmpty(_sounds.FlickedAwaySound))
            SoundController.Play(_sounds.FlickedAwaySound);
        else if (!wasOnLowHealth && !string.IsNullOrEmpty(_sounds.FlickedArmourSound))
            SoundController.Play(_sounds.FlickedArmourSound);
        else if (!string.IsNullOrEmpty(_sounds.FlickedSound))
            SoundController.Play(_sounds.FlickedSound);

        OnSomethingFlicked?.Invoke(this);

        if (flickedAway)
        {
            _animator.SetTrigger(s_destroyed);

            OnFlicked?.Invoke();

            Destroy(_movement);
            Destroy(this);
            foreach (var coll in _colliders)
                Destroy(coll);
        }
        else if (isOnLowHealth && !wasOnLowHealth)
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
        if (Vector3.SqrMagnitude(carrotPositionDelta) > EAT_DISTANCE_THRESHOLD_SQ)
            return;

        StartCoroutine(EatCarrot());

        return;


        IEnumerator EatCarrot()
        {
            _targetCarrot.Destroy();
            _targetCarrot = null;
            SoundController.PlayAtLocation("Bunny Eat Carrot", transform.position);

            if (_skipsEatingState)
                yield break;

            _animator.SetBool(s_eating, true);

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
        if (Vector3.SqrMagnitude(targetDelta) > EAT_DISTANCE_THRESHOLD_SQ)
            return;

        Destroy(gameObject);
    }
}
