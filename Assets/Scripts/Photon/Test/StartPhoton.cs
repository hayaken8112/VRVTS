using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPhoton : MonoBehaviour {

	public Canvas firstCanvas;
	public Canvas createRoomCanvas;
	public Canvas checkAllRoomCanvas;

	//ボタンプレハブ
	public GameObject btnPref;

	//ボタン表示数

	const int BUTTON_COUNT = 10;

	public void Awake()
        {
            // ここでPhotonに接続している
            // 0.0.1はゲームのバージョンを指定
            // （異なるバージョン同士でマッチングしないように？）
            PhotonNetwork.ConnectUsingSettings("0.0.1");
        }

	// Use this for initialization
	void Start () {
		firstCanvas.enabled = true;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = false;
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

	public void ToCreateRoom() {
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = true;
		checkAllRoomCanvas.enabled = false;
	}

	public void ToCheckRoom() {
		firstCanvas.enabled = false;
		createRoomCanvas.enabled = false;
		checkAllRoomCanvas.enabled = true;

		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        if (rooms.Length == 0) {
            Debug.Log ("ルームが一つもありません");
			//Content取得(ボタンを並べる場所)

			RectTransform content = GameObject.Find("CheckAllRoomCanvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();

			//Contentの高さ決定

			//(ボタンの高さ+ボタン同士の間隔)*ボタン数

			float btnSpace = content.GetComponent<VerticalLayoutGroup>().spacing;

			float btnHeight = btnPref.GetComponent<LayoutElement>().preferredHeight;

			content.sizeDelta = new Vector2(0, (btnHeight + btnSpace) * BUTTON_COUNT);

			for (int i = 0; i < BUTTON_COUNT; i++)

			{

			int no = i;

			//ボタン生成

			GameObject btn = (GameObject)Instantiate(btnPref);

			//ボタンをContentの子に設定

			btn.transform.SetParent(content, false);

			//ボタンのテキスト変更

			btn.transform.GetComponentInChildren<Text>().text = "Btn_"+no.ToString();

			//ボタンのクリックイベント登録

			btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(no));

			}
        } else {
			//Content取得(ボタンを並べる場所)

			RectTransform content = GameObject.Find("CheckAllRoomCanvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();

			//Contentの高さ決定

			//(ボタンの高さ+ボタン同士の間隔)*ボタン数

			float btnSpace = content.GetComponent<VerticalLayoutGroup>().spacing;

			float btnHeight = btnPref.GetComponent<LayoutElement>().preferredHeight;

			content.sizeDelta = new Vector2(0, (btnHeight + btnSpace) * rooms.Length);

            //ルームが1件以上ある時ループでRoomInfo情報をログ出力
            for (int i = 0; i < rooms.Length; i++) {
				int no = i;

				//ボタン生成

				GameObject btn = (GameObject)Instantiate(btnPref);

				//ボタンをContentの子に設定

				btn.transform.SetParent(content, false);

				//ボタンのテキスト変更

				btn.transform.GetComponentInChildren<Text>().text = "RoomName:"   + rooms [i].name;

				//ボタンのクリックイベント登録

				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(no));
            }
        }
	}

	public void OnClick(int no) {
		Debug.Log(no);
	}
	
}
