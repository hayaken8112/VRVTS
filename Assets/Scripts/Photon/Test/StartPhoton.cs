using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using DG.Tweening;

public class StartPhoton : MonoBehaviour {

	RoomInfo[] rooms;
	public InputField enterPasswordIF;
	public static RoomInfo nowRoom;

	public InputField roomNameIF;
	public static string roomName;
	public InputField createPasswordIF;
	public GameObject keyboardCanvas;

	//public GameObject roomToggle;
	public Text roomNameText;

	public Sprite[] images;
	public GameObject imageBtn;

	// Use this for initialization
	void Start () {
		// シーンの読み込みコールバックを登録.
        SceneManager.sceneLoaded += OnLoadedScene;
		keyboardCanvas.SetActive(false);
		LineUpPictures();
		// ここでPhotonに接続している
        PhotonNetwork.ConnectUsingSettings("0.0.1");
		PhotonNetwork.automaticallySyncScene = true;

		
		foreach(InputField vrKeyboard in Resources.FindObjectsOfTypeAll(typeof(InputField))){
			var trigger = vrKeyboard.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => { EditIF(vrKeyboard); });
			trigger.triggers.Add(entry);
		}
		
	}

	public void CanvasTapped(){
		//Debug.Log("ここは押してるんやで");
		PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);

		DisableKeyboard();

		/*

        foreach (RaycastResult raycastResult in result)
        {
            if(raycastResult.gameObject.GetComponent<InputField>()){
				Debug.Log("それはinputfieldや");
			}else{
				Debug.Log("それはちゃうなぁ");
			}
        }

		*/
	}

	void Update()
    {
		/*
        if (Input.GetMouseButtonDown(0))
        {
			Debug.Log("ボタンが押されてる！");
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.GetComponent<InputField>())
                {
					Debug.Log("keyboardがタップされた");
					InputField vrIF = hit.collider.gameObject.GetComponent<InputField>();
					EditIF(vrIF);
                }
				else
				{
					Debug.Log("それはkeyboardじゃないよ");
					DisableKeyboard();
				}
            }
        }
		*/
    }

	void LineUpPictures() {
		RectTransform content = GameObject.Find("Canvas/Tab View/Pages/Container/Offline/Scroll View/Viewport/Content").GetComponent<RectTransform>();
		for (int i = 0; i < images.Length / 3 + 1; i++){
			int no = i;
			int b = 0;
			GameObject img = (GameObject)Instantiate(imageBtn);
			foreach(Transform child in img.transform){
				int artNumber = 3 * no + b;
				if (artNumber >= images.Length){
					child.gameObject.SetActive(false);
					continue;
				}
				child.transform.GetComponent<Image>().sprite = images[artNumber];
				child.transform.GetComponent<Button>().onClick.AddListener(() => EnterWithOffLine(artNumber));
				b += 1;
			}
			//img.transform.position = new Vector2(0, -475f - 900 * no);
			img.transform.SetParent(content, false);
		}
		
	}

	void EnterWithOffLine(int no) {
		//PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("Offline");
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
			//RectTransform content = GameObject.Find("Canvas/Tab View/Pages/Container/Use Online/Tab View/Pages/Container/Page 2/Radio Buttons").GetComponent<RectTransform>();
			roomNameText.text = "No Room Here Sorry";
			
        } else {
			//RectTransform content = GameObject.Find("Canvas/Tab View/Pages/Container/Use Online/Tab View/Pages/Container/Page 2").GetComponent<RectTransform>();
			roomNameText.text = rooms[0].name;
			/*
			for (int i = 0; i < rooms.Length; i++) {
				
				int no = i;

				GameObject btn = (GameObject)Instantiate(roomToggle);

				//ボタンをContentの子に設定

				btn.transform.SetParent(content, false);

				//ボタンのテキスト変更
				string[] strList = rooms[no].name.Split('_');
				btn.transform.GetComponentInChildren<Text>().text = strList[0];
            }
			*/
        }
	}

	public void JoinPhotonRoom() {
		if (rooms.Length == 0){
			enterPasswordIF.text = "";
			return;
		}
		string[] strList = rooms[0].name.Split('_');
		if (enterPasswordIF.text != strList[1]) {
			enterPasswordIF.text = "";
			return;
		}
		//nowPlayerCount = nowRoom.playerCount;
		PhotonNetwork.JoinRoom(rooms[0].name);
		//PhotonNetwork.LoadLevel(1);
	}
	
	void OnJoinedRoom() {
		//Debug.Log(nowRoom.name + "に入室しました");
		//FadeManager.Instance.LoadScene("OculusMain", 1.0f);
		//PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("OculusMain");
	}

	private void OnLoadedScene( Scene i_scene, LoadSceneMode i_mode )
    {
		//PhotonNetwork.isMessageQueueRunning = true;
        // シーンの遷移が完了したら自分用のオブジェクトを生成.
        if( i_scene.name == "OculusMain" )
        {
			Debug.Log("OnLoadedSceneが呼ばれました");
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
		//Debug.Log("キーボードを消します");
		RectTransform content = keyboardCanvas.GetComponent<RectTransform>();
		content.DOMove(
			new Vector3(53, -889, 0),
			0.2f
		);
		keyboardCanvas.SetActive(false);
		
	}
	
}
