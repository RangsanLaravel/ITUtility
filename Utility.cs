﻿using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Web.Configuration;
using System.Globalization;
using Oracle.ManagedDataAccess.Types;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.Reflection;
using System.ComponentModel;
using System.ServiceModel;
using System.Web;
using System.Threading;

namespace ITUtility
{
    public static partial class Utility
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

                Utility.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
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
                    sIds = sIds + Utility.SQLValueString(id);
                }

                condition = whereColumn + " IN (" + sIds + ")\n";
            }
            else
            {

                condition = whereColumn + " = :v" + param_name;

                Utility.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
            }

            return condition;
        }
        /// <summary>
        /// เลขบัญชีเปิดหน้า 4 หลัง 3
        /// </summary>
        /// <param name="AccNo">เลขบัญชี</param>
        /// <returns>0000XXX000</returns>
        public static string maskAccountNo(string AccNo)
        {
            if (!string.IsNullOrEmpty(AccNo))
            {
                AccNo = AccNo.Replace("-", "").Trim();
                int acc_no_length = AccNo.Length;
                int front_length = 0, back_length = 0;

                if (acc_no_length == 10)
                {
                    front_length = 4;
                    back_length = 3;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }
                else if (acc_no_length == 12)
                {
                    front_length = 4;
                    back_length = 4;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }
                else if (acc_no_length == 15)
                {
                    front_length = 5;
                    back_length = 5;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }

            }
            return AccNo;
        }

        public static string maskIdCard(string IdCard)
        {
            if (!string.IsNullOrEmpty(IdCard))
            {
                IdCard = IdCard.Replace("-", "").Trim();
                if (IdCard.Length != 13)
                {
                    throw new Exception("Incorrect ID Card No.");
                }
                else
                {
                    string temp_IdCard_end = IdCard.Substring(6, 7);
                    IdCard = temp_IdCard_end.PadLeft(13, 'X');
                }
            }
            return IdCard;
        }


        private static string maskAccountDynamix(string AccNo, int acc_no_length, int front_length, int back_length)
        {
            int open_length = front_length + back_length;

            if (acc_no_length > open_length)
            {
                int count_remain = acc_no_length - open_length;
                string temp_accno_start = AccNo.Substring(0, front_length);
                string temp_accno_end = AccNo.Substring(AccNo.Length - back_length, back_length);
                AccNo = temp_accno_start + temp_accno_end.PadLeft(count_remain + back_length, 'X');
            }
            else
            {
                throw new Exception("Account No. Length > " + open_length + " Charector");
            }

            return AccNo;
        }

        public static string Account_No_Format(string acc_no)
        {

            string acc;
            string acc1 = "", acc2 = "", acc3 = "", acc4 = "", acc5 = "";
            if (string.IsNullOrEmpty(acc_no)) return acc_no;

            acc1 = acc_no.Substring(0, 3);
            acc2 = acc_no.Substring(3, 1);
            acc3 = acc_no.Substring(4, 5);
            acc4 = acc_no.Substring(9, 1);

            if (acc_no.Count() > 10)
            {
                acc5 = acc_no.Substring(10, acc_no.Count() - 10);
                if (acc2.Count() > 5) acc2 = acc2.Substring(0, 5);
                acc = string.Format("{0}{1}{2}{3}{4}", acc1, acc2, acc3, acc4, acc5);
            }
            else
            {

                acc = string.Format("{0}-{1}-{2}-{3}", acc1, acc2, acc3, acc4);
            }

            return acc;
        }

        /// <summary>
        /// จัด format เลขบัตรเครดิต
        /// </summary>
        /// <param name="CardNo">เลขบัตรเครดิต</param>
        /// <returns>รูปแบบ 0000-0000-0000-0000</returns>
        public static string CreditCardNumberFormat(string CardNo)
        {
            string new_cardno = "";

            if (!string.IsNullOrEmpty(CardNo))
            {
                if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
                for (int n = 4; n < CardNo.Length; n += 4)
                {
                    new_cardno = CardNo.Insert(n, "-");
                    n += 1;
                    CardNo = new_cardno;
                }
            }
            return new_cardno;
        }
        /// <summary>
        /// เปิดหน้า 6 หลัง 4
        /// </summary>
        /// <param name="CardNo">เลขบัตรเครดิต</param>
        /// <returns>เลขบัตรเครดิต mask X เปิดหน้า 6 หลัง 4</returns>
        public static string maskCreditCard(string CardNo)
        {
            if (!string.IsNullOrEmpty(CardNo))
            {
                if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
                int card_no_length = CardNo.Length;
                if (card_no_length > 10)
                {
                    int count_remain = card_no_length - 10;
                    string temp_cardno_start = CardNo.Substring(0, 6);
                    string temp_cardno_end = CardNo.Substring(CardNo.Length - 4, 4);
                    CardNo = temp_cardno_start + temp_cardno_end.PadLeft(count_remain + 4, 'X');
                }
            }
            return CardNo;
        }




        ///// <summary>
        ///// เลขบัญชีเปิดหน้า 4 หลัง 3
        ///// </summary>
        ///// <param name="AccNo">เลขบัญชี</param>
        ///// <returns>0000XXX000</returns>
        //public static string maskAccountNo(string AccNo)
        //{
        //    if (!string.IsNullOrEmpty(AccNo))
        //    {
        //        int acc_no_length = AccNo.Length;
        //        int open_length = 7;
        //        if (acc_no_length > open_length)
        //        {
        //            int count_remain = acc_no_length - open_length;
        //            string temp_accno_start = AccNo.Substring(0, 4);
        //            string temp_accno_end = AccNo.Substring(AccNo.Length - 3, 3);
        //            AccNo = temp_accno_start + temp_accno_end.PadLeft(count_remain, 'X');
        //        }
        //        else
        //        {
        //            throw new Exception("Account No. Length > 7 Charector");
        //        }
        //    }
        //    return AccNo;
        //}



        ///// <summary>
        ///// จัด format เลขบัตรเครดิต
        ///// </summary>
        ///// <param name="CardNo">เลขบัตรเครดิต</param>
        ///// <returns>รูปแบบ 0000-0000-0000-0000</returns>
        //public static string CreditCardNumberFormat(string CardNo)
        //{
        //    string new_cardno = "";

        //    if (!string.IsNullOrEmpty(CardNo))
        //    {
        //        if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
        //        for (int n = 4; n < CardNo.Length; n += 4)
        //        {
        //            new_cardno = CardNo.Insert(n, "-");
        //            n += 1;
        //            CardNo = new_cardno;
        //        }
        //    }
        //    return new_cardno;
        //}
        ///// <summary>
        ///// เปิดหน้า 6 หลัง 4
        ///// </summary>
        ///// <param name="CardNo">เลขบัตรเครดิต</param>
        ///// <returns>เลขบัตรเครดิต mask X เปิดหน้า 6 หลัง 4</returns>
        //public static string maskCreditCard(string CardNo)
        //{
        //    if (!string.IsNullOrEmpty(CardNo))
        //    {
        //        if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
        //        int card_no_length = CardNo.Length;
        //        if (card_no_length > 10)
        //        {
        //            int count_remain = card_no_length - 10;
        //            string temp_cardno_start = CardNo.Substring(0, 6);
        //            string temp_cardno_end = CardNo.Substring(CardNo.Length - 4, 4);
        //            CardNo = temp_cardno_start + temp_cardno_end.PadLeft(count_remain, 'X');
        //        }
        //    }
        //    return CardNo;
        //}



        public static DateTime javascriptStartDate
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, 0); }
        }

        public static DateTime convertJavascriptStartDate(DateTime dateFromJavascript)
        {
            if (dateFromJavascript == javascriptStartDate)
                return new DateTime();
            else
                return dateFromJavascript;
        }

        public static Object iif(Boolean condition, Object trueValue, Object falseValue)
        {
            if (condition)
                return trueValue;
            else
                return falseValue;
        }

        public static DateTime beginOfDay(DateTime refDateTime)
        {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 0, 0, 0);
        }
        public static DateTime endOfDay(DateTime refDateTime)
        {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 23, 59, 59, 999);
        }

        public static Int64 Int64TryParse(String sNumber)
        {
            Int64 returnValue;
            try
            {
                returnValue = Int64.Parse(sNumber);
            }
            catch
            {
                returnValue = 0;
            }
            return returnValue;
        }

        public static Int32 Int32TryParse(String sNumber)
        {
            Int32 returnValue;
            try
            {
                returnValue = Int32.Parse(sNumber);
            }
            catch
            {
                returnValue = 0;
            }
            return returnValue;
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

        public static Exception ManageException(Exception exception)
        {
            if (exception is OracleException)
            {
                return new Exception("BangkokLife Exception : EX-00001 Internal Exception");
            }
            else if (exception.Message.ToUpper().Contains("ORA-"))
            {
                return new Exception("BangkokLife Exception : EX-00001 Internal Exception");
            }
            else
            {
                return exception;
            }
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

        #region " Get AppSetting & ConnectionString "
        public static string ConnectionStrings(string connectionName)
        {
            if (!string.IsNullOrEmpty(connectionName))
            {
                System.Configuration.ConnectionStringSettingsCollection _ConnectionStringSettingsCollection;

                _ConnectionStringSettingsCollection = System.Web.Configuration.WebConfigurationManager.ConnectionStrings;
                if (_ConnectionStringSettingsCollection != null && _ConnectionStringSettingsCollection[connectionName] != null)
                    return _ConnectionStringSettingsCollection[connectionName].ConnectionString;

                _ConnectionStringSettingsCollection = System.Configuration.ConfigurationManager.ConnectionStrings;
                if (_ConnectionStringSettingsCollection != null && _ConnectionStringSettingsCollection[connectionName] != null)
                    return _ConnectionStringSettingsCollection[connectionName].ConnectionString;
            }
            return null;
        }
        public static string ConnectionStrings(int index)
        {
            if (index >= 0)
            {
                System.Configuration.ConnectionStringSettingsCollection _ConnectionStringSettingsCollection;

                _ConnectionStringSettingsCollection = System.Web.Configuration.WebConfigurationManager.ConnectionStrings;
                if (_ConnectionStringSettingsCollection != null && _ConnectionStringSettingsCollection[index] != null)
                    return _ConnectionStringSettingsCollection[index].ConnectionString;

                _ConnectionStringSettingsCollection = System.Configuration.ConfigurationManager.ConnectionStrings;
                if (_ConnectionStringSettingsCollection != null && _ConnectionStringSettingsCollection[index] != null)
                    return _ConnectionStringSettingsCollection[index].ConnectionString;
            }
            return null;
        }
        public static string ConnectionString(string connectionName)
        {
            return ConnectionStrings(connectionName);
        }
        public static string ConnectionString(int index)
        {
            return ConnectionStrings(index);
        }

        public static string AppSettings(string keyName)
        {
            if (!string.IsNullOrEmpty(keyName))
            {
                System.Collections.Specialized.NameValueCollection _AppSettingCollection;

                _AppSettingCollection = System.Web.Configuration.WebConfigurationManager.AppSettings;
                if (_AppSettingCollection != null && _AppSettingCollection[keyName] != null)
                    return _AppSettingCollection[keyName];

                _AppSettingCollection = System.Configuration.ConfigurationManager.AppSettings;
                if (_AppSettingCollection != null && _AppSettingCollection[keyName] != null)
                    return _AppSettingCollection[keyName];
            }
            return null;
        }
        public static string AppSettings(int index)
        {
            if (index >= 0)
            {
                System.Collections.Specialized.NameValueCollection _AppSettingCollection;

                _AppSettingCollection = System.Web.Configuration.WebConfigurationManager.AppSettings;
                if (_AppSettingCollection != null && _AppSettingCollection[index] != null)
                    return _AppSettingCollection[index];

                _AppSettingCollection = System.Configuration.ConfigurationManager.AppSettings;
                if (_AppSettingCollection != null && _AppSettingCollection[index] != null)
                    return _AppSettingCollection[index];
            }
            return null;
        }
        public static string AppSetting(string keyName)
        {
            return AppSettings(keyName);
        }
        public static string AppSetting(int index)
        {
            return AppSettings(index);
        }

        public enum ServerType
        {
            Unknown = 0,
            Develop = 1,
            PreUAT = 2,
            UAT = 3,
            Intranet = 4,
            Production = 99
        }
        public static ServerType GetServerType()
        {
            string _ServerType = AppSettings("ServerType") ?? "NONE";
            switch (_ServerType.ToUpper())
            {
                case "NONE":
                    return ServerType.Unknown;
                case "DEV":
                case "DEVELOP":
                    return ServerType.Develop;
                case "PRE":
                case "PREUAT":
                    return ServerType.PreUAT;
                case "UAT":
                    return ServerType.UAT;
                case "PRD":
                case "PRODUCTION":
                    return ServerType.Production;
                case "Intranet":
                    return ServerType.Production;
                default:
                    return ServerType.Unknown;
            }
        }
        #endregion

        #region " SQL - Where cause "
        /// <summary>
        /// To SQL where condition date.
        /// </summary>
        /// <param name="thaiDateString">e.g. "31/12/2559"</param>
        /// <returns>SQL: TO_DATE(...)</returns>
        public static string SQLFromThaiDate(string thaiDateString)
        {
            if (string.IsNullOrEmpty(thaiDateString))
                return "Null";

            return string.Format("TO_DATE('{0}','DD/MM/YYYY', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI')", thaiDateString);
        }

        /// <summary>
        /// To SQL where condition datetime.
        /// </summary>
        /// <param name="thaiDateTimeString">e.g. "31/12/2559 23:59:59"</param>
        /// <returns>SQL: TO_DATE(...)</returns>
        public static string SQLFromThaiDateTime(string thaiDateTimeString)
        {
            if (string.IsNullOrEmpty(thaiDateTimeString))
                return "Null";

            return string.Format("TO_DATE('{0}','DD/MM/YYYY  HH24:MI:SS', 'NLS_CALENDAR=''THAI BUDDHA'' NLS_DATE_LANGUAGE=THAI')", thaiDateTimeString);
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

        //public static string SQLValueString(object input)
        //{
        //    string returnStr = "";
        //    if (input == null)
        //    {
        //        returnStr = "Null";
        //    }
        //    else if (input is DBNull)
        //    {
        //        returnStr = "Null";
        //    }
        //    else
        //    {
        //        if (input is string)
        //        {

        //            if ((string)input == "")
        //            {
        //                returnStr = "Null";
        //            }
        //            else
        //            {
        //                returnStr = "'" + ((string)input).Replace("'", "''") + "'";
        //            }
        //        }
        //        else if (input is Char)
        //        {
        //            Char rtnChr = (Char)input;
        //            string rtnStr = rtnChr.ToString();
        //            rtnStr = rtnStr.Trim();
        //            if (rtnStr == "")
        //            {
        //                returnStr = "Null";
        //            }
        //            else
        //            {
        //                returnStr = "'" + rtnStr.Replace("'", "''") + "'";
        //            }
        //        }
        //        else if (input is Int16)
        //        {
        //            returnStr = ((Int16)input).ToString();
        //        }
        //        else if (input is Int32)
        //        {
        //            returnStr = ((Int32)input).ToString();
        //        }
        //        else if (input is Int64)
        //        {
        //            returnStr = ((Int64)input).ToString();
        //        }
        //        else if (input is Double)
        //        {
        //            returnStr = ((Double)input).ToString();
        //        }
        //        else if (input is Decimal)
        //        {
        //            returnStr = ((Decimal)input).ToString();
        //        }
        //        else if (input is Single)
        //        {
        //            returnStr = ((Single)input).ToString();
        //        }
        //        else if (input is DateTime)
        //        {
        //            returnStr = SQLDate((DateTime)input);
        //        }
        //        else
        //        {
        //            returnStr = input.ToString();
        //        }
        //    }
        //    return returnStr;
        //}
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
        public static String SQLValueStringN(Object input)
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
                        var asciiBytesCount = Encoding.ASCII.GetByteCount((String)input);
                        var unicodBytesCount = Encoding.UTF8.GetByteCount((String)input);
                        returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                        if (asciiBytesCount == unicodBytesCount)
                        {
                            returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                        }
                        else
                        {
                            returnStr = "N'" + ((String)input).Replace("'", "''") + "'";
                        }
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

                        returnStr = "N'" + rtnStr.Replace("'", "''") + "'";
                        var asciiBytesCount = Encoding.ASCII.GetByteCount(rtnStr);
                        var unicodBytesCount = Encoding.UTF8.GetByteCount(rtnStr);

                        if (asciiBytesCount == unicodBytesCount)
                        {
                            returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                        }
                        else
                        {
                            returnStr = "N'" + rtnStr.Replace("'", "''") + "'";
                        }


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
                        paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
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
        public static void SQLValueString(List<DBParameter> paramList, string name, object input, int size = 50)
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
                        paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
                    }
                    else
                    {
                        returnStr = "'" + ((string)input).Replace("'", "''") + "'";

                        paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2, size));
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
                        paramList.Add(new DBParameter(name, input, OracleDbType.Char, size));
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();

                    paramList.Add(new DBParameter(name, input, OracleDbType.Int16, size));
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int32, size));
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int64, size));
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Double, size));
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Decimal, size));
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Single, size));
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                    paramList.Add(new DBParameter(name, input, OracleDbType.Date, size));
                }
                else
                {
                    returnStr = input.ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2, size));
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

        public static System.Data.DbType getDbType(object item, ref OracleParameter oParam)
        {
            var typeMap = new Dictionary<Type, DbType>();

            if (item is byte) return DbType.Byte;
            if (item is sbyte) return DbType.SByte;
            if (item is short) return DbType.Int16;
            if (item is ushort) return DbType.UInt16;
            if (item is int) return DbType.Int32;
            if (item is uint) return DbType.UInt32;
            if (item is long) return DbType.Int64;
            if (item is ulong) return DbType.UInt64;
            if (item is float) return DbType.Single;
            if (item is double) return DbType.Double;
            if (item is decimal) return DbType.Decimal;
            if (item is bool) return DbType.Boolean;
            if (item is string) return DbType.String;
            if (item is char) { oParam.Size = 1; return DbType.StringFixedLength; }
            if (item is Guid) { return DbType.Guid; }
            if (item is DateTime) { return DbType.DateTime; }
            if (item is DateTimeOffset) { return DbType.DateTimeOffset; }
            if (item is byte[]) { return DbType.Binary; }
            if (item is byte?) { return DbType.Byte; }
            if (item is sbyte?) { return DbType.SByte; }
            if (item is short?) { return DbType.Int16; }
            if (item is ushort?) { return DbType.UInt16; }
            if (item is int?) { return DbType.Int32; }
            if (item is uint?) { return DbType.UInt32; }
            if (item is long?) { return DbType.Int64; }
            if (item is ulong?) { return DbType.UInt64; }
            if (item is float?) { return DbType.Single; }
            if (item is double?) { return DbType.Double; }
            if (item is decimal?) { return DbType.Decimal; }
            if (item is bool?) { return DbType.Boolean; }
            if (item is char?) { oParam.Size = 1; return DbType.StringFixedLength; }
            if (item is Guid?) { return DbType.Guid; }
            if (item is DateTime?) { return DbType.DateTime; }
            if (item is DateTimeOffset?) { return DbType.DateTimeOffset; }
            //if(item is System.Data.Linq.Binary) { return DbType.Binary; }

            return DbType.Object;
        }

        public static string SQLBindInValue(ref OracleCommand oCmd, ref int BindCount, Array values)
        {
            if (values == null || values.Length == 0)
                return "('')";
            string result = "(";
            foreach (object input in values)
                if (result.Length == 1)
                    result = result + SQLBindValue(ref oCmd, ref BindCount, input);
                else
                    result = result + "," + SQLBindValue(ref oCmd, ref BindCount, input);

            result = result + ")";

            return result;
        }

        public static string SQLBindValue(ref OracleCommand oCmd, ref int BindCount, object Value)
        {
            BindCount++;

            OracleParameter oParam = new OracleParameter();
            oParam.DbType = getDbType(Value, ref oParam);
            oParam.Value = Value;

            oCmd.Parameters.Add(oParam);

            return ":" + BindCount.ToString();
        }

        public enum EnumSQLLikeMode
        {
            Both = 0,
            Left = 1,
            Right = 2
        }
        public static string SQLValueLikeString(string input, EnumSQLLikeMode likemode = EnumSQLLikeMode.Both)
        {
            if (string.IsNullOrEmpty(input))
                return "''";
            switch (likemode)
            {
                case EnumSQLLikeMode.Both:
                    return SQLValueString("%" + input + "%");
                case EnumSQLLikeMode.Left:
                    return SQLValueString("%" + input);
                case EnumSQLLikeMode.Right:
                    return SQLValueString(input + "%");
                default: return SQLValueString(input);
            }
        }
        #endregion

        #region " MD5 "
        public static string GET_MD5(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath) || !System.IO.File.Exists(FilePath))
                return null;
            using (var md5 = MD5.Create())
            {
                using (System.IO.FileStream stream = System.IO.File.OpenRead(FilePath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                }
            }
        }
        public static bool CHECK_MD5(string MD5First, string MD5Second)
        {
            if (String.IsNullOrEmpty(MD5First) || String.IsNullOrEmpty(MD5Second))
                return false;
            if (MD5First.Length != MD5Second.Length)
                return false;
            if (MD5First.ToLower().Equals(MD5Second.ToLower()))
                return true;
            return false;
        }
        #endregion

        #region " Excel Convertor "
        public static int GetYearDiffBySpecificCulture(CultureInfo TargetCulture, CultureInfo OriginalCulture = null)
        {
            if (OriginalCulture == null)
                OriginalCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            DateTime _DateTime = new DateTime();
            int OriginalYear = Convert.ToInt32(_DateTime.ToString("yyyy", OriginalCulture));
            int TargetYear = Convert.ToInt32(_DateTime.ToString("yyyy", TargetCulture));
            return OriginalYear - TargetYear;
        }
        public static int GetYearDiffBySpecificCulture(string stringTargetCulture, string stringOriginalCulture = null)
        {
            CultureInfo TargetCulture = System.Globalization.CultureInfo.CreateSpecificCulture(stringTargetCulture);
            CultureInfo OriginalCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US"); ;
            if (!string.IsNullOrEmpty(stringOriginalCulture))
                OriginalCulture = System.Globalization.CultureInfo.CreateSpecificCulture(stringOriginalCulture);
            return GetYearDiffBySpecificCulture(TargetCulture, OriginalCulture);
        }

        public static DateTime? excelToDateTime(object excelField)
        {
            DateTime? r = null;
            try
            {
                r = DateTime.FromOADate((double)excelField);
                goto _r;
            }
            catch { }

            try
            {
                r = Convert.ToDateTime((string)excelField);
                goto _r;
            }
            catch { }

            try
            {
                r = (DateTime)excelField;
                goto _r;
            }
            catch { }

        _r:
            return r;
        }
        public static DateTime? excelToDateTime(object excelField, string stringInputCulture)
        {
            DateTime? r = null;

            try
            {
                r = DateTime.FromOADate((double)excelField);
                r = r.Value.AddYears(GetYearDiffBySpecificCulture(stringInputCulture));
                goto _r;
            }
            catch { }

            try
            {
                r = Convert.ToDateTime((string)excelField, System.Globalization.CultureInfo.CreateSpecificCulture(stringInputCulture));
                goto _r;
            }
            catch { }

            try
            {
                r = (DateTime)excelField;
                r = r.Value.AddYears(GetYearDiffBySpecificCulture(stringInputCulture));
                goto _r;
            }
            catch { }

        _r:
            return r;
        }
        public static DateTime? excelToDateTime(object excelField, CultureInfo inputCulture)
        {
            DateTime? r = null;

            try
            {
                r = DateTime.FromOADate((double)excelField);
                r = r.Value.AddYears(GetYearDiffBySpecificCulture(inputCulture));
                goto _r;
            }
            catch { }

            try
            {
                r = Convert.ToDateTime((string)excelField, inputCulture);
                goto _r;
            }
            catch { }

            try
            {
                r = (DateTime)excelField;
                r = r.Value.AddYears(GetYearDiffBySpecificCulture(inputCulture));
                goto _r;
            }
            catch { }

        _r:
            return r;
        }
        #endregion

        #region " <Dynamic> JsonConverter "
        public static object DeserializeToObject(string jsonString, Type objectType)
        {
            if (jsonString.IsNullOrDBNullOrEmpty())
                return null;
            return new JavaScriptSerializer().Deserialize(jsonString, objectType);
        }
        public static string SerializeObjectToJsonString(object thisObject)
        {
            if (thisObject == null)
                return "{}";
            return new JavaScriptSerializer().Serialize(thisObject);
        }
        public static object DeserializeToExpandoObject(string jsonString)
        {
            if (jsonString.IsNullOrDBNullOrEmpty())
                return null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            return serializer.Deserialize(jsonString, typeof(ExpandoObject));
        }
        public static string SerializeExpandoObjectToJsonString(object expandoObject)
        {
            if (expandoObject == null)
                return "{}";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            return serializer.Serialize(expandoObject);
        }
        #endregion

        #region " <Dynamic> Base64Converter "
        public static object DeserializeFromBase64String(string base64String, Type objectType)
        {
            if (base64String.IsNullOrDBNullOrEmpty())
                return null;
            return DeserializeToObject(Encoding.UTF8.GetString(Convert.FromBase64String(base64String)), objectType);
        }
        public static string SerializeToBase64String(object thisObject)
        {
            if (thisObject == null)
                return string.Empty;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeObjectToJsonString(thisObject)));
        }
        #endregion

        public static String dateTimeToString(DateTime input)
        {
            String returnStr = "";
            returnStr = input.Day.ToString() + "/" + input.Month.ToString() + "/" + input.Year.ToString() + " " + input.Hour.ToString() + ":" + input.Minute.ToString() + ":" + input.Second.ToString() + "." + input.Millisecond.ToString();
            return returnStr;
        }
        public static String dateTimeToString(DateTime? inputDate, string format, string era)
        {
            if (inputDate == null) return "";
            else return dateTimeToString(inputDate.Value, format, era);
        }
        public static String dateTimeToString(DateTime inputDate, string format, string era)
        {
            DateTime dateIn = inputDate;

            List<string> innerLs = new List<string>();
            char[] cs = format.ToCharArray();
            List<char> lc = new List<char>();
            List<char> innerLc = null;

            bool inner = false;

            foreach (char c in cs)
            {
                if (!inner)
                {
                    if (c != '[')
                    {
                        lc.Add(c);
                    }
                    else
                    { //inner start
                        inner = true;
                        innerLc = new List<char>();
                        lc.AddRange(("[" + innerLs.Count().ToString() + "]").ToCharArray());
                    }
                }
                else
                { //inner zone
                    if (c == ']')
                    {//inner end
                        innerLs.Add(new string(innerLc.ToArray()));
                        inner = false;
                        innerLc = null;
                    }
                    else
                    {
                        innerLc.Add(c);
                    }
                }
            }
            if (innerLc != null)
            {
                innerLs.Add(new string(innerLc.ToArray()));
            }

            string s1 = new string(lc.ToArray());
            string s2 = oldDateTimeToString(dateIn, s1, era);
            foreach (string innerS in innerLs)
            {
                s2 = s2.Replace("[" + innerLs.IndexOf(innerS).ToString() + "]", innerS);
            }
            return s2;
        }

        private static string oldDateTimeToString(DateTime inputDate, string format, string era)
        {
            string returnStr = "";

            if (format == "o")
                return inputDate.ToString("o");

            if (era == null) era = "";
            bool AD_era = true;
            string ERA = era.ToUpper();
            if (ERA == "BU" || ERA == "B.U." || ERA == "B.U")
                AD_era = false;


            string sDay = inputDate.Day.ToString().PadLeft(2, '0');
            string sMonth = inputDate.Month.ToString().PadLeft(2, '0');
            string sYear;
            if (!AD_era)
                sYear = (inputDate.Year + 543).ToString().PadLeft(4, '0');
            else
                sYear = inputDate.Year.ToString().PadLeft(4, '0');
            string sHour = inputDate.Hour.ToString().PadLeft(2, '0');
            string sMinute = inputDate.Minute.ToString().PadLeft(2, '0');
            string sSecond = inputDate.Second.ToString().PadLeft(2, '0');
            string sMilliSecond = inputDate.Millisecond.ToString().PadLeft(2, '0');
            #region :: Full Thai Month ::
            string tMonthName = "";
            switch (inputDate.Month)
            {
                case 1:
                    tMonthName = "มกราคม";
                    break;
                case 2:
                    tMonthName = "กุมภาพันธ์";
                    break;
                case 3:
                    tMonthName = "มีนาคม";
                    break;
                case 4:
                    tMonthName = "เมษายน";
                    break;
                case 5:
                    tMonthName = "พฤษภาคม";
                    break;
                case 6:
                    tMonthName = "มิถุนายน";
                    break;
                case 7:
                    tMonthName = "กรกฎาคม";
                    break;
                case 8:
                    tMonthName = "สิงหาคม";
                    break;
                case 9:
                    tMonthName = "กันยายน";
                    break;
                case 10:
                    tMonthName = "ตุลาคม";
                    break;
                case 11:
                    tMonthName = "พฤศจิกายน";
                    break;
                case 12:
                    tMonthName = "ธันวาคม";
                    break;
            }
            #endregion
            #region :: Short Thai Month ::
            string tShortMonthName = "";
            switch (inputDate.Month)
            {
                case 1:
                    tShortMonthName = "ม.ค.";
                    break;
                case 2:
                    tShortMonthName = "ก.พ.";
                    break;
                case 3:
                    tShortMonthName = "มี.ค.";
                    break;
                case 4:
                    tShortMonthName = "เม.ย.";
                    break;
                case 5:
                    tShortMonthName = "พ.ค.";
                    break;
                case 6:
                    tShortMonthName = "มิ.ย.";
                    break;
                case 7:
                    tShortMonthName = "ก.ค.";
                    break;
                case 8:
                    tShortMonthName = "ส.ค.";
                    break;
                case 9:
                    tShortMonthName = "ก.ย.";
                    break;
                case 10:
                    tShortMonthName = "ต.ค.";
                    break;
                case 11:
                    tShortMonthName = "พ.ย.";
                    break;
                case 12:
                    tShortMonthName = "ธ.ค.";
                    break;
            }
            #endregion

            #region :: Set Format ::
            int index = 0;
            List<string> listDateTimeValue = new List<string>();
            #region :: Day ::

            if (format.IndexOf("dddd") > -1)
            {
                format = format.Replace("dddd", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("dddd", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("dddd", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("ddd") > -1)
            {
                format = format.Replace("ddd", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("ddd", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("ddd", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("dd") > -1)
            {
                format = format.Replace("dd", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sDay);
                index++;
            }
            if (format.IndexOf("d") > -1)
            {
                format = format.Replace("d", "{" + index.ToString() + "}");
                listDateTimeValue.Add(inputDate.Day.ToString());
                index++;
            }
            #endregion
            #region :: Month ::
            if (format.IndexOf("MMMM") > -1)
            {
                format = format.Replace("MMMM", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("MMMM", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("MMM") > -1)
            {
                format = format.Replace("MMM", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("MMM", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("MMM", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("MM") > -1)
            {
                format = format.Replace("MM", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMonth);
                index++;
            }
            if (format.IndexOf("Month") > -1)
            {
                format = format.Replace("Month", "{" + index.ToString() + "}");
                listDateTimeValue.Add(tMonthName);
                index++;
            }
            if (format.IndexOf("M") > -1)
            {
                format = format.Replace("M", "{" + index.ToString() + "}");
                listDateTimeValue.Add(inputDate.Month.ToString());
                index++;
            }
            if (format.IndexOf("m.m.") > -1)
            {
                format = format.Replace("m.m.", "{" + index.ToString() + "}");
                listDateTimeValue.Add(tShortMonthName);
                index++;
            }
            #endregion
            #region :: Year ::
            if (format.IndexOf("yyyy") > -1)
            {
                format = format.Replace("yyyy", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("yyyy", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("yyyy", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("yyy") > -1)
            {

            }
            if (format.IndexOf("yy") > -1)
            {
                format = format.Replace("yy", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("yy", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("yy", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            #endregion
            #region :: Hour ::
            if (format.IndexOf("hh") > -1)
            {
                format = format.Replace("hh", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sHour);
                index++;
            }
            #endregion
            #region :: Minute ::
            if (format.IndexOf("mi") > -1)
            {
                format = format.Replace("mi", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMinute);
                index++;
            }
            #endregion
            #region :: Second ::
            if (format.IndexOf("ss") > -1)
            {
                format = format.Replace("ss", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sSecond);
                index++;
            }
            #endregion
            #region :: Millisecond ::
            if (format.IndexOf("ms") > -1)
            {
                format = format.Replace("ms", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMilliSecond);
                index++;
            }
            #endregion
            #region :: Replace Format ::
            returnStr = format;
            if (listDateTimeValue != null && listDateTimeValue.Any())
            {
                for (int i = 0; i < listDateTimeValue.Count; i++)
                {
                    returnStr = returnStr.Replace("{" + i.ToString() + "}", listDateTimeValue[i]);
                }
            }
            #endregion
            #endregion
            return returnStr;
        }

        public static String dateTimeToString()
        {
            DateTime input;
            input = DateTime.Now;
            String returnStr = "";
            returnStr = input.Day.ToString() + "/" + input.Month.ToString() + "/" + input.Year.ToString() + " " + input.Hour.ToString() + ":" + input.Minute.ToString() + ":" + input.Second.ToString() + "." + input.Millisecond.ToString();
            return returnStr;
        }

        public static DataTable SelectDistinct(DataTable SourceTable, String OrderField)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            String[] FieldNames = OrderField.Split(",".ToCharArray());
            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                    setLastValues(lastValues, row, FieldNames);
                }
            }

            return newTable;
        }

        public static DataTable SelectToNewTable(DataTable SourceTable, String condition, String sort)
        {
            if (SourceTable == null)
                return null;

            DataTable newTable = new DataTable();
            foreach (DataColumn col in SourceTable.Columns)
                newTable.Columns.Add(col.ColumnName, col.DataType);
            DataRow[] drArr = SourceTable.Select(condition);
            foreach (DataRow dr in drArr)
            {
                DataRow newRow = newTable.NewRow();
                foreach (DataColumn col in newTable.Columns)
                {
                    newRow[col.ColumnName] = dr[col.ColumnName];
                }
                newTable.Rows.Add(newRow);
            }
            return newTable;
        }

        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }

        public static String HTMLCharEncode(String input)
        {
            if (input == null)
            {
                return "";
            }
            else
            {
                String sReturn = input;
                sReturn = sReturn.Replace("&", "&amp;");
                sReturn = sReturn.Replace("<", "&lt;");
                sReturn = sReturn.Replace(">", "&gt;");
                sReturn = sReturn.Replace("\"", "&quot;");
                sReturn = sReturn.Replace(Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString(), "<br />");
                sReturn = sReturn.Replace(Convert.ToChar(13).ToString(), "<br />");
                return sReturn;
            }
        }

        public static String XMLCharEncode(String input)
        {
            if (input == null)
            {
                return "";
            }
            else
            {
                String sReturn = input;
                sReturn = sReturn.Replace("&", "&amp;");
                sReturn = sReturn.Replace("<", "&lt;");
                sReturn = sReturn.Replace(">", "&gt;");
                sReturn = sReturn.Replace(Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString(), "<![CDATA[\n]]>");
                return sReturn;
            }
        }

        public static DataFile UploadToDataFile(System.Web.HttpPostedFile postFile)
        {
            DataFile returnValue = new DataFile();
            Int32 fileLength = postFile.ContentLength;
            Byte[] dataByte = new Byte[fileLength];
            postFile.InputStream.Read(dataByte, 0, fileLength);
            returnValue.FileName = System.IO.Path.GetFileName(postFile.FileName);
            returnValue.ContentType = postFile.ContentType;
            returnValue.FileData = dataByte;
            return returnValue;
        }




        public static String randomString()
        {
            DateTime refDate = DateTime.Today;
            Random oRnd = new Random();
            return (Double.Parse(refDate.Year.ToString() + refDate.Month.ToString().PadLeft(2, '0') + refDate.Day.ToString().PadLeft(2, '0')) + oRnd.NextDouble()).ToString();
        }

        public static Boolean verifyIPAddress(String sIPAddress)
        {
            try
            {
                System.Net.IPAddress oIPAddress = System.Net.IPAddress.Parse(sIPAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Int64 getNextID(OracleConnection connection, OracleTransaction transaction, String schemaName, String tableName, String columnName)
        {
            String sqlStr =
                "Select NVL(Max(" + columnName + "),0) From " + schemaName + "." + tableName + "\n";
            OracleCommand cmd = new OracleCommand(sqlStr, connection);
            return Convert.ToInt64((decimal)cmd.ExecuteScalar()) + 1;
        }

        public static String w874ToUTF8(String sInput)
        {
            System.Text.Encoding w874 = System.Text.Encoding.GetEncoding("windows-874");
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] unicodeBytes = w874.GetBytes(sInput);
            return utf8.GetString(unicodeBytes);
        }

        public static String[] Split(String source, String devider)
        {
            return System.Text.RegularExpressions.Regex.Split(source, System.Text.RegularExpressions.Regex.Escape(devider));
        }

        public static String BuDateTimeString(DateTime refDate)
        {
            return
                refDate.Day.ToString().PadLeft(2, '0') + "/" +
                refDate.Month.ToString().PadLeft(2, '0') + "/" +
                (refDate.Year + 543).ToString() + " " +
                refDate.Hour.ToString().PadLeft(2, '0') + ":" +
                refDate.Minute.ToString().PadLeft(2, '0') + ":" +
                refDate.Second.ToString().PadLeft(2, '0');
        }

        public static String BuDateString(DateTime refDate)
        {
            return
                refDate.Day.ToString().PadLeft(2, '0') + "/" +
                refDate.Month.ToString().PadLeft(2, '0') + "/" +
                (refDate.Year + 543).ToString();
        }



        public static string getThaiMonth(int iMonth)
        {
            string returnValue = "";
            switch (iMonth)
            {
                case 1:
                    returnValue = "มกราคม";
                    break;
                case 2:
                    returnValue = "กุมภาพันธ์";
                    break;
                case 3:
                    returnValue = "มีนาคม";
                    break;
                case 4:
                    returnValue = "เมษายน";
                    break;
                case 5:
                    returnValue = "พฤษภาคม";
                    break;
                case 6:
                    returnValue = "มิถุนายน";
                    break;
                case 7:
                    returnValue = "กรกฎาคม";
                    break;
                case 8:
                    returnValue = "สิงหาคม";
                    break;
                case 9:
                    returnValue = "กันยายน";
                    break;
                case 10:
                    returnValue = "ตุลาคม";
                    break;
                case 11:
                    returnValue = "พฤศจิกายน";
                    break;
                case 12:
                    returnValue = "ธันวาคม";
                    break;
            }
            return returnValue;
        }
        public static string getThaiShortMonth(int month)
        {
            string returnValue = "";
            switch (month)
            {
                case 1:
                    returnValue = "ม.ค";
                    break;
                case 2:
                    returnValue = "ก.พ";
                    break;
                case 3:
                    returnValue = "มี.ค";
                    break;
                case 4:
                    returnValue = "เม.ย";
                    break;
                case 5:
                    returnValue = "พ.ค";
                    break;
                case 6:
                    returnValue = "มิ.ย";
                    break;
                case 7:
                    returnValue = "ก.ค";
                    break;
                case 8:
                    returnValue = "ส.ค";
                    break;
                case 9:
                    returnValue = "ก.ย";
                    break;
                case 10:
                    returnValue = "ต.ค";
                    break;
                case 11:
                    returnValue = "พ.ย";
                    break;
                case 12:
                    returnValue = "ธ.ค";
                    break;
            }
            return returnValue;
        }
        public static string GetThaiMonth(string myMonth)
        {
            string NameMonth;
            switch (myMonth)
            {
                case "00":
                    NameMonth = "ทุกเดือน";
                    break;
                case "01":
                    NameMonth = "มกราคม";
                    break;
                case "02":
                    NameMonth = "กุมภาพันธ์";
                    break;
                case "03":
                    NameMonth = "มีนาคม";
                    break;
                case "04":
                    NameMonth = "เมษายน";
                    break;
                case "05":
                    NameMonth = "พฤษภาคม";
                    break;
                case "06":
                    NameMonth = "มิถุนายน";
                    break;
                case "07":
                    NameMonth = "กรกฎาคม";
                    break;
                case "08":
                    NameMonth = "สิงหาคม";
                    break;
                case "09":
                    NameMonth = "กันยายน";
                    break;
                case "10":
                    NameMonth = "ตุลาคม";
                    break;
                case "11":
                    NameMonth = "พฤศจิกายน";
                    break;
                case "12":
                    NameMonth = "ธันวาคม";
                    break;
                default:
                    NameMonth = "Error";
                    break;
            }
            return NameMonth;
        }
        public static DateTime getDateFormatEn(String date)
        {
            if (date == "")
            {
                return new DateTime();
            }
            else
            {
                DateTime dateVal = new DateTime(int.Parse(date.Split("/".ToCharArray())[2]) - 543, int.Parse(date.Split("/".ToCharArray())[1]), int.Parse(date.Split("/".ToCharArray())[0]));
                return dateVal;
            }

        }
        public static String getDateFormatEnToString(String date)
        {
            String sDay = date.Split(" ".ToCharArray())[0].Split("/".ToCharArray())[0];
            String sMonth = date.Split(" ".ToCharArray())[0].Split("/".ToCharArray())[1];
            String sYear = date.Split(" ".ToCharArray())[0].Split("/".ToCharArray())[2];
            return sDay + "/" + sMonth + "/" + (Int32.Parse(sYear) - 543).ToString();
        }
        public static String getDateFormatThAddDay(DateTime date, double noDate)
        {
            String day = "";
            String month = "";
            String year = "";
            day = date.AddDays(noDate).Day.ToString().PadLeft(2, '0');
            month = GetThaiMonth(date.AddDays(noDate).Month.ToString().PadLeft(2, '0'));
            year = (date.AddDays(noDate).Year + 543).ToString();
            return day + " " + month + " " + year;
        }
        public static String getLastDayOfMonth(DateTime date)
        {
            String day = "";
            String month = "";
            String year = "";
            day = DateTime.DaysInMonth(date.Year, date.Month).ToString().PadLeft(2, '0');
            month = date.Month.ToString().PadLeft(2, '0');
            year = (date.Year + 543).ToString();
            return day + "/" + month + "/" + year;
        }
        public static String getDateFormatThAddDayShort(DateTime date, double noDate)
        {
            String day = "";
            String month = "";
            String year = "";
            day = date.AddDays(noDate).Day.ToString().PadLeft(2, '0');
            month = date.AddDays(noDate).Month.ToString().PadLeft(2, '0');
            year = (date.AddDays(noDate).Year + 543).ToString();
            return day + " " + month + " " + year;

        }
        public static String getStringDateNull(String dateVal)
        {
            if (dateVal == "01/01/544")
            {
                return "";
            }
            else
            {
                return dateVal;
            }
        }
        public static String GetCovertStrDate(string myDate)
        {
            string stDay, stMonth, stYear, stDate;
            stDate = "";
            stDay = "";
            stMonth = "";
            stYear = "";
            if (myDate == null)
            {
                stDate = "";
            }
            else if (myDate.Length == 6)
            {
                stYear = myDate.Substring(0, 2);
                stMonth = myDate.Substring(2, 2);
                stDay = myDate.Substring(4, 2);
                stDate = stDay + "/" + stMonth + "/25" + stYear;
            }
            else if (myDate.Length == 8)
            {
                stYear = myDate.Substring(0, 4);
                stMonth = myDate.Substring(4, 2);
                stDay = myDate.Substring(6, 2);
                stDate = stDay + "/" + stMonth + "/" + stYear;
            }
            else
            {
                stDate = "";
            }
            return stDate;
        }
        public static String GetDateNow()
        {
            string stDay, stMonth, stYear;
            stDay = "";
            stMonth = "";
            stYear = "";
            stYear = ((DateTime.Now.Year) + 543).ToString();
            stMonth = DateTime.Now.Month.ToString();
            stDay = DateTime.Now.Day.ToString();
            return stDay + "/" + stMonth + "/" + stYear;
        }
        public static DateTime StringToDate(String input)
        {
            string stDay, stMonth, stYear;
            int istDay, istMonth, istYear;
            stDay = input.Split("/".ToCharArray())[0];
            stMonth = input.Split("/".ToCharArray())[1];
            stYear = input.Split("/".ToCharArray())[2];
            istYear = int.Parse(stYear) - 543;
            istMonth = int.Parse(stMonth);
            istDay = int.Parse(stDay);
            return new DateTime(istYear, istMonth, istDay);
        }

        public static DateTime? StringToDateTime(String input, string era)
        {
            era = era.Replace(".", "");
            int eraDif = 0;
            try
            {
                if (
                    era.ToUpper().Trim() == "BU" ||
                    era.ToUpper().Trim() == "BC")
                {
                    eraDif = 543;
                }

                string sDate = input.Split(" ".ToCharArray())[0];
                string sTime = "";
                if (input.Split(" ".ToCharArray()).Length > 1)
                {
                    sTime = input.Split(" ".ToCharArray())[1];
                }


                string stDay, stMonth, stYear;
                int istDay, istMonth, istYear;
                stDay = sDate.Split("/".ToCharArray())[0];
                stMonth = sDate.Split("/".ToCharArray())[1];
                stYear = sDate.Split("/".ToCharArray())[2];
                istYear = int.Parse(stYear) - eraDif;
                istMonth = int.Parse(stMonth);
                istDay = int.Parse(stDay);

                if (sTime == "")
                {
                    return new DateTime(istYear, istMonth, istDay);
                }
                else
                {
                    string stHour, stMinute, stSecond;
                    int istHour, istMinute, istSecond;
                    stHour = sTime.Split(":".ToCharArray())[0];
                    stMinute = sTime.Split(":".ToCharArray())[1];
                    stSecond = sTime.Split(":".ToCharArray())[2];
                    istSecond = int.Parse(stSecond);
                    istMinute = int.Parse(stMinute);
                    istHour = int.Parse(stHour);

                    return new DateTime(istYear, istMonth, istDay, istHour, istMinute, istSecond);

                }
            }
            catch
            {
                return null;
            }
        }

        public static string ConvertDateTimeToDateStr(DateTime? data, string format = "dd/MM/yyyy")
        {
            string dateTimeString = "";
            // CultureInfo culture = new CultureInfo(langCulture);
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            DateTime dt = DateTime.MinValue;
            if (data != null)
            {
                dt = data.Value;


                return dt.ToString(format, currentCulture);

            }


            return dateTimeString;
        }

        public static string GetDateStrTHA(string data_str, string format = "dd/MM/yyyy")
        {
            CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            DateTime? dateValue = ConvertDateStrToDateTime(data_str, format);
            DateTime dateValueTH;

            if (dateValue == null) return null;

            if (DateTime.TryParseExact(dateValue.Value.ToString(format, currentCulture), format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValueTH))
            {

            }
            else
            {

                return null;
            }


            return dateValueTH.ToString(format, currentCulture);
        }

        public static DateTime? ConvertDateStrToDateTime(string data_str, string format = "dd/MM/yyyy")
        {

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            //CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            DateTime dateValue;
            if (DateTime.TryParseExact(data_str, format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
            {

            }
            else
            {

                return null;
            }

            return dateValue;
        }
        public static DateTime? ConvertDateStrToDateTime(string data_str, CultureInfo currentCulture, string format = "dd/MM/yyyy")
        {


            //CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            DateTime dateValue;
            if (DateTime.TryParseExact(data_str, format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
            {

            }
            else
            {

                return null;
            }

            return dateValue;
        }
        public static DateTime? ConvertDateStrToDateTimeTHA(string data_str, string format = "dd/MM/yyyy")
        {
            CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            DateTime? dateValue = ConvertDateStrToDateTime(data_str, format);
            DateTime dateValueTH;

            if (dateValue == null) return dateValue;

            if (DateTime.TryParseExact(dateValue.Value.ToString(format, currentCulture), format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValueTH))
            {

            }
            else
            {

                return null;
            }


            return dateValueTH;
        }
        private static string[] getFormatDateTime()
        {
            string[] formats = {"dd/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd","yyyy-M-d",
                                "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy",
                                "yyyy-MM-ddTHH:mm:ss","yyyy-MMM-ddTHH:mm:ss",
                                "yyyy-M-dTHH:mm:ss","dd/MM/yyyy HH.mm.ss","dd/MM/yyyy HH:mm:ss",
                                "yyyy-MM-ddTHH:mm","yyyy-MMM-ddTHH:mm",
                                "yyyy-M-dTHH:mm","dd/MM/yyyy HH.mm","dd/MM/yyyy HH:mm",
                                "yyyy-MM-ddTH:mm:ss","yyyy-MMM-ddTH:mm:ss",
                                "yyyy-M-dTH:mm:ss","dd/MM/yyyy H.mm.ss","dd/MM/yyyy H:mm:ss",
                                "yyyy-MM-ddTH:mm","yyyy-MMM-ddTH:mm",
                                "yyyy-M-dTH:mm","dd/MM/yyyy H.mm","dd/MM/yyyy H:mm",
                                "d/MM/yyyy H:mm:ss","d/MM/yyyy H:mm","d/MM/yyyyTH:mm:ss",
                                "d/MM/yyyyTHH:mm:ss","d/MM/yyyyTHH:mm",
                                "d/MM/yyyyTHH.mm.ss","d/MM/yyyyTHH.mm",
                                "d/MM/yyyy HH.mm.ss","d/MM/yyyy HH.mm",

                                "dd/MM/yy", "dd-MMM-yy", "yy-MM-dd","yy-M-d",
                                "dd-MM-yy", "M/d/yy", "dd MMM yy",
                                "yy-MM-ddTHH:mm:ss","yy-MMM-ddTHH:mm:ss",
                                "yy-M-dTHH:mm:ss","dd/MM/yy HH.mm.ss","dd/MM/yy HH:mm:ss",
                                "yy-MM-ddTHH:mm","yy-MMM-ddTHH:mm",
                                "yy-M-dTHH:mm","dd/MM/yy HH.mm","dd/MM/yy HH:mm",
                                "yy-MM-ddTH:mm:ss","yy-MMM-ddTH:mm:ss",
                                "yy-M-dTH:mm:ss","dd/MM/yy H.mm.ss","dd/MM/yy H:mm:ss",
                                "yy-MM-ddTH:mm","yy-MMM-ddTH:mm",
                                "yy-M-dTH:mm","dd/MM/yy H.mm","dd/MM/yy H:mm",
                                "d/MM/yy H:mm:ss","d/MM/yy H:mm","d/MM/yyTH:mm:ss",
                                "d/MM/yyTHH:mm:ss","d/MM/yyTHH:mm",
                                "d/MM/yyTHH.mm.ss","d/MM/yyTHH.mm",
                                "d/MM/yy HH.mm.ss","d/MM/yy HH.mm"
            };

            return formats;
        }
        public static DateTime? ConvertStringToDateTime(string strDatetime, string outFormat = "dd/MM/yyyy HH.mm.ss")
        {
            string[] formats = getFormatDateTime();
            if (!string.IsNullOrEmpty(strDatetime))
            {
                if (string.IsNullOrEmpty(outFormat))
                    outFormat = "dd/MM/yyyy HH.mm.ss";

                DateTime newDT;
                if (DateTime.TryParseExact(strDatetime, formats,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out newDT))
                {
                    newDT.ToString(outFormat, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                else
                {
                    newDT = new DateTime();
                }
                return newDT;
            }
            else
                return null;


        }

        public static DateTime? ConvertStringToDate(string strDatetime, string outFormat = "dd/MM/yyyy")
        {
            string[] formats = getFormatDateTime();

            if (!string.IsNullOrEmpty(strDatetime))
            {
                if (string.IsNullOrEmpty(outFormat))
                    outFormat = "dd/MM/yyyy";
                DateTime newDT;
                if (DateTime.TryParseExact(strDatetime, formats,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out newDT))
                {
                    newDT.ToString(outFormat, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                else
                {
                    newDT = new DateTime();
                }

                return newDT;
            }
            else
                return null;
        }


        public static bool ValidateIDCard(string pidNumber)
        {
            int total = 0;
            int runDown = 13;
            int value;
            bool result = false;
            if (pidNumber.Length != 13) return result;
            for (int i = 0; i < pidNumber.Length - 1; i++)
            {
                if (int.TryParse(pidNumber[i].ToString(), out value))
                {
                    total = total + (value * runDown);
                    runDown -= 1;
                }
                else
                {
                    return result;
                }
            }
            total = total % 11;
            total = 11 - total;
            total = total % 10;
            int.TryParse(pidNumber[12].ToString(), out value);
            if (total == value)
            {
                result = true;
            }
            return result;
        }
        public static String GetSex(String input)
        {
            String stSex;
            if (input == "F")
            {
                stSex = "หญิง";
            }
            else
            {
                stSex = "ชาย";
            }
            return stSex;
        }
        public static object NVL(object input, object nullValue)
        {
            if (input == null)
                return nullValue;
            else
                return input;
        }
        public static String NVL(object input)
        {
            if (input == null)
            {
                return "-";
            }
            else if (input is DBNull)
            {
                return "-";
            }
            else
            {
                return input.ToString();
            }
        }
        public static String GetAddressType(int adrType)
        {
            if (adrType == 0)
            {
                return "ปัจจุบัน";
            }
            else if (adrType == 1)
            {
                return "ที่ทำงาน";
            }
            else if (adrType == 2)
            {
                return "ทะเบียนบ้าน";
            }
            else
            {
                return "อื่นๆ";
            }
        }
        public static Boolean isEmptry(Object obj)
        {
            if (null == obj || "".Equals(obj))
                return true;
            return false;
        }
        public static string getProcessType(string type)
        {
            string processType = "";
            switch (type)
            {
                case "A":
                    processType = "เพิ่ม";
                    break;
                case "C":
                    processType = "แก้ไข";
                    break;
                case "D":
                    processType = "ยกเลิก";
                    break;
                default:
                    processType = "ปกติ";
                    break;
            }
            return processType;
        }
        public static string getVerify(string id)
        {
            string name = "";
            switch (id)
            {
                case "N":
                    name = "รอดำเนินการ";
                    break;
                case "Y":
                    name = "อนุมัติ";
                    break;
                case "R":
                    name = "ไม่อนุมัติ";
                    break;
                default:
                    name = "รอดำเนินการ";
                    break;
            }
            return name;
        }
        public static string Right(string input, int count)
        {
            int length = input.Length;

            int startIndex;
            if (count <= length)
                startIndex = length - count;
            else
                startIndex = 0;
            return input.Substring(startIndex);
        }

        public static string patternIDCard(string input)
        {
            if (input.Length < 13)
                return null;
            string result = null;
            if (input != null && !"".Equals(input))
            {
                result += input.Substring(0, 1) + "-";
                result += input.Substring(1, 4) + "-";
                result += input.Substring(5, 5) + "-";
                result += input.Substring(10, 2) + "-";
                result += input.Substring(12);
            }
            return result;
        }
        public static string patternMBPhone(string input)
        {
            string result = null;
            try
            {
                if (input != null && !"".Equals(input))
                {
                    result += input.Substring(0, 3) + "-";
                    result += input.Substring(3, 3) + "-";
                    result += input.Substring(6);
                }
            }
            catch { }
            return result;
        }
        public static string intFormat(long interal)
        {
            CultureInfo ci = new CultureInfo("en-US");
            return interal.ToString("N00", ci);
        }
        public static string getBUYear(string year)
        {
            return (Int32.Parse(year) + 543).ToString();
        }

        public static DateTime ConvertMiniDate(string sMiniDate)
        {
            DateTime output;
            string sYear = (DateTime.Now.Year + 543).ToString().Substring(0, 2) + sMiniDate.Substring(0, 2);
            int iYear = int.Parse(sYear) - 543;
            int iMonth = int.Parse(sMiniDate.Substring(2, 2));
            int iDay = int.Parse(sMiniDate.Substring(4, 2));
            int iHour = 0;
            int iMinute = 0;
            int iSecond = 0;
            if (sMiniDate.Length > 8)
            {
                iHour = int.Parse(sMiniDate.Substring(6, 2));
                iMinute = int.Parse(sMiniDate.Substring(8, 2));
                iSecond = int.Parse(sMiniDate.Substring(10, 2));
            }

            output = new DateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond);
            return output;
        }


        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
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
        private static bool IsIEnumerable(object input)
        {
            bool returnValue = false;

            Type inputType = input.GetType();
            if ((from i in inputType.GetInterfaces() where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>) select i).Count() > 0)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public static bool IsObjNullable<t>(t entity)
        {
            if (entity is Enum)
                throw new NotImplementedException("Entity is an enumeration - Use ConvertNum!");


            bool isNull = true;
            Type inputType = entity.GetType();


            if (IsIEnumerable(entity))
            {
                return isNull;
            }

            else
            {

                PropertyInfo[] inputProps = inputType.GetProperties();

                foreach (PropertyInfo inputProp in inputProps)
                {


                    try
                    {
                        object value = inputProp.GetValue(entity, null);
                        if (value != null)
                        {

                            isNull = false;
                            return isNull;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return isNull;
            }
        }

        //public static void ExecuteNonQuery(OracleConnection connection, string sqlStr,  List<DBParameter> param) 
        //{
        //    OracleCommand cmd = new OracleCommand(sqlStr, connection);
        //    if (param != null && param.Any())
        //    {
        //        cmd.Parameters.Clear();
        //        cmd.BindByName = true;
        //        foreach (DBParameter item in param)
        //        {
        //            cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
        //        }
        //    }
        //    //  cmd.Parameters.AddRange(param);
        //    cmd.ExecuteNonQuery();
        //    cmd.Dispose();
        //}

        /// <summary>
        /// this function is executed query string to object data-list.
        /// </summary>
        /// <typeparam name="T">typeparam of class property</typeparam>
        /// <param name="sqlStr">query string</param>
        /// <param name="connection">conditional of query string</param>
        /// <param name="param">List of DBParameter</param>
        /// <returns>data-list</returns>
        //public static List<T> ExecuteToList<T>(string sqlStr, OracleConnection connection, List<DBParameter> param = null) where T : new()
        //{
        //    OracleCommand cmd = new OracleCommand(sqlStr, connection);
        //    DataTable dt = new DataTable();
        //    IEnumerable<T> result; 
        //    if (param != null && param.Any())
        //    {
        //        cmd.Parameters.Clear();
        //        cmd.BindByName = true;
        //        foreach (DBParameter item in param)
        //        {
        //            cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
        //        }
        //    }
        //    using (OracleDataReader reader = cmd.ExecuteReader())
        //    {
        //        dt.Load(reader);
        //    }
        //    cmd.Dispose();
        //    result = dt.AsEnumerable<T>();
        //    dt.Dispose();
        //    return result.ToList();
        //}
        public static DataTable GetFilledDataTable(string sqlStr, OracleConnection connection)
        {
            return FillDataTable(sqlStr, connection);
        }

        public static bool IsLikeString(string pattern, string inputText)
        {
            if (pattern == null || inputText == null)
                return false;
            if (pattern.StartsWith("*") && pattern.EndsWith("*"))
            {
                return inputText.Contains(pattern.Replace("*", ""));
            }
            else if (pattern.StartsWith("*"))
            {
                return inputText.EndsWith(pattern.Replace("*", ""));
            }
            else if (pattern.EndsWith("*"))
            {
                return inputText.StartsWith(pattern.Replace("*", ""));
            }
            else
                return inputText == pattern;
            /*Regex regex = new Regex(pattern,RegexOptions.IgnoreCase);
            return regex.IsMatch(inputText);*/

        }

        public static double? differenceDay(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
                return null;
            TimeSpan span = endDate.Value - startDate.Value;
            return span.TotalDays;
        }

        public static string subPadRight(string input, int length)
        {
            return subPadRight(input, length, ' ');
        }

        public static string subPadRight(string input, int length, char padChar)
        {
            if (input == null)
                input = "";
            return input.PadRight(length, padChar).Substring(0, length);
        }

        public static string subPadLeft(string input, int length)
        {
            return subPadLeft(input, length, ' ');
        }

        public static string subPadLeft(string input, int length, char padChar)
        {
            if (input == null)
                input = "";
            return Right(input.PadLeft(length, padChar), length);
        }

        public static void AddFreeTextTester(string testText, OracleConnection connection)
        {
            string sqlStr =
                "INSERT INTO \"POLICY\".FREE_TEXT_TESTER(TEST_TEXT)\n" +
                "VALUES(" + SQLValueString(testText) + ")\n";
            OracleCommand oCmd = new OracleCommand(sqlStr, connection);
            oCmd.ExecuteNonQuery();
        }

        public static decimal? ConvertToNullableDecimal(object input)
        {
            if (input == null)
                return (decimal?)null;
            else
                return Convert.ToDecimal(input);
        }

        public static char? ConvertToNullableChar(object input)
        {
            if (input == null)
                return (char?)null;
            else if (input is string && ((string)input).Length == 0)
                return null;
            else
                return Convert.ToString(input).ToCharArray()[0];
        }

        public static string ConvertToString(object input)
        {
            if (input == null)
                return (string)null;
            else
                return Convert.ToString(input);
        }

        public static string JoinString(string[] input, string seperator)
        {
            if (input == null)
                return null;
            string returnValue = "";
            if (seperator == null)
                seperator = "";
            foreach (string s in input)
            {
                if (returnValue != "")
                    returnValue = returnValue + seperator;
                returnValue = returnValue + s;
            }
            return returnValue;
        }

        public static decimal? Inv(decimal? input)
        {
            return input == null ? (decimal?)null : 0m - input.Value;
        }

        public static double? Inv(double? input)
        {
            return input == null ? (double?)null : 0d - input.Value;
        }

        public static short? Inv(short? input)
        {
            return input == null ? (short?)null : Convert.ToInt16(0 - input.Value);
        }

        public static int? Inv(int? input)
        {
            return input == null ? (int?)null : Convert.ToInt32(0 - input.Value);
        }

        public static long? Inv(long? input)
        {
            return input == null ? (long?)null : Convert.ToInt64(0 - input.Value);
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
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
        public static long GetSeqNextVal(OracleConnection conn, string sequenceName)
        {
            string sqlStr =
                "select " + sequenceName + ".nextval from dual";
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            long returnValue = Convert.ToInt64(oCmd.ExecuteScalar());
            oCmd.Dispose();
            return returnValue;
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

        public static string FormatExceptionMessage(Exception e)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("Exception type: " + e.GetType().FullName);
            s.AppendLine("Message       : " + e.Message);
            s.AppendLine("Stacktrace:");
            s.AppendLine(e.StackTrace);
            s.AppendLine();
            return s.ToString();
        }

        public static List<T[]> GroupByLength<T>(IEnumerable<T> items, int groupLength)
        {
            int groupCount = items.Count() / groupLength;
            List<T[]> returnList = new List<T[]>();
            for (int i = 0; i < groupCount; i = i + 1)
            {
                int startIndex = i * groupLength;
                int length = groupLength;
                if (startIndex + groupLength - 1 > items.Count())
                    length = items.Count() - startIndex;

                T[] returnArray = new T[length];
                Array.Copy(items.ToArray(), startIndex, returnArray, 0, length);
                returnList.Add(returnArray);
            }
            return returnList;
        }

        public static GroupDivision[] DivideGroup(int itemCount, int groupLength)
        {
            int groupCount = (itemCount / groupLength);
            if (itemCount % groupLength > 0)
                groupCount = groupCount + 1;
            List<GroupDivision> groupDivList = new List<GroupDivision>();
            for (int i = 0; i < groupCount; i = i + 1)
            {
                int startIndex = i * groupLength;
                int length = groupLength;
                if (startIndex + groupLength > itemCount)
                    length = itemCount - startIndex;
                GroupDivision gd = new GroupDivision();
                gd.StartIndex = startIndex;
                gd.Length = length;
                groupDivList.Add(gd);
            }
            return groupDivList.ToArray();
        }

        public static string StripComments(string code)
        {
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            return System.Text.RegularExpressions.Regex.Replace(code, re, "$1");
        }

        public static List<T> IntersectAll<T>(IEnumerable<IEnumerable<T>> lists)
        {
            HashSet<T> hashSet = null;
            foreach (var list in lists)
            {
                if (hashSet == null)
                {
                    hashSet = new HashSet<T>(list);
                }
                else
                {
                    hashSet.IntersectWith(list);
                }
            }
            return hashSet == null ? new List<T>() : hashSet.ToList();
        }
    }

    public class GroupDivision
    {
        private int startIndex;

        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }
        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }

    #region " ExpandoObject - <Dynamic> JsonConverter "
    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return type == typeof(System.Dynamic.ExpandoObject) ? new DynamicJsonObject(dictionary) : null;
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            var dictionary = obj as IDictionary<string, object>;
            foreach (var item in dictionary)
                result.Add(item.Key, item.Value);
            return result;
        }
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { typeof(System.Dynamic.ExpandoObject) });
            }
        }

        #region " Nested type: DynamicJsonObject "
        private sealed class DynamicJsonObject : DynamicObject
        {
            private readonly IDictionary<string, object> _dictionary;

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                if (dictionary == null)
                    throw new ArgumentNullException("dictionary");
                _dictionary = dictionary;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("{");
                ToString(sb);
                return sb.ToString();
            }

            private void ToString(StringBuilder sb)
            {
                var firstInDictionary = true;
                foreach (var pair in _dictionary)
                {
                    if (!firstInDictionary)
                        sb.Append(",");
                    firstInDictionary = false;
                    var value = pair.Value;
                    var name = pair.Key;
                    if (value is string)
                    {
                        sb.AppendFormat("{0}:\"{1}\"", name, value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                    }
                    else if (value is ArrayList)
                    {
                        sb.Append(name + ":[");
                        var firstInArray = true;
                        foreach (var arrayValue in (ArrayList)value)
                        {
                            if (!firstInArray)
                                sb.Append(",");
                            firstInArray = false;
                            if (arrayValue is IDictionary<string, object>)
                                new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                            else if (arrayValue is string)
                                sb.AppendFormat("\"{0}\"", arrayValue);
                            else
                                sb.AppendFormat("{0}", arrayValue);

                        }
                        sb.Append("]");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1}", name, value);
                    }
                }
                sb.Append("}");
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!_dictionary.TryGetValue(binder.Name, out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                if (indexes.Length == 1 && indexes[0] != null)
                {
                    if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                    {
                        // return null to avoid exception.  caller can check for null this way...
                        result = null;
                        return true;
                    }

                    result = WrapResultObject(result);
                    return true;
                }

                return base.TryGetIndex(binder, indexes, out result);
            }

            private static object WrapResultObject(object result)
            {
                var dictionary = result as IDictionary<string, object>;
                if (dictionary != null)
                    return new DynamicJsonObject(dictionary);

                var arrayList = result as ArrayList;
                if (arrayList != null && arrayList.Count > 0)
                {
                    return arrayList[0] is IDictionary<string, object>
                        ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                        : new List<object>(arrayList.Cast<object>());
                }

                return result;
            }
        }
        #endregion
    }
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

    internal static class DataTableExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            //if (table.Rows.Count==0)
            //{

            //    yield return new T();
            //}
            //else
            //{
            foreach (var row in table.Rows)
            {
                yield return CreateItemFromRow<T>(row as DataRow, properties);
            }
            //}
        }
        public static T AsEnumerable<T>(this DataRow row) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return CreateItemFromRow<T>(row as DataRow, properties);
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (PropertyInfo property in properties)
            {
                if (!property.CanWrite) continue;
                if (!row.Table.Columns.Contains(property.Name)) continue;

                if (DBNull.Value != row[property.Name])
                {
                    property.SetValue(item, TypeExtension.ChangeType(row[property.Name], property.PropertyType), null);
                }
            }

            return item;
        }
    }

    internal static class TypeExtension
    {
        public static bool IsNullable(this Type type)
        {
            return ((type.IsGenericType) && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static IEnumerable<string> AppendAll(this IEnumerable<string> text, string prefix, string suffix)
        {
            foreach (string s in text)
            {
                yield return string.Format("{0}{1}{2}", prefix, s, suffix);
            }
        }

        public static void ApplyAll(this IEnumerable items, string propertyName, object data)
        {
            foreach (var item in items)
            {
                PropertyInfo property = item.GetType().GetProperty(propertyName);
                if (property == null) throw new NullReferenceException();
                if (!property.CanWrite) throw new InvalidOperationException(string.Format("Property or indexer '{0}' cannot be assign to, it is read only.", property.Name));

                property.SetValue(item, TypeExtension.ChangeType(data, property.PropertyType), null);
                //yield return item;
            }
        }
        public static IEnumerable<TResult> ConvertAll<TResult>(this IEnumerable items)
        {
            foreach (var item in items)
            {
                yield return (TResult)TypeExtension.ChangeType(item, typeof(TResult));
            }
        }
        public static object ChangeType(object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }
            // end if

            if (conversionType.IsEnum)
            {
                return ConvertToEnumType(value, conversionType);
            }

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                // It's a nullable type, so instead of calling Convert.ChangeType directly which would throw a
                // InvalidCastException (per http://weblogs.asp.net/pjohnson/archive/2006/02/07/437631.aspx),
                // determine what the underlying type is
                // If it's null, it won't convert to the underlying type, but that's fine since nulls don't really
                // have a type--so just return null
                // Note: We only do this check if we're converting to a nullable type, since doing it outside
                // would diverge from Convert.ChangeType's behavior, which throws an InvalidCastException if
                // value is null and conversionType is a value type.
                if (value == null)
                {
                    return null;
                } // end if

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            } // end if

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(value, conversionType);
        }

        private static object ConvertToEnumType(object value, Type type)
        {
            if (value is string)
            {
                return Enum.Parse(type, value as string);
            }
            else
            {
                if (!Enum.IsDefined(type, value))
                {
                    throw new FormatException("Undefined value for enum type");
                }

                return Enum.ToObject(type, value);
            }
        }
    }
    public static class UsingServiceClient
    {
        public static void Do<TClient>(TClient client, Action<TClient> execute)
            where TClient : class, ICommunicationObject
        {
            try
            {
                execute(client);
            }
            finally
            {
                client.DisposeSafely();
            }
        }

        public static void DisposeSafely(this ICommunicationObject client)
        {
            if (client == null)
            {
                return;
            }

            bool success = false;

            try
            {
                if (client.State != CommunicationState.Faulted)
                {
                    client.Close();
                    success = true;
                }
            }
            finally
            {
                if (!success)
                {
                    client.Abort();
                }
            }
        }
    }

    public static class Extensions
    {
        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
    }
    //get mime type for .net 4.0
    //public static class MimeExtensionHelper
    //{
    //    static object locker = new object();
    //    static object mimeMapping;
    //    static MethodInfo getMimeMappingMethodInfo;

    //    static MimeExtensionHelper()
    //    {
    //        Type mimeMappingType = Assembly.GetAssembly(typeof(HttpRuntime)).GetType("System.Web.MimeMapping");
    //        if (mimeMappingType == null)
    //            throw new SystemException("Couldnt find MimeMapping type");
    //        getMimeMappingMethodInfo = mimeMappingType.GetMethod("GetMimeMapping", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
    //        if (getMimeMappingMethodInfo == null)
    //            throw new SystemException("Couldnt find GetMimeMapping method");
    //        if (getMimeMappingMethodInfo.ReturnType != typeof(string))
    //            throw new SystemException("GetMimeMapping method has invalid return type");
    //        if (getMimeMappingMethodInfo.GetParameters().Length != 1 && getMimeMappingMethodInfo.GetParameters()[0].ParameterType != typeof(string))
    //            throw new SystemException("GetMimeMapping method has invalid parameters");
    //    }
    //    public static string GetMimeType(string filename)
    //    {
    //        lock (locker)
    //            return (string)getMimeMappingMethodInfo.Invoke(mimeMapping, new object[] { filename });
    //    }
    //}
}
