using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPointsManager : Singleton<EntryPointsManager>
{
    public List<EntryPoint> entryPoints;
    public List<EntryPoint> entryPointsOnCooldown;

    private void OnEnable()
    {
        EntryPoint.OnEntryTest += EntryPointDoneCooldown;
    }

    private void OnDisable()
    {
        EntryPoint.OnEntryTest -= EntryPointDoneCooldown;
    }

    public Vector3 GetEntryPoint()
    {
        int rand = Random.Range(0, entryPoints.Count);

        Vector3 entry = entryPoints[rand].UseEntry();

        entryPointsOnCooldown.Add(entryPoints[rand]);
        entryPoints.RemoveAt(rand);

        return entry;
    }

    public void EntryPointDoneCooldown(EntryPoint entryPoint)
    {
        Debug.Log("done cooldown, added back to original list");
        entryPointsOnCooldown.Remove(entryPoint);
        entryPoints.Add(entryPoint);
    }

}
