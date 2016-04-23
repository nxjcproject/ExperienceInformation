<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationEnergySavingSuggestions.aspx.cs" Inherits="ExperienceInformation.Web.UI_EnergyInnovation.OperationEnergySaving" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>操作节能挖潜建议</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>
   
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.all.min.js"> </script>

    <script type="text/javascript" src="js/page/OperationEnergySavingSuggestions.js" charset="utf-8"></script>

    <style>p{margin:2px;}</style>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div id="toolbar_EnergySavingSuggestions" style="display: none;">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>选择年份</td>
                                <td style="width: 150px;">
                                    <input id="numberspinner_ProposedYearF" class="easyui-numberspinner" data-options="min:1900,max:2999" style="width: 140px;" />
                                </td>
                                <td>关键字</td>
                                <td style="width: 200px;">
                                    <input id="TextBox_KeywordF" class="easyui-textbox" data-options="editable:true" style="width: 180px;" />
                                </td>

                                <td>
                                    <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                        onclick="QueryEnergySavingSuggestionsFun();">查询</a>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddEnergySavingSuggestionsFun();">添加</a>
                                </td>
                                <td>
                                    <div class="datagrid-btn-separator"></div>
                                </td>
                                <td>
                                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="QueryEnergySavingSuggestionsFun();">刷新</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div data-options="region:'center',border:false,collapsible:false">
            <table id="grid_EnergySavingSuggestions" data-options="fit:true,border:true"></table>
        </div>
    </div>
    <!------------------------------------dialog标签-------------------------------------->
    <div id="dlg_AddEnergySavingSuggestions" class="easyui-dialog" data-options="iconCls:'icon-save',resizable:false,modal:true">
        <fieldset>
            <legend>编辑节能挖潜建议</legend>
            <table class="table" style="width: 100%;">
                <tr>
                    <th style="width: 100px; height: 30px;">名称</th>
                    <td colspan="3">
                        <input id="SuggestionsName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写名称!', editable:true" style="width: 350px;" />
                    </td>
                    <th style="width: 100px;">岗位</th>
                    <td>
                        <input id="TextBox_PostName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写岗位!', editable:true" style="width: 120px;" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">提交人</th>
                    <td style="width: 130px; height: 30px;">
                        <input id="TextBox_Propounder" class="easyui-textbox" data-options="required:true,missingMessage:'请填写提交人', editable:true" style="width: 120px;" />
                    </td>
                    <th style="width: 100px;">提交时间</th>
                    <td style="width: 130px;">
                        <input id="Datebox_ProposedTime" class="easyui-datebox" data-options="validType:'md[\'2012-10\']', required:true" style="width: 120px" />
                    </td>
                    <th style="width: 100px;">关键字</th>
                    <td style="width: 130px;">
                        <input id="TextBox_Keyword" class="easyui-textbox" style="width: 120px;" />
                    </td>
                </tr>
                <tr>
                    <th>内容</th>
                    <td colspan="5" style ="width:510px;height: 300px;">
                        <script id="editor" type="text/plain" style="width:100%;height:100%;"></script>
                    </td>
                </tr>
                <tr>
                    <th  style="width: 100px;">备注</th>
                    <td colspan="5">
                        <input id="Textbox_Remarks" class="easyui-textbox" data-options="multiline:true" style="width: 100%; height: 30px;"/></td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="buttons_AddEnergySavingSuggestions">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveEnergySavingSuggestionsFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddEnergySavingSuggestions').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <!------------------------操作挖潜建议详细信息--------------------------->
    <div id="dlg_ViewTextDetail" class="easyui-dialog" data-options="iconCls:'icon-search',resizable:false,modal:true">
        <table class="table" style="width:100%;">
            <tr>
                <th id="TextTitle" colspan="4" style ="height:20px; font-size:14pt; font-weight:bold; text-align:center; vertical-align:middle;">
                </th>
            </tr>
            <tr>
                <th style ="width:90px; height:20px;text-align:center; vertical-align:middle;">提交人</th>
                <td id ="Propounder" style ="width:180px;text-align:center; vertical-align:middle;">
                </td>
                <th style ="width:90px;text-align:center; vertical-align:middle;">提交时间</th>
                <td id ="ProposedTime" style ="width:180px;text-align:center; vertical-align:middle;">
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
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveEnergySavingSuggestionsFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddEnergySavingSuggestions').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <form id="form_EnergySavingSuggestions" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
