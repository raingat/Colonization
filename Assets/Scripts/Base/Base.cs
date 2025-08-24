using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private BotRetreiver _botRetreiver;

    [SerializeField] private int _startBotCount;
    [SerializeField] private int _maxBotCount;

    [SerializeField] private StorageFoundResources _storageFoundResources;

    private List<Bot> _bots = new();

    private Queue<Bot> _enableBot = new();

    private Scanner _scanner;

    private BotSpawner _botSpawner;

    private Warehouse _warehouse;

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

        _scanner = GetComponent<Scanner>();
        _botSpawner = GetComponent<BotSpawner>();
        _warehouse = GetComponent<Warehouse>();
    }

    private void OnEnable()
    {
        _botRetreiver.BotCome += HandleDelivery;
    }

    private void Start()
    {
        CreateMultiplyBot(_startBotCount);
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

            TryCreateBot();
        }

        Work();
    }

    private void OnDisable()
    {
        _botRetreiver.BotCome -= HandleDelivery;
    }

    public void Initialize(StorageFoundResources storageFoundResources, BaseSpawner baseSpawner)
    {
        _storageFoundResources = storageFoundResources;
        _scanner.Initialize(storageFoundResources);
        _botSpawner.Initialize(baseSpawner);
    }

    public void GetBot(Bot bot)
    {
        _currentCountBot++;

        bot.SetCollectionPoint(_botRetreiver.transform);

        _bots.Add(bot);

        _enableBot.Enqueue(bot);
    }

    public bool IsMineBot(Bot bot)
    {
        return _bots.Contains(bot);
    }

    public void BuildFlag(Flag flag)
    {
        _flag = flag;

        _isBuildNewBase = true;
    }

    private void Work()
    {
        if (_enableBot.Count == 0)
            return;

        Resource resource = _storageFoundResources.GetResource();

        if (resource == null)
            return;

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

    private void CreateMultiplyBot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            TryCreateBot();
        }
    }

    private void TryCreateBot()
    {
        if (_currentCountBot < _maxBotCount)
        {
            Bot bot = CreateBot();

            HandleBot(bot);

            _currentCountBot++;
        }
    }

    private Bot CreateBot()
    {
        Bot bot = _botSpawner.Spawn();

        _enableBot.Enqueue(bot);

        _bots.Add(bot);

        return bot;
    }

    private void HandleBot(Bot bot)
    {
        bot.SetCollectionPoint(_botRetreiver.transform);

        bot.TransferringToNewBase += TransferToNewBase;
    }

    private void TransferToNewBase(Bot bot, Base newBase)
    {
        bot.TransferringToNewBase -= TransferToNewBase;

        _bots.Remove(bot);

        newBase.GetBot(bot);
    }
}
