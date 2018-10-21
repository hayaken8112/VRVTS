using System;

namespace SpeechData
{
    [Serializable]
    public class ConfigData
    {
        public string encoding;
        public int sampleRateHertz;
        public string languageCode;
        public bool enableWordTimeOffsets;
    }
    [Serializable]
    public class AudioData
    {
        // public string uri;
        public string content;
    }
    [Serializable]
    public class Form
    {
        public AudioData audio;
        public ConfigData config;
    }

    [Serializable]
    public class Response
    {
		public Result[] results;
		public Response() {
			results = new Result[1];
		}
    }
	[Serializable]
	public class Result {
		public Alternative[] alternatives;
		Result() {
			alternatives = new Alternative[1];
		}
	}

	[Serializable]
	public class Alternative {
		public string transcript;
		public float confidence;
		Alternative() {
			transcript = "";
			confidence = 0;
		}
	}



}
