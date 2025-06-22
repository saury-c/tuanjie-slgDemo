using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager
{
    public static readonly GameConfigManager Instance = new GameConfigManager();

    private readonly Dictionary<ConfigType, GameConfigData> _configs = new();

    public void Init()
    {
        LoadConfig(ConfigType.Card);
        LoadConfig(ConfigType.Enemy);
        LoadConfig(ConfigType.Level);
        LoadConfig(ConfigType.CardType);
        LoadConfig(ConfigType.EnemyAction);
    }

    private void LoadConfig(ConfigType type)
    {
        var asset = Resources.Load<TextAsset>($"Data/{type.ToString().ToLower()}");
        _configs[type] = new GameConfigData(asset?.text ?? string.Empty);
    }

    public List<Dictionary<string, string>> GetLines(ConfigType type) => _configs[type].GetLines();
    public Dictionary<string, string> GetById(ConfigType type, string id) => _configs[type].GetOneById(id);
}

public enum ConfigType
{
    Card,
    Enemy,
    Level,
    CardType,
    EnemyAction
}