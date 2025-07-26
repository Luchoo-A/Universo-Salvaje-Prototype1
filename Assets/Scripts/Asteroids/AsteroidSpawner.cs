using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spawnInterval = 5f;
    private float spawnTimer;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        GameObject asteroid = AsteroidPoolManager.Instance.GetAsteroid();
        asteroid.transform.position = GetSpawnPositionOutsideCamera();

        // Establecer como grande y aplicar stats
        AsteroidStats stats = asteroid.GetComponent<AsteroidStats>();
        if (stats != null)
        {
            stats.size = AsteroidStats.AsteroidSize.Big;
            stats.ApplyStats();
        }

        asteroid.SetActive(true);
    }

    Vector2 GetSpawnPositionOutsideCamera()
    {
        Vector2 camPos = cam.transform.position;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        int side = Random.Range(0, 4); // 0 = top, 1 = bottom, 2 = left, 3 = right

        switch (side)
        {
            case 0: // top
                return new Vector2(
                    Random.Range(camPos.x - width / 2f, camPos.x + width / 2f),
                    camPos.y + height / 2f + 1f
                );
            case 1: // bottom
                return new Vector2(
                    Random.Range(camPos.x - width / 2f, camPos.x + width / 2f),
                    camPos.y - height / 2f - 1f
                );
            case 2: // left
                return new Vector2(
                    camPos.x - width / 2f - 1f,
                    Random.Range(camPos.y - height / 2f, camPos.y + height / 2f)
                );
            case 3: // right
                return new Vector2(
                    camPos.x + width / 2f + 1f,
                    Random.Range(camPos.y - height / 2f, camPos.y + height / 2f)
                );
            default:
                return camPos;
        }
    }
}
