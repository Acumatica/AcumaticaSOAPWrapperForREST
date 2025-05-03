using System;

using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.ContractBasedApi.Model;

using SOAPLikeWrapperForREST;

namespace AcumaticaSoapLikeApiExample
{
    internal class SOAPLikeExample2
    {
        public static void ExampleMethod(string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
        {
            var client = new SOAPLikeClient(siteURL,
              requestInterceptor: RequestConsoleLogger.LogRequest,
              responseInterceptor: RequestConsoleLogger.LogResponse);

            try
            {
                client.Login(username, password);

                Project projectToBeCreated = new Project
                {
                    GLAccounts = new ProjectGLAccount { DefaultSubaccount = new StringValue { Value = "000-000" } },
                    ProjectID = new StringValue { Value = "TESTPR4" },
                    Customer = new StringValue { Value = "BESTYPEIMG" },
                    Description = new StringValue { Value = $"Test Desc {DateTime.Now.ToString()}" },
                    ProjectProperties = new ProjectProperties
                    {
                        StartDate = new DateTimeValue { Value = DateTime.Now },
                        EndDate = new DateTimeValue { Value = DateTime.Now },
                    },

                };

                //Create a customer record with the specified values
                Project newProject = client.Put(projectToBeCreated);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                client.Logout();
            }

        }
    }
}

