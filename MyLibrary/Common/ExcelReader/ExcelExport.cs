using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExcelReader
{
    /// <summary>
    /// Excel常用的表格导出逻辑封装,单表写入
    /// </summary>
    /// <example>调用示例：
    /// <code>
    /// 
    /// 01:匿名对象集合导出
    ///     Dictionary<string, string> fields = new Dictionary<string, string>();
    ///     fields.Add("ID", "主键");
    ///     fields.Add("Name", "姓名");
    ///     fields.Add("Age", "年龄");
    ///     fields.Add("Birthday", "生日");
    ///     ExcelExport _export = new ExcelExport(LocalPathHelper.GetCurrentData() + "/export1.xls", fields);
    ///     List<object> list = new List<object>() {
    ///         new {ID=1,Name="张三丰",Age=20,Birthday=DateTime.Now },
    ///         new {ID=2,Name="王芳",Age=30,Birthday=DateTime.Now }
    ///     };
    ///     _export.Export(list);
    ///     
    /// 02:List集合导出
    ///     TestOne _Context = new DBA.TestOne();
    ///     List<Member_Info> list = _Context.Member_Info.ToList();
    ///     Dictionary<string, string> fields = new Dictionary<string, string>();
    ///     fields.Add("MemberID", "主键");
    ///     fields.Add("code", "账号");
    ///     fields.Add("RealName", "姓名");
    ///     fields.Add("IsActive", "是否激活");
    ///     fields.Add("commission", "奖金余额");
    ///     ExcelExport _export = new ExcelExport(LocalPathHelper.GetCurrentData() + "\\export2.xls", fields);
    ///     _export.Export<Member_Info>(list);
    /// </code>
    /// </example>
    public class ExcelExport
    {
        /// <summary>
        /// 导出的Excel文件名称+路径
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 导出的字段名称和描述
        /// </summary>
        public Dictionary<string, string> Fields { get; set; }

        private HSSFWorkbook _workbook = null;
        private ISheet _sheet = null;

        /// <summary>
        /// 创建实例，验证导出文件名
        /// </summary>
        /// <param name="FullName">文件名，默认以.xlsx后缀</param>
        /// <param name="Fields"></param>
        public ExcelExport(string FullName, Dictionary<string, string> Fields)
        {
            this.FullName = FullName;
            this.Fields = Fields;
            Check();
            _workbook = new HSSFWorkbook();
            _sheet = _workbook.CreateSheet("Sheet1");
        }
        /// <summary>
        /// 验证Excel文件名
        /// </summary>
        private void Check()
        {
            try
            {
                FileInfo info = new FileInfo(this.FullName);
                string[] extentions = new string[] {
                    ".xls",
                    ".xlsx"
                };
                if (extentions.Any(q => q == info.Extension) == false)
                {
                    info = new FileInfo(this.FullName + ".xlsx");
                }
                if (info.Exists == false)
                {
                    info.Create().Close();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("创建Excel文件失败", ex);
            }
        }

        /// <summary>
        /// 执行导出操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void Export<T>(List<T> list)
        {
            //写入表格头
            WriteHead();
            //写入数据
            ICellStyle cellStyle = _workbook.CreateCellStyle();
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");//为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;

            IFont cellFont = _workbook.CreateFont();
            cellFont.Boldweight = (short)FontBoldWeight.Normal;
            cellStyle.SetFont(cellFont);

            //建立行内容，从1开始
            int rowInex = 1;

            //为了提高性能，使用字典把反射的信息保存下来，避免每一次循环都反射
            Dictionary<string, PropertyInfo> fieldNamePropertyInfoDic = new Dictionary<string, PropertyInfo>();
            foreach (var rowItem in list)
            {
                //创建行
                IRow row = _sheet.CreateRow(rowInex);
                row.HeightInPoints = 25;

                int cellIndex = 0;
                foreach (var cellItem in this.Fields)
                {
                    string key = cellItem.Key;
                    //创建单元格
                    ICell cell = row.CreateCell(cellIndex);

                    PropertyInfo info = !fieldNamePropertyInfoDic.ContainsKey(key) ?
                        rowItem.GetType().GetProperty(key) : fieldNamePropertyInfoDic[key];

                    if (info == null)
                    {
                        cell.SetCellValue(string.Format("'{0}'属性不存在", key));
                    }
                    else
                    {
                        if (!fieldNamePropertyInfoDic.ContainsKey(key))
                        {
                            fieldNamePropertyInfoDic.Add(key, info);
                        }
                        object value = info.GetValue(rowItem);
                        if (value != null)
                            cell.SetCellValue(value.ToString());
                    }
                    cell.CellStyle = cellStyle;
                    cellIndex++;
                }
                //进入下一次循环
                rowInex++;
            }

            //自适应列宽度,不建议使用，数据量大时非常消耗性能
            //for (int i = 0; i < this.Fields.Count; i++)
            //{
            //    _sheet.AutoSizeColumn(i);
            //}

            //导出到文件
            WriteFile();
        }
        /// <summary>
        /// 写入表头
        /// </summary>
        private void WriteHead()
        {
            //设置表头样式
            ICellStyle headStyle = _workbook.CreateCellStyle();
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.FillForegroundColor = HSSFColor.Blue.Index;
            headStyle.VerticalAlignment = VerticalAlignment.Center;

            IFont headFont = _workbook.CreateFont();
            headFont.Boldweight = (short)FontBoldWeight.Bold;
            headStyle.SetFont(headFont);

            IRow row = _sheet.CreateRow(0);
            row.HeightInPoints = 30;

            int index = 0;
            foreach (var item in this.Fields)
            {
                ICell cell = row.CreateCell(index);
                cell.SetCellValue(item.Value);
                cell.CellStyle = headStyle;
                index++;
            }
        }

        /// <summary>
        /// 创建文件到磁盘
        /// </summary>
        private void WriteFile()
        {
            using (FileStream fs = new FileStream(this.FullName, FileMode.OpenOrCreate))
            {
                _workbook.Write(fs);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
