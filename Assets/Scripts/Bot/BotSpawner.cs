using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : GenericSpawner<Bot>
{
    [SerializeField] private List<Transform> _places;

    [SerializeField] private float _heightSpawn;

    [SerializeField] private BaseSpawner _baseSpawner;

    private List<Transform> _busyPlaces = new();

    public void Initialize(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }

    protected override void HandleObject(Bot bot)
    {
        int randomPlace = Random.Range(0, _places.Count);

        bot.transform.position = new Vector3(_places[randomPlace].position.x, _heightSpawn, _places[randomPlace].position.z);

        bot.Initialize(_baseSpawner);

        _busyPlaces.Add(_places[randomPlace]);

        _places.RemoveAt(randomPlace);

        if (_places.Count == 0)
        {
            _places.AddRange(_busyPlaces);

            _busyPlaces.Clear();
        }
    }
}
