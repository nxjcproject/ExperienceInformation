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

namespace ExperienceInformation.Web.UI_DowntimeTreatment
{
    public partial class DowntimeTreatment : WebStyleBaseForEnergy.webStyleBase
    {
        private const string RecordTypeGroup = "DowntimeTreatment";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {

            }
        }
        [WebMethod]
        public static string GetDowntimeReasonInfo()
        {
            DataTable m_DowntimeTreatmentInfo = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimeReasonInfo();
            string m_DowntimeTreatmentJson = EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(m_DowntimeTreatmentInfo, "MachineHaltReasonId", "ReasonItemId", "ReasonText");
            return m_DowntimeTreatmentJson;
        }
        [WebMethod]
        public static string GetDowntimeTreatmentInfo(string myReasonItemId)
        {
            DataTable m_DowntimeTreatmentInfo = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimeTreatmentInfo(myReasonItemId);
            string m_DowntimeTreatmentJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_DowntimeTreatmentInfo);
            return m_DowntimeTreatmentJson;
        }
        [WebMethod]
        public static string GetDowntimeTreatmentInfoById(string myDowntimeTreatmentItemId)
        {
            DataTable m_DowntimeTreatmentInfo = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimeTreatmentInfoById(myDowntimeTreatmentItemId);
            string m_DowntimeTreatmentJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_DowntimeTreatmentInfo);
            return m_DowntimeTreatmentJson;
        }
        [WebMethod]
        public static string GetDowntimeTreatmentTextById(string myDowntimeTreatmentItemId)
        {
            string m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimeTreatmentTextById(myDowntimeTreatmentItemId);
            return m_DowntimeTreatmentText;
        }
        [WebMethod]
        public static string GetDowntimePhenomenonTextById(string myDowntimeTreatmentItemId)
        {
            string m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimePhenomenonTextById(myDowntimeTreatmentItemId);
            return m_DowntimeTreatmentText;
        }
        [WebMethod]
        public static string GetDowntimePhenomenonTreatmentTextById(string myDowntimeTreatmentItemId)
        {
            string m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.GetDowntimePhenomenonTreatmentTextById(myDowntimeTreatmentItemId);
            return m_DowntimeTreatmentText;
        }
        [WebMethod]

        public static string AddDowntimeTreatment(string myDowntimeTreatmentName, string myReasonItemId, string myPhenomenon, string myTreatment, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.AddDowntimeTreatment(myDowntimeTreatmentName, myReasonItemId, myPhenomenon, myTreatment, mUserId, myRemarks);
                int m_Result = m_DowntimeTreatmentText > 0 ? 1 : m_DowntimeTreatmentText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyDowntimeTreatment(string myDowntimeTreatmentItemId, string myDowntimeTreatmentName, string myReasonItemId, string myPhenomenon, string myTreatment, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.ModifyDowntimeTreatmentById(myDowntimeTreatmentItemId, myDowntimeTreatmentName, myReasonItemId, myPhenomenon, myTreatment, mUserId, myRemarks);
                int m_Result = m_DowntimeTreatmentText > 0 ? 1 : m_DowntimeTreatmentText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteDowntimeTreatmentById(string myDowntimeTreatmentItemId)
        {
            if (mUserId != "")
            {
                int m_DowntimeTreatmentText = ExperienceInformation.Service.DowntimeTreatment.DowntimeTreatment.DeleteDowntimeTreatmentById(myDowntimeTreatmentItemId);
                int m_Result = m_DowntimeTreatmentText > 0 ? 1 : m_DowntimeTreatmentText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
    }
}