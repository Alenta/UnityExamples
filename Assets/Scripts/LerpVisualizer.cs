using System;
using System.Collections.Generic;
using UnityEngine;

// Attachable MonoBehaviour to compare different movement interpolations

public enum InterpolationType { ExponentialDecay, LerpFixedDelta, LerpDeltaTime, MoveTowards }

public class LerpVisualizer : MonoBehaviour
{
    public GameObject MainObject;

    [Serializable]
    public class FollowTask
    {   // FollowTask is a class that holds a GameObject that will follow the MainObject. They are created in the inspector and assigned values there.
        // It uses the assigned interpolationtype to pick which function should be assigned to the Follow func. 
        public GameObject _Object;
        public Color _Color;
        public InterpolationType Interpolation;
        public Func<Vector3, Vector3> Follow;
    }

    // Main array of followTasks
    [SerializeField]
    private FollowTask[] followTasks;

    // Getter for the list of FollowTasks LerpUI needs to update UI labels, list should not be modified
    public IReadOnlyList<FollowTask> GetFollowTasks() { return Array.AsReadOnly(followTasks); }

    // This is Instantiated and used as the 'default' object if nothing is assigned in inspector
    [SerializeField]
    private GameObject FollowObjectPrefab;

    // Speed of MainCube
    [SerializeField]
    private float Speed = 1f;

    // Speed of all interpolations (Speed of the followObjects)
    [SerializeField]
    private float LerpSpeed = 0.1f;

    // Settings for FPS
    [SerializeField]
    private bool UseFPSLimit = false;
    [SerializeField]
    [Range(1,200)]
    private int FPSLimit = 30;
    [SerializeField]
    private bool UseRandomFPS = false;
    [SerializeField]
    [Range(1, 200)]
    private int RandomFPSMin;
    [SerializeField]
    [Range(1, 200)]
    private int RandomFPSMax;
    [SerializeField]
    private float RandomizationTime;

    // Input Movement Direction (WASD)
    protected Vector2 _inputDir = Vector2.zero;
    protected int _fpsLimit = 30;
    protected float timeSinceFrame = 0;
    protected float timeSinceRandomization = 0;
    protected Rigidbody2D rb;

    void Start() {
        rb = MainObject.GetComponent<Rigidbody2D>();
        InitializeTasks();      
    }

    private void Update() {
        // ==============INPUT HANDLING=================== //
        HandleInput();
        UpdatePositions();
    }

    private void FixedUpdate() {
        // ==============Physics / Motion=================== //
        if (_inputDir == Vector2.zero) { // Movement dampening
            rb.velocity = ExpDecay(rb.velocity, Vector2.zero, Speed*2, Time.deltaTime);
        }

        rb.AddForce(100 * Speed * Time.deltaTime * _inputDir);
    }

    void HandleInput() {
        if (Input.GetAxis("Vertical") != 0){
            _inputDir.y = Input.GetAxis("Vertical");
        }
        else _inputDir.y = 0;

        if (Input.GetAxis("Horizontal") != 0) {
            _inputDir.x = Input.GetAxis("Horizontal");
        }
        else _inputDir.x = 0;

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (UseRandomFPS) UseRandomFPS = false;
            else UseRandomFPS = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            if (UseRandomFPS) UseFPSLimit = false;
            if (UseFPSLimit) UseFPSLimit = false;
            else UseFPSLimit = true;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            if(FPSLimit < 200) FPSLimit += 1;
            else FPSLimit -= 1;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            if(FPSLimit > 1) FPSLimit -= 1;
            else FPSLimit = 1;
        }
    }

    void UpdatePositions()
    {
        if (UseFPSLimit || UseRandomFPS) {

            if (timeSinceFrame >= 1 / (float)_fpsLimit) {

                if (UseRandomFPS) {

                    if (timeSinceRandomization >= RandomizationTime) {
                        _fpsLimit = UnityEngine.Random.Range(RandomFPSMin, RandomFPSMax);
                        timeSinceRandomization = 0;
                    }
                }
                if (UseFPSLimit) _fpsLimit = FPSLimit;

                foreach (FollowTask task in followTasks) {
                    task._Object.transform.position = task.Follow(task._Object.transform.position);
                }

                timeSinceFrame = Time.deltaTime;
            }
            else timeSinceFrame += Time.deltaTime;
            if (UseRandomFPS) timeSinceRandomization += Time.deltaTime;
        }
        else {
            timeSinceFrame = 0;
            foreach (FollowTask task in followTasks) {
                task._Object.transform.position = task.Follow(task._Object.transform.position);
            }
        }
    }

    private void InitializeTasks() {
        for (int i = 0; i < followTasks.Length; i++) {

            if (followTasks[i]._Object == null) {   
                followTasks[i]._Object = Instantiate(FollowObjectPrefab, MainObject.transform.position, Quaternion.identity);
                followTasks[i]._Object.GetComponent<Renderer>().material.color = followTasks[i]._Color;
            }

            switch (followTasks[i].Interpolation) {
                case InterpolationType.ExponentialDecay:
                    followTasks[i].Follow = ExponentialDecay;
                    break;
                case InterpolationType.LerpFixedDelta:
                    followTasks[i].Follow = FramedependantSmoothingLerp;
                    break;
                case InterpolationType.LerpDeltaTime:
                    followTasks[i].Follow = SmoothingLerp;
                    break;
                case InterpolationType.MoveTowards:
                    followTasks[i].Follow = MoveTowardsExample;
                    break;
            }

        }
    }
    Vector3 FramedependantSmoothingLerp(Vector3 startPos) {
        return Vector3.Lerp(startPos, MainObject.transform.position, LerpSpeed * Time.fixedDeltaTime);
    }

    Vector3 SmoothingLerp(Vector3 startPos) {
        return Vector3.Lerp(startPos, MainObject.transform.position, LerpSpeed * GetDeltaTime());
    }

    Vector3 ExponentialDecay(Vector3 startPos) {
        return MainObject.transform.position + (startPos - MainObject.transform.position) * Mathf.Exp(-LerpSpeed * GetDeltaTime());
    }

    Vector3 MoveTowardsExample(Vector3 startPos) {
        return Vector3.MoveTowards(startPos, MainObject.transform.position, 5 * LerpSpeed * GetDeltaTime());
    }

    Vector3 ExpDecay(Vector3 start, Vector3 end, float stepValue, float dt) {
        return end + (start - end) * Mathf.Exp(-stepValue * dt);
    }
    
    float GetDeltaTime() {
        if (timeSinceFrame == 0) return Time.deltaTime;
        else return timeSinceFrame;
    }

}
