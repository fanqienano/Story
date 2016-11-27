using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

public class DialogInfo
{
	public int id { get; set; }
	public string avatar { get; set; }
	public string name { get; set; }
	public int actorId { get; set; }
	public string type { get; set; }
	public long delay { get; set; }
	public string text { get; set; }
	public string voice { get; set; }
	public string video { get; set; }
	public string image { get; set; }
	public string input { get; set; }
	public string option_1_text { get; set; }
	public string option_1_script { get; set; }
	public int option_1_id { get; set; }
	public string option_2_text { get; set; }
	public string option_2_script { get; set; }
	public int option_2_id { get; set; }
	public int continueKey { get; set; }
	public int continueValue { get; set; }
	public int actionKey { get; set; }
	public int actionValue { get; set; }
	public long activeTime { get; set; }

	public static DialogInfo dataToObject (SqliteDataReader reader)
	{
		DialogInfo dialogInfo = new DialogInfo ();
		dialogInfo.id = reader.GetInt16 (reader.GetOrdinal ("id"));
		dialogInfo.avatar = reader.GetString (reader.GetOrdinal ("avatar"));
		dialogInfo.name = reader.GetString (reader.GetOrdinal ("name"));
		dialogInfo.actorId = reader.GetInt16 (reader.GetOrdinal ("actorId"));
		dialogInfo.type = reader.GetString (reader.GetOrdinal ("type"));
		dialogInfo.delay = (long)reader.GetInt16 (reader.GetOrdinal ("delay"));
		dialogInfo.text = reader.GetString (reader.GetOrdinal ("text"));
		dialogInfo.voice = reader.GetString (reader.GetOrdinal ("voice"));
		dialogInfo.video = reader.GetString (reader.GetOrdinal ("video"));
		dialogInfo.image = reader.GetString (reader.GetOrdinal ("image"));
		dialogInfo.input = reader.GetString (reader.GetOrdinal("input"));
		dialogInfo.option_1_text = reader.GetString (reader.GetOrdinal ("option_1_text"));
		dialogInfo.option_1_script = reader.GetString (reader.GetOrdinal ("option_1_script"));
		dialogInfo.option_1_id = reader.GetInt16 (reader.GetOrdinal ("option_1_id"));
		dialogInfo.option_2_text = reader.GetString (reader.GetOrdinal ("option_2_text"));
		dialogInfo.option_2_script = reader.GetString (reader.GetOrdinal ("option_2_script"));
		dialogInfo.option_2_id = reader.GetInt16 (reader.GetOrdinal ("option_2_id"));
		dialogInfo.continueKey = reader.GetInt16 (reader.GetOrdinal ("continueKey"));
		dialogInfo.continueValue = reader.GetInt16 (reader.GetOrdinal ("continueValue"));
		dialogInfo.actionKey = reader.GetInt16 (reader.GetOrdinal ("actionKey"));
		dialogInfo.actionValue = reader.GetInt16 (reader.GetOrdinal ("actionValue"));
		dialogInfo.activeTime = (long)reader.GetInt16 (reader.GetOrdinal ("activeTime"));
		return dialogInfo;
	}

	public static List<DialogInfo> getDialogList ()
	{
		List<DialogInfo> dialogList = new List<DialogInfo> ();
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		SqliteDataReader reader = sqlUtils.readSql (Constants.DialogTable);
		while (reader.Read ()) {
			dialogList.Add (dataToObject (reader));
		}
		sqlUtils.closeConnection ();
//		dialogList.Sort ((x, y) => -(x.isActive.CompareTo(y.isActive)));
		return dialogList;
	}

	public static DialogInfo getDialogInfo(SessionInfo sessionInfo){
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		SqliteDataReader reader = sqlUtils.findSql (sessionInfo.dialogScript, sessionInfo.dialogId);
		reader.Read ();
		DialogInfo dialogInfo = dataToObject (reader);
		return dialogInfo;
	}

	public static DialogInfo getDialogInfo(string dialogScript, int dialogId){
		SQLiteUtils sqlUtils = new SQLiteUtils ();
		SqliteDataReader reader = sqlUtils.findSql (dialogScript, dialogId);
		reader.Read ();
		DialogInfo dialogInfo = dataToObject (reader);
		return dialogInfo;
	}

}
