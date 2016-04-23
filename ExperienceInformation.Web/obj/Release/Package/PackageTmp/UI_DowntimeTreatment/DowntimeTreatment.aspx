<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DowntimeTreatment.aspx.cs" Inherits="ExperienceInformation.Web.UI_DowntimeTreatment.DowntimeTreatment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>停机故障处理</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>
   
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.all.min.js"> </script>

    <script type="text/javascript" src="js/page/DowntimeTreatment.js" charset="utf-8"></script>

    <style>p{margin:2px;}</style>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div id="toolbar_DowntimeTreatment" style="display: none;">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 80px; text-align:right;">选择原因</td>
                                <td style="width: 260px;">
                                     <input id="Combobox_DowntimeReasonF" class="easyui-combotree" style="width: 240px;" />
                                </td>
                                <td>
                                    <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                        onclick="QueryDowntimeTreatmentFun();">查询</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddDowntimeTreatmentFun();">添加</a>
                                </td>
                                <td>
                                    <div class="datagrid-btn-separator"></div>
                                </td>
                                <td>
                                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="QueryDowntimeTreatmentFun();">刷新</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div data-options="region:'center',border:false,collapsible:false">
            <table id="grid_DowntimeTreatment" data-options="fit:true,border:true"></table>
        </div>
    </div>
    <!------------------------------------dialog标签-------------------------------------->
    <div id="dlg_AddDowntimeTreatment" class="easyui-dialog" data-options="iconCls:'icon-save',resizable:false,modal:true">
        <fieldset>
            <legend>停机故障处理</legend>
            <table class="table" style="width: 100%;">
                <tr>
                    <th style="width: 100px; height: 30px;">名称</th>
                    <td>
                        <input id="Textbox_DowntimeTreatmentName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写名称!', editable:true" style="width: 220px;" />
                    </td>
                    <th style="width: 100px;">停机原因</th>
                    <td>
                        <input id="Combobox_DowntimeReason" class="easyui-combotree" data-options="required:true,missingMessage:'请选择停机原因!' " style="width: 240px;" />
                    </td>
                </tr>
                <tr>
                    <th>现象描述</th>
                    <td colspan="3" style ="width:510px;height: 200px;">
                        <script id="editor_Description" type="text/plain" style="width:100%;height:100%;"></script>
                    </td>
                </tr>
                <tr>
                    <th>故障处理</th>
                    <td colspan="3" style ="width:510px;height: 300px;">
                        <script id="editor" type="text/plain" style="width:100%;height:100%;"></script>
                    </td>
                </tr>
                <tr>
                    <th  style="width: 100px;">备注</th>
                    <td colspan="3">
                        <input id="Textbox_Remarks" class="easyui-textbox" data-options="multiline:true" style="width: 100%; height: 30px;"/></td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="buttons_AddDowntimeTreatment">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveDowntimeTreatmentFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddDowntimeTreatment').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <!------------------------故障处理记录详细信息--------------------------->
    <div id="dlg_ViewTextDetail" class="easyui-dialog" data-options="iconCls:'icon-search',resizable:false,modal:true">
        <table class="table" style="width:100%;">
            <tr>
                <th id="TextTitle" colspan="4" style ="height:20px; font-size:14pt; font-weight:bold; text-align:center; vertical-align:middle;">
                </th>
            </tr>
            <tr>
                <th style ="width:90px; height:20px;text-align:center; vertical-align:middle;">编辑人</th>
                <td id ="Creator" style ="width:180px;text-align:center; vertical-align:middle;">
                </td>
                <th style ="width:90px;text-align:center; vertical-align:middle;">编辑时间</th>
                <td id ="CreateTime" style ="width:180px;text-align:center; vertical-align:middle;">
                </td>
            </tr>
            <tr>
                <td colspan="4" style ="height:310px; text-align:left; vertical-align:top;">
                    <div id ="TextDetail" style ="word-break:break-all; word-wrap:break-word; FONT-FAMILY: arial, helvetica,sans-serif; FONT-SIZE: 16px; line-height:22px;" >

                    </div>
                </td>
            </tr>
                
        </table>
    </div>
    <div id="button_ViewTextDetail">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveDowntimeTreatmentFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddDowntimeTreatment').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <form id="form_DowntimeTreatment" runat="server">
        <div>
        </div>
    </form>
</body>
</html>