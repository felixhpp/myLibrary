using System;
using System.Collections.Generic;

namespace CommonLib.PDMReader
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        public TableInfo()
        {
            this.Keys = new List<PdmKey>();
            this.Columns = new List<ColumnInfo>();

        }

        /// <summary>
        /// 表ID
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表代码,对应数据库表名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 物理选项
        /// </summary>
        public string PhysicalOptions { get; set; }

        /// <summary>
        /// 表列集合
        /// </summary>
        public IList<ColumnInfo> Columns { get; private set; }

        /// <summary>
        /// 表Key集合
        /// </summary>
        public IList<PdmKey> Keys { get; private set; }

        public void AddColumn(ColumnInfo mColumn)
        {
            if (this.Columns == null) {
                this.Columns = new List<ColumnInfo>();
            }
                
            this.Columns.Add(mColumn);
        }

        public void AddKey(PdmKey mKey)
        {
            if (this.Keys == null) {
                this.Keys = new List<PdmKey>();
            }

            this.Keys.Add(mKey);
        }
        /// <summary>
        /// 主键Key代码.=>KeyId
        /// </summary>
        public string PrimaryKeyRefCode { get; set; }

        /// <summary>
        /// 主关键字
        /// </summary>
        public PdmKey PrimaryKey
        {
            get
            {
                foreach (var key in this.Keys)
                {
                    if (key.KeyId == PrimaryKeyRefCode)
                    {
                        return key;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 表的描述=>PDM Notes.
        /// </summary>
        public string Description { get; set; }
    }
}
