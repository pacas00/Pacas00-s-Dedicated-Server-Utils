using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterCashelNet.Owin.WebpageGeneration.Options
{
    public class meta_Options
    {
        public List<string> lines = new List<string>();
        private void Add(string s)
        {
            lines.Add(s);
        }

        public meta_Options charset(string charset)
        {
            Add("<meta charset=\"" + charset + "\">");
            return this;
        }

        public meta_Options name_application_name(string content)
        {
            Add("<meta name=\"application-name\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options name_author(string content)
        {
            Add("<meta name=\"author\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options name_description(string content)
        {
            Add("<meta name=\"description\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options name_generator(string content)
        {
            Add("<meta name=\"generator\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options name_keywords(string content)
        {
            Add("<meta name=\"keywords\" content=\"" + content + "\">");
            return this;
        }
        
        public meta_Options http_equiv__content_type(string content)
        {
            Add("<meta http-equiv=\"content-type\" content=\"" + content + "\">");
            return this;
        }
        public meta_Options http_equiv__default_style(string content)
        {
            Add("<meta http-equiv=\"default-style\" content=\"" + content + "\">");
            return this;
        }
        public meta_Options http_equiv__refresh(string content)
        {
            Add("<meta http-equiv=\"refresh\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options http_equiv_custom(string equiv, string content)
        {
            Add("<meta http-equiv=\"" + equiv + "\" content=\"" + content + "\">");
            return this;
        }

        public meta_Options name_custom(string name, string content)
        {
            Add("<meta name=\"" + name + "\" content=\"" + content + "\">");
            return this;
        }

    }
}
