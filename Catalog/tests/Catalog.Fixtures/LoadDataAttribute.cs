using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace Catalog.Fixtures
{
    public class LoadDataAttribute: DataAttribute
    {
        private readonly string _fileName;
        private readonly string _section;

        public LoadDataAttribute(string section)
        {
            _fileName = @"d:\.NET-Projects\Hands-On RESTful Web Services with ASP.NET Core 3\Catalog\tests\Catalog.Fixtures\record-data.json";
            _section = section;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

            var path = Path.IsPathRooted(_fileName) ? _fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), _fileName);
            if (!File.Exists(path)) throw new ArgumentException($"File not found {path}");

            var fileData = File.ReadAllText(path);

            if (string.IsNullOrEmpty(_section)) return JsonConvert.DeserializeObject<List<string[]>>(fileData);

            var allData = JObject.Parse(fileData);
            var data = allData[_section];

            return new List<object[]> { new [] {data.ToObject(testMethod.GetParameters().First().ParameterType)} };
        }
    }
}
