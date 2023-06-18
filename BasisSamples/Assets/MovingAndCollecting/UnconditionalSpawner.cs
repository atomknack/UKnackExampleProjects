using UKnack.Attributes;
using UnityEngine;

public class UnconditionalSpawner : MonoBehaviour
{
    [System.Serializable]
    private struct RandRange
    {
        [SerializeField]
        float _min;
        [SerializeField]
        float _max;
        public float GetRand() => 
            _min>_max ? 
                throw new System.InvalidOperationException($"_min should be less or equal to _max") : 
                Random.Range(_min, _max);
    }

    [SerializeField]
    [MarkNullAsColor(0.4f, 0.4f, 0.2f, "if null will be instantiated in root of scene")]
    private Transform _newParent;

    [SerializeField]
    [ValidReference]
    private GameObject _spawn;

    [SerializeField]
    private RandRange _timeBetweenSpawns;

    private float _timeToSpawn;
    private void Awake()
    {
        if ( _spawn == null )
            throw new System.ArgumentNullException(nameof( _spawn ));
    }

    private void FixedUpdate()
    {
        if (_timeToSpawn < 0)
        {
            if (_newParent != null)
                Instantiate(_spawn, transform.position, transform.rotation, _newParent);
            else
                Instantiate(_spawn, transform.position, transform.rotation);
            _timeToSpawn = _timeBetweenSpawns.GetRand();
        }
        _timeToSpawn -= Time.deltaTime;
    }
}
