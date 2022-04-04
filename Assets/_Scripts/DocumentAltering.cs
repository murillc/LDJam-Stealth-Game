using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentAltering : Singleton<DocumentAltering>
{
    [SerializeField] public bool altered = false;
    [SerializeField] public bool finishedAltering;
    [SerializeField] public bool inDocumentRange;

    [SerializeField] public float timeToAlter;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AlterDocuments()
    {
        StartCoroutine(IE_AlterDocuments());
    }

    IEnumerator IE_AlterDocuments()
    {
        yield return null;
    }
}
