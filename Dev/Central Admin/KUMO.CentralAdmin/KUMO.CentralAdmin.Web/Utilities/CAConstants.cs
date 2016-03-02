using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KUMO.CentralAdmin.Web
{
    public class CAConstants
    {
        public const string EXTRA_ROUTE_DATA = "ExtraRouteData";
        public const string STRONG_PASSWORD_PATTERN = "(?=.*[A-Z])(?=.*[!@#$%^&*-_+=\\[\\]{}|\\:',.?/`~\"<>()])(?=.*[0-9])(?=.*[a-z]).{8,16}$";

        public const string STRONG_PASSWORD_PATTERN_2 = "(?=.*[A-Z].*[A-Z])(?=.*[!@#$%^&*-_+=\\[\\]{}|\\:',.?/`~\"<>()])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,16}$";
    }
}