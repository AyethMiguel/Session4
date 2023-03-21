using RestSharp;
using ServiceReference1;

namespace MM_API_Homework4
{
    [TestClass]
    public class CountryInfoService
    {
        // Global Service
        private readonly CountryInfoServiceSoapTypeClient countryInfoService =
            new(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

        [TestMethod]
        public void CountryCodeAscending()
        {
            // Validate the return of ListofCountryNamesByCode() by ascending order of Country Code
            var countryList = countryInfoService.ListOfCountryNamesByCode().OrderBy(x=>x.sISOCode).ToList();
            
            Assert.IsNotNull(countryList);
            Assert.IsTrue(countryList.Any());
            if (countryList.Count > 1)
            {
                var previous = countryList.First();
                for (var i = 1; i < countryList.Count; i++)
                {
                    Assert.IsTrue(string.CompareOrdinal(countryList[i].sISOCode, previous.sISOCode) > 0, "Country Code should be in ascending order.");
                    previous = countryList[i];
                }
            }
        }

        [TestMethod]
        public void CountryName()
        {
            // Validate passing of invalid Country Code to CountryName(), returns 'Country not found in the database'

            // Arrange
            var invalidCountryCode = "AC";

            // Act
            var country = countryInfoService.CountryName(invalidCountryCode);

            // Assert
            Assert.AreEqual(country, "Country not found in the database", "AC is not a valid Country Code");
        }

        [TestMethod]
        public void LastEntry()
        {
            // Arrange
            var countryList = countryInfoService.ListOfCountryNamesByCode().OrderBy(x => x.sISOCode).ToList();
            var lastCountryCode = countryList[countryList.Count - 1].sISOCode;

            // Act
            var country = countryInfoService.CountryName(lastCountryCode);

            // Assert
            Assert.AreEqual(country, countryList[countryList.Count - 1].sName, "Last country names should be the same with fetched");
        }
    }
}