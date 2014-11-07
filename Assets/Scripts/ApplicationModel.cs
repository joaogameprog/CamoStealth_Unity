using System;

public static class ApplicationModel {
	public const string SAVE_FILE_NAME = "camoSave.xml";
	public static int Level = 9;
    public static int World = 7;
	public static int LevelIndex { get{return ApplicationModel.World * 10 + ApplicationModel.Level;}} 
	public static int Points = 138940;
	public static int Cards = 13;
	public static int Stunts = 3;
	public static bool Won = false;
	public static int UnlockedLevels = 0;
	public static bool initSaveGame = true;
	public static SaveGame.SaveData SaveData = SaveGame.Instance.saveData; // {get{ return SaveGame.Instance.saveData;}}
	public static SaveGame Save = SaveGame.Instance;
}
