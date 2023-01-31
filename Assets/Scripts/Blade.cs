using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;
    private Collider BladeCollider;
    private TrailRenderer BladeTrail;
    private bool slicing = false;

    public Vector3 direction { get; private set; }
    public float minSliceVelocity = 0.01f;
    public float sliceForce = 5f;

    Vector2 startPos, endPos;
    public float timeInterval { get; private set; }
    float touchTimeStart, touchTimeFinish;

    private void Awake()
    {
        mainCamera = Camera.main;
        BladeCollider = GetComponent<Collider>();
        BladeTrail = GetComponentInChildren<TrailRenderer>();
    }

    public void OnEnable()
    {
        StopSlicing();
    }

    public void OnDisable()
    {
        StopSlicing();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { StartSlicing(); }
        else if (slicing) { ContinueSlicing(); }
        else { StopSlicing(); }
    }

    private void StartSlicing()
    {
        Vector3 newPosition = new Vector3();
        if (Input.touchCount > 0)
        {
            float posX = Input.GetTouch(0).position.x;
            float posY = Input.GetTouch(0).position.y;
            newPosition = mainCamera.ScreenToWorldPoint(new Vector3(posX, posY, 15f));
        }
        newPosition.z = mainCamera.transform.position.z + mainCamera.nearClipPlane;
        //transform.position = newPosition;

        slicing = true;
        BladeCollider.enabled = true;
        BladeTrail.enabled = true;
        BladeTrail.Clear();
    }

    private void StopSlicing()
    {
        slicing = false;
        BladeCollider.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition = new Vector3();
        if (Input.touchCount > 0)
        {
            float posX = Input.GetTouch(0).position.x;
            float posY = Input.GetTouch(0).position.y;
            newPosition = mainCamera.ScreenToWorldPoint(new Vector3(posX, posY, 15f));
        }
        

        float velocity = direction.magnitude / Time.deltaTime;
        BladeCollider.enabled = velocity > minSliceVelocity;

        if(newPosition != new Vector3(0,0,0)) transform.position = newPosition;
    }
}
