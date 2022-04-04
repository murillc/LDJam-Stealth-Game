using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    [SerializeField] List<Vector3> spawnLocations;
    [SerializeField] GameObject documentPrefab;

    public void SpawnDocumentAtLocation(int index)
    {
        Instantiate(documentPrefab, spawnLocations[index], new Quaternion(), this.transform);
    }

    public void SpawnDocumentRandom()
    {
        int length = spawnLocations.Count;

        if (length > 0)
        {
            int index = Random.Range(0, length);
            Instantiate(documentPrefab, spawnLocations[index], new Quaternion(), this.transform);
        } else
        {
            return;
        }

        
    }
}
