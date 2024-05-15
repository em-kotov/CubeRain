using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _startRadius;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private ObjectPool<Cube> _pool;
    private float maxLifeTime = 5f;
    private float minLifeTime = 2f;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_cubePrefab),
        actionOnGet: (cube) => Activate(cube),
        actionOnRelease: (cube) => Deactivate(cube),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private Vector3 GetRandomPositionXZ()
    {
        Vector3 randomPositionXZ = Random.insideUnitSphere * _startRadius;
        return new Vector3(randomPositionXZ.x, transform.position.y, randomPositionXZ.z);
    }

    private void Activate(Cube cube)
    {
        cube.transform.position = GetRandomPositionXZ();
        cube.EnteredPlatform += SetDeactivationTime;
        cube.gameObject.SetActive(true);
    }

    private void Deactivate(Cube cube)
    {
        cube.Reset();
        cube.transform.position = transform.position;
        cube.EnteredPlatform -= SetDeactivationTime;
        cube.gameObject.SetActive(false);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void SetDeactivationTime(Cube cube)
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        StartCoroutine(DeactivateRoutine(lifeTime, cube));
    }

    private IEnumerator DeactivateRoutine(float seconds, Cube cube)
    {
        yield return new WaitForSeconds(seconds);

        Deactivate(cube);
    }
}
