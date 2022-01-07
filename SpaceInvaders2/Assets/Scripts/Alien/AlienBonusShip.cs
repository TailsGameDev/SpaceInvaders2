using UnityEngine;

public class AlienBonusShip : Damageable
{
    [SerializeField]
    private Score score = null;

    [SerializeField]
    private Collider2D col = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Transform brokenAlienBonusShip = null;

    [SerializeField]
    private float averageDelayToSpawnBonusShip = 0.0f;
    [SerializeField]
    private float spawnDelayVariation = 0.0f;
    private float timeToSpawn = 0.0f;

    [SerializeField]
    private Transform rightLimit = null;
    private bool spawnedOnRight;

    [SerializeField]
    private float speed = 0.0f;

    [SerializeField]
    private int pointsToScoreOnDeath = 0;

    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private PauseableAudio pauseableAudio = null;

    private void OnEnable()
    {
        this.timeToSpawn = CalculateTimeToSpawn();
    }
    private void OnDisable()
    {
        SetComponentsEnabled(false);
    }
    private void SetComponentsEnabled(bool enable)
    {
        spriteRenderer.enabled = enable;
        col.enabled = enable;
        pauseableAudio.enabled = enable;

        if (enable)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (spriteRenderer.enabled)
        {
            transform.position += Vector3.right * speed;

            bool hasMovedFarEnough = Mathf.Abs(transform.position.x) > rightLimit.position.x;
            if (hasMovedFarEnough)
            {
                SetComponentsEnabled(false);
            }
        }
        else if (Time.time > timeToSpawn)
        {
            SpawnAndConfigure();
        }
    }
    private void SpawnAndConfigure()
    {
        this.timeToSpawn = CalculateTimeToSpawn();

        // Calculate spawnX
        float rightX = rightLimit.transform.position.x;
        float spawnX = spawnedOnRight ? -rightX : +rightX;
        spawnedOnRight = !spawnedOnRight;

        // Set x position to spawnX
        Vector3 position = transform.position;
        position.x = spawnX;
        transform.position = position;

        SetComponentsEnabled(true);

        // Configure speed so it goes to the right side on Update()
        if (spawnedOnRight)
        {
            speed = -Mathf.Abs(speed);
        }
        else
        {
            speed = +Mathf.Abs(speed);
        }
    }

    private float CalculateTimeToSpawn()
    {
        return Time.time + averageDelayToSpawnBonusShip + Random.Range(-spawnDelayVariation, +spawnDelayVariation);
    }

    public override void Die()
    {
        if (spriteRenderer.enabled)
        {
            SetComponentsEnabled(false);

            score.ScorePoints(pointsToScoreOnDeath);

            Instantiate(brokenAlienBonusShip, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("[AlienBonusShip] What is dead may never die", this);
        }
    }
}
