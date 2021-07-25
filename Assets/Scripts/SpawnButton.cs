using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [Header("Spawn Parameters")]
    public GameObject spawnItem;
    public GameObject spawnLocation;
    public Button spawnButton;
    public Slider currentSlider;
    public float spawnTime;

    public bool isBuilding = false;

    private Queue<GameObject> unitQueue = new Queue<GameObject>();

    private enum SpawnState { TRAINING, IDLE };
    private SpawnState state = SpawnState.IDLE;

    private Coroutine trainingCoroutine;

    void Start()
    {
        Button btn = spawnButton.GetComponent<Button>();

        btn.onClick.AddListener(TaskOnClick);

        currentSlider.maxValue = spawnTime;
    }

    void TaskOnClick()
    {
        unitQueue.Enqueue(spawnItem.gameObject);

        if (trainingCoroutine == null)
        {
            trainingCoroutine = StartCoroutine(TrainingUnit());
        }
    }

    IEnumerator TrainingUnit()
    {
        isBuilding = true;

        WaitForSeconds trainingTime = new WaitForSeconds(spawnTime);
        state = SpawnState.TRAINING;
        while (unitQueue.Count > 0)
        {
            currentSlider.value += Time.deltaTime;
            yield return trainingTime;
            SpawnUnit(unitQueue.Dequeue());
        }
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
        if (isBuilding)
            currentSlider.value += Time.deltaTime;
    }
}