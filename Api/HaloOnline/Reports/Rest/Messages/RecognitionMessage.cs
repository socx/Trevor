using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    using Domain;

    public class RecognitionMessage
    {
        public List<Recognition> Recognitions { get; set; }
    }
}