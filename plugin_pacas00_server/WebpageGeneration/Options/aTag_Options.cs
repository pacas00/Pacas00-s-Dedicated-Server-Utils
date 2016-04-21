namespace PeterCashelNet.Owin.WebpageGeneration
{
    public class aTag_Options
    {
        string _download = string.Empty;
        bool _download_blank = false;
        string _hreflang = string.Empty;
        string _media = string.Empty;
        string _rel = string.Empty;
        string _target = string.Empty;
        string _type = string.Empty;
        string _classs = string.Empty;

        public aTag_Options download(string filename)
        {
            _download = filename;
            return this;
        }
        public aTag_Options download(bool add_Option_With_No_Filename)
        {
            _download_blank = add_Option_With_No_Filename;
            return this;
        }

        public aTag_Options hreflang(string language_code)
        {
            _hreflang = language_code;
            return this;
        }

        public aTag_Options media(string value)
        {
            _media = value;
            return this;
        }

        public aTag_Options rel(string value)
        {
            _rel = value;
            return this;
        }

        public aTag_Options target(string framename)
        {
            _target = framename;
            return this;
        }

        public aTag_Options type(string media_type)
        {
            _type = media_type;
            return this;
        }

        public aTag_Options classs(string classs)
        {
            _classs = classs;
            return this;
        }

        public override string ToString()
        {
            string options = "";
            if (!(string.Empty == _download)) options += "download=\"" + _download + "\" ";
            else if (_download_blank) options += "download ";

            if (!(string.Empty == _hreflang)) options += "hreflang=\"" + _hreflang + "\" ";
            if (!(string.Empty == _media)) options += "media=\"" + _media + "\" ";
            if (!(string.Empty == _rel)) options += "rel=\"" + _rel + "\" ";
            if (!(string.Empty == _target)) options += "target=\"" + _target + "\" ";
            if (!(string.Empty == _type)) options += "type=\"" + _type + "\" ";
            if (!(string.Empty == _classs)) options += "class=\"" + _classs + "\" ";
            return options.Trim();
        }
    }
}