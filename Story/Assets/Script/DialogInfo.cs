using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

public class DialogInfo
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
	public string input;
	public string option_1_text;
	public string option_1_script;
	public int option_1_id;
	public string option_2_text;
	public string option_2_script;
	public int option_2_id;
	public string continueKey;
	public int continueValue;
	public string actionKey;
	public int actionValue;

	public static DialogInfo dataToObject (SqliteDataReader reader)
	{
		DialogInfo dialogInfo = new DialogInfo ();
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
		dialogInfo.input = reader.GetString (reader.GetOrdinal("input"));
		dialogInfo.option_1_text = reader.GetString (reader.GetOrdinal ("option_1_text"));
		dialogInfo.option_1_script = reader.GetString (reader.GetOrdinal ("option_1_script"));
		dialogInfo.option_1_id = reader.GetInt16 (reader.GetOrdinal ("option_1_id"));
		dialogInfo.option_2_text = reader.GetString (reader.GetOrdinal ("option_2_text"));
		dialogInfo.option_2_script = reader.GetString (reader.GetOrdinal ("option_2_script"));
		dialogInfo.option_2_id = reader.GetInt16 (reader.GetOrdinal ("option_2_id"));
		dialogInfo.continueKey = reader.GetString (reader.GetOrdinal ("continueKey"));
		dialogInfo.continueValue = reader.GetInt16 (reader.GetOrdinal ("continueValue"));
		dialogInfo.actionKey = reader.GetString (reader.GetOrdinal ("actionKey"));
		dialogInfo.actionValue = reader.GetInt16 (reader.GetOrdinal ("actionValue"));
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

}
