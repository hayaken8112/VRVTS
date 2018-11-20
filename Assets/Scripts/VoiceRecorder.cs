using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecorder : MonoBehaviour
{

    AudioClip audioClip;
    AudioSource audioSource;
    string micName = "null";
    const int samplingFrequency = 16000;

    const int maxTime_s = 30;
    string enc_data = "";
    public Text debugText;

    // Use this for initialization
    void Start()
    {

        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
            micName = device;
        }
        // audioSource = audioSourceCube.GetComponent<AudioSource>();
        // debugText.text = Microphone.devices.Length.ToString();

    }

    public void StopRecord()
    {
        //マイクの録音位置を取得
        int position = Microphone.GetPosition(deviceName: micName);

        //マイクの録音を強制的に終了
        Microphone.End(deviceName: micName);

        //再生時間を確認すると、停止した時間に関わらず、maxDurationの値になっている。これは無音を含んでいる？
        Debug.Log("修正前の録音時間: " + audioClip.length);

        //音声データ一時退避用の領域を確保し、audioClipからのデータを格納
        float[] soundData = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(soundData, 0);

        //新しい音声データ領域を確保し、positonの分だけ格納できるサイズにする。
        float[] newData = new float[position * audioClip.channels];

        //positionの分だけデータをコピー
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }

        //新しいAudioClipのインスタンスを生成し、音声データをセット
        AudioClip newClip = AudioClip.Create(audioClip.name, position, audioClip.channels, audioClip.frequency, false);
        newClip.SetData(newData, 0);

        //audioClipを新しいものに差し替え
        AudioClip.Destroy(audioClip);
        audioClip = newClip;

        //再生時間
        Debug.Log("修正後の録音時間: " + audioClip.length);
    }

    public void StartRecord()
    {
        Debug.Log("record start");
        audioClip = Microphone.Start(deviceName: micName, loop: false, lengthSec: maxTime_s, frequency: samplingFrequency);
        debugText.text = "start";
    }

    public string FinishRecord()
    {
        if (Microphone.IsRecording(deviceName: micName) == true)
        {
            Debug.Log("recording stoped");
            debugText.text = "stop";
            StopRecord();
        }
        else
        {
            Debug.Log("not recorded");
            debugText.text = "not recoded";
        }
        Debug.Log(audioClip.samples);
        // debugText.text = audioClip.samples.ToString();
        // audioSource.clip = audioClip;
        byte[] data = WavUtility.FromAudioClip(audioClip);
        enc_data = Convert.ToBase64String(data);
        return enc_data;
        // AudioRecognizer recognizer = this.GetComponent<AudioRecognizer>();
        // recognizer.SpeechToText(enc_data);
    }
}
