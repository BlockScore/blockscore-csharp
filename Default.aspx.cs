using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlockScoreAPI;
using System.Collections.Generic;
using System.Globalization;

namespace TestBlockScore
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnverifyus_Click(object sender, EventArgs e)
        {            
            VerifyUs();
        }

        protected void btnverifyinternational_Click(object sender, EventArgs e)
        {
            VerifyInternational();
        }

        
        public void VerifyInternational()
        {
            //Instantiate new BlockscoreAPI object
            BlockscoreAPI blockscore = new BlockscoreAPI();

            //Prepare a request for an International ID.  For this example sample values are entered
            BlockScoreVerifyInternationalRequest blockScoreInternationlRequest = GetBlockScoreInternationalRequest();            

            //Call Blockscore /verifications and return results
            BlockScoreResponse verifyInternationalResult = VerifyInternational(blockscore, blockScoreInternationlRequest);

            //Output the request and response values
            lblverificationrequest.Text = GetblockscoreInternationalRequestoutput(blockScoreInternationlRequest);
            lblverificationresponse.Text = GetblockscoreResponseoutput(verifyInternationalResult);

            //Hide the Questions panel as there are no questions for international IDs
            pnlquestions.Visible = false;
            //Display the verifications panel
            pnlverifications.Visible = true;
        }

        public void VerifyUs()
        {
            //Instantiate new BlockscoreAPI object
            BlockscoreAPI blockscore = new BlockscoreAPI();

            //Prepare a request for a Domestic ID.  For this example sample values are entered
            BlockScoreVerifyDomesticRequest blockScoreDomesticRequest = GetBlockScoreDomesticRequest();

            //Call Blockscore /verifications and return results
            BlockScoreResponse verifyUsResult = VerifyUs(blockscore, blockScoreDomesticRequest);

            //Output the request and response values
            lblverificationrequest.Text = GetblockscoreDomesticRequestoutput(blockScoreDomesticRequest);
            lblverificationresponse.Text = GetblockscoreResponseoutput(verifyUsResult);

            //Display the Questions panel as there are questions for US IDs
            pnlquestions.Visible = true;
            //Display the verifications panel
            pnlverifications.Visible = true;

            // Question Set Retrieval
            HandleQuestionSet(verifyUsResult, blockscore);
        }

        /*
        * Ouput Domestic ID Verification Request object to a string
        */
        private string GetblockscoreDomesticRequestoutput(BlockScoreVerifyDomesticRequest blockscorerequest)
        {
            string strreturn = "";

            strreturn += "LastFourDigitsOfSSN = " + blockscorerequest.LastFourDigitsOfSSN;
            strreturn += "<br/>CountryCode = US";
            strreturn += "<br/>CitizenshipType = " + blockscorerequest.CitizenshipType;
            strreturn += "<br/>FirstName = " + blockscorerequest.FirstName;
            strreturn += "<br/>MiddleName = " + blockscorerequest.MiddleName;
            strreturn += "<br/>LastName = " + blockscorerequest.LastName;
            strreturn += "<br/>DateOfBirth = " + blockscorerequest.DateOfBirth;
            strreturn += "<br/>Street1 = " + blockscorerequest.Street1;
            strreturn += "<br/>Street2 = " + blockscorerequest.Street2;
            strreturn += "<br/>City = " + blockscorerequest.City;
            strreturn += "<br/>State = " + blockscorerequest.State;
            strreturn += "<br/>PostalCode = " + blockscorerequest.PostalCode;

            return strreturn;
        }

        /*
        * Ouput International Verification Request object to a string
        */
        private string GetblockscoreInternationalRequestoutput(BlockScoreVerifyInternationalRequest blockscorerequest)
        {
            string strreturn = "";

            strreturn += "Gender = " + blockscorerequest.Gender;
            strreturn += "<br/>CountryCode = " + blockscorerequest.CountryCode;
            strreturn += "<br/>PassportNumber = " + blockscorerequest.PassportNumber;
            strreturn += "<br/>FirstName = " + blockscorerequest.FirstName;
            strreturn += "<br/>MiddleName = " + blockscorerequest.MiddleName;
            strreturn += "<br/>LastName = " + blockscorerequest.LastName;
            strreturn += "<br/>DateOfBirth = " + blockscorerequest.DateOfBirth;
            strreturn += "<br/>Street1 = " + blockscorerequest.Street1;
            strreturn += "<br/>Street2 = " + blockscorerequest.Street2;
            strreturn += "<br/>City = " + blockscorerequest.City;
            strreturn += "<br/>State = " + blockscorerequest.State;
            strreturn += "<br/>PostalCode = " + blockscorerequest.PostalCode;

            return strreturn;
        }
        
        /*
        * Ouput Blockscore Response object to a string
        */
        private string GetblockscoreResponseoutput(BlockScoreResponse blockscoreresponse)
        {
            string strreturn = "";

            strreturn += "<br/>id = " + blockscoreresponse.id;
            strreturn += "<br/>verification_id = " + blockscoreresponse.verification_id;
            strreturn += "<br/>question_set_id = " + blockscoreresponse.question_set_id;
            if (blockscoreresponse.error != null)
            {
                strreturn += "<br/>error_code = " + blockscoreresponse.error.code;
                strreturn += "<br/>error_message = " + blockscoreresponse.error.message;
                strreturn += "<br/>error_type = " + blockscoreresponse.error.type;
            }            

            return strreturn;
        }

        /*
        * Ouput Blockscore Questions Response object to a string
        */
        private string GetblockscoreQuestionsResponseoutput(BlockScoreQuestionsResponse blockscoreresponse)
        {              
            string strreturn = "";

            foreach (Question tempquestion in blockscoreresponse.questions)
            {
                strreturn += "<br/><br/>Question id: " + tempquestion.id.ToString();;                                
                strreturn += "<br/>question:  " + tempquestion.question.ToString();
                strreturn += "<br/>Answers:  ";

                strreturn += "<div class='Tab1'>";
                foreach (Answer tempanswer in tempquestion.answers)
                {
                    
                    strreturn += "<br/>id  " + tempanswer.answer_id.ToString();
                    strreturn += "<br/>answer  " + tempanswer.answer.ToString();                    
                }
                strreturn += "</div>";
            }

            return strreturn;
        }

        /*
        * Ouput Blockscore Questions Request object to a string
        */
        private string GetblockscoreQuestionsRequestoutput(BlockScoreResponse questionset, List<BlockScoreAnswer> answerlist)
        {
            string strreturn = "";

            strreturn += "verification_id = " + questionset.verification_id;
            strreturn += "<br/>question_set_id = " + questionset.question_set_id;
            strreturn += "<br/>answers : ";
            
            foreach (BlockScoreAnswer tempanswer in answerlist)
            {
                strreturn += "<div class='Tab1'>";
                strreturn += "<br/> answer_id = " + tempanswer.answer_id;
                strreturn += "<br/> question_id = " + tempanswer.question_id;
                strreturn += "</div>";
            }
            

            return strreturn;
        }

        /*
        * Ouput Blockscore Questions Score Response object to a string
        */
        private string GetblockscoreQuestionsScoreResponseoutput(BlockScoreQuestionsScoreResponse blockscoreresponse)
        {
            string strreturn = "";

            strreturn += "<br/>question_set_id = " + blockscoreresponse.question_set_id;
            strreturn += "<br/>score = " + blockscoreresponse.score;

            return strreturn;
        }

        /*
        * Get the questions set from Blockscore
        */
        private void HandleQuestionSet(BlockScoreResponse verifyUsResult, BlockscoreAPI blockscore)
        {            
            if (!string.IsNullOrEmpty(verifyUsResult.id))
            {
                lblquestionrequest.Text = "verification_id = " + verifyUsResult.id.ToString();
                
                BlockScoreQuestionsResponse questionSet = GetQuestionSet(blockscore);

                lblquestionresponse.Text = GetblockscoreQuestionsResponseoutput(questionSet);                

                CheckAnswers(blockscore, questionSet);
            }
        }

        /*
        * Verify the Answers with Blockscore
        */
        private void CheckAnswers(BlockscoreAPI blockscore, BlockScoreResponse questionSet)
        {
            if (!string.IsNullOrEmpty(questionSet.question_set_id))
            {
                try
                {
                    var random = new Random();

                    List<BlockScoreAnswer> answerlist = new List<BlockScoreAnswer>
                    {
                        new BlockScoreAnswer{question_id = "1", answer_id= random.Next(1, 5).ToString(CultureInfo.InvariantCulture)},
                        new BlockScoreAnswer{ question_id= "2", answer_id= random.Next(1, 5).ToString(CultureInfo.InvariantCulture)},
                        new BlockScoreAnswer{ question_id= "3", answer_id= random.Next(1, 5).ToString(CultureInfo.InvariantCulture)},
                        new BlockScoreAnswer{ question_id= "4", answer_id= random.Next(1, 5).ToString(CultureInfo.InvariantCulture)},
                        new BlockScoreAnswer{question_id = "5", answer_id= random.Next(1, 5).ToString(CultureInfo.InvariantCulture)},
                    };

                    lblquestionscorerequest.Text = GetblockscoreQuestionsRequestoutput(questionSet, answerlist);

                    BlockScoreQuestionsScoreResponse blockscorequestionscoreresponse = blockscore.CheckQuestionAnswers(answerlist);

                    lblquestionscoreresponse.Text = GetblockscoreQuestionsScoreResponseoutput(blockscorequestionscoreresponse);

                }
                catch (Exception e)
                {
                    throw new Exception("Caught exception: " + e.Message + "\n");
                }
            }
        }

        /*
        * Get the questions set from Blockscore
        */
        private static BlockScoreQuestionsResponse GetQuestionSet(BlockscoreAPI blockscore)
        {
            BlockScoreQuestionsResponse questionSet;
            try
            {
                questionSet = blockscore.QuestionSet();
            }
            catch (Exception e)
            {
                throw new Exception("Caught exception: " + e.Message + "\n");
            }
            return questionSet;
        }

        /*
        * Verify US ID
        */
        private static BlockScoreResponse VerifyUs(BlockscoreAPI blockscore,
            BlockScoreVerifyDomesticRequest blockScoreVerifyInternationlRequest)
        {
            BlockScoreResponse verifyUsResult = null;
            try
            {
                verifyUsResult = blockscore.VerifyUs(blockScoreVerifyInternationlRequest);
            }
            catch (Exception e)
            {
                throw new Exception("Caught exception: " + e.Message + "\n");
            }
            return verifyUsResult;
        }

        /*
        * Static values for a Domestic ID
        */
        private static BlockScoreVerifyDomesticRequest GetBlockScoreDomesticRequest()
        {
            var blockScoreInternationlRequest = new BlockScoreVerifyDomesticRequest
            {
                FirstName = "John",
                MiddleName = "W",
                LastName = "Smith",
                LastFourDigitsOfSSN = "0000",
                DateOfBirth = "1980-10-10",
                Street1 = "123 Broadway Ave",
                Street2 = "",
                City = "New York",
                State = "NY",
                PostalCode = "10011"
            };
            return blockScoreInternationlRequest;
        }

        /*
        * Verify International ID
        */
        private static BlockScoreResponse VerifyInternational(BlockscoreAPI blockscore, BlockScoreVerifyInternationalRequest blockScoreVerifyInternationlRequest)
        {
            BlockScoreResponse verifyIntlResult;
            try
            {
                verifyIntlResult = blockscore.VerifyIntl(blockScoreVerifyInternationlRequest);
            }
            catch (Exception e)
            {
                throw new Exception("Caught exception:" + e.Message);
            }
            return verifyIntlResult;
        }

        /*
        * Static values for an International ID
        */
        private static BlockScoreVerifyInternationalRequest GetBlockScoreInternationalRequest()
        {
            var blockScoreInternationlRequest = new BlockScoreVerifyInternationalRequest
            {
                FirstName = "John",
                MiddleName = "W",
                LastName = "Smith",
                Gender = "M",
                DateOfBirth = "1980-10-10",
                PassportNumber = "X110000",
                Street1 = "Bahnhofstrasse 70",
                Street2 = "",
                City = "Zurich",
                State = "ZH",
                PostalCode = "8001",
                CountryCode = "CH"
            };
            return blockScoreInternationlRequest;
        }
        
    }
}