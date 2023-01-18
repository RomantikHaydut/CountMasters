using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public int myIndex;

    public float myCapacity;

    public List<GameObject> myStickmans = new List<GameObject>();

    private GameObject player;

    private GameObject stickmanPrefab;

    private float stickmanRadius;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        stickmanPrefab = FindObjectOfType<CloneController>().stickmanPrefab;
        CapsuleCollider capsuleCollider = stickmanPrefab.GetComponentInChildren(typeof(CapsuleCollider)) as CapsuleCollider;
        stickmanRadius = capsuleCollider.radius;
    }


    public bool HaveStickman()
    {
        if (ActiveStickmansCount() > 0)
            return true;
        else
            return false;
    }
    public bool CircleFull()
    {
        if (ActiveStickmansCount() < (int)myCapacity)
            return false;
        else
            return true;
    }

    public List<GameObject> ActiveStickmanList()
    {
        List<GameObject> activeStickmanList = new List<GameObject>();
        for (int i = 0; i < myStickmans.Count; i++)
        {
            if (myStickmans[i].activeInHierarchy)
                activeStickmanList.Add(myStickmans[i]);
        }
        return activeStickmanList;
    }

    public List<GameObject> InactiveStickmanList()
    {
        List<GameObject> activeStickmanList = new List<GameObject>();
        for (int i = 0; i < myStickmans.Count; i++)
        {
            if (!myStickmans[i].activeInHierarchy)
                activeStickmanList.Add(myStickmans[i]);
        }
        return activeStickmanList;
    }
    public int ActiveStickmansCount()
    {
        int activeStickmanCount = 0;
        for (int i = 0; i < myStickmans.Count; i++)
        {
            if (myStickmans[i].activeInHierarchy)
                activeStickmanCount++;
        }

        return activeStickmanCount;
    }

    public int InactiveStickmansCount()
    {
        int inActiveStickmanCount = 0;
        for (int i = 0; i < myStickmans.Count; i++)
        {
            if (!myStickmans[i].activeInHierarchy)
                inActiveStickmanCount++;
        }

        return inActiveStickmanCount;
    }

    public void PlaceMyStickmans()
    {
        for (int i = 0; i < ActiveStickmanList().Count; i++)
        {
            StartCoroutine(PlaceStickman(ActiveStickmanList()[i], i));
        }
    }

    private IEnumerator PlaceStickman(GameObject stickman, int number)
    {
        float speed = 4f;
        float timer = 0.0f;
        float stopTime = 1f;
        float leftOverCount = myCapacity - (int)myCapacity; // This value is splitting all angles.
        float angle = (number * 2 * Mathf.PI + leftOverCount) / ActiveStickmansCount();

        Vector3 targetPos = new Vector3(Mathf.Sin(angle) * stickmanRadius * myIndex, 0, Mathf.Cos(angle) * stickmanRadius * myIndex);
        Vector3 startPos = stickman.transform.position;
        while (true)
        {
            yield return null;
            if (timer >= stopTime)
            {
                stickman.transform.position = player.transform.position + targetPos;
                yield break;
            }
            timer += Time.deltaTime * speed;
            stickman.transform.position = Vector3.Lerp(startPos, player.transform.position + targetPos, timer);
        }
    }
}
