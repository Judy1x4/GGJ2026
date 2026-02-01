using UnityEngine;
using System.Collections.Generic;

public class Secrets: MonoBehaviour
{
    [SerializeField] private List<string> secrets = new();

    private int index = 0;

    public string GetNextSecret()
    {
        if (secrets == null || secrets.Count == 0)
        {
            Debug.LogWarning("Secrets list is empty.");
            return null;
        }

        if (index >= secrets.Count)
        {
            Debug.Log("All secrets exhausted.");
            return null;
        }

        return secrets[index++];
    }
}
