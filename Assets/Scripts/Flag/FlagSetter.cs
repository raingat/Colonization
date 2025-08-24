using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _prefab;

    private Base _base;

    private Flag _activeFlag;

    private float _heightPlace;

    private bool _isFlagActive;

    private void Awake()
    {
        _isFlagActive = false;

        _heightPlace = _prefab.transform.position.y;

        _base = GetComponent<Base>();
    }

    public Flag Build(Vector3 point)
    {
        if (_isFlagActive)
        {
            _activeFlag.transform.position = new Vector3(point.x, _heightPlace, point.z);

            return _activeFlag;
        }
        else
        {
            _activeFlag = Instantiate(_prefab, new Vector3(point.x, _heightPlace, point.z), Quaternion.identity);

            _activeFlag.Built += IsBuilt;

            _isFlagActive = true;

            _base.BuildFlag(_activeFlag);

            return _activeFlag;
        }
    }

    private void IsBuilt()
    {
        _activeFlag.Built -= IsBuilt;

        _isFlagActive = false;
    }
}
