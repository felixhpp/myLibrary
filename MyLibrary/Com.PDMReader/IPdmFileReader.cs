
namespace Com.PDMReader
{
    public interface IPdmFileReader
    {
        PdmModels getPdmModels();
        TableInfo FindTableByTableName(string tableName);

        /// <summary>
        /// 读取指定Pdm文件的实体集合
        /// </summary>
        /// <param name="pdmFile">Pdm文件名(全路径名)</param>
        /// <returns>实体集合</returns>
        PdmModels ReadFromFile(string pdmFile);
    }
}
