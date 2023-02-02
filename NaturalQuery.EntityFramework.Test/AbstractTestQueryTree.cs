using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.EntityFramework.Test
{
    [DeploymentItem("App_Data\\NORTHWND.mdf")]
    public class AbstractTestQueryTree
    {
        static StoreModelContext ctx = new StoreModelContext();
        TestContext testContextInstance;

        public static SqlProviderServices EnsureAssemblySqlServerIsCopied { get; set; }

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public StoreModelContext StoreModelContext
        {
            get { return ctx; }
            set { ctx = value; }
        }
        public dynamic Given
        {
            get
            {
                return TestContext.Properties["Given"];
            }
            set
            {
                if (!TestContext.Properties.Contains("Given"))
                {
                    TestContext.Properties.Add("Given", value);
                }
                else
                {
                    TestContext.Properties["Given"] = value;
                }
            }
        }


        public dynamic Expected
        {
            get
            {
                return TestContext.Properties["Expected"];
            }
            set
            {
                if (!TestContext.Properties.Contains("Expected"))
                {
                    TestContext.Properties.Add("Expected", value);
                }
                else
                {
                    TestContext.Properties["Expected"] = value;
                }
            }
        }

        public dynamic Actual
        {
            get
            {
                return TestContext.Properties["Actual"];
            }
            set
            {
                if (!TestContext.Properties.Contains("Actual"))
                {
                    TestContext.Properties.Add("Actual", value);
                }
                else
                {
                    TestContext.Properties["Actual"] = value;
                }
            }
        }

        static AbstractTestQueryTree()
        {
            //Cache Entity Framework initialization
            try
            {
                string absolute = Path.GetFullPath("NORTHWND.mdf");
                absolute = Path.GetDirectoryName(absolute);
                AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

                var temp = ctx.Employees.Select(e => e.EmployeeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.InnerException.Message);
                }
            }
        }

        [TestInitialize]
        public void Test_Initialize()
        {
            TestDescriptionAttribute attr = this.GetType().GetMethod(TestContext.TestName).GetCustomAttributes(typeof(TestDescriptionAttribute), false).FirstOrDefault() as TestDescriptionAttribute;

            if (attr != null)
            {
                Debug.WriteLine("Testing the following sentence:");
                Debug.WriteLine(attr.Description);
                Debug.WriteLine(string.Empty);
            }
        }
    }
}
