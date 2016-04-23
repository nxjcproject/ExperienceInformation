using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
namespace WebUserContorls.Web.UI_WebUserControls.OrganizationSelector
{
    public partial class OrganisationTree : System.Web.UI.UserControl
    {
        private List<string> _Organizations;                 //有权限的组织机构
        private string _PageName;                            //页面名称
        private List<string> _OrganizationTypeItems;        //可以显示的产线类型
        private int _LeveDepth;                //层次码深度
        protected void Page_Load(object sender, EventArgs e)
        {
            HiddenField_PageName.Value = _PageName;
           
            if (!IsPostBack)
            {
                string m_FunctionName = Request.Form["myFunctionName"] == null ? "" : Request.Form["myFunctionName"].ToString();             //方法名称,调用后台不同的方法
                string m_Type = Request.Form["myType"] == null ? "" : Request.Form["myType"].ToString();             //方法名称,调用后台不同的方法
                           
                if (m_FunctionName == "GetOrganisationTree")
                {
                    string m_OrganizationTree = GetOrganisationTree(m_Type);
                    Response.Clear();
                    Response.Write(m_OrganizationTree);
                    Response.End();
                }
                else if (m_FunctionName == "GetProductionLineType")
                {
                    string m_ProductionLineType = GetProductionLineType();
                    Response.Clear();
                    Response.Write(m_ProductionLineType);
                    Response.End();
                }
            }
        }
        public List<string> Organizations
        {
            get
            {
                return _Organizations;
            }
            set
            {

                _Organizations = value;
            }
        }
        public List<string> OrganizationTypeItems
        {
            get
            {
                if (_OrganizationTypeItems == null)
                {
                    _OrganizationTypeItems = new List<string>();       //初始化可以显示的产线类型
                }
                return _OrganizationTypeItems;
            }
        }
        public string PageName
        {
            get
            {
                return _PageName;
            }
            set
            {
                _PageName = value;
            }
        }
        public int LeveDepth
        {
            get
            {
                return _LeveDepth;
            }
            set
            {
                _LeveDepth = value;
            }
        }
        private string GetOrganisationTree(string myType)
        {
            int m_LeveDepth;
            if (_LeveDepth < 1)
            {
                m_LeveDepth = 7;
            }
            else
            {
                m_LeveDepth = _LeveDepth;
            }
            DataTable m_OrganisationInfo = WebUserControls.Service.OrganizationSelector.OrganisationTree.GetOrganisationTree(_Organizations, myType, _OrganizationTypeItems, m_LeveDepth, true);
            return EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCode(m_OrganisationInfo, "LevelCode", "Name", new string[] { "OrganizationId", "OrganizationType" });
        }
        private string GetProductionLineType()
        {
            DataTable m_OrganisationInfo = WebUserControls.Service.OrganizationSelector.OrganisationTree.GetProductionLineType(_OrganizationTypeItems);
            return EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_OrganisationInfo);
        }
    }
}