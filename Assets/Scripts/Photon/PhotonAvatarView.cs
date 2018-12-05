using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//同期するものを入れたコード
public class PhotonAvatarView : MonoBehaviour {

	private PhotonView photonView;
	private OvrAvatar ovrAvatar;
	private OvrAvatarRemoteDriver remoteDriver;

	private List<byte[]> packetData;

	// Use this for initialization
	public void Start () {
		photonView = GetComponent<PhotonView>();

		if (photonView.isMine)
		{
			//データをネットワーク経由で送信する前にすべてのアバター関連の入力イベントを格納するバイト配列のリストをインスタンス化します
			ovrAvatar = GetComponent<OvrAvatar>();
			ovrAvatar.RecordPackets = true;
			ovrAvatar.PacketRecorded += OnLocalAvatarPacketRecorded;

			packetData = new List<byte[]>();
		}
		else
		{
			Debug.Log("photonavatarviewのremoteDriverが初期化されている");
			remoteDriver = GetComponent<OvrAvatarRemoteDriver>();
		}
	}

	//私たちのジェスチャーを含む記録パケットを開始したり停止したりするために使用する
	public void OnDisable()
	{
		if (photonView.isMine)
		{
			ovrAvatar.RecordPackets = false;
			ovrAvatar.PacketRecorded -= OnLocalAvatarPacketRecorded;
		}
	}

	private int localSequence;

	//パケットはPUNがサポートするバイト配列にシリアル化されます
	public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
	{
		if (!PhotonNetwork.inRoom || (PhotonNetwork.room.PlayerCount < 2))
		{
			//Debug.Log("最初のプレイヤーはこの条件式に当てはまる");
			return;
		}
		//Debug.Log("二人目のプレイヤーはここがよく呼ばれるのか！？");

		using (MemoryStream outputStream = new MemoryStream())
		{
			BinaryWriter writer = new BinaryWriter(outputStream);

			var size = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
			byte[] data = new byte[size];
			Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

			writer.Write(localSequence++);
			writer.Write(size);
			writer.Write(data);

			packetData.Add(outputStream.ToArray());
		}
	}

	//受信したパケットを非直列化するのに役立つデシリアライザー
	private void DeserializeAndQueuePacketData(byte[] data)
	{
		using (MemoryStream inputStream = new MemoryStream(data))
		{
			BinaryReader reader = new BinaryReader(inputStream);
			int remoteSequence = reader.ReadInt32();

			int size = reader.ReadInt32();
			byte[] sdkData = reader.ReadBytes(size);

			System.IntPtr packet = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((System.UInt32)data.Length, sdkData);
			remoteDriver.QueuePacket(remoteSequence, new OvrAvatarPacket { ovrNativePacket = packet });
		}
	}

	//データを同期する
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//データの送信
		if (stream.isWriting)
		{
			if (packetData.Count == 0)
			{
				return;
			}

			stream.SendNext(packetData.Count);

			foreach (byte[] b in packetData)
			{
				stream.SendNext(b);
			}

			packetData.Clear();
		}

		//データの受信
		if (stream.isReading)
		{
			int num = (int)stream.ReceiveNext();

			for (int counter = 0; counter < num; ++counter)
			{
				byte[] data = (byte[])stream.ReceiveNext();

				DeserializeAndQueuePacketData(data);
			}
		}
	}
}
