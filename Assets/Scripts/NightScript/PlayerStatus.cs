using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public List<string> secretsObtained = new();
    [SerializeField] public int energyCount = 0;

    public void SetAttempts(int patience)
    {
        if (patience <= 0)
        {
            energyCount = 0;
        }
        else if (patience <= 10)
        {
            energyCount = 1;
        }
        else if (patience <= 20)
        {
            energyCount = 2;
        }
        else if (patience <= 30)
        {
            energyCount = 3;
        }
    }

    public void AddSecret(string secret)
    {
        secretsObtained.Add(secret);
    }
}
