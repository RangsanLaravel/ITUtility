using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace ITUtility
{
    public static partial class UtilityOracle
    {
        public static string ConditionInOrEqual(string whereColumn, object[] datas, List<DBParameter> param)
        {
            string condition, param_name = whereColumn;
            whereColumn = whereColumn.Trim();
            if (whereColumn.IndexOf(' ') > -1)
            {
                throw new Exception("ConditionInOrEqual : column " + whereColumn + " ไม่สามารถมีช่องว่างได้");
            }
            if (whereColumn.IndexOf(".") > -1)
            {
                param_name = whereColumn.Replace('.', '_');
            }
            if (datas.Count() > 1)
            {
                string sIds = "";
                foreach (object id in datas)
                {
                    if (sIds != "")
                        sIds = sIds + ", ";
                    sIds = sIds + Utility.SQLValueString(id);
                }

                condition = whereColumn + " IN (" + sIds + ")\n";
            }
            else
            {

                condition = whereColumn + " = :v" + param_name;

                UtilityOracle.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
            }

            return condition;
        }

        public static string ConditionInOrEqual(string whereColumn, long?[] datas, List<DBParameter> param)
        {
            string condition, param_name = whereColumn;
            whereColumn = whereColumn.Trim();
            if (whereColumn.IndexOf(' ') > -1)
            {
                throw new Exception("ConditionInOrEqual : column " + whereColumn + " ไม่สามารถมีช่องว่างได้");
            }
            if (whereColumn.IndexOf(".") > -1)
            {
                param_name = whereColumn.Replace('.', '_');
            }
            if (datas.Count() > 1)
            {
                string sIds = "";
                foreach (object id in datas)
                {
                    if (sIds != "")
                        sIds = sIds + ", ";
                    sIds = sIds + UtilityOracle.SQLValueString(id);
                }

                condition = whereColumn + " IN (" + sIds + ")\n";
            }
            else
            {

                condition = whereColumn + " = :v" + param_name;

                UtilityOracle.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
            }

            return condition;
        }

        public static DataTable FillDataTable(OracleCommand oCmd)
        {
            using (OracleDataReader oReader = oCmd.ExecuteReader())
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Load(oReader);
                    oReader.Close();
                    return dataTable;
                }
            }
        }

        public static DataTable FillDataTable(string sqlStr, OracleConnection connection)
        {
            OracleDataAdapter oAdpt = new OracleDataAdapter(sqlStr, connection);
            DataTable dt = new DataTable();
            oAdpt.Fill(dt);
            oAdpt.Dispose();
            return dt;
        }

        public static DataTable FillDataTable(string sqlStr, OracleConnection connection, List<DBParameter> param = null)
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();


            cmd.Parameters.Clear();
            cmd.BindByName = true;
            if (param != null && param.Any())
            {
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }

            using (OracleDataReader reader = cmd.ExecuteReader())
            {

                dt.Load(reader);

            }


            cmd.Dispose();


            return dt;
        }

        public static T[] FillDataTable<T>(string sqlStr, OracleConnection connection) where T : new()
        {
            OracleDataAdapter oAdpt = new OracleDataAdapter(sqlStr, connection);
            DataTable dt = new DataTable();
            IEnumerable<T> result;
            oAdpt.Fill(dt);
            oAdpt.Dispose();
            result = dt.AsEnumerable<T>();
            dt.Dispose();
            return result.ToArray();
        }

        public static T[] FillDataTable<T>(string sqlStr, OracleConnection connection, List<DBParameter> param) where T : new()
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();
            IEnumerable<T> result;

            if (param != null && param.Any())
            {
                cmd.Parameters.Clear();
                cmd.BindByName = true;
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;

                }
            }
            //  cmd.Parameters.AddRange(param);

            using (OracleDataReader reader = cmd.ExecuteReader())
            {

                dt.Load(reader);

            }

            cmd.Dispose();

            result = dt.AsEnumerable<T>();

            dt.Dispose();

            return result.ToArray();
        }

        public static object ExecuteScalar(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            object returnValue = oCmd.ExecuteScalar();
            oCmd.Dispose();
            return returnValue;
        }
        public static object ExecuteScalar(OracleConnection conn, string sqlStr, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            object returnValue = oCmd.ExecuteScalar();
            oCmd.Dispose();
            return returnValue;
        }

        public static int ExecuteNonQuery(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            int i = oCmd.ExecuteNonQuery();
            oCmd.Dispose();
            return i;
        }
        public static int ExecuteNonQuery(string sqlStr, OracleConnection conn, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            int i = oCmd.ExecuteNonQuery();
            oCmd.Dispose();
            return i;
        }

        public static String SQLValueString(Object input)
        {
            String returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
            }
            else
            {
                if (input is String)
                {

                    if ((String)input == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    String rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                }
                else
                {
                    returnStr = input.ToString();
                }
            }
            return returnStr;
        }

        public static void SQLValueString(List<DBParameter> paramList, string name, object input)
        {
            string returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));

            }
            else
            {
                if (input is string)
                {

                    if ((string)input == "")
                    {
                        returnStr = "Null";
                        paramList.Add(new DBParameter(name, DBNull.Value , OracleDbType.Varchar2));
                    }
                    else
                    {
                        returnStr = "'" + ((string)input).Replace("'", "''") + "'";

                        paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2));
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    string rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                        paramList.Add(new DBParameter(name, input, OracleDbType.Char));
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();

                    paramList.Add(new DBParameter(name, input, OracleDbType.Int16));
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int32));
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int64));
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Double));
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Decimal));
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Single));
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                    paramList.Add(new DBParameter(name, input, OracleDbType.Date));
                }
                else
                {
                    returnStr = input.ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2));
                }
            }
        }

        public static string SQLValueInString(Array inputs)
        {
            if (inputs == null || inputs.Length == 0)
                return "('')";
            string result = "(";
            foreach (object input in inputs)
                if (result.Length == 1)
                    result = result + SQLValueString(input);
                else
                    result = result + "," + SQLValueString(input);

            result = result + ")";

            return result;
        }

        public static string SQLDate(DateTime refDate)
        {
            if (refDate == new DateTime())
                return "Null";

            return string.Format("TO_DATE('{0}-{1}-{2} {3}:{4}:{5}','RRRR-MM-DD HH24:MI:SS', 'NLS_CALENDAR=''GREGORIAN''')"
                , refDate.Year.ToString()
                , refDate.Month.ToString().PadLeft(2, '0')
                , refDate.Day.ToString().PadLeft(2, '0')
                , refDate.Hour.ToString().PadLeft(2, '0')
                , refDate.Minute.ToString().PadLeft(2, '0')
                , refDate.Second.ToString().PadLeft(2, '0')
                );
        }

        public static string SQLShortDate(DateTime refDate)
        {
            if (refDate == new DateTime())
                return "Null";

            return string.Format("TO_DATE('{0}-{1}-{2}','RRRR-MM-DD', 'NLS_CALENDAR=''GREGORIAN''')"
                , refDate.Year.ToString()
                , refDate.Month.ToString().PadLeft(2, '0')
                , refDate.Day.ToString().PadLeft(2, '0')
                );
        }

        public static string SQLDate(DateTime? refDate)
        {
            if (refDate != null)
                return SQLDate(refDate.Value);
            else
                return "Null";
        }

        public static string SQLShortDate(DateTime? refDate)
        {
            if (refDate != null)
                return SQLShortDate(refDate.Value);
            else
                return "Null";
        }


        public static decimal? GetOraParamDecimal(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is decimal)
                return (decimal)input.Value;
            if (!(input.Value is OracleDecimal))
                return null;
            if (((OracleDecimal)input.Value).IsNull)
                return null;
            return ((OracleDecimal)input.Value).Value;
        }

        public static DateTime? GetOraParamDate(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is DateTime)
                return (DateTime)input.Value;
            if (!(input.Value is OracleDate))
                return null;
            if (((OracleDate)input.Value).IsNull)
                return null;
            return ((OracleDate)input.Value).Value;
        }

        public static string GetOraParamString(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is string)
                return (string)input.Value;
            if (input.Value is OracleString)
            {
                if (((OracleString)input.Value).IsNull)
                    return null;
                else
                    return ((OracleString)input.Value).Value;
            }

            return null;
        }

        public static int? GetOraParamInt32(OracleParameter input)
        {
            decimal? dInput = GetOraParamDecimal(input);
            if (dInput == null)
                return null;
            return Convert.ToInt32(dInput);
        }

        public static long? GetOraParamInt64(OracleParameter input)
        {
            decimal? dInput = GetOraParamDecimal(input);
            if (dInput == null)
                return null;
            return Convert.ToInt64(dInput);
        }

        public static char? GetOraParamChar(OracleParameter input)
        {
            string dInput = GetOraParamString(input);
            if (dInput == null)
                return null;
            if (dInput.Length == 0)
                return null;
            return dInput.ToCharArray()[0];
        }

        public static Int64 getNextID(OracleConnection connection, OracleTransaction transaction, String schemaName, String tableName, String columnName)
        {
            String sqlStr =
                "Select NVL(Max(" + columnName + "),0) From " + schemaName + "." + tableName + "\n";
            OracleCommand cmd = new OracleCommand(sqlStr, connection);
            return Convert.ToInt64((decimal)cmd.ExecuteScalar()) + 1;
        }

        public static System.Array CreateArrayObject(OracleDataAdapter oAdpt, Type oType)
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();

            DataTable dt = new DataTable();
            oAdpt.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                DataRow[] ArrRow = new DataRow[1];
                ArrRow[0] = dr;
                al.Add(Activator.CreateInstance(oType, ArrRow));
            }
            if (al.Count == 0)
                return null;
            else
                return al.ToArray(oType);
        }

        public static System.Array CreateArrayObject(OracleCommand command, Type oType)
        {
            OracleDataAdapter oAdpt = new OracleDataAdapter(command);
            DataTable dt = new DataTable();
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            oAdpt.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                DataRow[] ArrRow = new DataRow[1];
                ArrRow[0] = dr;
                al.Add(Activator.CreateInstance(oType, ArrRow));
            }
            if (al.Count == 0)
                return null;
            else
                return al.ToArray(oType);
        }

        #region " Get DB Value "
        public static bool DataRowHasValue(DataRow dr, string columnName)
        {
            if (dr == null || !dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
                return false;
            return true;
        }

        public static object GetDBValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (input is string)
                return ((string)input).Trim();
            return input;
        }
        public static object GetDBValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBValue(dr[fieldName]);
        }

        public static DateTime? GetDBDateTimeValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is DateTime))
                return null;
            return (DateTime)input;
        }
        public static DateTime? GetDBDateTimeValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBDateTimeValue(dr[fieldName]);
        }

        public static string GetDBStringValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is string))
                return null;
            if (!string.IsNullOrEmpty((string)input))
                return ((string)input).Trim();
            return (string)input;
        }

        public static string GetDBStringValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBStringValue(dr[fieldName]);
        }
        public static string GetDBStringValueNotTrim(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is string))
                return null;
            if (!string.IsNullOrEmpty((string)input))
                return ((string)input);
            return (string)input;
        }
        public static string GetDBStringValueNotTrim(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBStringValueNotTrim(dr[fieldName]);
        }

        public static char? GetDBCharValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is string))
                return null;
            if (string.IsNullOrEmpty((string)input))
                return null;
            return ((string)input).ToCharArray()[0];
        }
        public static char? GetDBCharValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBCharValue(dr[fieldName]);
        }

        #region " Integer Data Types "
        public static bool GetDBBoolValue(object input, bool defaultValue = false)
        {
            if (input.IsNullOrDBNull())
                return defaultValue;
            if (!(input is bool))
                return defaultValue;
            return Convert.ToBoolean(input);
        }
        public static bool GetDBBoolValue(DataRow dr, string fieldName, bool defaultValue = false)
        {
            if (!DataRowHasValue(dr, fieldName))
                return defaultValue;
            return GetDBBoolValue(dr[fieldName], defaultValue);
        }

        public static byte? GetDBByteValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToByte(input);
        }
        public static byte? GetDBByteValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBByteValue(dr[fieldName]);
        }

        public static byte[] GetDBBytesValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is byte[]))
                return null;
            return (byte[])(input);
        }
        public static byte[] GetDBBytesValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBBytesValue(dr[fieldName]);
        }

        public static short? GetDBInt16Value(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToInt16(input);
        }
        public static short? GetDBInt16Value(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBInt16Value(dr[fieldName]);
        }

        public static short? GetDBShortValue(object input)
        {
            return GetDBInt16Value(input);
        }
        public static short? GetDBShortValue(DataRow dr, string fieldName)
        {
            return GetDBInt16Value(dr, fieldName);
        }

        public static int? GetDBInt32Value(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToInt32(input);
        }
        public static int? GetDBInt32Value(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBInt32Value(dr[fieldName]);
        }

        public static int? GetDBIntValue(object input)
        {
            return GetDBInt32Value(input);
        }
        public static int? GetDBIntValue(DataRow dr, string fieldName)
        {
            return GetDBInt32Value(dr, fieldName);
        }

        public static long? GetDBInt64Value(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToInt64(input);
        }
        public static long? GetDBInt64Value(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBInt64Value(dr[fieldName]);
        }

        public static long? GetDBLongValue(object input)
        {
            return GetDBInt64Value(input);
        }
        public static long? GetDBLongValue(DataRow dr, string fieldName)
        {
            return GetDBInt64Value(dr, fieldName);
        }
        #endregion

        #region " Non-Integer Data Types "
        public static float? GetDBSingleValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToSingle(input);
        }
        public static float? GetDBSingleValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBSingleValue(dr[fieldName]);
        }

        public static float? GetDBFloatValue(object input)
        {
            return GetDBSingleValue(input);
        }
        public static float? GetDBFloatValue(DataRow dr, string fieldName)
        {
            return GetDBSingleValue(dr, fieldName);
        }

        public static double? GetDBDoubleValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToDouble(input);
        }
        public static double? GetDBDoubleValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBDoubleValue(dr[fieldName]);
        }

        public static decimal? GetDBDecimalValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Decimal.Parse(input.ToString(), CultureInfo.CreateSpecificCulture("th-TH"));
        }
        public static decimal? GetDBDecimalValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBDecimalValue(dr[fieldName]);
        }
        #endregion

        public static DateTime GetDBDate(OracleConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            OracleCommand cmd = new OracleCommand("SELECT SYSDATE FROM DUAL", connection);
            return (DateTime)cmd.ExecuteScalar();
        }
        public static DateTime getDB_Date(OracleConnection connection)
        {
            return GetDBDate(connection);
        } /* Obsolate */
        #endregion

        public class DBParameter
        {
            public string NAME { get; set; }
            public object VALUE { get; set; }
            public OracleDbType TYPE { get; set; }
            public int SIZE { get; set; }

            public DBParameter(string IN_NAME, object IN_VALUE, OracleDbType IN_TYPE = OracleDbType.Varchar2, int IN_SIZE = 20)
            {
                this.NAME = IN_NAME;
                if (IN_VALUE == null)
                {
                    this.VALUE = DBNull.Value;
                }
                else if (IN_VALUE is DBNull)
                {
                    this.VALUE = DBNull.Value;

                }
                else

                {
                    this.VALUE = IN_VALUE;
                }

                this.TYPE = IN_TYPE;
                this.SIZE = IN_SIZE;
            }
            public DBParameter(string IN_NAME, object IN_VALUE, OracleDbType IN_TYPE = OracleDbType.Varchar2)
            {
                this.NAME = IN_NAME;
                if (IN_VALUE == null)
                {
                    this.VALUE = DBNull.Value;
                }
                else if (IN_VALUE is DBNull)
                {
                    this.VALUE = DBNull.Value;

                }
                else

                {
                    this.VALUE = IN_VALUE;
                }

                this.TYPE = IN_TYPE;
            }
            public DBParameter(string IN_NAME, object IN_VALUE)
            {
                this.NAME = IN_NAME;
                if (IN_VALUE == null)
                {
                    this.VALUE = DBNull.Value;
                }
                else if (IN_VALUE is DBNull)
                {
                    this.VALUE = DBNull.Value;

                }
                else

                {
                    this.VALUE = IN_VALUE;
                }

                this.TYPE = OracleDbType.Varchar2;
            }
        }
    }


}
