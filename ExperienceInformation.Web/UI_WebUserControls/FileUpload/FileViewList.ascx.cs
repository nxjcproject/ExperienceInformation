using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace WebUserContorls.Web.UI_WebUserControls.FileUpload
{
    public partial class FileViewList : System.Web.UI.UserControl
    {
        private const string FunctionName_UploadFile = "UploadFile";
        private const string FunctionName_GetFileList = "GetFileList";
        private const string FunctionName_DownloadFile = "DownloadFile";
        private const string FunctionName_DeleteFile = "DeleteFile";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Form.HasKeys())
                {
                    string m_FunctionName = Request.Form["FunctionName"].ToString();

                    if (m_FunctionName == FunctionName_UploadFile)
                    {
                        HttpFileCollection files = Request.Files;
                        string m_FileGroupId = Request.Form["FileGroup"].ToString();
                        string m_UserId = Request.Form["UserId"].ToString();
                        string m_FolderPath = Request.Form["FolderPath"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();
                        if (files != null && files.Count > 0 && m_FolderPath != null && m_FolderPath != "")
                        {
                            string msg = string.Empty;
                            string error = string.Empty;
                            string imgurl;
                            if (files.Count > 0)
                            {
                                string m_FileName = System.IO.Path.GetFileName(files[0].FileName);
                                string m_UploadFilePath = m_FolderPath + "\\" + m_FileName;
                                bool m_AddFileFlag = AddFileInfo(m_FileName, m_FileGroupId, m_FileClassify, m_UploadFilePath, m_UserId);
                                if (m_AddFileFlag == true)            //先把信息保存到数据库，再上载文件
                                {
                                    files[0].SaveAs(m_UploadFilePath);
                                    msg = " 成功! 文件大小为:" + files[0].ContentLength;
                                    imgurl = "";
                                    string res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
                                    Response.ContentType = "text/plain";
                                    Response.Clear();
                                    Response.Write(res);
                                    Response.End();
                                }
                            }
                        }
                    }
                    else if (m_FunctionName == FunctionName_GetFileList)                 //获得附件列表
                    {
                        string m_FileGroupId = Request.Form["FileGroup"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();
                        DataTable m_FileListTable = WebUserControls.Service.FileUploader.FileUploader.GetFileInfo(m_FileGroupId, m_FileClassify);
                        string m_RetrunJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_FileListTable);

                        Response.ContentType = "text/plain";
                        Response.Clear();
                        Response.Write(m_RetrunJson);
                        Response.End();
                    }
                    else if (m_FunctionName == FunctionName_DeleteFile)                  //删除附件
                    {
                        bool m_SuccessFlag = false;
                        string m_FileId = Request.Form["FileId"].ToString();
                        string m_FilePath = Request.Form["FilePath"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();
                        if (File.Exists(m_FilePath))
                        {
                            try
                            {
                                File.Delete(m_FilePath);              //先删除文件后删除数据库
                                DeleteFileInfo(m_FileId, m_FileClassify);
                                m_SuccessFlag = true;
                                Response.ContentType = "text/plain";
                                Response.Clear();
                                Response.Write("{\"Message\":\"1\"}");
                                Response.End();
                            }
                            catch
                            {
                                if (m_SuccessFlag == false)
                                {
                                    Response.ContentType = "text/plain";
                                    Response.Clear();
                                    Response.Write("{\"Message\":\"0\"}");
                                    Response.End();
                                }
                            }

                        }
                    }
                    else if (m_FunctionName == FunctionName_DownloadFile)                //下载附件
                    {
                        string m_FileName = Request.Form["FileName"].ToString();
                        string m_FilePath = Request.Form["FilePath"].ToString();
                        DownloadFile(m_FileName, m_FilePath);
                    }
                }
            }
        }
        /// <summary>
        /// 保存文件信息到数据库
        /// </summary>
        /// <param name="myFileName"></param>
        /// <param name="myFileGroupId"></param>
        /// <param name="myFileClassify"></param>
        /// <param name="myFilePath"></param>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        public static bool AddFileInfo(string myFileName, string myFileGroupId, string myFileClassify, string myFilePath, string myUserId)
        {
            int m_FileListTable = WebUserControls.Service.FileUploader.FileUploader.AddFileInfo(myFileName, myFileGroupId, myFileClassify, myFilePath, myUserId);
            return m_FileListTable > 0 ? true : false;
        }
        public static bool DeleteFileInfo(string myFileItemId, string myFileClassify)
        {
            int m_FileListTable = WebUserControls.Service.FileUploader.FileUploader.DeleteFileInfo(myFileItemId, myFileClassify);
            return m_FileListTable > 0 ? true : false;
        }

        public void DownloadFile(string myFileName, string myFilePath)
        {
            try
            {
                if (!File.Exists(myFilePath))
                {
                    Response.ContentType = "text/plain";
                    Response.Clear();
                    Response.Write("{\"Message\":\"该文件不存在!\"}");
                    Response.End();
                }
                else
                {

                    //以字符流的形式下载文件
                    FileStream fs = new FileStream(myFilePath, FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Buffer = false;
                    Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(myFileName, System.Text.Encoding.UTF8));
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();

                    //FileInfo m_DownloadFile = new FileInfo(m_FilePath);
                    //Response.Clear();
                    //Response.ClearHeaders();
                    //Response.Buffer = false;
                    //Response.ContentType = "application/octet-stream";
                    ////通知浏览器下载文件而不是打开
                    //Response.AddHeader("Content-Disposition", "inline; filename=" + HttpUtility.UrlEncode(m_FileName, System.Text.Encoding.UTF8));
                    //Response.WriteFile(m_FilePath);
                    //Response.Flush();
                    //Response.End();


                    //FileInfo fileInfo = new FileInfo(myFilePath);
                    //Response.Clear();
                    //Response.ClearContent();
                    //Response.ClearHeaders();
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + myFileName);
                    //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    //Response.AddHeader("Content-Transfer-Encoding", "binary");
                    //Response.ContentType = "application/octet-stream";
                    //Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                    //Response.WriteFile(fileInfo.FullName);
                    //Response.Flush();
                    //Response.End();



                }
            }
            catch (Exception)
            {

            }
        }
    }
}