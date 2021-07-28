using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnQue : MonoBehaviour
{
    Slider currentSlider;
    float spawnTime;
    Transform spawnLocation;

    public bool isBuilding = false;

    [Header("Spawn Parameter: Capsule")]
    public GameObject spawnCapsuleItem;
    public GameObject capsuleSpawnLocation;
    public Button capsuleSpawnButton;
    public Slider capsuleSlider;
    public float capsuleSpawnTime;

    [Header("Spawn Parameter: Cube")]
    public GameObject spawnCubeItem;
    public GameObject cubeSpawnLocation;
    public Button cubeSpawnButton;
    public Slider cubeSlider;
    public float cubeSpawnTime;

    [Header("Spawn Parameter: Sphere")]
    public GameObject spawnSphereItem;
    public GameObject sphereSpawnLocation;
    public Button sphereSpawnButton;
    public Slider sphereSlider;
    public float sphereSpawnTime;


    private Queue<GameObject> unitQueue = new Queue<GameObject>();

    private enum SpawnState { TRAINING, IDLE };
    private SpawnState state = SpawnState.IDLE;

    private Coroutine trainingCoroutine;

    void Start()
    {
        capsuleSpawnButton.onClick.AddListener(() => TaskOnClick(spawnCapsuleItem));
        cubeSpawnButton.onClick.AddListener(() => TaskOnClick(spawnCubeItem));
        sphereSpawnButton.onClick.AddListener(() => TaskOnClick(spawnSphereItem));

        //currentSlider.maxValue = spawnTime;
    }

    void TaskOnClick(GameObject item)
    {
        unitQueue.Enqueue(item);

        if (trainingCoroutine == null)
        {
            //trainingCoroutine = StartCoroutine(TrainingUnit(item));
        }
    }

    void StartTrainingUnit(GameObject item)
    {
        isBuilding = true;

        if (item.tag == "Capsule")
        {
            spawnTime = capsuleSpawnTime;
            currentSlider = capsuleSlider;
            spawnLocation = capsuleSpawnLocation.transform;
        }
        else if (item.tag == "Cube")
        {
            spawnTime = cubeSpawnTime;
            currentSlider = cubeSlider;
            spawnLocation = cubeSpawnLocation.transform;
        }
        else if (item.tag == "Sphere")
        {
            spawnTime = sphereSpawnTime;
            currentSlider = sphereSlider;
            spawnLocation = sphereSpawnLocation.transform;
        }

        currentSlider.maxValue = spawnTime;
        StartCoroutine(TrainingUnit(item));
    }

    IEnumerator TrainingUnit(GameObject item)
    {
        yield return new WaitForSeconds(spawnTime);
        state = SpawnState.TRAINING;
        SpawnUnit(item);
        currentSlider.value = 0;
        state = SpawnState.IDLE;
        trainingCoroutine = null;
        isBuilding = false;
    }

    void SpawnUnit(GameObject spawnUnit)
    {
        Instantiate(spawnUnit, spawnLocation.transform.position, spawnLocation.transform.rotation);
    }

    private void Update()
    {
        if (unitQueue.Count > 0 && !isBuilding)
            StartTrainingUnit(unitQueue.Dequeue());

        if (isBuilding)
            currentSlider.value += Time.deltaTime;
    }
}