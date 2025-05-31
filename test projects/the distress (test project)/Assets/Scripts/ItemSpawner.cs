using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject HealPrefab;
    private List<GameObject> HealObjList = new List<GameObject>();
    public GameObject HurtPrefab;
    private List<GameObject> HurtObjList = new List<GameObject>();
    public int maxItems;    //how many items of each type are allowed at once
    public float spawnDelay;
    private float nextSpawnTime;

    private void Start()
    {
        maxItems = 2;
        spawnDelay = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if we have reached next spawn time, decide whether to
        //spawn a hurt or heal item
        if(Time.time > nextSpawnTime)
        {
            //decide what to spawn
            CoinFlip();
            //find next spawn time
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    //decide whether to spawn a hurt or heal and call to spawn it
    void CoinFlip()
    {
        //choose random number
        int coin = Random.Range(0, 100);

        if (coin % 2 == 0)
            StartCoroutine(SpawnHeal());
        else
            StartCoroutine(SpawnHurt());
    }

    //spawns an item using heal prefab
    IEnumerator SpawnHeal()
    {
        //generate random x and y within canvas
        float xRand = Random.Range(-7.5f, 7.5f);
        float yRand = Random.Range(-4.5f, 4.5f);
        var position = new Vector2(xRand, yRand);

        HealObjList.Add((GameObject)Instantiate(HealPrefab, position, Quaternion.identity));

        //if max number are already on screen, remove oldest item
        if (HealObjList.Count > maxItems)
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(HealObjList[0]);
            HealObjList.RemoveAt(0);
        }
    }

    //spawns an item using hurt prefab
    IEnumerator SpawnHurt()
    {
        //generate random x and y within canvas
        float xRand = Random.Range(-7.5f, 7.5f);
        float yRand = Random.Range(-4.5f, 4.5f);
        var position = new Vector2(xRand, yRand);        

        HurtObjList.Add((GameObject)Instantiate(HurtPrefab, position, Quaternion.identity));

        //if max number are already on screen, remove oldest item
        if (HurtObjList.Count > maxItems)
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(HurtObjList[0]);
            HurtObjList.RemoveAt(0);
        }
    }
}
