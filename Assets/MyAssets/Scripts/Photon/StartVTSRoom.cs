using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class StartVTSRoom : MonoBehaviour {

	Vector3[] place = new Vector3[] {new Vector3(9.41f, 1.7f, 14.301f), new Vector3(13f, 1.7f, 14.301f), new Vector3(6f, 1.7f, 14.301f)};
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
				Core.Initialize();

				Users.GetLoggedInUser().OnComplete(msg => {
					var avatarObject = Instantiate(Resources.Load("LocalAvatar"), new Vector3(9.41f, 1.7f, 14.301f), q) as GameObject;
					var avatar = avatarObject.GetComponent<OvrAvatar>();
					if (avatar != null) {
						avatar.ShowThirdPerson = true;
						avatar.ShowFirstPerson = false;
						avatar.oculusUserID = msg.GetUser().ID.ToString();
					}
				});
				Debug.Log("アバターのpositionは；" + go.transform.position);
			}
			else
			{
				Core.Initialize();

				Users.GetLoggedInUser().OnComplete(msg => {
					var avatarObject = Instantiate(Resources.Load("RemoteAvatar"), new Vector3(9.41f, 1.7f, 14.301f), q) as GameObject;
					var avatar = avatarObject.GetComponent<OvrAvatar>();
					if (avatar != null) {
						avatar.ShowThirdPerson = true;
						avatar.ShowFirstPerson = false;
						avatar.oculusUserID = msg.GetUser().ID.ToString();
					}
				});
				Debug.Log("アバターのpositionは；" + go.transform.position);
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
