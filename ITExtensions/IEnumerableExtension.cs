using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ITUtility.SplitHelper;

namespace ITUtility
{
    public static class IEnumerableExtension
    {
        public static T SingleOrNull<T>(this IEnumerable<T> Data, string DataName)
        {
            try
            {
                T ResultData = default(T);
                if (DataName.IsNotNullOrEmpty())
                {
                    if (Data != null && Data.Any())
                    {
                        if (Data.Count() == 1)
                        {
                            ResultData = Data.First();
                        }
                        else
                        {
                            throw new Exception("พบข้อมูล " + DataName + " มากกว่า 1รายการ!");
                        }
                    }
                }
                else
                {
                    throw new Exception("ไม่พบข้อมูล DataName!");
                }
                return ResultData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T Single<T>(this IEnumerable<T> Data, string DataName)
        {
            try
            {
                T ResultData;
                if (DataName.IsNotNullOrEmpty())
                {
                    if (Data != null && Data.Any())
                    {
                        if (Data.Count() == 1)
                        {
                            ResultData = Data.First();
                        }
                        else
                        {
                            throw new Exception("พบข้อมูล " + DataName + " มากกว่า 1รายการ!");
                        }
                    }
                    else
                    {
                        throw new Exception("ไม่พบข้อมูล " + DataName + "!");
                    }
                }
                else
                {
                    throw new Exception("ไม่พบข้อมูล DataName!");
                }
                return ResultData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T First<T>(this IEnumerable<T> Data, string DataName)
        {
            try
            {
                T ResultData;
                if (DataName.IsNotNullOrEmpty())
                {
                    if (Data != null && Data.Any())
                    {
                        ResultData = Data.First();
                    }
                    else
                    {
                        throw new Exception("ไม่พบข้อมูล " + DataName + "!");
                    }
                }
                else
                {
                    throw new Exception("ไม่พบข้อมูล DataName!");
                }
                return ResultData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GroupMember<T>> SplitItemExt<T>(this IEnumerable<T> dataObjects)
        {
            return SplitItem<T>(dataObjects);
        }

        public static List<GroupMember<T>> SplitItemExt<T>(this IEnumerable<T> dataObjects, int? Take)
        {
            return SplitItem<T>(dataObjects, Take);
        }

    }
}
