using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int objectDistance;
    [SerializeField] private GameObject multiplierPrefab;
    public List<GameObject> multiplierList = new List<GameObject>();
    [SerializeField] private int multiplierCount;
    private void Awake()
    {
        CreateMultipliers();
    }

    private void CreateMultipliers()
    {
        for (int i = 0; i < multiplierCount; i++)
        {
            Vector3 spawnPos = new Vector3(0, 0, objectDistance + i * objectDistance);
            GameObject multiplierClone = Instantiate(multiplierPrefab, spawnPos, Quaternion.identity);
            multiplierList.Add(multiplierClone);

            if (i == 0)
            {
                multiplierClone.GetComponent<MultiplierController>().SetBothOptionsGood();
            }
            else if (i == 1)
            {
                multiplierClone.GetComponent<MultiplierController>().SetOptionsMixed();
            }
            else
            {
                multiplierClone.GetComponent<MultiplierController>().SetRandomOptions();
            }
        }
    }
}
