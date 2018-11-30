using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVTSRoom : MonoBehaviour {

	//public GameObject photonObject;

	public readonly byte InstantiateVrAvatarEventCode = 123;
	int playerCount;

	// Use this for initialization
	void Start () {

		int viewId = PhotonNetwork.AllocateViewID();

    	PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All });

		/*
		PhotonNetwork.Instantiate(
			photonObject.name,
			new Vector3(9.71f, 0f, 12.55f),
			Quaternion.identity, 0
		);
		*/
	}

	//オブジェクトが有効/アクティブになった時に呼び出される
	//スクリプトが呼び出される順番はStartよりも早い
	public void OnEnable()
	{
		playerCount = StartPhoton.nowPlayerCount;
		//RaiseEventを扱うメソッドを"+="を使って登録する
		PhotonNetwork.OnEventCall += OnEvent;
	}

	//オブジェクトが無効になった時に呼び出される
	public void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnEvent;
	}

	//正しいプレハブがインスタンス化されていることを確認する
	//送信者のIDとローカルクライアントのIDを比較します
	private void OnEvent(byte eventcode, object content, int senderid)
	{
		if (eventcode == InstantiateVrAvatarEventCode)
		{
			//demo
			Vector3 myPos = new Vector3(9.71f, 1.5f, 12.55f);
			Vector3 othPos = new Vector3(12f, 1.5f, 12.55f);
			Quaternion q = new Quaternion();
			q= Quaternion.identity;
			Debug.Log("senderIdは：" + senderid);

			GameObject go = null;

			//ローカルのPhotonPlayer
			if (PhotonNetwork.player.ID == senderid)
			{
				//Resources関数はPathを指定しなければResourcesフォルダ限定らしい
				//Instantiateは今はVector3を指定していない
				go = Instantiate(Resources.Load("LocalAvatar"), myPos, q) as GameObject;
			}
			else
			{
				go = Instantiate(Resources.Load("RemoteAvatar"), othPos, q) as GameObject;
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
}
