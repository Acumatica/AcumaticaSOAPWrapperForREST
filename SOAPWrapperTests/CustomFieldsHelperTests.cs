using Acumatica.RESTClient.ContractBasedApi.Model;

using SOAPLikeWrapperForREST.Helpers;

namespace SOAPWrapperTests
{
    public class CustomFieldsHelperTests
    {
        [Fact]
        public void GetCustomFields_FindsTopLevelCustomFields()
        {
            var testEntity = new TestEntityWithoutDetails()
            {
                CustomFields = new CustomField[]
                 {
                    new CustomDecimalField {ViewName = "TestView", FieldName = "TestField"},
                 }
            };
            CustomFieldsHelper.ComposeCustomParameter(testEntity)
                .Should().Be("TestView.TestField");
        }
        [Fact]
        public void GetCustomFields_FindsDetailLevelCustomFields()
        {
            var testEntity = new TestEntityWithDetails()
            {
                DetailEntity1 = new List<TestEntityWithoutDetails>()
                {
                    new TestEntityWithoutDetails()
                    {
                         CustomFields = new CustomField[]
                         {
                            new CustomDecimalField {ViewName = "TestView", FieldName = "TestField"},
                         }
                    }
                }
            };
            CustomFieldsHelper.ComposeCustomParameter(testEntity)
                .Should().Be($"{nameof(TestEntityWithDetails.DetailEntity1)}/TestView.TestField");
        }
        [Fact]
        public void GetCustomFields_MergesCustomFieldsFromDifferentLevels()
        {
            var testEntity = new TestEntityWithDetails()
            {
                CustomFields = new CustomField[]
                {
                    new CustomDecimalField {ViewName = "TestView0", FieldName = "TestField0"},
                },
                DetailEntity1 = new List<TestEntityWithoutDetails>()
                {
                    new TestEntityWithoutDetails()
                    {
                         CustomFields = new CustomField[]
                         {
                            new CustomDecimalField {ViewName = "TestView1", FieldName = "TestField1"},
                         }
                    },

                    new TestEntityWithoutDetails()
                    {
                         CustomFields = new CustomField[]
                         {
                            new CustomStringField {ViewName = "TestView2", FieldName = "TestField2"},
                            new CustomIntField {ViewName = "TestView3", FieldName = "TestField3"},
                         }
                    }
                },
                DetailEntity2 = new TestEntityWithoutDetails[]
                {
                    new TestEntityWithoutDetails()
                    {
                         CustomFields = new CustomField[]
                         {
                            new CustomDecimalField {ViewName = "TestView4", FieldName = "TestField4"},
                         }
                    }
                }
            };
            CustomFieldsHelper.ComposeCustomParameter(testEntity)
                .Should().ContainAll(new[]
                {
                    $"TestView0.TestField0",
                    $"{nameof(TestEntityWithDetails.DetailEntity1)}/TestView1.TestField1",
                    $"{nameof(TestEntityWithDetails.DetailEntity1)}/TestView2.TestField2",
                    $"{nameof(TestEntityWithDetails.DetailEntity1)}/TestView3.TestField3",
                    $"{nameof(TestEntityWithDetails.DetailEntity2)}/TestView4.TestField4",
                });
        }
    }
}
