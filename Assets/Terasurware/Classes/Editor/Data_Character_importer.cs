using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Data_Character_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Data/Data_Character.xlsx";
	private static readonly string exportPath = "Assets/Data/Data_Character.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Data_Character data = (Data_Character)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Data_Character));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Data_Character> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Data_Character.Sheet s = new Data_Character.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Data_Character.Param p = new Data_Character.Param ();
						
					cell = row.GetCell(0); p.Name = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(1); p.Atk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Def = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.MaxHp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.Mp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.MaxMp = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
