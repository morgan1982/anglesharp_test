﻿using AngleSharp;
using AngleSharp.Html.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AngleSharp.Html.Parser;
using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace AngleShardDemo1
{
    internal class RegexFactory
    {
        public static string FontSize { get; } = "font-size";
        public static string FontColor { get; } = "";
        public static string BackGroundColor { get; } = "";
        public static string FontFamily { get; } = "";
        public static string PickParentAttributes { get; } = "<(.*?)>";
        public static string PickStyle { get; } = "style=\"([^\"]+\")";

        public static Regex CreateRegex(string args) => new Regex(args);
     
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Task<List<string>> parse = Task.Run(() => Parser());
            //parse.Wait();

            //List<string> results = parse.Result;

            //foreach (var title in results)
            //{
            //    Console.WriteLine(title);
            //}
             

            using (StreamReader reader = new StreamReader("C:\\templates\\grass.html"))
            {
                string content = reader.ReadToEnd();

                var parser = new HtmlParser();
                var document = parser.ParseDocument(content);
                //TestMod(document);

                // Parser

                StringBuilder attributesString = new StringBuilder();
                var elements = Parser2(document);

                elements.ForEach(element =>  MainEngine(attributesString, element));

                var final = document.DocumentElement.OuterHtml;
              
                Console.WriteLine(final);
            }

            Console.ReadLine();

        }

        // replaces the bold 

        // takes the string and prepares the list of the elements
        static List<IElement> Parser2(IHtmlDocument document)
        {

            // use a main StringBuilder and add and remove string to it.
            var ElementsForModification = new List<IElement>()
            {
                document.GetElementById("__DESCRIPTION__"),
                document.GetElementById("__MANUFACTURER__")
            };

            void add(IHtmlCollection<IElement> elements)
            {
                foreach (var element in elements)
                {
                    ElementsForModification.Add(element);
                }
            }

            add(document.GetElementsByName("__FEATURES__"));
            add(document.GetElementsByName("__DESCRIPTION__"));
            add(document.GetElementsByName("__MANUFACTURER__"));

            return ElementsForModification;
            // pass the elements list to prepare for modification
        }

        // Sanitizer MainEngine
        private static void MainEngine(StringBuilder elementString, IElement element)
        {
                if (element == null)
                {
                    return;
                }else
                {
                    elementString.Clear(); // clear the builder
                    string inner = element.InnerHtml;
                    string outer = element.OuterHtml;
                    var pickParentRegex = RegexFactory.CreateRegex(RegexFactory.PickParentAttributes);
                    string parentAttributes = pickParentRegex.Match(outer).Value;

                    bool ParentHasStyle = parentAttributes.Contains("style"); // checks if style
                    if (ParentHasStyle)
                    {
                        var pickParentStyleRegex = RegexFactory.CreateRegex(RegexFactory.PickStyle);
                        string parentStyle = pickParentStyleRegex.Match(parentAttributes).Value;
                        elementString.Append(parentStyle);
                    }else
                    {
                        elementString.Append(parentAttributes);
                        
                    }
                }

        }

        // Sanitizer Core
        private static void Core(string innerElement, StringBuilder elementString, bool ParentHasStyle)
        {
            //Check for the needed regexes
            bool innerHasColor = innerElement.Contains("color:");
            bool innerHasBackGroundColor = innerElement.Contains("background-color:");
            bool innerHasFontFamily = innerElement.Contains("font-family:");
            // check the other Matches
            bool innerHasBold = innerElement.Contains("<b>");
            bool innerHasUnderline = innerElement.Contains("<u>");
            bool innerHasStrikeThrough = innerElement.Contains("<strike>");
            bool innerHasItalics = innerElement.Contains("<i>");

            // create the needed regexes
            if(innerElement.Contains("font-size:"))
            {
                var fontSizeRegex = RegexFactory.CreateRegex(RegexFactory.FontSize);
                string innerFontSize = fontSizeRegex.Match(innerElement).Value;



                
            }

            // math the element with all the regexes

        }


        static void TestMod(IHtmlDocument document)
        {
            var description = document.GetElementById("__DESCRIPTION__");

            var styleRegex = new Regex("style=\"([^\"]+\")");

            // used by the string builder to insert the style to the correct index
            string parentStyle = styleRegex.Match(description.OuterHtml).Value;
            int semiIndex = parentStyle.IndexOf(";"); // find the index of the first semicolon


            StringBuilder styleOfParentBuilder = new StringBuilder(styleRegex.Match(description.OuterHtml).Value);


            string inner = description.InnerHtml;
            bool isBold = inner.Contains("<b>");

            if (isBold)
            {
                styleOfParentBuilder.Insert(semiIndex, "; font-weight: bold "); // inserts the attribute to the style
                string finalStyle = styleOfParentBuilder.ToString();
                var splitStyleOfParent = finalStyle.Split('"');

                description.SetAttribute("style", splitStyleOfParent[1]);
            }


        }


        static void UserModificationsSanitizer(IElement element)
        {
            if (element == null)
            {
                return;
            }
            //var span = document.CreateElement("span");
            //span.SetAttribute("style", "font-size:10px;");
            //span.TextContent = "hello there";

            //descriptionElement.Append(span);

            string findFontSize = "font-size([^px]*px)"; // 3

            // bold -> match the <b> -> create font-wieght: 700 attibute

            Regex fontSizeFinder = new Regex(findFontSize);
            Regex pickParent = new Regex("<(.*?)>");

            string innerMatch = fontSizeFinder.Match(element.InnerHtml).Value; // font-size of inner
            // selects the outer element  Consider to use stringBuilder
            string outerMatch = pickParent.Match(element.OuterHtml).Value; // the outer element it self to sting
            // has to find if it as font-size first and then make it general

            // function to check if certain styles are in existance.. don't invoke the engine if it is not needed
            bool parentHasStyle = outerMatch.Contains("style");

            // BOOLS TO CHECK IF VALUE EXISTS
            bool parentHasFontSize = outerMatch.Contains("font-size");


            StringBuilder outer = new StringBuilder(element.OuterHtml);

            if (parentHasFontSize)
            {
                string outerStyle = fontSizeFinder.Match(element.OuterHtml).Value; // finds the font-size in the outer html

                outer.Replace(outerStyle, innerMatch);
                element.OuterHtml = outer.ToString();

            }
            else
            {
                element.SetAttribute("style", $"{ innerMatch }"); // have to change destoys the old attributes
            }
        }






        // ANGLE PARSER
        static string Parser(string template)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(template);

            // ELEMENTS FOR MOD
            var descriptionElements = document.GetElementsByName("__DESCRIPTION__");
            var titles = document.GetElementsByName("__TITLE__");

            // MODIFICATIONS
            //description[0].InnerHtml = "einai poly kalo content";
            titles[0].InnerHtml = "hello there rooster!";
            //titles[1].InnerHtml = "hello there rooster again!";

            var description = descriptionElements[0];

            string span = description.InnerHtml; // 1
            string outer = description.OuterHtml;// 2
            // select reg
            //string regStr = "font(.*)px";
            //string regStr = "/font.\{px}\/";
            string regStr = "font([^px]*)px"; // 3
            var reg = new Regex(regStr);
            var matches = reg.Matches(span);
            string fontOfParrent = matches[0].ToString(); // 4

            string newOuter = Regex.Replace(outer, regStr, fontOfParrent); // 5

            description.OuterHtml = newOuter;
            //var attributtes = description[0].Attributes; // [1] return the string of the styles;
            //var childs = description[0].ChildNodes;
            ////var span = description[0].FirstChild;



            string final = document.DocumentElement.OuterHtml;

            return final;
        }
 
        static async Task<List<string>> Parser()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(address);
            var cellSelector = "tr.vevent td:nth-child(3)";
            var cells = document.QuerySelectorAll(cellSelector);
            var titles = cells.Select(m => m.TextContent).ToList();
            return titles;
        }
        
    }
    class Employee : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
