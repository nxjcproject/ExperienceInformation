<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnergyProcessRecord.aspx.cs" Inherits="ExperienceInformation.Web.UI_EnergyProcessNote.EnergyProcessRecord" %>

<%@ Register Src="../UI_WebUserControls/FileUpload/FileUploader.ascx" TagName="FileUploader" TagPrefix="uc1" %>

<%@ Register src="../UI_WebUserControls/FileUpload/FileViewList.ascx" tagname="FileViewList" tagprefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>能源管控过程记录</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />


    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.all.min.js"> </script>

    <script type="text/javascript" src="js/page/EnergyProcessRecord.js" charset="utf-8"></script>

</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div id="toolbar_EnergyProcessRecord" style="display: none;">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 80px; text-align: right;">选择时间</td>
                                <td style="width: 260px;">
                                    <input id="Datebox_StartTimeF" class="easyui-datebox" data-options="validType:'md[\'2012-10\']', required:true" style="width: 120px" />--
                                    <input id="Datebox_EndTimeF" class="easyui-datebox" data-options="validType:'md[\'2012-10\']', required:true" style="width: 120px" />
                                </td>
                                <td style="width: 40px; text-align: right;">名称</td>
                                <td style="width: 150px;">
                                    <input id="TextBox_RecordNameF" class="easyui-textbox" style="width: 140px;" />
                                </td>
                                <td style="width: 40px; text-align: right;">部门</td>
                                <td style="width: 140px;">
                                    <input id="TextBox_DepartmentNameF" class="easyui-textbox" style="width: 130px;" />
                                </td>
                                <td>
                                    <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                        onclick="QueryEnergyProcessRecordFun();">查询</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddEnergyProcessRecordFun();">添加</a>
                                </td>
                                <td>
                                    <div class="datagrid-btn-separator"></div>
                                </td>
                                <td>
                                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="QueryEnergyProcessRecordFun();">刷新</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div data-options="region:'center',border:false,collapsible:false">
            <table id="grid_EnergyProcessRecord" data-options="fit:true,border:true"></table>
        </div>
    </div>
    <!------------------------------------dialog标签-------------------------------------->
    <form id="form_EnergyProcessRecord" runat="server">
        <div id="dlg_AddEnergyProcessRecord" class="easyui-dialog" data-options="iconCls:'icon-save',resizable:false,modal:true">
            <fieldset>
                <legend>编辑运行过程记录</legend>
                <table class="table" style="width: 100%;">
                    <tr>
                        <th style="width: 75px; height: 30px;">名称</th>
                        <td>
                            <input id="Textbox_RecordName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写名称!', editable:true" style="width: 210px;" />
                        </td>
                        <th style="width: 75px; height: 30px;">部门</th>
                        <td>
                            <input id="Textbox_DepartmentName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写名称!', editable:true" style="width: 100px;" />
                        </td>
                        <th style="width: 75px;">记录时间</th>
                        <td>
                            <input id="Datebox_RecordTime" class="easyui-datebox" data-options="validType:'md[\'2012-10\']', required:true" style="width: 150px" />
                        </td>
                    </tr>
                    <tr>
                        <th>内容</th>
                        <td colspan="5" style="width: 510px; height: 300px;">
                            <script id="editor" type="text/plain" style="width: 100%; height: 100%;"></script>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 100px;">备注</th>
                        <td colspan="5">
                            <input id="Textbox_Remarks" class="easyui-textbox" data-options="multiline:true" style="width: 100%; height: 30px;" /></td>
                    </tr>
                    <tr>
                        <th style="width: 100px;">附件</th>
                        <td colspan="5">   
                            <uc1:FileUploader ID="FileUploader" runat="server" />   
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="buttons_AddEnergyProcessRecord">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveEnergyProcessRecordFun();">保存</a>
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="CloseRecordEdit()">取消</a>
                    </td>
                </tr>
            </table>
        </div>
        <!------------------------运行过程记录详细信息--------------------------->
        <div id="dlg_ViewTextDetail" class="easyui-dialog" data-options="iconCls:'icon-search',resizable:false,modal:true">
            <table class="table" style="width: 100%;">
                <tr>
                    <th id="TextTitle" colspan="6" style="height: 20px; font-size: 14pt; font-weight: bold; text-align: center; vertical-align: middle;"></th>
                </tr>
                <tr>
                    <th style="width: 75px; height: 20px; text-align: center; vertical-align: middle;">记录人</th>
                    <td id="Recorder" style="width: 200px; text-align: center; vertical-align: middle;"></td>
                    <th style="width: 75px; text-align: center; vertical-align: middle;">部门</th>
                    <td id="DepartmentName" style="width: 150px; text-align: center; vertical-align: middle;"></td>
                    <th style="width: 75px; text-align: center; vertical-align: middle;">记录时间</th>
                    <td id="RecordTime" style="width: 130px; text-align: center; vertical-align: middle;"></td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 310px; text-align: left; vertical-align: top;">
                        <div id="TextDetail" style="word-break: break-all; word-wrap: break-word; FONT-FAMILY: arial, helvetica,sans-serif; FONT-SIZE: 16px; line-height: 22px;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: left; vertical-align: top;">
                        <uc2:FileViewList ID="FileViewList" runat="server" />
                    </td>
                </tr>

            </table>
        </div>
        <div id="button_ViewTextDetail">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveEnergyProcessRecordFun();">保存</a>
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddEnergyProcessRecord').dialog('close');">取消</a>
                    </td>
                </tr>
            </table>
        </div>
        <div>
        </div>
        <asp:HiddenField ID="HiddenField_UserId" runat="server" />
        <asp:HiddenField ID="HiddenField_FolderPath" runat="server" />
    </form>
</body>
</html>
