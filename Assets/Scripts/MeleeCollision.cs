using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{

    public List<Collider> damageableColliders = new List<Collider>();
    [SerializeField] Collider characterControllerCollider;

    private void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), characterControllerCollider, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        damageableColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (damageableColliders.Contains(other))
        {
            damageableColliders.Remove(other);
        }
    }

}
