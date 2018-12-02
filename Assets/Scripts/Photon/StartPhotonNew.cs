﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartPhotonNew : MonoBehaviour {

	RoomInfo[] rooms;
	public Text roomnames;
	public InputField enterPasswordIF;
	public static RoomInfo nowRoom;

	public InputField roomNameIF;
	public static string roomName;
	public InputField createPasswordIF;
	public Canvas keyboardCanvas;

	// Use this for initialization
	void Start () {
		// シーンの読み込みコールバックを登録.
        SceneManager.sceneLoaded += OnLoadedScene;
		keyboardCanvas.enabled = false;
		// ここでPhotonに接続している
        PhotonNetwork.ConnectUsingSettings("0.0.1");
		PhotonNetwork.automaticallySyncScene = true;

		foreach(InputField vrKeyboard in GameObject.FindObjectsOfTypeAll(typeof(InputField))){
			var trigger = vrKeyboard.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => { EditIF(vrKeyboard); });
			trigger.triggers.Add(entry);
		}
	}
	//ロビーに入った時に呼ばれるメソッド
	//今回は、Auto-join Lobbyにチェックを入れているので、ロビーが存在すると自動的に入る
	//ルームに対する操作（ルーム一覧、作成、入室など）ができる
	void OnJoinedLobby(){
		Debug.Log("勝手にロビー入ってます");
		//現在使われているロビーの、有効なRoomのどれかに入室する。
		//有効なRoomがなければ失敗する。
        //PhotonNetwork.JoinRandomRoom();
    }

	//部屋が更新されると呼ばれる
	void OnReceivedRoomListUpdate(){
		rooms = PhotonNetwork.GetRoomList();
		if (rooms.Length == 0) {
            Debug.Log ("ルームが一つもありません");
			roomnames.text = "No Room Now";
        } else {
			roomnames.text = "You";
        }
	}

	public void JoinPhotonRoom() {
		string[] strList = nowRoom.name.Split('_');
		if (enterPasswordIF.text != strList[1]) {
			enterPasswordIF.text = "";
			enterPasswordIF.placeholder.GetComponent<Text>().text = "passwordが間違っています";
			return;
		}
		//nowPlayerCount = nowRoom.playerCount;
		PhotonNetwork.JoinRoom(nowRoom.name);
		//PhotonNetwork.LoadLevel(1);
	}
	
	void OnJoinedRoom() {
		//Debug.Log(nowRoom.name + "に入室しました");
		//FadeManager.Instance.LoadScene("OculusMain", 1.0f);
		PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("OculusMain");
	}

	private void OnLoadedScene( Scene i_scene, LoadSceneMode i_mode )
    {
		PhotonNetwork.isMessageQueueRunning = true;
        // シーンの遷移が完了したら自分用のオブジェクトを生成.
        if( i_scene.name == "OculusMain" )
        {
			PhotonNetwork.OnEventCall += OnEvent;

			int viewId = PhotonNetwork.AllocateViewID();
			//string sendContent = viewId + "_" + StartPhoton.nowPlayerCount;

			//oneventのeventcodeに最初の引数が、contentに2番目の引数が、senderidはプレイヤーのidで自動的に決定している
    		PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All });
        }
    }

	public readonly byte InstantiateVrAvatarEventCode = 123;
	//public GameObject myCamera;

	Vector3[] place = new Vector3[] {new Vector3(13f, 1.5f, 14f), new Vector3(10f, 1.5f, 14f), new Vector3(7f, 1.5f, 12.55f)};
	public static Vector3 myPosition;

	//正しいプレハブがインスタンス化されていることを確認する
	//送信者のIDとローカルクライアントのIDを比較します
	private void OnEvent(byte eventcode, object content, int senderid)
	{
		if (eventcode == InstantiateVrAvatarEventCode)
		{
			//demo
			Debug.Log("contentは。。。。。。。。" + content);
			Vector3 pos = place[PhotonNetwork.room.playerCount];
			Quaternion q = new Quaternion();
			q= Quaternion.identity;
			Debug.Log("senderIdは：" + senderid);

			GameObject go = null;

			//ローカルのPhotonPlayer
			if (PhotonNetwork.player.ID == senderid)
			{
				//Resources関数はPathを指定しなければResourcesフォルダ限定らしい
				//Instantiateは今はVector3を指定していない
				go = Instantiate(Resources.Load("LocalAvatar"), pos, q) as GameObject;
				myPosition = pos;
				Debug.Log("ボボ簿おぼっぼっぼっぼぼ" + myPosition);
				Instantiate(Resources.Load("OVRCameraRig"), pos, q);
			}
			else
			{
				go = Instantiate(Resources.Load("RemoteAvatar"), pos, q) as GameObject;
			}

			if (go != null)
			{
				PhotonView pView = go.GetComponent<PhotonView>();

				if (pView != null)
				{
					pView.viewID = (int)content;
				}
			}
		}
	}

	public void CreatePhotonRoom() {
		roomName = roomNameIF.text;
		string password = createPasswordIF.text;
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
		Debug.Log(roomName);
        //userIdが名前のルームがなければ作って入室、あれば普通に入室する。
        PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, null);
	}

	public void EditIF(InputField enterIF){
		GameObject sceneObj = GameObject.Find("SceneObj");
		sceneObj.GetComponent<MakeKeyboard>().CreateKeyboard(enterIF);
	}
	
	public void DisableKeyboard() {
		Debug.Log("キーボードを消します");
		keyboardCanvas.enabled = false;
	}

}
