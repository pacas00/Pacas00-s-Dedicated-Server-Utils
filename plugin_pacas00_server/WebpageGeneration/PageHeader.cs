using PeterCashelNet.Owin.WebpageGeneration.Enum;
using PeterCashelNet.Owin.WebpageGeneration.Options;
using System.Collections.Generic;

namespace PeterCashelNet.Owin.WebpageGeneration
{
    public class PageHeader
    {
        public List<string> lines = new List<string>();
        private void Add(string s)
        {
            lines.Add("    " + s + "\n");
        }

        public PageHeader()
        {
            //Always add The Shiv. For IE Support.
            Add("<!--[if lt IE 9]>");
            Add("<script src=\"http://html5shiv.googlecode.com/svn/trunk/html5.js\"></script>");
            Add("<![endif]-->");    
        }

        public override string ToString()
        {
            string s = "<head>" + "\n";

            foreach (string ln in lines)
            {
                s = s + ln;
            }

            return  s + "</head>" + "\n";
        }

        public PageHeader customMarkup(string s)
        {
            Add(s);
            return this;
        }

        public PageHeader title(string s)
        {
            Add("<title>" + s + "</title>");
            return this;
        }
        public PageHeader link(string rel, string href)
        {
            Add("<link rel=\"" + rel + "\" href=\"" + href + "\">");
            return this;
        }

        public PageHeader link(string rel, string href, string type)
        {
            Add("<link rel=\"" + rel + "\" type=\"" + type + "\" href=\""+ href + "\">");
            return this;
        }

        public PageHeader link(string rel, string href, CrossoriginEnum cross = CrossoriginEnum.none, string integrity = "EMPTY")
        {
            string options = "";
            if (cross != CrossoriginEnum.none)
            {
                if (cross == CrossoriginEnum.anonymous)
                {
                    options += "crossorigin=\"anonymous\" ";
                }
                else
                {
                    options += "crossorigin=\"use-credentials\" ";
                }
            }
            if (integrity != "EMPTY")
            {
                options += "integrity=\"" + integrity + "\" ";
            }
            Add("<link rel=\"" + rel + "\" href=\"" + href + "\"" + options + ">");
            return this;
        }

        public PageHeader script(string src, CrossoriginEnum cross = CrossoriginEnum.none, string integrity = "EMPTY")
        {
            string options = "";
            if (cross != CrossoriginEnum.none)
            {
                if (cross == CrossoriginEnum.anonymous)
                {
                    options += "crossorigin=\"anonymous\" ";
                }
                else
                {
                    options += "crossorigin=\"use-credentials\" ";
                }
            }
            if (integrity != "EMPTY")
            {
                options += "integrity=\"" + integrity + "\" ";
            }
            Add("<script src=\"" + src + "\" " + options.Trim() + "></script>");
            return this;
        }

        public PageHeader meta(meta_Options opt)
        {
            foreach (string s in opt.lines) Add(s);
            return this;
        }

    }
}