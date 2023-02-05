using UnityEngine;

namespace DefaultNamespace
{
    public class CriticalFlick : MonoBehaviour
    {
        [SerializeField] int increasedDamage = 2;
        [SerializeField] float meshRotationDegreesPerSecond = 20;

        void Update() =>
            transform.Rotate(new Vector3(0, meshRotationDegreesPerSecond, 0) * Time.deltaTime);

        public void StartExtraDamage() =>
            Bunny.FlickDamage += increasedDamage;

        public void EndExtraDamage() =>
            Bunny.FlickDamage -= increasedDamage;
    }
}