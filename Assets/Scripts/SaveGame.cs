using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class SaveGame : Singleton<SaveGame>{
	#region Save variables
	
	public class LevelRecord
	{
		[XmlAttribute ("Level")]
		public int Level;
		[XmlAttribute ("Stars")]
		public int Stars;
		[XmlAttribute ("TotalTime")]
		public float TotalTime;
		public LevelRecord () { }
		
		public LevelRecord (int level, int stars, float time)
		{
			Level = level;
			Stars = stars;
			TotalTime = time;
		}
	}
	
	
	public class WearingItem
	{
		[XmlAttribute("Item")]
		public int Item;
		
		public WearingItem()
		{
		}
		
		public WearingItem(int item)
		{
			Item = item;
		}
	}
	
	
	public class NonConsumableItem
	{
		[XmlAttribute("Item")]
		public int Item;
		
		public NonConsumableItem()
		{
		}
		
		public NonConsumableItem(int item)
		{
			Item = item;
		}
	}
	
	
	
	[XmlRoot("SaveData")]
	public class SaveData
	{
		[XmlAttribute("SaveInitiated")]
		public bool SaveInitiated;
		
		[XmlAttribute("Volume")]
		public float Volume;
		
		[XmlAttribute("Lifes")]
		public int Lifes;
		
		[XmlAttribute("Boosts")]
		public int Boosts;
		
		[XmlAttribute("LastTime")]
		public DateTime LastTime;
		
		[XmlAttribute("Language")]
		public int Language;
				
		public SaveData () : this(true)
		{
		}
		public SaveData(bool init)
		{
			if(init)
			{
				SaveInitiated = true;
				Language = (int)Enum.Parse(typeof(TranslationsLanguages.Languages), Application.systemLanguage.ToString());
				Lifes = 4;
				Boosts = 0;
				LastTime = DateTime.Now;
				Volume = 0.7f;
			}
		}
	}
	#endregion
	
	public SaveData saveData;
	public SaveData GetSaveData()
	{
		return ApplicationModel.SaveData;
	}
	
	
	// Use this for initialization
	public void Awake()
	{
		//LongLaserGameUnity.SettingsUnity.Initialize (ApplicationModel.GAME_ID);
		//save = new XMLSaveLoad<SaveData> (ApplicationModel.SAVE_FILE_NAME, ApplicationModel.SaveData);
		//Load();
		Load();
		if(!saveData.SaveInitiated)
		{
			saveData = new SaveData(true);
			Save (Path.Combine(Application.dataPath, ApplicationModel.SAVE_FILE_NAME));
		}
		/*
		if (save.MyData == null || !save.MyData.SaveInitiated) 
		{
			saveData = new SaveData (true);
			Save ();
		}
		else {
			saveData = save.MyData;
			//saveData = save.MyData;
		}
		*/
	}
	
	public void Save(string path)
	{
		try
		{
			var serializer = new XmlSerializer(typeof(SaveData));
			using(var stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, saveData);
			}
		}catch(Exception e)
		{
			Debug.Log("Erro save  " + e.Message);
		}
	}
	
	public SaveData Load(string path)
	{
		
		try{
			var serializer = new XmlSerializer(typeof(SaveData));
			using(var stream = new FileStream(path, FileMode.Open))
			{
				return serializer.Deserialize(stream) as SaveData;
			}
		}catch(Exception e)
		{
			var message = e.Message;
			Debug.Log(e.Message);
			saveData = new SaveData(true);
			Save (path);
			return saveData;
			
		}
	}
	
	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static SaveData LoadFromText(string text) 
	{
		var serializer = new XmlSerializer(typeof(SaveData));
		return serializer.Deserialize(new StringReader(text)) as SaveData;
	}
	
	
	public void Save()
	{
		Save (Path.Combine(Application.persistentDataPath, ApplicationModel.SAVE_FILE_NAME));
	}
	
	public void Load()
	{
		saveData = Load (Path.Combine(Application.persistentDataPath, ApplicationModel.SAVE_FILE_NAME));
	}
	
}



