using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GrowCarrotsPowerUp : MonoBehaviour
    {
        [SerializeField] int GrowAmmountMin = 3;
        [SerializeField] int GrowAmmountMax = 7;
        [SerializeField] float growthRadius = 1.4f;
        [SerializeField] GameObject ExplosionEffect;
        [SerializeField] GameObject CarrotMeshRotaionPoint;
        [SerializeField] float meshRotationDegreesPerSecond =20;
        bool rotatingMesh;

        void Awake()
        {
            Assert.IsNotNull(ExplosionEffect);
            Assert.IsNotNull(CarrotMeshRotaionPoint);
            rotatingMesh = true;
            ExplosionEffect.SetActive(false);
        }

        void Update()
        {
            if (rotatingMesh)
            {
                transform.Rotate(new Vector3(0, meshRotationDegreesPerSecond, 0) * Time.deltaTime);
            }
        }

        public void StartCarrotGrowthExplosion()
        {
            rotatingMesh = false;
            ExplosionEffect.SetActive(true);
            Vector3 spawnLocation = transform.position;
            Quaternion spawnRoation = transform.rotation;
            GameObject carrotPrefab = Resources.Load<GameObject>(Constants.Resources.CARROT_PREFAB);
            Instantiate(carrotPrefab, spawnLocation, spawnRoation);
            int howManyCarrots = Random.Range(GrowAmmountMin, GrowAmmountMax);
            for (int i = 0; i < howManyCarrots; i++)
            {
                float randomX = Random.Range(-growthRadius, growthRadius);
                float randomZ = Random.Range(-growthRadius, growthRadius);
                Vector3 randomSpawnLocation = spawnLocation + new Vector3(randomX,0,randomZ);
                Instantiate(carrotPrefab, randomSpawnLocation, spawnRoation);
            }
        }
    }
}