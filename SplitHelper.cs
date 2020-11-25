using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITUtility
{
    public class SplitHelper
    {

        public static List<GroupMember<T>> SplitItem<T>(IEnumerable<T> dataObjects)
        {
            return SplitItem<T>(dataObjects, 1000);
        }

        public static List<GroupMember<T>> SplitItem<T>(IEnumerable<T> Data, int? Take)
        {
            List<GroupMember<T>> ListData = new List<GroupMember<T>>();
            if (Data != null && Data.Any())
            {
                Data = Data.Distinct().ToArray();
                if (Take == null || Take.Value == 0)
                {
                    Take = 1000;
                }
                decimal CalData = (decimal)Data.Count() / (decimal)Take.Value;
                decimal power = (decimal)Math.Pow(10, 0);
                decimal Count = Math.Ceiling(CalData * power) / power;
                for (decimal i = 1; i <= Count; i++)
                {
                    GroupMember<T> DATA = new GroupMember<T>();
                    int Index = Convert.ToInt32(i - 1);
                    DATA.Index = Index;
                    T[] GetData = Data.Skip(Index * Take.Value).Take(Take.Value).ToArray();
                    DATA.Data = GetData.ToList();
                    ListData.Add(DATA);
                }
            }
            else
            {
                throw new Exception("ไม่พบข้อมูลที่ต้องการ WKGroupID!");
            }

            return ListData;
        }

        public class GroupMember<T>
        {
            public int? Index { get; set; }

            public List<T> Data { get; set; }

        }
    }


}
