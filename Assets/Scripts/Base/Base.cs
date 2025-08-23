using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StorageFoundResources))]
[RequireComponent(typeof(FlagSetter))]
public class Base : MonoBehaviour
{
    [SerializeField] private GenericSpawner<Bot> _spawner;

    [SerializeField] private BotRetreiver _botRetreiver;

    [SerializeField] private int _startBotCount;
    [SerializeField] private int _maxBotCount;

    private List<Bot> bots = new();

    private Queue<Bot> _enableBot = new();

    private Warehouse _warehouse;

    private StorageFoundResources _storageFoundResources;

    private FlagSetter _flagSetter;
    private Flag _flag;

    private int _currentCountBot;

    private int _costCreateBot = 3;
    private int _costCreateBase = 5;

    private bool _isBuildNewBase;

    private bool CanCreateBot => _warehouse.CurrentResourceCount >= _costCreateBot && _currentCountBot < _maxBotCount;
    private bool CanCreateNewBase => _warehouse.CurrentResourceCount >= _costCreateBase;

    private void Awake()
    {
        _isBuildNewBase = false;

        _flagSetter = GetComponent<FlagSetter>();
        _warehouse = GetComponent<Warehouse>();
        _storageFoundResources = GetComponent<StorageFoundResources>();
    }

    private void OnEnable()
    {
        _botRetreiver.BotCome += HandleDelivery;
    }

    private void Start()
    {
        CreateBot(_startBotCount);
    }

    private void Update()
    {
        if (_isBuildNewBase)
        {
            if (CanCreateNewBase && _enableBot.Count > 0 && _currentCountBot > 1)
            {
                Bot bot = _enableBot.Dequeue();

                _warehouse.RemoveResource(_costCreateBase);

                bot.BuildNewBase(_flag);

                _isBuildNewBase = false;

                _flag = null;
            }
        }

        if (CanCreateBot && _isBuildNewBase == false)
        {
            _warehouse.RemoveResource(_costCreateBot);

            CreateBot();
        }

        Work();
    }

    private void OnDisable()
    {
        _botRetreiver.BotCome -= HandleDelivery;
    }

    public void GetBot(Bot bot)
    {
        _currentCountBot++;

        bot.SetCollectionPoint(_botRetreiver.transform);

        bots.Add(bot);

        _enableBot.Enqueue(bot);
    }

    public bool IsMineBot(Bot bot)
    {
        return bots.Contains(bot);
    }
    public void BuildFlag(Vector3 point)
    {
        _flag = _flagSetter.Build(point);

        _isBuildNewBase = true;
    }

    private void Work()
    {
        if (_enableBot.Count == 0)
            return;

        Resource resource = _storageFoundResources.GetResource();

        if (resource == null)
            return;

        Bot bot = _enableBot.Dequeue();

        bot.CollectResource(resource);
    }

    private void HandleDelivery(Bot bot)
    {
        Resource resource = bot.GiveAwayResource();

        _enableBot.Enqueue(bot);

        _warehouse.AddResource(resource);

        _storageFoundResources.RemoveResource(resource);

        resource.Collect();
    }

    private void CreateBot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_currentCountBot < _startBotCount)
            {
                Bot bot = _spawner.Spawn();

                bot.SetCollectionPoint(_botRetreiver.transform);

                bot.TransferringToNewBase += TransferToNewBase;

                _enableBot.Enqueue(bot);

                bots.Add(bot);

                _currentCountBot++;
            }
        }
    }

    private void CreateBot()
    {
        if (_currentCountBot < _maxBotCount)
        {
            Bot bot = _spawner.Spawn();

            bot.SetCollectionPoint(_botRetreiver.transform);

            bot.TransferringToNewBase += TransferToNewBase;

            _enableBot.Enqueue(bot);

            bots.Add(bot);

            _currentCountBot++;
        }
    }

    private void TransferToNewBase(Bot bot, Base newBase)
    {
        bot.TransferringToNewBase -= TransferToNewBase;

        bots.Remove(bot);

        newBase.GetBot(bot);
    }
}
