using System;
using System.Collections.Generic;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SQLite;
using System.IO;
using System.Text;


namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = SqlLiteQueryFactory();

            var old = db.Query("accounts").Where("currency_id", "=", "USD").Get();

            foreach (var a in old)
            {
                Console.WriteLine(a);
            }
            Console.WriteLine("New code demo");
            var accounts = db.Query("accounts").Where("currency_id", "=", "USD").MyGet<Accounts>();

            foreach (var a in accounts)
            {
                Console.WriteLine(a.id);
                Console.WriteLine(a.currency_id);
                Console.WriteLine(a.created_at);
                Console.WriteLine(a.name);
            }

        }

        

        private static QueryFactory SqlLiteQueryFactory()
        {
            var compiler = new SqliteCompiler();

            var connection = new SQLiteConnection("Data Source=Demo.db");

            var db = new QueryFactory(connection, compiler);

            //db.Logger = result => { Console.WriteLine(result.ToString()); };

            if (!File.Exists("Demo.db"))
            {
                Console.WriteLine("db not exists creating db");

                SQLiteConnection.CreateFile("Demo.db");

                db.Statement(
                    "create table accounts(id integer primary key autoincrement, name varchar, currency_id varchar, created_at datetime);");
            }

            return db;
        }
    }
}