using UnityEngine;

namespace DefaultNamespace
{
    public class CriticalFlick : MonoBehaviour
    {
        [SerializeField] int increasedDamage = 3;

        public void StartExtraDamage()
        {
            Bunny.Click_Damage = increasedDamage;
        }

        public void EndExtraDamage()
        {
            Bunny.Click_Damage = 1;
        }
    }
}