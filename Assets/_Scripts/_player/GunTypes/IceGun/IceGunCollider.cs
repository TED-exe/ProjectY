using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGunCollider : MonoBehaviour
{
    [SerializeField] private List<GameObject> freezingObject = new List<GameObject>();
    [SerializeField] private so_GunParameters parameters;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IFrozenable>(out IFrozenable freeze))
        {
            freezingObject.Add(other.gameObject);
            freeze.Freezing(parameters.gunBaseDmg, parameters.gunEffectDmg);
        }
        else
        {
            var parent = other.transform.parent;
            if (parent.TryGetComponent<IFrozenable>(out IFrozenable freezeParent))
            {
                freezingObject.Add(parent.gameObject);
                freezeParent.Freezing(parameters.gunBaseDmg, parameters.gunEffectDmg);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IFrozenable>(out IFrozenable freeze))
        {
            freezingObject.Remove(other.gameObject);
            freeze.StartDeleyToUnfreez(parameters.gunEffectDuration);
        }
        else
        {
            var parent = other.transform.parent;
            if (parent.TryGetComponent<IFrozenable>(out IFrozenable freezeParent))
            {
                freezingObject.Remove(parent.gameObject);
                freezeParent.StartDeleyToUnfreez(parameters.gunEffectDuration);
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in freezingObject)
        {
            if (obj != null && obj.TryGetComponent<IFrozenable>(out IFrozenable freeze))
                freeze.StartDeleyToUnfreez(parameters.gunEffectDuration);
        }
        freezingObject.Clear();
    }
}
