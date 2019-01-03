using System;
using System.Collections.Generic;

namespace CommonLib.PDMReader
{
    /// <summary>
    /// 关键字
    /// </summary>
    public class PdmKey
    {
        private TableInfo _OwnerTable = null;
        private List<string> _ColumnObjCodes = new List<string>();
        /// <summary>
        /// 关键字标识
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// 对象Id
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// Key名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Key代码,对应数据库中的Key.
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
        /// Key涉及的列
        /// </summary>
        public IList<ColumnInfo> Columns { get; set; }

        public void AddColumn(ColumnInfo mColumn)
        {
            if (this.Columns == null)
                this.Columns = new List<ColumnInfo>();
            this.Columns.Add(mColumn);
        }

        /// <summary>
        /// Key涉及的列代码，根据此可访问到列信息.对应列的ColumnId
        /// </summary>
        public List<string> ColumnObjCodes
        {
            get { return _ColumnObjCodes; }
        }

        public void AddColumnObjCode(string ObjCode)
        {
            _ColumnObjCodes.Add(ObjCode);
        }

        public PdmKey(TableInfo table)
        {
            _OwnerTable = table;
        }
    }
}
