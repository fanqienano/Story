using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

public class DialogHistoryInfo : DialogInfo
{
	public int id;
	public string avatar;
	public string name;
	public int actorId;
	public string type;
	public int delay;
	public string text;
	public string voice;
	public string video;
	public string image;
	public string send;

	public static DialogHistoryInfo dataToObject (SqliteDataReader reader)
	{
		DialogHistoryInfo dialogInfo = new DialogHistoryInfo ();
		dialogInfo.id = reader.GetInt16 (reader.GetOrdinal ("id"));
		dialogInfo.avatar = reader.GetString (reader.GetOrdinal ("avatar"));
		dialogInfo.name = reader.GetString (reader.GetOrdinal ("name"));
		dialogInfo.actorId = reader.GetInt16 (reader.GetOrdinal ("actorId"));
		dialogInfo.type = reader.GetString (reader.GetOrdinal ("type"));
		dialogInfo.delay = reader.GetInt16 (reader.GetOrdinal ("delay"));
		dialogInfo.text = reader.GetString (reader.GetOrdinal ("text"));
		dialogInfo.voice = reader.GetString (reader.GetOrdinal ("voice"));
		dialogInfo.video = reader.GetString (reader.GetOrdinal ("video"));
		dialogInfo.image = reader.GetString (reader.GetOrdinal ("image"));
		dialogInfo.send = reader.GetString (reader.GetOrdinal ("send"));
		return dialogInfo;
	}

	public static List<DialogHistoryInfo> getDialogHistoryList ()
	{
		List<DialogHistoryInfo> dialogList = new List<DialogHistoryInfo> ();
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		SqliteDataReader reader = sqlUtils.readSql (Constants.DialogTable);
		while (reader.Read ()) {
			dialogList.Add (dataToObject (reader));
		}
		sqlUtils.closeConnection ();
		//		dialogList.Sort ((x, y) => -(x.isActive.CompareTo(y.isActive)));
		return dialogList;
	}

}
