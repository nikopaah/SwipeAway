using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea;

    public GameObject[] jellyPrefabs; //*
    public AudioClip[] soundsPositive, soundsNegative;
    private AudioClip clip = null;
    public string address;

    public float minSpawnDelay = 0.40f;
    public float maxSpawnDelay = 2f;

    public float minAngle = 50f;
    public float maxAngle = 60f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifeTime = 8f; //Seconds
    public float force { get; set; }

    private void Awake() // Runs automatically when the script is initialized
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable() // Runs when the script is enable
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable() // Rusn when the script is disable
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn() {
        yield return new WaitForSeconds(2f);

        while (enabled) {
            // Choose a prefab
            // ## 0 > Positive/Blue  |  1 > Negative/Red ##
            int type = Random.Range(0, jellyPrefabs.Length);
            GameObject prefab = jellyPrefabs[type];

            // Get a random position and angle to spawn it
            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            Quaternion rotation = Quaternion.Euler(Random.Range(minAngle, maxAngle), 0f, 0f);

            // Create a gameObject
            GameObject jelly = Instantiate(prefab, position, rotation);

            // ## 0 > Positive/Blue  |  1 > Negative/Red ##
            if (type == 0)
            {
                clip = soundsPositive[Random.Range(0, soundsPositive.Length)];
            }
            else 
            {
                clip = soundsNegative[Random.Range(0, soundsNegative.Length)];
            }

            jelly.AddComponent<AudioSource>().PlayOneShot(clip);
    

            // Destroy the jelly after the max life time
            Destroy(jelly, maxLifeTime);

            force = Random.Range(minForce, maxForce);
                                                           // Direction * force
            jelly.GetComponent<Rigidbody>().AddForce(jelly.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
