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
using WebUserContorls.Web.UI_WebUserControls.FileUpload;
namespace ExperienceInformation.Web.UI_OperationGuide
{
    public partial class PostOperationGuide : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_qtx_efc", "zc_nxjc_byc_byf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#endif
                HiddenField_UserId.Value = mUserId;
                HiddenField_FolderPath.Value = "\\UploadFile\\PostOperationGuide";
            }
        }
        [WebMethod]
        public static string GetPostOperationGuideInfo(string myCreateYear, string myKeyword)
        {
            DataTable m_PostOperationGuideInfo = ExperienceInformation.Service.OperationGuide.PostOperationGuide.GetPostOperationGuideInfo(myCreateYear, myKeyword);
            string m_PostOperationGuideJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_PostOperationGuideInfo);
            return m_PostOperationGuideJson;
        }
        [WebMethod]
        public static string GetPostOperationGuideInfoById(string myPostOperationKnowledgeId)
        {
            DataTable m_PostOperationGuideInfo = ExperienceInformation.Service.OperationGuide.PostOperationGuide.GetPostOperationGuideInfoById(myPostOperationKnowledgeId);
            string m_PostOperationGuideJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_PostOperationGuideInfo);
            return m_PostOperationGuideJson;
        }
        [WebMethod]
        public static string GetPostOperationGuideTextById(string myPostOperationKnowledgeId)
        {
            string m_PostOperationGuideText = ExperienceInformation.Service.OperationGuide.PostOperationGuide.GetPostOperationGuideTextById(myPostOperationKnowledgeId);
            string aa = HttpContext.Current.Server.HtmlEncode("内容");
            return m_PostOperationGuideText;
        }

        [WebMethod]
        public static string AddOperationGuide(string myPostOperationKnowledgeId, string myPostOperationKnowledgeName, string myKeyword, string myPostName, string myPostOperationKnowledgeText, string myPropounder, string myProposedTime, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_PostOperationGuideText = ExperienceInformation.Service.OperationGuide.PostOperationGuide.AddOperationGuide(myPostOperationKnowledgeId, myPostOperationKnowledgeName, myKeyword, myPostName, "PostOperation", "OperationGuide",
                       myPostOperationKnowledgeText, myPropounder, myProposedTime, mUserId, myRemarks);
                int m_Result = m_PostOperationGuideText > 0 ? 1 : m_PostOperationGuideText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyOperationGuide(string myPostOperationKnowledgeId, string myPostOperationKnowledgeName, string myKeyword, string myOrganizationId, string myPostName, string myPostOperationKnowledgeText, string myPropounder, string myProposedTime, string myRemarks)
        {
            string m_OrganizationId = ExperienceInformation.Service.OperationGuide.PostOperationGuide.GetStationId();
            if (mUserId != "" && myOrganizationId == m_OrganizationId)
            {

                int m_PostOperationGuideText = ExperienceInformation.Service.OperationGuide.PostOperationGuide.ModifyOperationGuideById(myPostOperationKnowledgeId, myPostOperationKnowledgeName, myKeyword, m_OrganizationId, myPostName, "PostOperation", "OperationGuide",
                       myPostOperationKnowledgeText, myPropounder, myProposedTime, mUserId, myRemarks);
                int m_Result = m_PostOperationGuideText > 0 ? 1 : m_PostOperationGuideText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteOperationGuideById(string myPostOperationKnowledgeId)
        {
            if (mUserId != "")
            {
                ///////////////////////////删除所有已上传的控件/////////////////////////
                string m_FileClassify = "PostOperationGuide";
                DataTable m_FilePathInfoTable = FileUploader.GetFileInfo("", myPostOperationKnowledgeId, m_FileClassify);
                bool DeleteFileSuccess = true;
                if (m_FilePathInfoTable != null)
                {
                    for (int i = 0; i < m_FilePathInfoTable.Rows.Count; i++)                   //删除所有文件
                    {
                        string m_FileItemId = m_FilePathInfoTable.Rows[i]["FileItemId"].ToString();
                        string m_FilePath = m_FilePathInfoTable.Rows[i]["FilePath"].ToString();
                        int m_DeleteFileSuccess = FileUploader.DeleteFileInfo(m_FileItemId, m_FileClassify, m_FilePath);
                        if (m_DeleteFileSuccess != 1)
                        {
                            DeleteFileSuccess = false;
                        }
                    }
                    if (DeleteFileSuccess == true)
                    {

                        ///////////////////////////删除记录本身/////////////////////////////////
                        int m_PostOperationGuideText = ExperienceInformation.Service.OperationGuide.PostOperationGuide.DeleteOperationGuideById(myPostOperationKnowledgeId);
                        int m_Result = m_PostOperationGuideText > 0 ? 1 : m_PostOperationGuideText;
                        return m_Result.ToString();
                    }
                    else
                    {
                        return "文件删除失败!";
                    }
                }
                else
                {
                    return "文件删除失败!";
                }
            }
            else
            {
                return "非法的用户操作!";
            }

            
        }
        [WebMethod]
        public static string GetServerGuid()
        {
            string m_Guid = Guid.NewGuid().ToString();
            return m_Guid;

        }
    }
}