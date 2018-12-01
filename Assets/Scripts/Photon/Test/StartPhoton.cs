using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartPhoton : MonoBehaviour {

	public Canvas firstCanvas;
	public Canvas createRoomCanvas;
	public Canvas checkAllRoomCanvas;

	public Canvas passwordCanvas;
	public Canvas keyboardCanvas;

	//ボタンプレハブ
	public GameObject btnPref;

	RoomInfo[] rooms;

	public static RoomInfo nowRoom;
	//public static int nowPlayerCount;
	public InputField passwordIF;
	public void Awake()
        {
        }

	// Use this for initialization
	void Start () {
		// シーンの読み込みコールバックを登録.
        SceneManager.sceneLoaded += OnLoadedScene;
		// ここでPhotonに接続している
        PhotonNetwork.ConnectUsingSettings("0.0.1");
		PhotonNetwork.automaticallySyncScene = true;
		firstCanvas.enabled = true;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
		passwordCanvas.enabled = false;
		keyboardCanvas.enabled = false;
		//PhotonNetwork.JoinLobby();
	}

	//ロビーに入った時に呼ばれるメソッド
	//今回は、Auto-join Lobbyにチェックを入れているので、ロビーが存在すると自動的に入る
	//ルームに対する操作（ルーム一覧、作成、入室など）ができる
	void OnJoinedLobby(){
		//現在使われているロビーの、有効なRoomのどれかに入室する。
		//有効なRoomがなければ失敗する。
		Debug.Log("ここはおそらく失敗する");
        //PhotonNetwork.JoinRandomRoom();
    }

	//部屋が更新されると呼ばれる
	void OnReceivedRoomListUpdate(){
		rooms = PhotonNetwork.GetRoomList();
	}

	public void ToCreateRoom() {
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = true;
		checkAllRoomCanvas.enabled = false;
		passwordCanvas.enabled = false;
		keyboardCanvas.enabled = false;
	}

	public void ToCheckRoom() {
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = true;
		passwordCanvas.enabled = false;
		keyboardCanvas.enabled = false;

        if (rooms.Length == 0) {
            Debug.Log ("ルームが一つもありません");
        } else {
			//Content取得(ボタンを並べる場所)

			RectTransform content = GameObject.Find("CheckAllRoomCanvas").GetComponent<RectTransform>();

            //ルームが1件以上ある時ループでRoomInfo情報をログ出力
            for (int i = 0; i < rooms.Length; i++) {
				int no = i;

				//ボタン生成

				GameObject btn = (GameObject)Instantiate(btnPref);

				//ボタンをContentの子に設定

				btn.transform.SetParent(content, false);

				//ボタンのテキスト変更
				string[] strList = rooms[no].name.Split('_');
				btn.transform.GetComponentInChildren<Text>().text = strList[0];

				//ボタンのクリックイベント登録

				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(rooms[no]));
            }
        }
	}

	public void OnClick(RoomInfo roomName) {
		nowRoom = roomName;
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
		passwordCanvas.enabled = true;
		keyboardCanvas.enabled = false;
	}

	//TODOシーンの切り替え
	public void JoinPhotonRoom() {
		string[] strList = nowRoom.name.Split('_');
		if (passwordIF.text != strList[1]) {
			passwordIF.text = "";
			passwordIF.placeholder.GetComponent<Text>().text = "passwordが間違っています";
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
			/* 
			string str = (string)content;
			string[] strList = str.Split('_');
			int photonViewId = int.Parse(strList[0]);
			int peopleNumber = int.Parse(strList[1]);
			*/
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

	public void BackFirstCanvas() {
		firstCanvas.enabled = true;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
		passwordCanvas.enabled = false;
		keyboardCanvas.enabled = false;
	}

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
		Debug.Log(roomName);
        //userIdが名前のルームがなければ作って入室、あれば普通に入室する。
        PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, null);
	}

	void OnGUI(){
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

	public void DisableKeyboard() {
		Debug.Log("キーボードを消します");
		keyboardCanvas.enabled = false;
	}
	
}
