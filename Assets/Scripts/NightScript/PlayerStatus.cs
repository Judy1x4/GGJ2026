using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public List<string> secretsObtained = new();
    [SerializeField] public int energyCount = 0;
}
