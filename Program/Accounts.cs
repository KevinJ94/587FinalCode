using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Program
{
    public class Accounts
    {

        public long id { get; set; }
        public string name { get; set; }
        public string currency_id { get; set; }
        public DateTime created_at { get; set; }


    }
}
