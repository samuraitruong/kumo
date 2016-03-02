using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test_kumo_eip0001web.Utility
{
    public class KumoConstants
    {
        public  const string STRONG_PASSWORD_PATTERN = "(?=.*[A-Z])(?=.*[!@#$%^&*-_+=\\[\\]{}|\\:',.?/`~\"<>()])(?=.*[0-9])(?=.*[a-z]).{8,16}$";

        public  const string STRONG_PASSWORD_PATTERN_2 = "(?=.*[A-Z].*[A-Z])(?=.*[!@#$%^&*-_+=\\[\\]{}|\\:',.?/`~\"<>()])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,16}$";

        public const string EXTRA_ROUTE_DATA = "EXTRA_ROUTE_DATA";

        public const string DOCUMENT_ROOT_ROUTE_NAME = "DocumentRoots";

        public const string DMS_ROOT_ROUTE_NAME = "DMSRoot";
    }
}