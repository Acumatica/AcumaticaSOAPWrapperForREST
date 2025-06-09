using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi.Model;
using Acumatica.RESTClient.ContractBasedApi;

using static SOAPLikeWrapperForREST.Helpers.EntityStructureHelper;

namespace SOAPLikeWrapperForREST.Helpers
{
    internal static class CustomFieldsHelper
    {
        public static string ComposeCustomParameter(Entity entity)
        {
            return string.Join(",", GetCustomFieldNames(entity).Distinct());
        }

        private static IEnumerable<string> GetCustomFieldNames(Entity entity)
        {
            foreach (var customField in CollectCustomFieldNames(entity))
            {
                yield return customField;
            }
            foreach (var linkedEntity in GetLinkedEntitiesWithValues(entity))
            {
                foreach (var customField in GetCustomFieldNames(linkedEntity.Value))
                {
                    yield return $"{linkedEntity.Name}/{customField}";
                }
            }
            foreach (var detailEntity in GetDetailEntitiesWithValues(entity))
            {
                foreach (var detailEntityValue in detailEntity.Details)
                {
                    foreach (var customField in GetCustomFieldNames(detailEntityValue))
                    {
                        yield return $"{detailEntity.Name}/{customField}";
                    }
                }
            }
        }

        private static IEnumerable<string> CollectCustomFieldNames(Entity entity)
        {
            if (entity?.Custom != null)
            {
                foreach (var view in entity.Custom)
                {
                    foreach (var field in view.Value)
                    {
                        yield return ($"{view.Key}.{field.Key}");
                    }
                }
            }
        }
    }
}
