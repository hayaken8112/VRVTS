using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateRoomPushed : MonoBehaviour {

	public InputField roomField;
	public static string roomName;
	public InputField nameField;
	public static string userName;

	public void Pushed() {
		roomName = roomField.text;
		userName = nameField.text;
		PhotonNetwork.autoCleanUpPlayerObjects = false;
		//カスタムプロパティ
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add ("userName", userName); //ユーザ名
        customProp.Add ("userId", roomName); //ルーム名
		PhotonNetwork.SetPlayerCustomProperties(customProp);

        RoomOptions roomOptions = new RoomOptions ();
        roomOptions.customRoomProperties = customProp;
        //ロビーで見えるルーム情報としてカスタムプロパティのuserName,userIdを使いますよという宣言
        roomOptions.customRoomPropertiesForLobby = new string[]{ "userName","userId"};
        roomOptions.maxPlayers = 2; //部屋の最大人数
        roomOptions.isOpen = true; //入室許可する
        roomOptions.isVisible = true; //ロビーから見えるようにする
        //userIdが名前のルームがなければ作って入室、あれば普通に入室する。
        PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, null);
		Debug.Log("room名:" + roomName + "user名" + userName);
	}

	void OnJoinedRoom(){
		Debug.Log("ルームに入室しました");
		FadeManager.Instance.LoadScene("OculusMain", 4.0f);
    }

	void OnGUI(){
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

}
