using System.Collections;
using System.Text;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Utils {
	public static readonly string scriptFile = Constants.PathURL + "main.json";
	public static readonly string sessionFile = Constants.PathURL + "session.json";
	public static readonly string actorFile = Constants.PathURL + "actor.json";
	public static readonly long dayTime = 1000 * 60 * 60 * 24;

	public static string readFile(string filePath) {
		//		Android Use
		//		WWW www = new WWW(filePath);
		//		while (!www.isDone) {}
		//		return www.text;
		string text = System.IO.File.ReadAllText(filePath);
		return text;
	}

//	public static void saveRecord(DialogInfo dialogInfo, int num, string curJsonFile, int curId) {
//		saveHistory (dialogInfo, num);
//		string[] lines = { curJsonFile, curId.ToString() };
//		File.WriteAllLines(@recordPath, lines, Encoding.UTF8);
//	}

//	private static void saveHistory(DialogInfo dialogInfo, int num){
//		Debug.Log (dialogInfo.getContent());
//		StreamWriter sw = new StreamWriter (@historyPath, true);
//		if (!File.Exists (@historyPath)) {
//			sw.Write("");
//		}
//		if (dialogInfo.getType () == DialogType.Dialog) {
//			if (dialogInfo.getVoice().Equals(string.Empty)){
//				sw.WriteLine(dialogInfo.getContent());
//			}else{
//				sw.WriteLine(dialogInfo.getContent() + voiceSplitStr + dialogInfo.getVoice());
//			}
//		} else {
//			sw.WriteLine(num.ToString() + splitStr + dialogInfo.getSelect()[0].getOption() + splitStr + dialogInfo.getSelect()[1].getOption());
//		}
//		sw.Close();
//	}

//	public static List<DialogInfo> readHistory(){
//		List<DialogInfo> historyList = new List<DialogInfo> ();
//		if (File.Exists (@historyPath)) {
//			string[] lines = File.ReadAllLines (@historyPath);
//			foreach (string line in lines){
//				DialogInfo di = new DialogInfo();
//				if (line.Contains(splitStr)){
//					di.setType(DialogType.Select);
//					string[] info = Regex.Split(line, splitStr, RegexOptions.IgnoreCase);
//					Option op1 = new Option();
//					op1.setOption(info[1]);
//					Option op2 = new Option();
//					op2.setOption(info[2]);
//					int clicked = int.Parse(info[0]);
//					if (clicked == 0){
//						op1.setClicked(true);
//					}else{
//						op2.setClicked(true);
//					}
//					di.addOption(op1);
//					di.addOption(op2);
//				}else if (line.Contains(voiceSplitStr)){
//					string[] info = Regex.Split(line, voiceSplitStr, RegexOptions.IgnoreCase);
//					di.setType(DialogType.Dialog);
//					di.setContent(info[0]);
//					di.setVoice(info[1]);
//				}else{
//					di.setType(DialogType.Dialog);
//					di.setContent(line);
//				}
//				historyList.Add(di);
//			}
//			return historyList;
//		}
//		return historyList;
//	}

//	public static void clearHistoryRecord(){
//		if (File.Exists (@historyPath)) {
//			File.Delete(@historyPath);
//		}
//		if (File.Exists (@recordPath)) {
//			File.Delete(@recordPath);
//		}
//	}

//	public static string[] readRecord() {
//		if (File.Exists (@recordPath)) {
//			string[] lines = File.ReadAllLines (@recordPath);
//			return lines;
//		}
//		return new string[] {mainJsonPath, "0"};
//	}

//	public static long saveWakeTimeStamp(long wake){
//		string[] lines = { wake.ToString() };
//		File.WriteAllLines(@wakeTimeStamp, lines, Encoding.UTF8);
//		return wake;
//	}

//	public static long getTimeStampNow(){
//		return Utils.dataTimeToLong(DateTime.Now);
//	}

//	public static long readWakeTimeStamp(){
//		if (File.Exists (@wakeTimeStamp)) {
//			string[] lines = File.ReadAllLines (@wakeTimeStamp);
//			long time = long.Parse(lines[0]);
//			return time;
//		}
//		return 0;
//	}

	public static long dataTimeToLong(DateTime dt)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long timeStamp = (long)(dt - dtStart).TotalMilliseconds;
		return timeStamp;
	}

}
