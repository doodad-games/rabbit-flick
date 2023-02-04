using UnityEngine;

namespace DefaultNamespace
{
    public class CriticalFlick : MonoBehaviour
    {
        [SerializeField] int increasedDamage = 2;

        public void StartExtraDamage() =>
            Bunny.FlickDamage += increasedDamage;

        public void EndExtraDamage() =>
            Bunny.FlickDamage -= increasedDamage;
    }
}