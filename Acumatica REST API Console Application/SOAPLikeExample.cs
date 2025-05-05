using System;
using System.Collections.Generic;

using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.ContractBasedApi.Model;
using Acumatica.RESTClient.Loggers;

using SOAPLikeWrapperForREST;

using CustomStringField = SOAPLikeWrapperForREST.CustomStringField;

namespace AcumaticaSoapLikeApiExample
{
    public class SOAPLikeExample
    {
        public static void ExampleMethod(string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
        {
            var client = new SOAPLikeClient(siteURL,
                requestInterceptor: ConsoleRequestLogger.LogRequest,
                responseInterceptor: ConsoleRequestLogger.LogResponse);

            try
            {
                client.Login(username, password, tenant, branch, locale);
                Console.WriteLine("Logged In");


                //  Contact contactToSearch = new Contact() { ContactID = new IntSearch() { Value = 123 } };

                //  restClient.Get(contactToSearch);
                Shipment shipment = new Shipment()
                {
                    ShipmentNbr = new StringSearch() { Value = "004372" },
                    Packages = new List<ShipmentPackage>()
                    {
                        new ShipmentPackage {
                            ReturnBehavior=  ReturnBehavior.All,
                            PackageContents = new List<ShipmentPackageDetail>()
                            {
                                new ShipmentPackageDetail() { ReturnBehavior= ReturnBehavior.All}
                            }
                        }
                    },
                    CustomFields = new CustomField[] { new CustomStringField { viewName = "Document", fieldName = "ShipmentNbr" } }

                };
                shipment = (Shipment)client.Get(shipment);

                Console.WriteLine("Shipped Qty= " + shipment.ShippedQty.Value);


                Console.WriteLine("Get Sales Order custom fields schema");
                client.GetCustomFieldSchema(new SalesOrder());

                Console.WriteLine("Confirming the shipment.");
                client.WaitInvoke(new ConfirmShipment(shipment));

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                client.Logout();
                Console.WriteLine("Logged Out");
            }

        }
    }
}
