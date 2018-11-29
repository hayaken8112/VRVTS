using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVTSRoom : MonoBehaviour {

	//public GameObject photonObject;

	public readonly byte InstantiateVrAvatarEventCode = 123;

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

	public void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnEvent;
	}

	public void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnEvent;
	}

	private void OnEvent(byte eventcode, object content, int senderid)
	{
		if (eventcode == InstantiateVrAvatarEventCode)
		{
			GameObject go = null;

			if (PhotonNetwork.player.ID == senderid)
			{
				go = Instantiate(Resources.Load("LocalAvatar")) as GameObject;
			}
			else
			{
				go = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
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
