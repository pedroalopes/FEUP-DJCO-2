using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDrops : MonoBehaviour
{
    public GameObject smallDrop;
    public GameObject mediumDrop;
    public GameObject bigDrop;
    private GameObject[] dropList;
    public Transform spawnPoint;

    private Vector3 spawnPos;
    private float nextDropTime;
    private float dropRate;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateDrops();
    }

    // Update is called once per frame
    void Update()
    {
        nextDropTime -= Time.deltaTime;

        if (nextDropTime <= 0)
        {
            // Debug.Log("SPAWN");
            SpawnDrop();
            nextDropTime = dropRate;
            setNextDropTime();
            setNextSpawnPos();
        }
    }

    private void InstantiateDrops()
    {
        GameObject drop;
        dropList = new GameObject[3];

        drop = Instantiate(smallDrop);
        drop.SetActive(false);
        dropList[0] = drop;

        drop = Instantiate(mediumDrop);
        drop.SetActive(false);
        dropList[1] = drop;

        drop = Instantiate(bigDrop);
        drop.SetActive(false);
        dropList[2] = drop;

    }

    private void SpawnDrop()
    {
        GameObject drop = dropList[Random.Range(0, 3)];

        drop.SetActive(true);
        drop.transform.position = spawnPos;
        drop.transform.rotation = Quaternion.identity;
        drop.GetComponent<onDropCollision>().SpawnObject();
    }

    private void setNextDropTime()
    {
        nextDropTime = Time.deltaTime + Random.Range(2f, 3f);
    }

    private void setNextSpawnPos()
    {
        spawnPos = spawnPoint.position;
    }
}
