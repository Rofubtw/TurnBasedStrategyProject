using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollPrefab;

    [SerializeField]
    private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        if(TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            this.healthSystem = healthSystem;

            healthSystem.OnDead += HealthSystem_OnDead;
        }
    }

    private void HealthSystem_OnDead()
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        if(ragdollTransform.TryGetComponent<UnitRagdoll>(out UnitRagdoll unitRagdoll))
        {
            unitRagdoll.Setup(originalRootBone);
        }
    }
}
