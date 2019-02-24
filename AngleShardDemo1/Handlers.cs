using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AngleShardDemo1
{
    public class Handlers
    {
        public static void HandleDescription(IHtmlCollection<IElement> elements)
        {
            var description = elements[0];

            string Inner = description.InnerHtml; // 1
            string Outer = description.OuterHtml; // 2
            string findFontStyle = "font-size([^px]*px)"; // 3

            Regex reg = new Regex(findFontStyle);

            string innerMatch = reg.Match(Inner).Value; // 4

            string outerMatch = reg.Match(Outer).Value; // 5 creates obj on heap 

            StringBuilder outerstr = new StringBuilder(Outer); // 6 // creates obj in heap
            outerstr.Replace(outerMatch, innerMatch);

            description.OuterHtml = outerstr.ToString();






        }
    }
}
