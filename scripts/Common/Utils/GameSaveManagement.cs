using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameSaveManagement
{
    public static void SaveGame(string saveFileName)
    {
        var now = DateTime.Now;
        var saveGame = new File();
        saveGame.Open($"user://{now.Ticks}.sav", File.ModeFlags.Write);

        var mainGameStateData = MainGameState.Instance.AllData;
        saveGame.StoreLine(JsonConvert.SerializeObject(mainGameStateData));

        saveGame.Close();

        var metaAsRead = new File();
        metaAsRead.Open($"user://meta.sav", File.ModeFlags.Read);
        var metaText = metaAsRead.GetAsText();
        metaAsRead.Close();

        var metaInfo = JsonConvert.DeserializeObject<MetaGameInfo>(metaText);
        if(metaInfo == null)
        {
            metaInfo = new MetaGameInfo();
        }
        var existingSave = metaInfo.GameSaves.FirstOrDefault(x => x.Name == saveFileName);
        if(existingSave != null)
        {
            metaInfo.GameSaves.Remove(existingSave);
            var dir = new Directory();
            dir.Remove($"user://{existingSave.FilePath}");
        }
        var newSave = new GameSave
        {
            Name = saveFileName,
            Created = now,
            FilePath = $"{now.Ticks}.sav"
        };
        metaInfo.GameSaves.Add(newSave);
        var meta = new File();
        meta.Open($"user://meta.sav", File.ModeFlags.Write);
        meta.StoreLine(JsonConvert.SerializeObject(metaInfo));
        meta.Close();
    }

    public static void LoadGame(string saveFileName)
    {
        var meta = new File();
        meta.Open($"user://meta.sav", File.ModeFlags.Read);
        var metaText = meta.GetAsText();
        meta.Close();
        var metaInfo = JsonConvert.DeserializeObject<MetaGameInfo>(metaText);
        var save = metaInfo.GameSaves.FirstOrDefault(x => x.Name == saveFileName);

        var saveGame = new File();
        saveGame.Open($"user://{save.FilePath}", File.ModeFlags.Read);
        var saveText = saveGame.GetAsText();
        saveGame.Close();

        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(saveText);
        MainGameState.Instance.Clear();
        MainGameState.Instance.AddData(dict);
    }

    public static List<GameSave> GetSaves()
    {
        var meta = new File();
        if(meta.FileExists($"user://meta.sav"))
        {
            meta.Open($"user://meta.sav", File.ModeFlags.Read);
            var metaText = meta.GetAsText();
            meta.Close();
            var metaInfo = JsonConvert.DeserializeObject<MetaGameInfo>(metaText);
            return metaInfo.GameSaves;
        }
        return null;
    }
}
