using UnityEngine;
using System.Collections;

public class SpawnZombie : MonoBehaviour
{
    public Transform spawnDestination;
    public GameObject zombiePrefab;

    public void OnClick()
    {
        Object.Instantiate(zombiePrefab, spawnDestination);
    }
}
