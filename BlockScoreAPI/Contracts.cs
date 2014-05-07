
using System.Collections.Generic;

namespace BlockScoreAPI
{
    public class BlockScoreQuestionsResponse : BlockScoreResponse
    {
        public IList<Question> questions { get; set; }
    }

    public class Question
        {
            public string question { get; set; }
            public string id { get; set; }
            public IList<Answer> answers { get; set; }
        }

        public class Answer
        {
            public string answer { get; set; }
            public string answer_id { get; set; }
        }

        public class BlockScoreAnswers
        {
            public string verification_id { get; set; }
            public string question_set_id { get; set; }
            public List<BlockScoreAnswer> answers { get; set; }
        }

        public class BlockScoreAnswer
        {
            public string question_id { get; set; }
            public string answer_id { get; set; }
        }

        

        public class Identification
        {
            public string ssn { get; set; }
        }

        public class Name
        {
            public string first { get; set; }
            public string middle { get; set; }
            public string last { get; set; }
        }

        public class Error
        {
            public string code { get; set; }
            public string message { get; set; }
            public string type { get; set; }
        }

        public class Address
        {
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postal_code { get; set; }
            public string country_code { get; set; }
        }

        public class BlockScoreResponse
        {
            public string id { get; set; }
            public string verification_id { get; set; }
            public string question_set_id { get; set; }
            public Error error { get; set; }            
        }

    public class BlockScoreQuestionsScoreResponse : BlockScoreResponse
    {
        public string percentage_correct { get; set; }
        public string score { get; set; }
        public string question_set_id { get; set; }
    }

    public class BlockScoreverificationsResponse : BlockScoreResponse
        {
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string type { get; set; }
            public string livemode { get; set; }
            public string status { get; set; }
            public Identification identification { get; set; }
            public string date_of_birth { get; set; }

            public Address address { get; set; }
            public string verification_id { get; set; }
            public string question_set_id { get; set; }
            public Name name { get; set; }
           
        }

        public abstract class BlockScoreVerifyRequest
        {
            public string FirstName;
            public string MiddleName;
            public string LastName;
            public string DateOfBirth;
            public string Street1;
            public string Street2;
            public string City;
            public string State;
            public string PostalCode;
            public string CountryCode;
        }
        public class BlockScoreVerifyDomesticRequest : BlockScoreVerifyRequest
        {
            public string LastFourDigitsOfSSN;
            public new static string CountryCode{get { return "US"; }}
            public string CitizenshipType{get { return "us_citizen"; }}
        }
        public class BlockScoreVerifyInternationalRequest : BlockScoreVerifyRequest
        {
            public string Gender;
            public string CitizenshipType{get { return "international_citizen";}}
            public string PassportNumber;
        }
    }

