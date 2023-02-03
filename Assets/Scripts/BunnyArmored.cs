using UnityEngine;
namespace DefaultNamespace
{
    public class BunnyArmored : MonoBehaviour
    {
        [SerializeField] GameObject armour;
        [SerializeField] float armourFallForce = 14.1f;
        [SerializeField] float armourDisapearTime = 2.1f;
        public void DropArmour()
        {
            if (armour!= null)
            {
                armour.transform.parent = null;
                armour.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                Rigidbody armourRigidbody = armour.AddComponent<Rigidbody>();
                armourRigidbody.AddExplosionForce(
                    armourFallForce,
                    new Vector3(
                        Random.Range(1.0f,-1.0f),
                        1,
                        Random.Range(1.0f,-1.0f)),
                    armourFallForce, armourFallForce,ForceMode.Impulse);
                Invoke(nameof(DestroyArmour), armourDisapearTime);
            }
        }
        void DestroyArmour()
        {
            if (armour != null)
            {
                Destroy(armour);
            }
        }
    }
}
