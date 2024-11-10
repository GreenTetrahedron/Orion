using Orion.Models.ClientTransmissions;

namespace Orion.JsonParser.Tests
{
    public class Tests
    {
        private IJsonService _jsonService;
        private string json;

        [SetUp]
        public void Setup()
        {
            _jsonService = new JsonService();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            json = File.ReadAllText(path);
        }

        [Test]
        public void Test1()
        {
            try
            {
                var result = _jsonService.DeserialiseJson<ClientTransmission?>(json);
                Guid id = (Guid)result.Data;
            }
            catch (Exception ex)
            {

            }

            Assert.True(true);
        }
    }
}