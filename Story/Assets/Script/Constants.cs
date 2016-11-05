using UnityEngine;
using System.Collections;

public class Constants {
	public static readonly string PathURL =
		#if UNITY_ANDROID   //安卓
		Application.streamingAssetsPath + "/";
		#elif UNITY_IPHONE  //iPhone
		Application.dataPath + "/Raw/";
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
		Application.dataPath + "/StreamingAssets/";
		#else
		string.Empty;
		#endif
	public static readonly string FilesPathURL =
		#if UNITY_ANDROID   //安卓
		Application.persistentDataPath + "/";
		#elif UNITY_IPHONE  //iPhone
		Application.dataPath + "/Raw/";
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
		Application.dataPath + "/StreamingAssets/";
		#else
		string.Empty;
		#endif

	public static readonly string SessionTable = "session";
}