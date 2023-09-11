using UnityEngine;
using Unity.VisualScripting;

public class Jelly : MonoBehaviour
{
    public GameObject jelly;

    private Rigidbody jellyRigidbody;
    private Collider jellyCollider;
    private ParticleSystem juiceParticleEffect;
    private AudioSource juiceAudio;

    public float sizeChange = 0.0025f;

    [Range(0.05f, 1f)]
    public float throwForce = 0.3f;

    private void Awake() // Runs automatically when the script is initialized
    {
        jellyCollider = GetComponent<Collider>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        juiceAudio = GetComponent<AudioSource>();
    }

    private void Update() // Runs when the script is enable
    {
        ChangeSize(jelly);
    }

    private void ChangeSize(GameObject jelly)
    {
        jelly.transform.localScale += new Vector3(sizeChange, 0f, sizeChange);
    }


    private void CollectJelly(Vector3 direction, Vector3 position, float force, float timeInterval) {
        jellyCollider.enabled = false;
        juiceParticleEffect.Play();

        jellyRigidbody = GetComponent<Rigidbody>();
        jellyRigidbody.AddForce((-direction / timeInterval * throwForce), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            print("got itt");
            Blade blade = other.GetComponent<Blade>();
            CollectJelly(blade.direction, blade.transform.position, blade.sliceForce, blade.timeInterval);
        }
    }
}
