using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour {

	public InputField nameIF;

	public static string userName;

	public void Regsiter(){

		Debug.Log("押された！");

		userName = nameIF.text;

		if (userName == ""){
			Debug.Log("名前が入力されていません");
			return;
		}

		SqliteDatabase sqlDB = new SqliteDatabase("config.db");
		string query = "create table ddd(name integer, age integer)";
        sqlDB.ExecuteNonQuery(query);

		string querysecond = "insert into ddd values('" + userName + "', 19)";
        sqlDB.ExecuteNonQuery(querysecond);
	}
}

