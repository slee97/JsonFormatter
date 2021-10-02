using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace JsonFormatter
{
    // Class representation of the JSON, for the purpose of parsing
    class JsonObject
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Batter { get; set; }
        public string Topping { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int argc = args.Length;
                // check that program is invoked with EXACTLY 2 arguments
                if (argc != 2)
                {
                    Console.Error.WriteLine("Usage: JsonFormatter <input file> <output file>");
                    return;
                }
                String ifn = args[0];
                String ofn = args[1];

                // now check existence of input file and hardcoded json schema file in current directory
                if (!File.Exists(ifn))
                {
                    Console.Error.WriteLine("Error: Inputfile '" + ifn + "' does not exist!");
                    return;
                }

                // check for existence of JSON schema file with hardcoded name schema.json
                // TODO: pickup the JSON schema filename in the App.config
                if (!File.Exists("schema.json"))
                {
                    Console.Error.WriteLine("Error: cannot locate Json schema 'schema.json'!");
                    return;
                }

                // validate json against json schema
                StreamReader file = File.OpenText(@"schema.json");
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JSchema schema = JSchema.Load(reader);

                    // validate JSON
                    IList<string> errorMessages;
                    String jsonstring = File.ReadAllText(ifn);
                    JToken json = JToken.Parse(jsonstring);
                    bool isValid = json.IsValid(schema, out errorMessages);

                    if (isValid)
                    {
                        // open the output file in overwrite mode, any exception will be caught by the catch down there
                        StreamWriter sw = new StreamWriter(ofn, false);

                        // json file validated with schema successful, now sort it
                        var listOb = JsonConvert.DeserializeObject<List<JsonObject>>(jsonstring);
                        var descListOb = listOb.OrderBy(x => x.Id);

                        // write header if list is not empty
                        if (listOb.Count > 0)
                        {
                            sw.WriteLine("{0,-4} {1,-19} {2,-20} {3,-15} {4}", "Id", "Type", "Name", "Batter", "Topping");
                        }

                        // now write out the list members
                        foreach (JsonObject j in descListOb)
                        {
                            sw.WriteLine("{0,-4} {1,-19} {2,-20} {3,-15} {4}", j.Id, j.Type, j.Name, j.Batter, j.Topping);
                        }

                        sw.Close();  // lest not forget to close the file stream
                    }
                    else
                    {
                        // Json validation error, loop and output validation errors
                        foreach (string errString in errorMessages)
                        {
                            Console.Error.WriteLine(errString);
                        }
                    }
                }  // end using
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }
        } // end main
    } // end class Program
}  // end namespace
