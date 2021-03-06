#!usr/bin/python
#coding=utf-8

import sys
import pandas
import numpy
import json

# class KeyType(object):
# 	def __init__(self, key, class_):
# 		self.key = key
# 		self.class_ = class_

def convertExcelToJson(typeFile, excelFile):
	keyType = {}
	result = []
	df = pandas.read_excel(typeFile)
	del df[u'备注']
	del df['value']
	keys = df.values
	for i in keys:
		if i[1].strip().lower() in ('int', 'long'):
			keyType[i[0]] = int
		elif i[1].strip().lower() == 'string':
			keyType[i[0]] = unicode
	# print keyType
	edf = pandas.read_excel(excelFile)
	# print edf
	dfd = edf.to_dict()
	for i in dfd:
		for d in dfd[i]:
			if len(result) < int(d) + 1:
				jd = {}
				jd[i] = dfd[i][d]
				result.append(jd)
			else:
				result[int(d)][i] = dfd[i][d]
	for d in result:
		for k in d:
			if isinstance(d[k], numpy.float) and numpy.isnan(d[k]):
				if keyType[k] == unicode:
					d[k] = ''
				if keyType[k] == int:
					d[k] = 0
			else:
				d[k] = keyType[k](d[k])
	print json.dumps(result, indent = 4)
	return result

if __name__ == '__main__':
	if len(sys.argv) > 2:
		convertExcelToJson(sys.argv[1], sys.argv[2])