using CVSkill.Models;
using System;

namespace CVSkill.Services
{
    public class CVWebRequest : Bottlecap.Json.Web.JsonWebRequestItem<CV>
    {
        public CVWebRequest()
            : base(new Uri("http://www.davidskendall.co.uk/cv/resources/cv.json"), Bottlecap.Web.WebRequestType.Get)
        {

        }
    }
}
