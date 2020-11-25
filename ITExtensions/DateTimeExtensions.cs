﻿using System;

namespace ITUtility {
    public static class DateTimeExtensions {  

        public static DateTime BeginOfDay(this DateTime refDateTime) {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 0, 0, 0);
        }
        
        public static DateTime EndOfDay(this DateTime refDateTime) {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 23, 59, 59, 999);
        }
    }
}
