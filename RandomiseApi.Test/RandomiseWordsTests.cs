using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RandomiseApi.Test.Models;
using RestSharp;
using RestSharp.Serialization.Json;

namespace RandomiseApi.Test
{
    public class Tests: BaseTest
    {
        private RestClient _client;
        private Dictionary<string, string> headers;
        private RestRequest restRequest;
        private List<WordItem> wordItems;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient("https://randomiseapi.azurewebsites.net");

            RandomiseApiResponseModel responseModel = new RandomiseApiResponseModel();
            restRequest = new RestRequest("/api/Words/GetRandomWords", Method.POST);
            headers = new Dictionary<string, string>();
           
        }

        #region helpers

        private IRestResponse MakeApiRequest(RestRequest request)
        {
            IRestResponse restResponse = _client.Execute(request);
            JsonDeserializer jd = new JsonDeserializer();
            wordItems = jd.Deserialize<List<WordItem>>(restResponse);
            return restResponse;
        }

        private void SetupDefaultHeader(int wordCount, int minWordLength, int maxWordLength)
        {
            headers.Add("Authorization", "MatthewIsTheGreatest1234");
            headers.Add("Token", "Test"); // token not used in this version 
            _client.AddDefaultHeaders(headers);
            WordRequestModel word = new WordRequestModel() { WordCount = wordCount, MaxWordLength = maxWordLength, MinWordLength = minWordLength };
            restRequest.AddJsonBody(word);
        }

        #endregion


        [TestCase(7, 8, 9)]
        [TestCase(6, 5, 6)]
        [TestCase(1, 2, 4)]
        public void GetRandomWord_Success(int wordCount, int minWordLength, int maxWordLength)
        {
            // ********* ARRANGE ***********
            SetupDefaultHeader(wordCount, minWordLength, maxWordLength);

             // ******* ACT ***********
             IRestResponse restResponse = MakeApiRequest(restRequest);

            //********* ASSERT ********** 

            //check api response
            Assert.IsTrue(restResponse.IsSuccessful);
            Assert.IsTrue(restResponse.ResponseStatus == ResponseStatus.Completed);
            Assert.IsTrue(wordItems.Count == wordCount);

            // check word lengths
            WordItem exceedMaxWordLengthItem = wordItems.FirstOrDefault(x => x.word.Length > maxWordLength);
            WordItem exceedMinWordLengthItem = wordItems.FirstOrDefault(x => x.word.Length < minWordLength);
            Assert.IsNull(exceedMinWordLengthItem);
            Assert.IsNull(exceedMaxWordLengthItem);
        }


        [TestCase(5, 0, 0)]
        public void GetRandomWord_ZeroMinMaxCount(int wordCount, int minWordLength, int maxWordLength)
        {
            // ********* ARRANGE ***********
            SetupDefaultHeader(wordCount, minWordLength, maxWordLength);

            // ******* ACT ***********
            IRestResponse restResponse = MakeApiRequest(restRequest);

            //********* ASSERT ********** 

            //check api response
            Assert.IsTrue(restResponse.IsSuccessful);
            Assert.IsTrue(restResponse.ResponseStatus == ResponseStatus.Completed);
            Assert.IsTrue(wordItems.Count == 0);

            
        }
    }
}