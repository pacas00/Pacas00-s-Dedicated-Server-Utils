using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterCashelNet.Owin.WebpageGeneration
{
    class WebPage
    {

        PageHeader _head;
        PagePart _body;

        public WebPage(PageHeader head, PagePart body)
        {
            _head = head;
            _body = body;
        }


        public override string ToString()
        {
            return "<!DOCTYPE html>\n<html>\n" +  _head.ToString() + "<body>\n" + _body.ToString() + "</body>\n</html>\n";
        }
    }
}
