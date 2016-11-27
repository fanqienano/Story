using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

public class SessionInfo {
	public int id { get; set; }
	public string name { get; set; }
	public string avatar { get; set; }
	public string desc { get; set; }
	public int isShow { get; set; }
	public int isActive { get; set; }
	public string dialogScript { get; set; }
	public int dialogId { get; set; }
	public int continueValue { get; set; }

	public static SessionInfo dataToObject(SqliteDataReader reader){
		SessionInfo sessionInfo = new SessionInfo ();
		sessionInfo.id = reader.GetInt16(reader.GetOrdinal("id"));
		sessionInfo.name = reader.GetString(reader.GetOrdinal("name"));
		sessionInfo.avatar = reader.GetString(reader.GetOrdinal("avatar"));
		sessionInfo.desc = reader.GetString(reader.GetOrdinal("desc"));
		//		sessionInfo.activeTime = long.Parse (jsonData["activeTime"].ToString());
		sessionInfo.isShow = reader.GetInt16(reader.GetOrdinal("isShow"));
		sessionInfo.isActive = reader.GetInt16(reader.GetOrdinal("isActive"));
		sessionInfo.dialogScript = reader.GetString(reader.GetOrdinal("dialogScript"));
		sessionInfo.dialogId = reader.GetInt16(reader.GetOrdinal("dialogId"));
		sessionInfo.continueValue = reader.GetInt16(reader.GetOrdinal("continueValue"));
		return sessionInfo;
	}

	public static List<SessionInfo> getSessionList(){
		List<SessionInfo> sessionList = new List<SessionInfo> ();
		SQLiteUtils sqlUtils = new SQLiteUtils();
		SqliteDataReader reader = sqlUtils.readSql (Constants.SessionTable);
		while (reader.Read ()) {
			sessionList.Add (dataToObject(reader));
		}
		sqlUtils.closeConnection ();
		sessionList.Sort ((x, y) => -(x.isActive.CompareTo(y.isActive)));
		return sessionList;
	}

	public static SessionInfo getSessionInfo(int sessionId){
		SessionInfo sessionInfo = new SessionInfo ();
		SQLiteUtils sqlUtils = new SQLiteUtils();
		SqliteDataReader reader = sqlUtils.findSql (Constants.SessionTable, sessionId);
		reader.Read ();
		sessionInfo = dataToObject(reader);
		sqlUtils.closeConnection ();
		return sessionInfo;
	}
}
