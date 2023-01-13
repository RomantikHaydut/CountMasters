using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    // Movement.
    [SerializeField] private bool mouseMovement;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float horizontalSpeed = 1f;
    private float mouseClickPosX;
    private float mouseActivePosX;
    // Clone

    [Header("Clone Variables")]
    [SerializeField] private GameObject firstStickman;
    [SerializeField] private int CloneAmountsToAdd = 1;
    [SerializeField] private int CloneAmountsToDestroy = 1;
    [SerializeField] private GameObject stickmanPrefab;
    [SerializeField] private float stickmanRadius;
    [SerializeField] private int stickCount = 1; // Clone count.
    [SerializeField] private int stickCountInLastCircle = 0;
    //[SerializeField] private List<GameObject> stickmanListInactive = new List<GameObject>(); // For pooling
    [SerializeField] private int circleCount = 0;
    [SerializeField] private bool isLastCircleFull = false;
    [SerializeField] private List<GameObject> stickmanList = new List<GameObject>();



    private void Awake()
    {
        isLastCircleFull = true;
        circleCount = 1;
        stickCountInLastCircle = 0;
        anim = GetComponent<Animator>();
        CapsuleCollider capsuleCollider = stickmanPrefab.GetComponentInChildren(typeof(CapsuleCollider)) as CapsuleCollider;
        stickmanRadius = capsuleCollider.radius;
        stickmanList.Add(firstStickman);

    }

    void Update()
    {
        //Movement();
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    DestroyStickmans2(CloneAmountsToDestroy);
        //}
        //else if (Input.GetKeyDown(KeyCode.A))
        //{
        //    CloneStickman4(CloneAmountsToAdd);
        //}
    }

    private void Movement()
    {
        //Forward movement.
        transform.position += Vector3.forward * Time.deltaTime * forwardSpeed;

        //Horizontal Movement keyboard.
        if (!mouseMovement)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.position += Vector3.right * Time.deltaTime * horizontalInput * horizontalSpeed;
        }
        //Horizontal Movement mouse.
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClickPosX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                mouseActivePosX = Input.mousePosition.x;
                float horizontalInput = (mouseActivePosX - mouseClickPosX <= 0) ? -1 : 1;
                transform.position += Vector3.right * Time.deltaTime * horizontalInput * horizontalSpeed;
            }
        }
    }

    // Animations
    private void StartRun()
    {
        if (!anim.GetBool("Running"))
        {
            anim.SetBool("Running", true);
        }
    }


    #region Clone Methods
    private void CloneStickman(int cloneCount)
    {
        int wantedCloneCount = stickCount + cloneCount;
        for (int i = 0; i <= Mathf.Infinity; i++)
        {
            CheckLastCircleFull();

            float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
            float capacityOfCircle = circleLenght / stickmanRadius;
            int countInt = (int)capacityOfCircle - stickCountInLastCircle;
            float leftOverCount = capacityOfCircle - (int)capacityOfCircle; // This value is splitting all angles.

            for (int j = 0; j < countInt; j++)
            {

                float angle = (j * 2 * Mathf.PI + leftOverCount) / (countInt);

                Vector3 spawnPos = transform.position + new Vector3(Mathf.Sin(angle) * stickmanRadius * circleCount, 0, Mathf.Cos(angle) * stickmanRadius * circleCount);

                GameObject stickmanClone = Instantiate(stickmanPrefab, spawnPos, Quaternion.identity, transform);
                stickmanClone.name = circleCount.ToString();
                stickmanList.Add(stickmanClone);

                stickCount++;

                CalculateStickmanCountInLastCircle();

                if (stickCount >= wantedCloneCount)
                    goto finish;
            }
        }

    finish:;

        SortCircle2(circleCount);
    }

    private void CloneStickman2(int cloneCount)
    {
        int wantedCloneCount = stickCount + cloneCount;
        for (int i = 0; i <= Mathf.Infinity; i++)
        {
            if (IsLastCircleFull())
            {
                circleCount++;
                stickCountInLastCircle = 0;
            }

            float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
            float capacityOfCircle = circleLenght / stickmanRadius;
            int countInt = (int)capacityOfCircle;

            for (int j = 0; j < countInt; j++)
            {
                if (IsLastCircleFull())
                {
                    SortLastCircle();
                    circleCount++;
                    stickCountInLastCircle = 0;
                }

                GameObject stickmanClone = Instantiate(stickmanPrefab, transform.position, Quaternion.identity, transform);
                stickmanClone.name = circleCount.ToString();
                stickmanList.Add(stickmanClone);
                stickCount++;

                if (stickCount >= wantedCloneCount)
                    goto finish;

            }
            SortLastCircle();
        }

    finish:;
        CalculateStickmanCountInLastCircle();
        SortCircle2(circleCount);
    }
    private void CloneStickman3(int cloneCount)
    {
        for (int i = 0; i < cloneCount; i++)
        {
            if (IsLastCircleFull())
            {
                SortLastCircle();
                IncreaseCircle();
                stickCountInLastCircle = 0;
            }

            GameObject stickmanClone = Instantiate(stickmanPrefab, transform.position, Quaternion.identity, transform);
            stickmanClone.name = circleCount.ToString();
            stickmanList.Add(stickmanClone);
            stickCount++;
        }

        CalculateStickmanCountInLastCircle();
        SortLastCircle();
    }

    private void CloneStickman4(int cloneCount)
    {
        for (int i = 0; i < cloneCount; i++)
        {
            if (stickCountInLastCircle == (int)CapacityOfLastCircle())
            {
                SortLastCircle2();
                IncreaseCircle2();
                stickCountInLastCircle = 0;
            }
            GameObject stickmanClone = Instantiate(stickmanPrefab, transform.position, Quaternion.identity, transform);
            stickmanClone.name = circleCount.ToString();
            stickmanList.Add(stickmanClone);
            stickCount++;
            stickCountInLastCircle++;
        }

        //CalculateStickmanCountInLastCircle();
        SortLastCircle2();
    }
    private void CheckLastCircleFull()
    {
        CalculateStickmanCountInLastCircle();
        float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
        int capacityOfCircle = (int)(circleLenght / stickmanRadius);
        if (stickCountInLastCircle == capacityOfCircle) // Pass to new circle.
        {
            SortLastCircle();
            circleCount++;
            isLastCircleFull = true;
        }
        else
        {
            isLastCircleFull = false;
        }

    }

    private bool IsLastCircleFull()
    {
        CalculateStickmanCountInLastCircle();
        float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
        int capacityOfCircle = (int)(circleLenght / stickmanRadius);
        if (stickCountInLastCircle == capacityOfCircle) // Pass to new circle.
            isLastCircleFull = true;
        else
            isLastCircleFull = false;

        return isLastCircleFull;
    }
    private void IncreaseCircle()
    {
        circleCount++;
        CalculateStickmanCountInLastCircle();
    }
    private void IncreaseCircle2()
    {
        circleCount++;

    }
    #endregion


    #region Destroy Methods
    /*private void DestroyStickmans(int destroyAmount)
    {
        for (int i = 0; i < destroyAmount; i++)
        {
            if (stickCount > 1)
            {
                GameObject destroyingStickman = stickmanList[stickmanList.Count - 1].gameObject;
                stickmanList.Remove(destroyingStickman);
                Destroy(destroyingStickman);
                stickCount--;

                if (stickCountInLastCircle >= 1)
                    stickCountInLastCircle--;
                else
                {
                    DecreaseCircle();

                }

                //if (stickCountInLastCircle == 0)
                //    DecreaseCircle();

                CalculateStickmanCountInLastCircle();
                SortLastCircle();
            }
        }

        //circleCount = int.Parse(stickmanList[stickmanList.Count - 1].gameObject.name);
        //CalculateCircleCount();
        //CalculateStickmanCountInLastCircle();
        //SortLastCircle();
    }*/

    private void DestroyStickmans2(int destroyAmount)
    {
        for (int i = 0; i < destroyAmount; i++)
        {
            if (stickCount > 1)
            {
                GameObject destroyingStickman = stickmanList[stickmanList.Count - 1].gameObject;
                stickmanList.Remove(destroyingStickman);
                Destroy(destroyingStickman);
                stickCount--;

                if (stickCountInLastCircle > 1)
                    stickCountInLastCircle--;
                else
                {
                    DecreaseCircle2();
                }

            }
        }

        SortLastCircle2();

    }
    /*private void DecreaseCircle()
    {
        if (circleCount >= 1)
        {
            circleCount--;
            CalculateStickmanCountInLastCircle();
        }
    }*/
    private void DecreaseCircle2()
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
        if (circleCount > 0 && stickCount > 1)
        {
            List<GameObject> stickmansListInCircle = new List<GameObject>();
            GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
            for (int i = 0; i < stickmanArray.Length; i++)
            {
                if (stickmanArray[i].name == circleCount.ToString())
                {
                    stickmansListInCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
                }
            }

            float capacityOfCircle = CapacityOfLastCircle();
            float leftOverCount = capacityOfCircle - (int)capacityOfCircle; // This value is splitting all angles.

            for (int i = 0; i < stickmansListInCircle.Count; i++)
            {
                float angle = (i * 2 * Mathf.PI + leftOverCount) / (stickmansListInCircle.Count);
                stickmansListInCircle[i].transform.position = transform.position + new Vector3(Mathf.Sin(angle) * stickmanRadius * circleCount, 0, Mathf.Cos(angle) * stickmanRadius * circleCount); // Split stickmans in last circle.
            }
        }


    }
    private void SortLastCircle2()
    {
        if (circleCount > 0 && stickCount > 1)
        {
            List<GameObject> stickmansListInCircle = new List<GameObject>();
            GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
            for (int i = 0; i < stickmanArray.Length; i++)
            {
                if (stickmanArray[i].name == circleCount.ToString())
                {
                    stickmansListInCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
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
    #endregion


    /* private int StickmanCountInLastCircle()
     {
         int count = 0;
         GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
         int stickCount = stickmanArray.Length;
         for (int i = 0; i < stickCount; i++)
         {
             if (stickmanArray[i].name == circleCount.ToString())
             {
                 count++;
             }
         }
         return count;
     }*/

    private int NumberForFillLastCirlce()
    {
        float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
        int capacityOfCircle = (int)(circleLenght / stickmanRadius);

        int number = capacityOfCircle - stickCountInLastCircle;
        return number;
    }

    private float CapacityOfLastCircle()
    {
        float circleLenght = Mathf.PI * 2 * stickmanRadius * circleCount;
        float capacityOfCircle = (circleLenght / stickmanRadius);

        return capacityOfCircle;

    }

    private int CircleCount()
    {
        if (stickmanList.Count > 1)
            circleCount = int.Parse(stickmanList[stickmanList.Count - 1].name);
        else
            circleCount = 1;

        return circleCount;
    }
  

    private void SortLastCircle(int sticmanCountInLastCircle, float radiusOfLastCircle)
    {
        for (int i = 0; i < sticmanCountInLastCircle; i++)
        {
            float angle = (i * 2 * Mathf.PI) / (sticmanCountInLastCircle);
            stickmanList[stickmanList.Count - 1 - i].transform.position = transform.position + new Vector3(Mathf.Sin(angle) * radiusOfLastCircle, 0, Mathf.Cos(angle) * radiusOfLastCircle);
        }
    }
    //private void SortCircle(int circleNumber)
    //{
    //    if (circleNumber > 0 && stickCount > 1)
    //    {
    //        List<GameObject> stickmansListInCircle = new List<GameObject>();
    //        GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
    //        int stickCount = stickmanArray.Length;
    //        for (int i = 0; i < stickCount; i++)
    //        {
    //            if (stickmanArray[i].name == circleNumber.ToString())
    //            {
    //                stickmansListInCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
    //            }
    //        }

    //        float radius = Vector3.Distance(stickmansListInCircle[0].transform.position, transform.position); // Last circles radius.
    //        for (int i = 0; i < stickmansListInCircle.Count; i++)
    //        {
    //            print("ababa");
    //            float angle = (i * 2 * Mathf.PI) / (stickmansListInCircle.Count);
    //            stickmansListInCircle[i].transform.position = transform.position + new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius); // Split stickmans in last circle.
    //        }
    //    }


    //}
    private void SortCircle2(int circleNumber)
    {
        if (circleNumber > 0 && stickCount > 1)
        {
            List<GameObject> stickmansListInCircle = new List<GameObject>();
            GameObject[] stickmanArray = GameObject.FindGameObjectsWithTag("Stickman");
            int stickCount = stickmanArray.Length;
            for (int i = 0; i < stickCount; i++)
            {
                if (stickmanArray[i].name == circleNumber.ToString())
                {
                    stickmansListInCircle.Add(stickmanArray[i].gameObject); // Stickmans in last circle
                }
            }

            float circleLenght = 2 * Mathf.PI * stickmanRadius * circleCount;
            float capacityOfCircle = circleLenght / stickmanRadius;
            int countInt = (int)capacityOfCircle - stickCountInLastCircle;
            float leftOverCount = capacityOfCircle - (int)capacityOfCircle; // This value is splitting all angles.

            for (int i = 0; i < stickmansListInCircle.Count; i++)
            {
                float angle = (i * 2 * Mathf.PI + leftOverCount) / (stickmansListInCircle.Count);
                stickmansListInCircle[i].transform.position = transform.position + new Vector3(Mathf.Sin(angle) * stickmanRadius * circleCount, 0, Mathf.Cos(angle) * stickmanRadius * circleCount); // Split stickmans in last circle.
            }
        }


    }

   




}
