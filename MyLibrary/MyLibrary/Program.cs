using CommonLib.ExcelReader;
using CommonLib.Files;
using CommonLib.PDMReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
           //PdmReaderTest();
            string currentDirectory = System.Environment.CurrentDirectory;  //XXXX\bin\Debug
            string path = currentDirectory + "\\test.txt";

            Console.ReadLine();
        }

        /// <summary>
        /// PDM文件读取测试
        /// </summary>
        static void PdmReaderTest()
        {
            //D:\csmsearch_anzhen\CSMQuery\doc\表结构设计模型\安贞医院
            IPdmFileReader mTest = new PdmFileReader("D:\\csmsearch_anzhen\\CSMQuery\\doc\\表结构设计模型\\安贞医院\\存储结构new.pdm");
            var curPdmModels = mTest.getPdmModels();
            string Text = Convert.ToString(curPdmModels.Tables.Count);
            var i = 1;
            Dictionary<string, string> fields = new Dictionary<string, string>();

            fields.Add("ID", "主键");
            fields.Add("Name", "表名称");
            fields.Add("EName", "表英文名称");
            fields.Add("ColumnName", "字段名称");
            fields.Add("ColumnEName", "字段英文名称");
            fields.Add("DataType", "字段类型");
            fields.Add("DataTypeMap", "字段类型Map");
            ExcelExport _export = new ExcelExport("d://export1.xls", fields);
            List<object> list = new List<object>();
            foreach (var t in curPdmModels.Tables)
            {

                string cTableName = t.Name;
                string cTableEname = t.Code;
                foreach (var c in t.Columns)
                {
                    i++;
                    string curDataType = ConvertDataType(c.DataType);
                    list.Add(new
                    {
                        ID = i,
                        Name = cTableName,
                        EName = cTableEname,
                        ColumnName = c.Name,
                        ColumnEName = c.Code,
                        DataType = c.DataType,
                        DataTypeMap = curDataType

                    });
                }
            }

            //_export.Export(list);
        }

        static string ConvertDataType(string dataType)
        {
            string cDatatype = "";
            switch (dataType)
            {
                case "int":
                    cDatatype = "number";
                    break;
                case "float":
                    cDatatype = "number";
                    break;
                case "datetime":
                    cDatatype = "date";
                    break;
                case "date":
                    cDatatype = "date";
                    break;
                case "ntext":
                    cDatatype = "text";
                    break;
                default:
                    cDatatype = "string";
                    break;
            }

            return cDatatype;
        }
    }
}
