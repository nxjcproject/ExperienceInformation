using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using WebStyleBaseForEnergy;
using EasyUIJsonParser;

namespace ExperienceInformation.Web.UI_OperationGuide
{
    public partial class CaseAnalysis : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {

            }
        }
        
        [WebMethod]
        public static string GetCaseAnalysisType()
        {
            DataTable m_CaseAnalysisType = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.GetCaseAnalysisType("CaseAnalysisType");
            string m_CaseAnalysisTypeJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_CaseAnalysisType);
            return m_CaseAnalysisTypeJson;
        }
        [WebMethod]
        public static string GetCaseAnalysisInfo(string myCreateYear, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisNature)
        {
            DataTable m_CaseAnalysisInfo = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.GetCaseAnalysisInfo(myCreateYear, myKeyword, myCaseAnalysisType, myCaseAnalysisNature);
            string m_CaseAnalysisJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_CaseAnalysisInfo);
            return m_CaseAnalysisJson;
        }
        [WebMethod]
        public static string GetCaseAnalysisInfoById(string myCaseAnalysisId)
        {
            DataTable m_CaseAnalysisInfo = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.GetCaseAnalysisInfoById(myCaseAnalysisId);
            string m_CaseAnalysisJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_CaseAnalysisInfo);
            return m_CaseAnalysisJson;
        }
        [WebMethod]
        public static string GetCaseAnalysisTextById(string myCaseAnalysisId)
        {
            string m_CaseAnalysisText = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.GetCaseAnalysisTextById(myCaseAnalysisId);
            return m_CaseAnalysisText;
        }

        [WebMethod]
        public static string AddCaseAnalysis(string myCaseAnalysisName, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisLevel,
                   string myCaseAnalysisNature, string myCaseAnalysisText, string myCaseAnalysisParticipants, string myCaseAnalysisTime)
        {
            if (mUserId != "")
            {
                int m_CaseAnalysisText = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.AddCaseAnalysis(myCaseAnalysisName, myKeyword, myCaseAnalysisType, myCaseAnalysisLevel,
                   myCaseAnalysisNature, myCaseAnalysisText, myCaseAnalysisParticipants, myCaseAnalysisTime, mUserId);
                int m_Result = m_CaseAnalysisText > 0 ? 1 : m_CaseAnalysisText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyCaseAnalysis(string myCaseAnalysisId, string myCaseAnalysisName, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisLevel,
                   string myCaseAnalysisNature, string myCaseAnalysisText, string myCaseAnalysisParticipants, string myCaseAnalysisTime)
        {
            if (mUserId != "")
            {
                int m_CaseAnalysisText = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.ModifyCaseAnalysisById(myCaseAnalysisId, myCaseAnalysisName, myKeyword, myCaseAnalysisType, myCaseAnalysisLevel,
                   myCaseAnalysisNature, myCaseAnalysisText, myCaseAnalysisParticipants, myCaseAnalysisTime, mUserId);
                int m_Result = m_CaseAnalysisText > 0 ? 1 : m_CaseAnalysisText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteCaseAnalysisById(string myCaseAnalysisId)
        {
            if (mUserId != "")
            {
                int m_CaseAnalysisText = ExperienceInformation.Service.CaseAnalysis.CaseAnalysis.DeleteCaseAnalysisById(myCaseAnalysisId);
                int m_Result = m_CaseAnalysisText > 0 ? 1 : m_CaseAnalysisText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
    }
}