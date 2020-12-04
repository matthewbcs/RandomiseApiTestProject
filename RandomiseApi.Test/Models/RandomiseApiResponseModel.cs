using System;
using System.Collections.Generic;
using System.Text;

namespace RandomiseApi.Test.Models
{
   public class RandomiseApiResponseModel
    {
        public List<WordItem> RequestItems { get; set; }
        public string StatusCode { get; set; }
        public string StatusValue { get; set; }
        public string ApiResponse { get; set; }
    }
}
