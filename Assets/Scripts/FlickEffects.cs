using UnityEngine;

public class FlickEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem _regularFlickParticles;
    [SerializeField] ParticleSystem _criticalFlickParticles;

    public void OnEnable() =>
        Bunny.OnSomethingFlicked += HandleSomethingFlicked;
    public void OnDisable() =>
        Bunny.OnSomethingFlicked -= HandleSomethingFlicked;

    void HandleSomethingFlicked(Bunny bunny)
    {
        var emitParams = new ParticleSystem.EmitParams { position = bunny.transform.position };

        if (Bunny.FlickDamage == 1)
            _regularFlickParticles.Emit(emitParams, count: 3);
        else _criticalFlickParticles.Emit(emitParams, count: 4);
    }
}
