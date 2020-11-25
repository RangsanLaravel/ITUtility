﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITUtility {
    public static class BlaExtensions {

        public static DateTime BeginOfDay(this DateTime refDateTime) {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 0, 0, 0);
        }
        
        public static DateTime EndOfDay(this DateTime refDateTime) {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 23, 59, 59, 999);
        }



    }
}
