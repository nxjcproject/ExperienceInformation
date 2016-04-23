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
    public partial class FileUploader : System.Web.UI.UserControl
    {
        private const string FunctionName_UploadFile = "UploadFile";
        private const string FunctionName_GetFileList = "GetFileList";
        private const string FunctionName_DownloadFile = "DownloadFile";
        private const string FunctionName_DeleteFile = "DeleteFile";
        private const string KeyWord = "UploadFiles";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Form.HasKeys())
                {
                    string m_FunctionName = Request.Form["FunctionName"].ToString();

                    if (m_FunctionName == FunctionName_UploadFile)   //上传文件
                    {
                        HttpFileCollection files = Request.Files;
                        string m_FileGroupId = Request.Form["FileGroup"].ToString();
                        string m_UserId = Request.Form["UserId"].ToString();
                        string m_FolderPath = Request.Form["FolderPath"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();
                        string m_FileName = System.IO.Path.GetFileName(files[0].FileName);
                        float m_FileSize = files[0].ContentLength / 1024;
                        byte[] m_FileStream = new byte[(int)files[0].InputStream.Length];
                        files[0].InputStream.Read(m_FileStream, 0, m_FileStream.Length);

                        string msg = string.Empty;
                        string error = string.Empty;
                        string imgurl = string.Empty;
                        if (files != null && files.Count > 0 && m_FolderPath != null && m_FolderPath != "")
                        {
                            ;
                            if (files.Count > 0)
                            {
                                string m_UploadFilePath = m_FolderPath + "\\" + Guid.NewGuid() + m_FileName;
                                int m_AddFileFlag = AddFileInfo(m_FileStream, m_FileName, m_FileGroupId, m_FileClassify, m_FileSize, m_UploadFilePath, m_UserId);
                                if (m_AddFileFlag == 1)            //先把信息保存到数据库，再上载文件
                                {
                                    msg = "文件上传成功!大小: " + m_FileSize + "KB";
                                    imgurl = "";
                                }
                                else if (m_AddFileFlag == 2)
                                {
                                    error = "文件上传失败!重复的文件名称!";
                                }
                                else if (m_AddFileFlag == 3)
                                {
                                    error = "文件上传失败!上传操作不合法!";
                                }
                                else
                                {
                                    error = "文件上传失败!其它错误原因!";
                                }
                            }
                            else
                            {
                                error = "文件上传失败!无可上传文件!";
                            }
                        }
                        else
                        {
                            error = "文件上传失败!无可上传文件!";
                        }
                        string res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
                        Response.ContentType = "text/plain";
                        Response.Clear();
                        Response.Write(res);
                        Response.End();
                    }
                    else if (m_FunctionName == FunctionName_GetFileList)                 //获得附件列表
                    {
                        string m_FileGroupId = Request.Form["FileGroup"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();
                        DataTable m_FileListTable = GetFileInfo("", m_FileGroupId, m_FileClassify);
                        string m_RetrunJson = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_FileListTable);

                        Response.ContentType = "text/plain";
                        Response.Clear();
                        Response.Write(m_RetrunJson);
                        Response.End();
                    }
                    else if (m_FunctionName == FunctionName_DeleteFile)                  //删除附件
                    {
                        string m_FileId = Request.Form["FileId"].ToString();
                        string m_FilePath = Request.Form["FilePath"].ToString();
                        string m_FileClassify = Request.Form["FileClassify"].ToString();

                        try
                        {
                            int m_SuccessFlag = DeleteFileInfo(m_FileId, m_FileClassify, m_FilePath);
                            if (m_SuccessFlag == 1)
                            {
                                Response.ContentType = "text/plain";
                                Response.Clear();
                                Response.Write("{\"Message\":\"1\"}");
                                Response.End();
                            }
                            else
                            {
                                Response.ContentType = "text/plain";
                                Response.Clear();
                                Response.Write("{\"Message\":\"0\"}");
                                Response.End();
                            }
                        }
                        catch
                        {
                            Response.ContentType = "text/plain";
                            Response.Clear();
                            Response.Write("{\"Message\":\"0\"}");
                            Response.End();
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
        public static int AddFileInfo(byte[] myFileBuffer, string myFileName, string myFileGroupId, string myFileClassify, float myFileSize, string myFilePath, string myUserId)
        {
            ServiceReference_FilesTransport.FilesTransportSoapClient m_FilesTransport = new ServiceReference_FilesTransport.FilesTransportSoapClient();
            int m_UploadResult = m_FilesTransport.Upload(myFileBuffer, myFileName, myFileGroupId, myFileClassify, myFileSize, myFilePath, myUserId + ";" + KeyWord);
            return m_UploadResult;
        }
        public static DataTable GetFileInfo(string myFileName, string myFileGroupId, string myFileClassify)
        {
            ServiceReference_FilesTransport.FilesTransportSoapClient m_FilesTransport = new ServiceReference_FilesTransport.FilesTransportSoapClient();
            DataTable m_FilesListTable = m_FilesTransport.GetFilesList(myFileName, myFileGroupId, myFileClassify, "Guest;" + KeyWord);
            return m_FilesListTable;
        }
        public static int DeleteFileInfo(string myFileItemId, string myFileClassify, string myFilePath)
        {
            ServiceReference_FilesTransport.FilesTransportSoapClient m_FilesTransport = new ServiceReference_FilesTransport.FilesTransportSoapClient();
            int m_DeleteResult = m_FilesTransport.Delete(myFileItemId, myFileClassify, myFilePath, "Guest;" + KeyWord);
            return m_DeleteResult;
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
                    ServiceReference_FilesTransport.FilesTransportSoapClient m_FilesTransport = new ServiceReference_FilesTransport.FilesTransportSoapClient();

                    byte[] bytes = m_FilesTransport.DownLoad(myFileName, myFilePath, "Guest;" + KeyWord);
                    //fs.Read(bytes, 0, bytes.Length);
                    //fs.Close();
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