using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public delegate void entryTest(EntryPoint entryPoint);
    public static event entryTest OnEntryTest;

    public float entryCooldown = 5f;
    public Vector3 entryPos = Vector3.zero;
    public bool onCooldown = false;

    private void Awake()
    {
        entryPos = transform.position;
    }

    public Vector3 UseEntry()
    {
        onCooldown = true;

        StartCoroutine(EntryCooldown());

        return entryPos;
    }

    IEnumerator EntryCooldown()
    {
        yield return new WaitForSeconds(entryCooldown);

        onCooldown = false;

        if (OnEntryTest != null)
        {
            OnEntryTest(this);
        }
    }
}
