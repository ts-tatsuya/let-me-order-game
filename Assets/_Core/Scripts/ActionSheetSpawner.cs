using System.Collections.Generic;
using UnityEngine;

public class ActionSheetSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private RectTransform _spawnArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBurstInArea(20);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Onable()
    {

    }

    public GameObject SpawnPrefabInArea()
    {
        return SpawnPrefabInArea(_spawnArea);
    }

    public GameObject SpawnPrefabInArea(RectTransform spawnArea)
    {
        if (_prefab == null || spawnArea == null)
        {
            return null;
        }

        var position = GetRandomLocalPositionInsideRect(spawnArea);
        var worldPosition = spawnArea.TransformPoint(position);
        var instance = Instantiate(_prefab, transform, false);
        instance.transform.position = worldPosition;

        var rectTransform = instance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = worldPosition;
        }

        return instance;
    }

    public GameObject[] SpawnBurstInArea(int count, float minDistance = 100f)
    {
        return SpawnBurstInArea(count, minDistance, _spawnArea);
    }

    public GameObject[] SpawnBurstInArea(int count, float minDistance, RectTransform spawnArea)
    {
        if (_prefab == null || spawnArea == null || count <= 0)
        {
            return System.Array.Empty<GameObject>();
        }

        var instances = new List<GameObject>(count);
        var usedPositions = new List<Vector2>(count);

        for (int i = 0; i < count; i++)
        {
            var position = GetScatteredPositionInsideRect(spawnArea, usedPositions, minDistance, 20);
            var worldPosition = spawnArea.TransformPoint(position);
            var instance = Instantiate(_prefab, transform, false);
            instance.transform.position = worldPosition;

            var rectTransform = instance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.position = worldPosition;
            }

            usedPositions.Add(position);
            instances.Add(instance);
        }

        return instances.ToArray();
    }

    private static Vector2 GetScatteredPositionInsideRect(RectTransform rectTransform, List<Vector2> existingPositions, float minDistance, int maxAttempts)
    {
        var rect = rectTransform.rect;
        var bestPosition = GetRandomLocalPositionInsideRect(rectTransform);
        var bestMinDistance = -1f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var candidate = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
            var closestDistance = float.MaxValue;

            for (int j = 0; j < existingPositions.Count; j++)
            {
                closestDistance = Mathf.Min(closestDistance, Vector2.Distance(candidate, existingPositions[j]));
            }

            if (closestDistance >= minDistance)
            {
                return candidate;
            }

            if (closestDistance > bestMinDistance)
            {
                bestMinDistance = closestDistance;
                bestPosition = candidate;
            }
        }

        return bestPosition;
    }

    private static Vector2 GetRandomLocalPositionInsideRect(RectTransform rectTransform)
    {
        var rect = rectTransform.rect;
        var x = Random.Range(rect.xMin, rect.xMax);
        var y = Random.Range(rect.yMin, rect.yMax);
        return new Vector2(x, y);
    }
}
