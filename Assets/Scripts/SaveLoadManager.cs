using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int health, hunger, sanity;
    public int currentDay, food, collectibles, medicine, actionPoints;
    public string currentRegionId;
    public List<string> unlockedRegions = new();
    public string saveTime;
}

public static class SaveLoadManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame(GameManager gm, RegionMapUIController mapCtrl)
    {
        if (gm == null) { Debug.LogError("[Save] GameManager 为空"); return; }

        var data = new SaveData
        {
            health = gm.Health,
            hunger = gm.Hunger,
            sanity = gm.Sanity,
            currentDay = gm.CurrentDay,
            food = gm.Food,
            collectibles = gm.Collectibles,
            medicine = gm.Medicine,
            actionPoints = gm.ActionPoints,
            currentRegionId = (mapCtrl && mapCtrl.currentRegion) ? mapCtrl.currentRegion.regionId : null,
            saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        // 以后有“已探索/解锁”标记时再按条件加；先全量保存 ID 作为示例
        if (mapCtrl != null)
            foreach (var r in mapCtrl.allRegions) if (r) data.unlockedRegions.Add(r.regionId);

        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        Debug.Log($"[Save] 存档完成：{SavePath}");
    }

    public static bool LoadGame(GameManager gm, RegionMapUIController mapCtrl)
    {
        if (!File.Exists(SavePath)) { Debug.LogWarning("[Load] 无存档"); return false; }

        var json = File.ReadAllText(SavePath);
        var data = JsonUtility.FromJson<SaveData>(json);
        if (data == null) { Debug.LogError("[Load] JSON 解析失败"); return false; }

        gm?.ApplySaveData(
            data.health, data.hunger, data.sanity,
            data.currentDay, data.food, data.collectibles, data.medicine, data.actionPoints
        );

        if (mapCtrl != null && !string.IsNullOrEmpty(data.currentRegionId))
        {
            var region = mapCtrl.allRegions.Find(r => r && r.regionId == data.currentRegionId);
            if (region) mapCtrl.currentRegion = region;
            mapCtrl.SetHover(null); // 刷一遍颜色
        }

        Debug.Log($"[Load] 读档完成：{SavePath}（{data.saveTime}）");
        return true;
    }
}
