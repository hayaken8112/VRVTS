using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPhoton : MonoBehaviour {

	public Canvas firstCanvas;
	public Canvas createRoomCanvas;
	public Canvas checkAllRoomCanvas;

	public Canvas passwordCanvas;

	//ボタンプレハブ
	public GameObject btnPref;

	RoomInfo[] rooms;

	public static RoomInfo nowRoom;
	public static int nowPlayerCount;
	public InputField passwordIF;
	public void Awake()
        {
        }

	// Use this for initialization
	void Start () {
		// ここでPhotonに接続している
        PhotonNetwork.ConnectUsingSettings("0.0.1");
		PhotonNetwork.automaticallySyncScene = true;
		firstCanvas.enabled = true;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
		passwordCanvas.enabled = false;
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
	}

	public void ToCheckRoom() {
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = true;
		passwordCanvas.enabled = false;

        if (rooms.Length == 0) {
            Debug.Log ("ルームが一つもありません");
			//Content取得(ボタンを並べる場所)

			RectTransform content = GameObject.Find("CheckAllRoomCanvas").GetComponent<RectTransform>();

			GameObject btn = (GameObject)Instantiate(btnPref);

			//ボタンをContentの子に設定

			btn.transform.SetParent(content, false);

			//ボタンのテキスト変更

			btn.transform.GetComponentInChildren<Text>().text = "No Room Back";

			//ボタンのクリックイベント登録

			btn.transform.GetComponent<Button>().onClick.AddListener(() => BackFirstCanvas());

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
	}

	//TODOシーンの切り替え
	public void JoinPhotonRoom() {
		string[] strList = nowRoom.name.Split('_');
		if (passwordIF.text != strList[1]) {
			passwordIF.text = "";
			passwordIF.placeholder.GetComponent<Text>().text = "passwordが間違っています";
			return;
		}
		nowPlayerCount = nowRoom.playerCount;
		PhotonNetwork.JoinRoom(nowRoom.name);
		Debug.Log(nowRoom.name + "に入室しました");
		FadeManager.Instance.LoadScene("OculusMain", 1.0f);
		//PhotonNetwork.LoadLevel(1);
	}

	public void BackFirstCanvas() {
		firstCanvas.enabled = true;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
	}

	
}
