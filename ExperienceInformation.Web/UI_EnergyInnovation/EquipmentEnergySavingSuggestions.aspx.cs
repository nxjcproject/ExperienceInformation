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

namespace ExperienceInformation.Web.UI_EnergyInnovation
{
    public partial class EquipmentEnergySavingSuggestions : WebStyleBaseForEnergy.webStyleBase
    {
        private const string SuggestionsType = "Equipment";
        private const string EnergySavingSuggestions = "EnergySavingSuggestions";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {

            }
        }
        [WebMethod]
        public static string GetEnergySavingSuggestionsInfo(string myCreateYear, string myKeyword)
        {
            DataTable m_EnergySavingSuggestionsInfo = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.GetEnergySavingSuggestionsInfo(myCreateYear, myKeyword, SuggestionsType);
            string m_EnergySavingSuggestionsJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_EnergySavingSuggestionsInfo);
            return m_EnergySavingSuggestionsJson;
        }
        [WebMethod]
        public static string GetEnergySavingSuggestionsInfoById(string mySuggestionsId)
        {
            DataTable m_EnergySavingSuggestionsInfo = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.GetEnergySavingSuggestionsInfoById(mySuggestionsId);
            string m_EnergySavingSuggestionsJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_EnergySavingSuggestionsInfo);
            return m_EnergySavingSuggestionsJson;
        }
        [WebMethod]
        public static string GetEnergySavingSuggestionsTextById(string mySuggestionsId)
        {
            string m_EnergySavingSuggestionsText = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.GetEnergySavingSuggestionsTextById(mySuggestionsId);
            return m_EnergySavingSuggestionsText;
        }

        [WebMethod]
        public static string AddEnergySavingSuggestions(string mySuggestionsName, string myKeyword, string myPostName, string mySuggestionsText, string myPropounder, string myProposedTime, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_EnergySavingSuggestionsText = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.AddEnergySavingSuggestions(mySuggestionsName, myKeyword, myPostName, "", SuggestionsType, EnergySavingSuggestions,
                       mySuggestionsText, myPropounder, myProposedTime, mUserId, myRemarks);
                int m_Result = m_EnergySavingSuggestionsText > 0 ? 1 : m_EnergySavingSuggestionsText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyEnergySavingSuggestions(string mySuggestionsId, string mySuggestionsName, string myKeyword, string myPostName, string mySuggestionsText, string myPropounder, string myProposedTime, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_EnergySavingSuggestionsText = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.ModifyEnergySavingSuggestionsById(mySuggestionsId, mySuggestionsName, myKeyword, myPostName, "", SuggestionsType, EnergySavingSuggestions,
                       mySuggestionsText, myPropounder, myProposedTime, mUserId, myRemarks);
                int m_Result = m_EnergySavingSuggestionsText > 0 ? 1 : m_EnergySavingSuggestionsText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteEnergySavingSuggestionsById(string mySuggestionsId)
        {
            if (mUserId != "")
            {
                int m_EnergySavingSuggestionsText = ExperienceInformation.Service.EnergyInnovation.EnergySavingSuggestions.DeleteEnergySavingSuggestionsById(mySuggestionsId);
                int m_Result = m_EnergySavingSuggestionsText > 0 ? 1 : m_EnergySavingSuggestionsText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
    }
}