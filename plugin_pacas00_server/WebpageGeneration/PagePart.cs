using PeterCashelNet.Owin.WebpageGeneration.Enum;
using System.Collections.Generic;

namespace PeterCashelNet.Owin.WebpageGeneration
{
    public class PagePart
    {
        public List<string> lines = new List<string>();

        private void Add(string s)
        {
            lines.Add("    " + s + "\n");
        }

        private void Add_NoNewLine(string s)
        {
            lines.Add("    " + s);
        }

        public PagePart()
        {

        }

        public override string ToString()
        {
            string s = "";

            foreach (string ln in lines)
            {
                s = s + ln;
            }

            return s;
        }

        //Implementation run 1 - Proof testing

        public PagePart customMarkup(string s)
        {
            Add(s);
            return this;
        }

        public PagePart p(string s)
        {
            Add("<p>" + s + "</p>");
            return this;
        }

        public PagePart h(string s, int i)
        {
            Add("<h" + i + ">" + s + "</h" + i + ">");
            return this;
        }

        public PagePart br()
        {
            Add("<br/>");
            return this;
        }

        public PagePart hr()
        {
            Add("<hr>");
            return this;
        }

        public PagePart div(PagePart part)
        {
            Add("<div>");
            foreach (string s in part.lines) Add_NoNewLine(s);
            Add("</div>");
            return this;
        }

        //Implementation run 2 - Add the options and their things that i might use

        public PagePart a(string href, string text)
        {
            Add("<a href=\"" + href + "\">" + text + "</a>");
            return this;
        }

        public PagePart a(string href, string text, aTag_Options opt)
        {
            Add("<a href=\"" + href + "\" " + opt.ToString() + ">" + text + "</a>");
            return this;
        }


        public PagePart dl(List<string> items, string classs = "")
        {
            if(classs == "") Add("<dl>");
            else Add("<dl class=\"" + classs + "\">");
            foreach (string s in items)
            {
                Add("    <dt>" + s + "</dt>");
            }
            Add("</dl>");
            return this;
        }

        public PagePart footer(PagePart part)
        {
            Add("<footer>");
            foreach (string s in part.lines) Add_NoNewLine(s);
            Add("</footer>");
            return this;
        }

        public PagePart ol(List<string> items, string classs = "")
        {
            if(classs == "") Add("<ol>");
            else Add("<ol class=\"" + classs + "\">");
            foreach (string s in items)
            {
                Add("    <li>" + s + "</li>");
            }
            Add("</ol>");
            return this;
        }

        public PagePart ul(List<string> items, string classs = "")
        {
            if (classs == "") Add("<ul>");
            else Add("<ul class=\"" + classs + "\">");
            foreach (string s in items)
            {
                Add("    <li>" + s + "</li>");
            }
            Add("</ul>");
            return this;
        }

        public PagePart divStyled(PagePart part, string style)
        {
            Add("<div style=\"" + style + "\">");
            foreach (string s in part.lines) Add_NoNewLine(s);
            Add("</div>");
            return this;
        }

        public PagePart div(PagePart part, string classs = "", string id = "")
        {
            Add("<div class=\"" + classs + "\" id=\"" + id + "\">");
            foreach (string s in part.lines) Add_NoNewLine(s);
            Add("</div>");
            return this;
        }

        public PagePart img(string src, string alt = "", int height = -1, int width = -1, CrossoriginEnum crossorigin = CrossoriginEnum.none)
        {
            string options = "";
            options += "alt=\"" + alt + "\" ";
            if (height != -1) options += "height=\"" + height + "\" ";
            if (width != -1) options += "width=\"" + width + "\" ";
            if (crossorigin != CrossoriginEnum.none)
            {
                if (crossorigin == CrossoriginEnum.anonymous)
                {
                    options += "crossorigin=\"anonymous\" ";
                } else
                {
                    options += "crossorigin=\"use-credentials\" ";
                }
            }
            Add("<img src=\"" + src + "\" " + options + ">");
            return this;
        }

        public PagePart embed(string src, string type = null, int height = -1, int width = -1)
        {
            string options = "";
            if (type != null) options += "type=\"" + type + "\" ";
            if (height != -1) options += "height=\"" + height + "\" ";
            if (width != -1) options += "width=\"" + width + "\" ";

            Add("<embed src=\"" + src + "\" " + options + ">");
            return this;
        }

        public PagePart nav(PagePart part, string classs = "", string id = "")
        {
            Add("<nav class=\"" + classs + "\" id=\"" + id + "\">");
            foreach (string s in part.lines) Add_NoNewLine(s);
            Add("</nav>");
            return this;
        }

        public PagePart table(List<tablerow> tablerows)
        {
            Add("<table>");
            foreach (tablerow row in tablerows)
            {
                Add("    " + "<tr>");
                foreach (string s in row.cells)
                {
                    Add("    " + "    " + "<td>" + s + "</td>");
                }              
                Add("    " + "</tr>");
            }
            Add("</table>");
            return this;
        }

        public class tablerow {

            public List<string> cells = new List<string>();

            public tablerow Add(string s)
            {
                cells.Add(s);
                return this;
            }
        }
    }
}