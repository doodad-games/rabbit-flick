using UnityEngine;
namespace DefaultNamespace
{
    public class BunnyArmored : MonoBehaviour
    {
        [SerializeField] GameObject armour;
        [SerializeField] float _armourKnockOffForce = 14.1f;
        [SerializeField] float _armourDisapearTime = 2.1f;

        public void DropArmour()
        {
            if (armour != null)
            {
                armour.AddComponent<Armor>()
                    .InitializeArmor(_armourKnockOffForce, _armourDisapearTime);
            }
        }
    }
}
