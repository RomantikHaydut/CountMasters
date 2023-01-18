using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneController : MonoBehaviour
{
    [Header("Clone Variables")]
    public int maxCloneCount = 100;
    [SerializeField] private GameObject firstStickman;
    [SerializeField] private int CloneAmountsToAdd = 1;
    [SerializeField] private int CloneAmountsToDestroy = 1;
    public GameObject stickmanPrefab;
    [SerializeField] private GameObject circlePrefab;
    public float stickmanRadius;
    public int stickCount = 1;
    [SerializeField] private int circleCount = 0;
    [SerializeField] private List<GameObject> stickmanList = new List<GameObject>();
    [SerializeField] private List<GameObject> destroyedStickmanList = new List<GameObject>();
    [SerializeField] private List<CircleController> circleControllerList = new List<CircleController>();
    [SerializeField] private int destroyedStickmanCount = 0;
    [SerializeField] private int destroyIndexedStickman = 0;
    [SerializeField] private Text cloneCountText;
    private UIManager uiManager;

    private void Awake()
    {
        circleCount = 0;
        CapsuleCollider capsuleCollider = stickmanPrefab.GetComponentInChildren(typeof(CapsuleCollider)) as CapsuleCollider;
        stickmanRadius = capsuleCollider.radius;
        uiManager = FindObjectOfType<UIManager>();
        CloneStickmanStart(maxCloneCount);
        DisplayCloneCount();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DestroyStickmans3(CloneAmountsToDestroy);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            CloneStickman4(CloneAmountsToAdd);
        }
    }


    #region Clone Methods

    private void CloneStickmanStart(int cloneCount)
    {
        stickmanList.Add(firstStickman);
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject stickmanClone = Instantiate(stickmanPrefab, transform.position, Quaternion.identity, transform);
            stickmanClone.name = circleCount.ToString();
            stickmanList.Add(stickmanClone);
            stickmanClone.SetActive(false);
        }
        CalculateCapacityOfCircles();
        SplitStickmansToCircles();
    }

    public void CloneStickman4(int cloneCount)
    {
        if (stickCount < maxCloneCount)
        {
            for (int i = 0; i < cloneCount; i++)
            {
                if (stickCount == maxCloneCount)
                    break;


                GameObject stickmanClone;
                if (destroyedStickmanList.Count > 0)
                {
                    stickmanClone = destroyedStickmanList[destroyedStickmanList.Count - 1];
                    stickmanClone.SetActive(true);
                    destroyedStickmanList.Remove(stickmanClone);
                    stickCount++;
                }
                else
                {
                    stickmanClone = stickmanList[stickCount];
                    stickmanClone.SetActive(true);
                    stickCount++;
                }
            }
            CheckAndSortCircles2();
            PlaceStickmansInAllCircle2();
        }

    }

    private void CheckAndSortCircles2()
    {
        for (int i = 0; i < stickmanList.Count; i++)
        {
            stickmanList[i].SetActive(false);
        }

        for (int i = 0; i < stickCount; i++)
        {
            stickmanList[i].SetActive(true);
        }
    }
    #endregion

    #region Destroy Methods


    public void DestroySpecificStickman(GameObject destroying)
    {
        if (stickCount > 1)
        {
            int index = stickmanList.IndexOf(destroying);
            stickmanList[index].SetActive(false);
            destroyedStickmanList.Add(stickmanList[index]);
            stickCount--;
        }
        DisplayCloneCount();

    }

    public void DestroyStickmans3(int destroyAmount)
    {
        if (stickCount > 1)
        {
            int newStickCount = stickCount;
            List<GameObject> destroyingStickmanList = new List<GameObject>();
            for (int i = 0; i < destroyAmount; i++)
            {
                if (newStickCount > 1)
                {
                    GameObject destroyingStickman = stickmanList[stickCount - 1 - i].gameObject;
                    destroyingStickmanList.Add(destroyingStickman);
                    destroyedStickmanList.Add(destroyingStickman);
                    newStickCount--;
                }
                else
                    break;
                   
            }
            StartCoroutine(DestroyStickmans_Coroutine(destroyingStickmanList));
        }
    }
    private IEnumerator DestroyStickmans_Coroutine(List<GameObject> stickmanList)
    {
        float speed = 4f;
        float timer = 0.0f;
        float stopTime = 1f;
        List<Vector3> startPosList = new List<Vector3>();
        for (int i = 0; i < stickmanList.Count; i++)
        {
            startPosList.Add(stickmanList[i].transform.position);
        }
        while (true)
        {
            yield return null;
            if (timer >= stopTime)
            {
                for (int i = 0; i < stickmanList.Count; i++)
                {
                    stickmanList[i].transform.position = transform.position;
                    stickmanList[i].SetActive(false);
                    stickCount--;
                    
                }
                CheckAndSortCircles2();
                PlaceStickmansInAllCircle2();
                DisplayCloneCount();
                yield break;
            }
            timer += Time.deltaTime * speed;
            for (int i = 0; i < stickmanList.Count; i++)
            {
                stickmanList[i].transform.position = Vector3.Lerp(startPosList[i], transform.position, timer);
            }
        }
    }
    #endregion


    #region Common Methods For Clone and Destroy


    private void PlaceStickmansInAllCircle2()
    {
        for (int i = 0; i < circleControllerList.Count; i++)
        {
            circleControllerList[i].PlaceMyStickmans();
        }
        DisplayCloneCount();
    }

    private float CapacityOfNumberedCircle(int circleNumber)
    {
        float capacityOfCircle;
        if (circleNumber != 0)
        {
            float circleLenght = Mathf.PI * 2 * stickmanRadius * circleNumber;
            capacityOfCircle = (circleLenght / stickmanRadius);
        }
        else
        {
            capacityOfCircle = 1f;
        }
        return capacityOfCircle;
    }

    private void CalculateCapacityOfCircles()
    {
        int circleNumber = 0;
        int stickCount = 0;
        for (int i = 0; i < stickmanList.Count; i++)
        {
            if (stickCount == (int)CapacityOfNumberedCircle(circleNumber))
            {
                GameObject circleControllerObject = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
                CircleController circleController = circleControllerObject.GetComponent<CircleController>();
                if (circleController != null)
                {
                    circleController.myIndex = circleNumber;
                    circleController.myCapacity = CapacityOfNumberedCircle(circleNumber);
                    circleControllerList.Add(circleController);
                    circleNumber++;
                    stickCount = 0;
                }
            }
            stickCount++;

        }
        GameObject LastCircleControllerObject = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
        CircleController LastCircleController = LastCircleControllerObject.GetComponent<CircleController>();
        LastCircleController.myIndex = circleNumber;
        LastCircleController.myCapacity = stickCount;
        circleControllerList.Add(LastCircleController);
    }

    private void SplitStickmansToCircles()
    {
        int circleNumber = 0;
        int stickCount = 0;
        for (int i = 0; i < stickmanList.Count; i++)
        {
            if (stickCount == (int)CapacityOfNumberedCircle(circleNumber))
            {
                stickCount = 0;
                circleNumber++;
            }
            GameObject stickmanClone = stickmanList[i].gameObject;
            stickCount++;
            circleControllerList[circleNumber].GetComponent<CircleController>().myStickmans.Add(stickmanClone);
        }
    }

    public void DisplayCloneCount()
    {
        uiManager.DisplayText(cloneCountText, stickCount.ToString());
    }
    #endregion
}
