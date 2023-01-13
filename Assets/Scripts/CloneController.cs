using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    // Clone
    [Header("Clone Variables")]
    [SerializeField] private int maxCloneCount = 100;
    [SerializeField] private GameObject firstStickman;
    [SerializeField] private int CloneAmountsToAdd = 1;
    [SerializeField] private int CloneAmountsToDestroy = 1;
    [SerializeField] private GameObject stickmanPrefab;
    [SerializeField] private float stickmanRadius;
    [SerializeField] private int stickCount = 1;
    [SerializeField] private int stickCountInLastCircle = 0;
    [SerializeField] private int circleCount = 0;
    [SerializeField] private List<GameObject> stickmanList = new List<GameObject>();


    private void Awake()
    {
        circleCount = 1;
        stickCountInLastCircle = 0;
        CapsuleCollider capsuleCollider = stickmanPrefab.GetComponentInChildren(typeof(CapsuleCollider)) as CapsuleCollider;
        stickmanRadius = capsuleCollider.radius;
        stickmanList.Add(firstStickman);

        CloneStickmanStart(maxCloneCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            DestroyStickmans(CloneAmountsToDestroy);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CloneStickman(CloneAmountsToAdd);
        }
    }


    #region Clone Methods

    private void CloneStickmanStart(int cloneCount)
    {
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject stickmanClone = Instantiate(stickmanPrefab, transform.position, Quaternion.identity, transform);
            stickmanClone.name = circleCount.ToString();
            stickmanList.Add(stickmanClone);


            stickmanClone.SetActive(false);

        }
    }

    private void CloneStickman(int cloneCount)
    {
        if (stickCount <= maxCloneCount)
        {
            for (int i = 0; i < cloneCount; i++)
            {
                if (stickCount > maxCloneCount)
                    break;

                if (stickCountInLastCircle == (int)CapacityOfLastCircle())
                {
                    SortLastCircle();
                    circleCount++;
                    stickCountInLastCircle = 0;
                }

                GameObject stickmanClone = stickmanList[stickCount];
                stickmanClone.name = circleCount.ToString();
                stickmanClone.SetActive(true);
                stickCount++;
                stickCountInLastCircle++;
            }

            SortLastCircle();
        }
        
    }

    #endregion

    #region Destroy Methods

    private void DestroyStickmans(int destroyAmount)
    {
        for (int i = 0; i < destroyAmount; i++)
        {
            if (stickCount > 1)
            {
                GameObject destroyingStickman = stickmanList[stickCount - 1].gameObject;
                destroyingStickman.SetActive(false);
                stickCount--;

                if (stickCountInLastCircle > 1)
                    stickCountInLastCircle--;
                else
                {
                    DecreaseCircle();
                }

            }
        }

        SortLastCircle();

    }
    private void DecreaseCircle()
    {
        if (circleCount >= 1)
        {
            circleCount--;
            stickCountInLastCircle = (int)CapacityOfLastCircle();
        }
    }
    #endregion


    #region Common Methods For Clone and Destroy
    
    private void SortLastCircle()
    {
        if (circleCount > 0 && stickCount > 1 && stickCountInLastCircle > 0)
        {
            List<GameObject> stickmansListInCircle = new List<GameObject>();
            GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
            for (int i = 0; i < stickmanArray.Length; i++)
            {
                if (stickmanArray[i].name == circleCount.ToString())
                {
                    if (stickmanArray[i].gameObject.activeInHierarchy)
                    {
                        stickmansListInCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
                    }
                }
            }

            float capacityOfCircle = CapacityOfLastCircle();
            float leftOverCount = capacityOfCircle - (int)capacityOfCircle; // This value is splitting all angles.

            for (int i = 0; i < stickCountInLastCircle; i++)
            {
                float angle = (i * 2 * Mathf.PI + leftOverCount) / (stickCountInLastCircle);
                stickmansListInCircle[i].transform.position = transform.position + new Vector3(Mathf.Sin(angle) * stickmanRadius * circleCount, 0, Mathf.Cos(angle) * stickmanRadius * circleCount); // Split stickmans in last circle.
            }
        }


    }

    private void CalculateStickmanCountInLastCircle()
    {
        List<GameObject> stickmansListInLastCircle = new List<GameObject>();
        GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
        for (int i = 0; i < stickmanArray.Length; i++)
        {
            if (stickmanArray[i].name == circleCount.ToString())
            {
                stickmansListInLastCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
            }
        }

        stickCountInLastCircle = stickmansListInLastCircle.Count;
    }

    private float CapacityOfLastCircle()
    {
        float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
        float capacityOfCircle = (circleLenght / stickmanRadius);

        return capacityOfCircle;

    }
    #endregion
}
