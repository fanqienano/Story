using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using System.Reflection;

public class DialogHistoryInfo : DialogInfo
{
	public int id { get; set; }
	public string avatar { get; set; }
	public string name { get; set; }
	public int actorId { get; set; }
	public string type { get; set; }
	public int delay { get; set; }
	public string text { get; set; }
	public string voice { get; set; }
	public string video { get; set; }
	public string image { get; set; }
	public string send { get; set; }

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

	public static List<DialogHistoryInfo> getDialogHistoryList (string tableName)
	{
		List<DialogHistoryInfo> dialogList = new List<DialogHistoryInfo> ();
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		SqliteDataReader reader = sqlUtils.readSql (tableName + "_history");
		while (reader.Read ()) {
			dialogList.Add (dataToObject (reader));
		}
		sqlUtils.closeConnection ();
		//		dialogList.Sort ((x, y) => -(x.isActive.CompareTo(y.isActive)));
		return dialogList;
	}

	public static void writeDialogHistory(DialogHistoryInfo di){
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		string[] keys = new string[di.GetType ().GetProperties ().Length];
		for (int i = 0; i < di.GetType ().GetProperties ().Length; i++) {
			PropertyInfo pi = di.GetType ().GetProperties () [0];
			keys [i] = pi.Name;
		}
		string[] values = new string[di.GetType ().GetProperties ().Length];
		for (int i = 0; i < di.GetType ().GetProperties ().Length; i++) {
			PropertyInfo pi = di.GetType ().GetProperties () [0];
			values [i] = pi.GetValue(keys[i], null);
		}
		sqlUtils.writeHistory (Constants.HistoryTable, keys, values);
		sqlUtils.closeConnection ();
	}

}
