using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _startRadius;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private ObjectPool<GameObject> _pool;
    private float maxLifeTime = 5f;
    private float minLifeTime = 2f;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => ActionOnRelease(obj),
        actionOnDestroy: (obj) => Destroy(obj),
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

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomPositionXZ();
        obj.GetComponent<Cube>().EnteredPlatform += Deactivate;
        obj.SetActive(true);
    }

    private void ActionOnRelease(GameObject obj)
    {
        Cube cube = obj.GetComponent<Cube>();
        cube.Reset();
        cube.EnteredPlatform -= Deactivate;
        obj.transform.position = this.transform.position;
        obj.SetActive(false);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void Deactivate(Cube cube)
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        StartCoroutine(DeactivateRoutine(lifeTime, cube));
    }

    private IEnumerator DeactivateRoutine(float seconds, Cube cube)
    {
        yield return new WaitForSeconds(seconds);

        ActionOnRelease(cube.gameObject);
    }
}
