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
using System.IO;
using WebUserContorls.Web.UI_WebUserControls.FileUpload;

namespace ExperienceInformation.Web.UI_EnergyProcessNote
{
    public partial class EnergyProcessRecord : WebStyleBaseForEnergy.webStyleBase
    {
        private const string RecordTypeGroup = "EnergyProcessRecord";
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
                HiddenField_FolderPath.Value = "\\UploadFile\\EnergyProcessRecord";
            }
        }
        [WebMethod]
        public static string GetEnergyProcessRecordInfo(string myStartTime, string myEndTime, string myRecordName, string myDepartmentName, string myRecordType)
        {
            DataTable m_EnergyProcessRecordInfo = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.GetEnergyProcessRecordInfo(myStartTime, myEndTime, myRecordName, myDepartmentName, myRecordType, RecordTypeGroup, mUserId);
            string m_EnergyProcessRecordJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_EnergyProcessRecordInfo);
            return m_EnergyProcessRecordJson;
        }
        [WebMethod]
        public static string GetEnergyProcessRecordInfoById(string myRecordItemId)
        {
            DataTable m_EnergyProcessRecordInfo = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.GetEnergyProcessRecordInfoById(myRecordItemId);
            string m_EnergyProcessRecordJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_EnergyProcessRecordInfo);
            return m_EnergyProcessRecordJson;
        }
        [WebMethod]
        public static string GetEnergyProcessRecordTextById(string myRecordItemId)
        {
            string m_EnergyProcessRecordText = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.GetEnergyProcessRecordTextById(myRecordItemId);
            return m_EnergyProcessRecordText;
        }

        [WebMethod]
        public static string AddEnergyProcessRecord(string myRecordItemId, string myRecordName, string myDepartmentName, string myRecordTime, string myRecordText, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_EnergyProcessRecordText = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.AddEnergyProcessRecord(myRecordItemId, myRecordName, myDepartmentName, "", "", RecordTypeGroup, mUserId,
                       myRecordTime, myRecordText, mUserId, myRemarks);
                int m_Result = m_EnergyProcessRecordText > 0 ? 1 : m_EnergyProcessRecordText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyEnergyProcessRecord(string myRecordItemId, string myRecordName, string myDepartmentName, string myRecordTime, string myRecordText, string myRemarks)
        {
            if (mUserId != "")
            {
                int m_EnergyProcessRecordText = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.ModifyEnergyProcessRecordById(myRecordItemId, myRecordName, myDepartmentName, "", "", RecordTypeGroup, mUserId,
                       myRecordTime, myRecordText, mUserId, myRemarks);
                int m_Result = m_EnergyProcessRecordText > 0 ? 1 : m_EnergyProcessRecordText;
                return m_Result.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteEnergyProcessRecordById(string myRecordItemId, string myFileClassify)
        {
            if (mUserId != "")
            {
                ///////////////////////////删除所有已上传的控件/////////////////////////
                
                DataTable m_FilePathInfoTable = FileUploader.GetFileInfo("",myRecordItemId, myFileClassify);
                bool DeleteFileSuccess = true;
                if (m_FilePathInfoTable != null)
                {
                    for (int i = 0; i < m_FilePathInfoTable.Rows.Count; i++)                   //删除所有文件
                    {
                        string m_FileItemId = m_FilePathInfoTable.Rows[i]["FileItemId"].ToString();
                        string m_FilePath = m_FilePathInfoTable.Rows[i]["FilePath"].ToString();
                        int m_DeleteFileSuccess = FileUploader.DeleteFileInfo(m_FileItemId, myFileClassify, m_FilePath);
                        if (m_DeleteFileSuccess != 1)
                        {
                            DeleteFileSuccess = false;
                        }
                    }
                    if (DeleteFileSuccess == true)
                    {

                        ///////////////////////////删除记录本身/////////////////////////////////

                        int m_EnergyProcessRecordText = ExperienceInformation.Service.EnergyProcessNote.EnergyProcessRecord.DeleteEnergyProcessRecordById(myRecordItemId);
                        int m_Result = m_EnergyProcessRecordText > 0 ? 1 : m_EnergyProcessRecordText;
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