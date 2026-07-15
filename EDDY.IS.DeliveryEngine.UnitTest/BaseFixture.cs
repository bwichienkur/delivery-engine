using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDY.IS.DeliveryEngine.UnitTest
{
    [TestClass]
    public class BaseFixture
    {
        protected IDictionary<string, string> CreateValuesDictionary(string queryList)
        {
            IDictionary<string, string> responseValues = new Dictionary<string, string>();
            foreach (string nameValuePair in queryList.Replace("?", "").Split('&').ToList())
            {
                if (nameValuePair.Split('=').Count() > 1)
                {
                    string name = nameValuePair.Split('=')[0];
                    string value = nameValuePair.Split('=')[1];
                    responseValues.Add(new KeyValuePair<string, string>(name.ToLower(), value));
                }
            }
            return responseValues;
        }

        public void CheckExpectedValuesAgainstActualValues(IDictionary<string, string> expectedValues, IDictionary<string, string> testValues)
        {
            //Checking all the name value pairs in the expected query string are in the query string we create.
            foreach (KeyValuePair<string, string> nameValuePair in expectedValues)
            {
                string expectedName = nameValuePair.Key;
                string expectedValue = nameValuePair.Value;
                string testValue = testValues[expectedName];

                Assert.IsTrue(testValues.ContainsKey(expectedName));
                Assert.AreEqual(expectedValue, testValue);
            }
        }
    }
}

