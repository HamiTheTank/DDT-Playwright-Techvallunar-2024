using Microsoft.Playwright;
using NUnit.Framework.Interfaces;

namespace UI_Test__Playwright_
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class UITests : PageTest
    {
        //STEPS
        // - navigate to home page
        // - handle cookie window
        // - go to parfumes
        // - filter parfumes
        // - assert search result count vs. expected count

        [Test]
        [Category("DDT")]
        [TestCaseSource(nameof(GetTestData))]

        public async Task FilterParfumes(DataModels.FilterCriteria testData)
        {
            //SETUP            
            MainPage mainPage = new MainPage(Page);
            await mainPage.GotoAsync();
            await mainPage.HandleCookieWindow();
            await mainPage.ClickParfumesBtn();

            //ACT
            await mainPage.FilterProduktart(testData.Produktart);
            await mainPage.FilterMarke(testData.Marke);
            await mainPage.FilterFurWen(testData.FurWen);
            await mainPage.FilterHighlights(testData.Highlights);
            await mainPage.FilterGeschenkFur(testData.GeschenkFur);

            //ASSERT
            int count = mainPage.GetResultCount().Result;
            Assert.That(count, Is.EqualTo(testData.ExpectedCount));
            
            await mainPage.GetResultCount();
            await mainPage.CloseAsync();        
        }

        public static IEnumerable<DataModels.FilterCriteria> GetTestData()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testdata.json");
            var jsonText = File.ReadAllText(jsonPath);
            var testData = System.Text.Json.JsonSerializer.Deserialize<List<DataModels.FilterCriteria>>(jsonText);

            foreach (var item in testData)
            {
                yield return item;
            }
        }        
    }    
}
