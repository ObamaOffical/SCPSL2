using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // needed for checking if the player is looking in the direction of the enemy
    new Camera camera;
    Plane[] cameraFrustum;
    new Collider collider;

    [Header("Enemy Aggression")]
    [SerializeField] private PlayerStat playerstat;
    [SerializeField] private float killDistance = 4f;


    [Header("Movement and Tracking")]
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private Transform ray1;
    [SerializeField] private Transform ray2;
    [SerializeField] private Transform ray3;
    [SerializeField] private Transform ray4;

    [Header("Wall Detection")]
    [SerializeField] private LayerMask wallLayer; // Layer for walls
    [SerializeField] private bool DisplayRaycasts = true;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip movingSound;
    private AudioSource audioSource;

    [Header("Sound Volume")]
    [Range(0, 100)]
    [SerializeField] private float maxVolume = 50f; // Maximum volume from 0-100
    [SerializeField] private float maxSoundDistance = 10f; // Distance at which sound becomes inaudible

    [Header("Stun Mechanics")]
    private bool canSeePlayer = false;

    void Start()
    {
        camera = Camera.main;
        collider = GetComponent<Collider>();

        // Ensure we have an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Plugs in the moving sound for the enemy
        audioSource.clip = movingSound;
    }

    void Update()
    {

        // Update volume based on proximity
        UpdateProximityVolume();

        // Check for walls to see if the player can see the enemy
        CheckPlayerVisibility();

        // Check if enemy is seen
        CheckStunCondition();

        // Move only if not stunned
        if (!enemy.isStopped)
        {
            MoveTowardsPlayer();
            ProximityAggression();
        }
    }

    void UpdateProximityVolume()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float proximityVolume = Mathf.Lerp(maxVolume, 0, Mathf.Clamp01(distance / maxSoundDistance));
        audioSource.volume = proximityVolume / 100f;
    }

    void CheckPlayerVisibility()
    {
        Transform[] rayTransforms = { ray1, ray2, ray3, ray4 };
        bool[] rayVisibilities = new bool[rayTransforms.Length];

        for (int i = 0; i < rayTransforms.Length; i++)
        {
            Vector3 directionToPlayer = player.position - rayTransforms[i].position;
            float distanceToPlayer = directionToPlayer.magnitude;

            rayVisibilities[i] = !Physics.Raycast(
                rayTransforms[i].position,
                directionToPlayer,
                distanceToPlayer,
                wallLayer
            );

            if (DisplayRaycasts)
            {
                Debug.DrawRay(rayTransforms[i].position, directionToPlayer, rayVisibilities[i] ? Color.green : Color.red);
            }
        }

        canSeePlayer = rayVisibilities.Any(visibility => visibility);
    }

    void MoveTowardsPlayer()
    {
        // Set destination to player
        enemy.SetDestination(player.position);

        // Sound player, checks velocity so it doesn't play when it's not moving and not looked at
        if (enemy.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void CheckStunCondition()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(camera);
        bool isInCameraView = GeometryUtility.TestPlanesAABB(cameraFrustum, collider.bounds);

        enemy.isStopped = isInCameraView && canSeePlayer;

        if (enemy.isStopped)
        {
            audioSource.Stop();
        }
    }

    void ProximityAggression()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < killDistance)
        {
            playerstat.TakeDamage(1);
        }
    }
}