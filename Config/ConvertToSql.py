#!usr/bin/python
#coding=utf-8

# import os
import sys
import sqlite3
import ConvertToJson

class ConvertToSql(object):
	def __init__(self, typeFile, excelFile, tableName, sqlFile = 'story.db'):
		self.typeFile = typeFile
		self.excelFile = excelFile
		self.tableName = tableName
		self.sqlFile = sqlFile
		self.connect = sqlite3.connect(sqlFile)
		self.cursor = self.connect.cursor()
		self.jsonData = self.getJsonData()
		self.keyOrder = []

	def main(self):
		self.creatDbTable()
		self.insertDataToTable()

	def getJsonData(self):
		jsonFile = ConvertToJson.convertExcelToJson(self.typeFile, self.excelFile)
		return jsonFile

	def creatDbTable(self):
		# if os.path.isfile(self.sqlFile):
		# 	os.remove(self.sqlFile)
		cmd = self.makeCreatCmd()
		self.cursor.execute(cmd)
		self.connect.commit()

	def makeCreatCmd(self):
		self.keyOrder = []
		cmd = 'CREATE TABLE `%s` ('%self.tableName
		temp = self.jsonData[0]
		for k in temp:
			self.keyOrder.append(k)
			if isinstance(temp[k], (int, long)):
				cmd = cmd + '`%s` int(16) NOT NULL, '%k
			elif isinstance(temp[k], (str, unicode)):
				cmd = cmd + '`%s` TEXT NOT NULL, '%k
		cmd = cmd.split(' ,') + ')'
		print cmd
		return cmd

	def insertDataToTable(self):
		for d in self.jsonData:
			cmd, data = self.makeInsertCmd(d)
			self.cursor.execute(cmd, data)
			self.connect.commit()

	def makeInsertCmd(self, data):
		cmdData = []
		cmd = 'INSERT INTO %s VALUES ('%self.tableName
		for k in self.keyOrder:
			cmd = cmd + '?, '
			cmdData.append(data[k])
		cmd = cmd.split(' ,') + ')'
		print cmd, cmdData
		return cmd, cmdData

if __name__ == '__main__':
	obj = ConvertToSql(sys.argv[1], sys.argv[2], sys.argv[3])
	obj.main()