using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Armor : MonoBehaviour
    {
        float _knockOffForce = 14.1f;
        float _disapearTime = 2.1f;

        public void InitializeArmor(float armourKnockOffForce, float armourDisapearTime)
        {
            _knockOffForce = armourKnockOffForce;
            _disapearTime = armourDisapearTime;

            StartCoroutine(ArmourLifeCycle());
        }

        IEnumerator ArmourLifeCycle()
        {
            transform.parent = null;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            Rigidbody armourRigidbody = gameObject.AddComponent<Rigidbody>();
            armourRigidbody.AddExplosionForce(
                _knockOffForce,
                 new Vector3(
                     Random.Range(1.0f,-1.0f),
                     1,
                     Random.Range(1.0f,-1.0f)),
                _knockOffForce, _knockOffForce,ForceMode.Impulse);
            yield return new WaitForSeconds(_disapearTime);
            while (transform.localScale.magnitude > 0.1f)
            {
                Vector3 newScale  = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) * (1-Time.deltaTime*5);
                transform.localScale = newScale;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}