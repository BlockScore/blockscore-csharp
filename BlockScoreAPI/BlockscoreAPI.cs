using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace BlockScoreAPI
{
    public class BlockscoreAPI
    {
        private const string BaseUrl = @"https://api.blockscore.com/";
        private const char ApiVersion = '2';

        private static string _authKey;
        private string _verificationId;
        private string _questionSetId;

        /**
	 * Constructor function for all new blockscore instances
	 * 
	 * Store key for later requests
	 *
	 * @param String key
	 * @throws Exception if not a valid authentication key
	 */

        public BlockscoreAPI()
        {
            string apikey = ConfigurationSettings.AppSettings["Blockscore_API_Key"];

            if (!string.IsNullOrEmpty(apikey))
                _authKey = apikey;
            else
                throw new Exception("blockscore: No key (or empty key) provided to constructor");
        }


     /**
	 * Verify US
	 * @param BlockScoreVerifyDomesticRequest All three names of consumer e.g. array('first'=>'Joe', 'middle'=>'', 'last'=>'Smith')
	 *                                  Date of birth in YYYY-MM-DD format
	 *                                  Last four digits of SSN
	 *                                  Full address of consumer e.g. 'street1'=>'20 Main', 'street2'=>'Ste 4', 'city'=>'Springfield', 'state'=>'IL', 'postal_code'=>'99999'
	 * @throws Exception if request fails (see private function request() for details)
	 * @return BlockScoreResponse
	 */

        public BlockScoreResponse VerifyUs(BlockScoreVerifyDomesticRequest request)
        {
            var formData = GetFormattedDomesticVerifyRequeset(request);

            return Request("POST","verifications", formData);
        }

        private static NameValueCollection GetFormattedDomesticVerifyRequeset(BlockScoreVerifyDomesticRequest request)
        {
            var formData = new NameValueCollection();
            formData["name[first]"] = request.FirstName;
            formData["name[middle]"] = request.MiddleName;
            formData["name[last]"] = request.LastName;
            formData["date_of_birth"] = request.DateOfBirth;
            formData["type"] = request.CitizenshipType;
            formData["identification[ssn]"] = request.LastFourDigitsOfSSN;
            formData["address[street1]"] = request.Street1;
            formData["address[street2]"] = request.Street2;
            formData["address[city]"] = request.City;
            formData["address[state]"] = request.State;
            formData["address[postal_code]"] = request.PostalCode;
            formData["address[country_code]"] = BlockScoreVerifyDomesticRequest.CountryCode;
            return formData;
        }

        /**
	 * Verify International
	 * @param BlockScoreResponse All three names of consumer plus gender e.g. array('first'=>'Joe', 'middle'=>'', 'last'=>'Smith', 'gender'=>'M')
	 *                           Date of birth in YYYY-MM-DD format
	 *                           Full passport number
	 *                           Full address of consumer e.g. 'street1'=>'Bahnhofstrasse 70', 'street2'=>'', 'city'=>'Zurich', 'state'=>'ZH', 'postal_code'=>'8001', 'country_code'=>'CH'
	 * @throws Exception if request fails (see private function request() for details)
	 * @return Array
	 */

        public BlockScoreResponse VerifyIntl(BlockScoreVerifyInternationalRequest request)
        {
            var formData = GetFromattedIntRequest(request);

            return Request("POST","verifications", formData);
        }

        private static NameValueCollection GetFromattedIntRequest(BlockScoreVerifyInternationalRequest request)
        {
            var formData = new NameValueCollection();
            formData["name[first]"] = request.FirstName;
            formData["name[middle]"] = request.MiddleName;
            formData["name[last]"] = request.LastName;
            formData["date_of_birth"] = request.DateOfBirth;
            formData["type"] = request.CitizenshipType;
            formData["identification[ssn]"] = request.PassportNumber;
            formData["address[street1]"] = request.Street1;
            formData["address[street2]"] = request.Street2;
            formData["address[city]"] = request.City;
            formData["address[state]"] = request.State;
            formData["address[postal_code]"] = request.PostalCode;
            formData["address[country_code]"] = request.CountryCode;
            return formData;
        }


        /**
	    * List past verifications
	    * @param Int offset How many items to offset the results (optional)
	    * @param Int count Number of results to show, 0-100 (optional)
	    * @param Int start_date Unix timestamp of start date (optional)
	    * @param Int end_date Unix timestamp of end date (optional)
	    * @throws Exception if request fails (see private function request() for details)
	    * @return Array
	    */

        public BlockScoreResponse ListVerificationResults(int offset = 0, int count = 100, int startDate = 0, int endDate = 0)
        {
            var webClient = new WebClient();

            var queryParameters = string.Format("verifications?offset={0}&count={1}", offset, count);
            if (startDate > 0)
                queryParameters += string.Format("&start_date={0}", startDate);
            if (endDate > 0)
                queryParameters += string.Format("&end_date={0}", endDate);

            return Request("Get",queryParameters);
        }

        /**
	    * List one verification by verification_id
	    * @param String erificationid Verification ID from prior request (optional, assuming prior request was with the same object)
	    * @throws Exception if empty/unset verification_id, or if request fails (see private function request() for details)
	    * @return BlockScoreResponse
	    */

        public BlockScoreResponse GetVerificationResult(string verificationid ="")
        {
            if (string.IsNullOrEmpty(verificationid) && string.IsNullOrEmpty((_verificationId)))
                throw new Exception("blockscore: No (or empty) verification_id provided to GetVerificationResult");
            if (string.IsNullOrEmpty(verificationid) && !string.IsNullOrEmpty(_verificationId))
                verificationid = _verificationId;

            return Request("Get", "verifications/" + verificationid);
        }

        /**
	    * Get question set
	    * @param String verificationid Verification ID from prior request (optional, assuming prior request was with the same object)
	    * @throws Exception if empty/unset verification_id, or if request fails (see private function request() for details)
	    * @return Array
	    */
        public BlockScoreQuestionsResponse QuestionSet(string verificationid = "")
        {
            if (string.IsNullOrEmpty(verificationid) && string.IsNullOrEmpty(_verificationId))
                throw new Exception("blockscore: No (or empty) verification_id provided to QuestionSet");
            else 
            {if(string.IsNullOrEmpty(verificationid) && !string.IsNullOrEmpty(_verificationId))
                verificationid = _verificationId;}

            var formData = new NameValueCollection();
            formData["verification_id"] = verificationid;
            return (BlockScoreQuestionsResponse)Request("POST", "questions", formData); 
        }


        /**
	    * Retrieve previously-requested question set
	    *  **** WARNING:  THIS ENDPOINT APPEARS TO BE BROKEN (404) AS OF 15-MAR-2014 ****
	    * @param String $questionsetid Question Set ID from prior request (optional, assuming prior request was with the same object)
	    * @throws Exception if empty/unset question_set_id, or if request fails (see private function request() for details)
	    * @return Array
	    */
        public BlockScoreResponse RetrieveQuestionSet(string questionsetid)
        {
            if(string.IsNullOrEmpty(questionsetid) && string.IsNullOrEmpty(_questionSetId))
                throw new Exception("blockscore: No (or empty) question_set_id provided to RetrieveQuestionSet");
            if (string.IsNullOrEmpty(questionsetid) && !string.IsNullOrEmpty(_questionSetId))
                questionsetid = _questionSetId;
			
            return Request("GET", "questions/"+questionsetid); 
        }


        /*
	    * Check the answers to a set of challenge questions
	    * @param IList<BlockScoreAnswer> answers All answers by question ID, e.g. array( array('question_id'=>1, 'answer_id'=>2), array('question_id'=>2, 'answer_id'=>4) );
	    * @param String verificationid Verification ID from prior request (optional, assuming prior request was with the same object)
	    * @param String questionsetid Question Set ID from prior request (optional, assuming prior request was with the same object)
	    * @throws Exception if empty/unset verification_id/question_set_id, or if request fails (see private function request() for details)
	    * @return BlockScoreResponse
	    */
        public BlockScoreQuestionsScoreResponse CheckQuestionAnswers(IList<BlockScoreAnswer> answers, string verificationid = "", string questionsetid = "") 
       { 
		if(string.IsNullOrEmpty(verificationid) && string.IsNullOrEmpty(_verificationId))
			throw new Exception("blockscore: No (or empty) verification_id provided to CheckQuestionAnswers");
		else if(string.IsNullOrEmpty(verificationid) && !string.IsNullOrEmpty(_verificationId))
			verificationid = _verificationId;
		
		if(string.IsNullOrEmpty(questionsetid) && string.IsNullOrEmpty(_questionSetId))
			throw new Exception("blockscore: No (or empty) question_set_id provided to CheckQuestionAnswers");
		else if(string.IsNullOrEmpty(questionsetid) && !string.IsNullOrEmpty(_questionSetId))
			questionsetid = _questionSetId;
		
		if(answers.Count == 0)
			throw new Exception("blockscore: No (or empty) answer array provided to CheckQuestionAnswers");
		

            var formData = new NameValueCollection();
            formData["verification_id"] = verificationid;
            formData["question_set_id"] = questionsetid;
            var jsonAnswers  =JsonConvert.SerializeObject(answers);
           formData["answers"] = jsonAnswers;

           return (BlockScoreQuestionsScoreResponse)Request("POST", "questions/score", formData); 
	}


        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /**
	    * (Private) Build HTTP request
	    * @param String method E.g. GET or POST
	    * @param String verb Path to API method, e.g. 'verifications' or 'questions/score'
	    * @param NameValueCollection postData Path to API method, e.g. 'verifications' or 'questions/score'
	    * @throws Exception if request fails (HTTP error, API-level error, or response parsing error)
	    * @return Array
	    */

        private BlockScoreResponse Request(string method, string verb, NameValueCollection postData = null)
        {
            var webHeaderCollection = new WebHeaderCollection
            {
                "Accept: application/vnd.blockscore+json;version=" + ApiVersion,
                "Authorization: Basic " + Base64Encode(_authKey) + ":"
            };

            if (method == "POST")
            {
                webHeaderCollection.Add(verb == "questions/score"
                    ? "Content-type: application/json"
                    : "Content-type: application/x-www-form-urlencoded");
            }

            var client = new WebClient
            {
                Credentials = new NetworkCredential("YOUR API KEY HERE", ""),
                Headers = webHeaderCollection
            };

            var response = GetResponse(method, verb, postData, client);

            var blockScoreResponse = GetBlockScoreResponse(verb, response);

            if(blockScoreResponse != null && 
                (!string.IsNullOrEmpty(blockScoreResponse.id) || 
                !string.IsNullOrEmpty(blockScoreResponse.verification_id) || 
                !string.IsNullOrEmpty(blockScoreResponse.question_set_id)))
            {
                if(!string.IsNullOrEmpty(blockScoreResponse.id))
                    _verificationId = blockScoreResponse.id;
                if(!string.IsNullOrEmpty(blockScoreResponse.question_set_id))
                    _questionSetId =blockScoreResponse.question_set_id;
            }
            if(blockScoreResponse != null && blockScoreResponse.error != null)
                throw new Exception("blockscore: API Error "+blockScoreResponse.error.type +": "+blockScoreResponse.error.message);
            return blockScoreResponse;
        }

        private static BlockScoreResponse GetBlockScoreResponse(string verb, string response)
        {
            BlockScoreResponse blockScoreResponse = null;
            if (!string.IsNullOrEmpty(response))
                try
                {
                    blockScoreResponse = GetDesirializedResponse(verb, response);
                }
                catch (Exception e)
                {
                    throw new Exception("blockscore: Unable to retrieve " + verb + " : " + e.Message);
                }
            return blockScoreResponse;
        }

        private static BlockScoreResponse GetDesirializedResponse(string verb,string response)
        {
            if (verb == "verifications")
                return JsonConvert.DeserializeObject<BlockScoreverificationsResponse>(response);
            else if (verb == "questions")
                return JsonConvert.DeserializeObject<BlockScoreQuestionsResponse>(response);
            else if (verb == "questions/score")
                return JsonConvert.DeserializeObject<BlockScoreQuestionsScoreResponse>(response);
            return null;
        }

        private static string GetResponse(string method, string verb, NameValueCollection postData, WebClient client)
        {
            if (verb == "questions/score")
            {
                var request = Utility.ConvertToJsonRequest(postData);
                return client.UploadString(BaseUrl + verb, method, request);
            }

            var bytesResult = client.UploadValues(BaseUrl + verb, method, postData);
            
            return Encoding.Default.GetString(bytesResult);
        }
    }
}


