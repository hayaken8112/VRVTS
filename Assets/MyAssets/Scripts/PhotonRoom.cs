using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoom : MonoBehaviour {

	public GameObject photonObject;

	// Use this for initialization
	void Start () {
		//エディタで設定するためにPhotonと接続する
		//ロビー空間
		//引数は、クライアントのバーション番号。
		//ユーザーはゲームバージョンで個々に分断される。
		//なので、バーションが違う人通しで通信させたくない場合これを変えればいい
		//今回は特になんでもよし
		Debug.Log("バーションを確認します");
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	//ロビーに入った時に呼ばれるメソッド
	//今回は、Auto-join Lobbyにチェックを入れているので、ロビーが存在すると自動的に入る
	//ルームに対する操作（ルーム一覧、作成、入室など）ができる
	void OnJoinedLobby(){
		//現在使われているロビーの、有効なRoomのどれかに入室する。
		//有効なRoomがなければ失敗する。
		Debug.Log("ここはおそらく失敗する");
        PhotonNetwork.JoinRandomRoom();
    }

	void OnReceivedRoomListUpdate(){
        //ルーム一覧を取る
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        if (rooms.Length == 0) {
            Debug.Log ("ルームが一つもありません");
        } else {
            //ルームが1件以上ある時ループでRoomInfo情報をログ出力
            for (int i = 0; i < rooms.Length; i++) {
                Debug.Log ("RoomName:"   + rooms [i].name);
                Debug.Log ("userName:" + rooms[i].customProperties["userName"]);
                Debug.Log ("userId:"   + rooms[i].customProperties["userId"]);
                //GameObject.Find("StatusText").GetComponent<Text>().text = rooms [i].name;
            }
        }
    }

	//Roomが存在せず、入室に失敗した時に呼ばれるメソッド
	void OnPhotonRandomJoinFailed(){
		//Roomを作成するメソッド
		//同一名のRoomが存在する場合は失敗する。
		//マスターサーバーでのみ呼び出し可能。
		//固有のルームメイを作成しない場合は、nullにする。
		//Roomの作成に成功した場合は、OnCreateRoomとOnJoinedRoomコールバックを呼び出す。
        //PhotonNetwork.CreateRoom(null);

		Debug.Log("なのでおそらくこのメソッドが呼ばれる");

		string userName = "ユーザ1";
        string userId = "user1";
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        //カスタムプロパティ
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add ("userName", userName); //ユーザ名
        customProp.Add ("userId", userId); //ユーザID
        PhotonNetwork.SetPlayerCustomProperties(customProp);

        RoomOptions roomOptions = new RoomOptions ();
        roomOptions.customRoomProperties = customProp;
        //ロビーで見えるルーム情報としてカスタムプロパティのuserName,userIdを使いますよという宣言
        roomOptions.customRoomPropertiesForLobby = new string[]{ "userName","userId"};
        roomOptions.maxPlayers = 2; //部屋の最大人数
        roomOptions.isOpen = true; //入室許可する
        roomOptions.isVisible = true; //ロビーから見えるようにする
        //userIdが名前のルームがなければ作って入室、あれば普通に入室する。
        PhotonNetwork.JoinOrCreateRoom (userId, roomOptions, null);
    }

	//Roomに入室した時に呼ばれるメソッド
	void OnJoinedRoom(){
		//同期したいオブジェクトをInstatiateする時に使用。
		//第４引数はPhotonViewのグループ番号が入る
		Debug.Log("unityちゃんを作成する");
        PhotonNetwork.Instantiate(
            photonObject.name,
            new Vector3(0f, 1f, 0f),
            Quaternion.identity, 0
        );

        GameObject mainCamera = 
            GameObject.FindWithTag("MainCamera");
        mainCamera.GetComponent<ThirdPersonCamera>().enabled = true;
    }
	
	void OnGUI(){
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
