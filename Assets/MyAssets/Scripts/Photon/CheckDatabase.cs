using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckDatabase: MonoBehaviour
{
	/*
	[RuntimeInitializeOnLoadMethod()]
	static void Init()
	{

		bool yes;
		SqliteDatabase sqlDB = new SqliteDatabase("config.db");
		try {
			sqlDB.ExecuteQuery("select * from ddd");
			yes = true;
		} catch (SqliteException e) {
			yes = false;
		}

		if (yes){
			Debug.Log("このデータベースはありました");
			SceneManager.LoadScene("New Scene");
		}else{
			Debug.Log("このデータベースはありません");
		}
		
	}
	*/
}
