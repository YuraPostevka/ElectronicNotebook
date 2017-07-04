using Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {

        static void Main(string[] args)
        {
            RestClient client = new RestClient("http://localhost:55624/api/");

            RestRequest req = new RestRequest("ToDoItem/GetAll", Method.GET);
            var resp = client.Execute<List<ToDoItem>>(req).Data;

            var id = 1;

            var putReq = new RestRequest("ToDoItem/MarkAsCompleted/?id=" + id, Method.PUT);


            client.Execute(putReq);


        }
    }
}

