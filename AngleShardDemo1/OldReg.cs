using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AngleShardDemo1
{
    class RegexFactory
    {
        public static Regex FontSize() => new Regex("");

        public static Regex FontColor() => new Regex("");

        public static Regex BackgroundColor() => new Regex("");

        public static Regex FontFamily() => new Regex("");

        public static Regex PickParentAttributes() => new Regex("<(.*?)>");

        public static Regex PickStyle() => new Regex("style=\"([^\"]+\")");
    }
}
