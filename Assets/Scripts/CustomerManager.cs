using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    public List<Customer> customersToSpawn = new List<Customer>();

    public List<NavPoint> entryPointsLeft, entryPointsRight;

    public float timeBetweenCustomers;
    private float spawnCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        Instantiate(customersToSpawn[Random.Range(0, customersToSpawn.Count)]);

        spawnCounter = timeBetweenCustomers * Random.Range(0.75f, 1.25f);
    }

    public List<NavPoint> GetEntryPoints()
    {
        List<NavPoint> points = new List<NavPoint>();

        if (Random.value < 0.5f)
        {
            points.AddRange(entryPointsLeft);
        }
        else
        {
            points.AddRange(entryPointsRight);
        }

        return points;
    }

    public List<NavPoint> GetExitPoints()
    {
        List<NavPoint> points = new List<NavPoint>();

        if (Random.value < 0.5f)
        {
            points.AddRange(entryPointsLeft);
        }
        else
        {
            points.AddRange(entryPointsRight);
        }

        points.Reverse();

        return points;
    }
}
