using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVTSRoom : MonoBehaviour {

	

	public readonly byte InstantiateVrAvatarEventCode = 123;

	public void OnEnable()
	{
		Debug.Log("OnEventが呼ばれました");
		PhotonNetwork.OnEventCall += OnEvent;
		int viewId = PhotonNetwork.AllocateViewID();
    	PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All });
	}

	public void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnEvent;
		int viewId = PhotonNetwork.AllocateViewID();
    	PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All });
	}

	private void OnEvent(byte eventcode, object content, int senderid)
	{
		if (eventcode == InstantiateVrAvatarEventCode)
		{
			GameObject go = null;
			Quaternion q = new Quaternion();
			q= Quaternion.Euler(0, 180, 0);

			if (PhotonNetwork.player.ID == senderid)
			{
				go = Instantiate(Resources.Load("LocalAvatar"), new Vector3(9.41f, 1.7f, 14.301f), q) as GameObject;
				Debug.Log("アバターのpositionは；" + go.transform.position);
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
