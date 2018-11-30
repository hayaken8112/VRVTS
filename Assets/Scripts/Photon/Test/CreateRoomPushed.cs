using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateRoomPushed : MonoBehaviour {

	public InputField roomField;
	public static string roomName;
	public InputField passwordField;

	public void Pushed() {
		roomName = roomField.text;
		string password = passwordField.text;
		PhotonNetwork.autoCleanUpPlayerObjects = false;
		//カスタムプロパティ
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        //customProp.Add ("userName", userName); //ユーザ名
        customProp.Add ("roomName", roomName); //ルーム名
		PhotonNetwork.SetPlayerCustomProperties(customProp);

        RoomOptions roomOptions = new RoomOptions ();
        roomOptions.customRoomProperties = customProp;
        //ロビーで見えるルーム情報としてカスタムプロパティのuserName,userIdを使いますよという宣言
        roomOptions.customRoomPropertiesForLobby = new string[]{ "roomName"};
        roomOptions.maxPlayers = 10; //部屋の最大人数
        roomOptions.isOpen = true; //入室許可する
        roomOptions.isVisible = true; //ロビーから見えるようにする
		Debug.Log("room名:" + roomName);
		//Photonサーバーに送信するルーム名前はパスワード付き
		if(!string.IsNullOrEmpty(password)) roomName += "_" + password;
        //userIdが名前のルームがなければ作って入室、あれば普通に入室する。
        PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, null);
	}

	void OnJoinedRoom(){
		Debug.Log("ルームに入室しました");
		FadeManager.Instance.LoadScene("OculusMain", 1.0f);
    }

	void OnGUI(){
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

}
