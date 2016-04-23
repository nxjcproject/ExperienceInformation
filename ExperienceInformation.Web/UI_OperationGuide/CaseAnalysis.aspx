<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaseAnalysis.aspx.cs" Inherits="ExperienceInformation.Web.UI_OperationGuide.CaseAnalysis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>案例分析</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/lib/ueditor/ueditor.all.min.js"> </script>
   
    <script type="text/javascript" src="js/page/CaseAnalysis.js" charset="utf-8"></script>

    <style>p{margin:2px;}</style>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div id="toolbar_CaseAnalysis" style="display: none;">
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
                                <td style="width: 120px;">
                                    <input id="TextBox_KeywordF" class="easyui-textbox" data-options="editable:true" style="width: 100px;" />
                                </td>
                                <td>案例类别</td>
                                <td style="width: 120px;">
                                    <input id="Combobox_CaseAnalysisTypeF" class="easyui-combobox" style="width: 100px;" />
                                </td>
                                <td>案例性质</td>
                                <td style="width: 110px; vertical-align:middle; padding-left:5px;">
                                    <input type="radio" name="SelectRadio_CaseAnalysisNatureF" id="Radio_CaseAnalysisNatureOnF" value="good" checked="checked"/>经验
                                    <input type="radio" name="SelectRadio_CaseAnalysisNatureF" id="Radio_CaseAnalysisNatureOffF" value="bad"  />教训
                                </td>
                                <td>
                                    <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                        onclick="QueryCaseAnalysisFun();">查询</a>
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
                                <td><a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddCaseAnalysisFun();">添加</a>
                                </td>
                                <td>
                                    <div class="datagrid-btn-separator"></div>
                                </td>
                                <td>
                                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="QueryCaseAnalysisFun();">刷新</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div data-options="region:'center',border:false,collapsible:false">
            <table id="grid_CaseAnalysis" data-options="fit:true,border:true"></table>
        </div>
    </div>
    <!------------------------------------dialog标签-------------------------------------->
    <div id="dlg_AddCaseAnalysis" class="easyui-dialog" data-options="iconCls:'icon-save',resizable:false,modal:true">
        <fieldset>
            <legend>编辑案例分析</legend>
            <table class="table" style="width: 100%;">
                <tr>
                    <th style="width: 100px; height: 30px;">名称</th>
                    <td style="width: 140px; height: 30px;">
                        <input id="Textbox_CaseAnalysisName" class="easyui-textbox" data-options="required:true,missingMessage:'请填写名称!', editable:true" style="width: 130px;" />
                    </td>
                    <th style="width: 90px;">类别</th>
                    <td style="width: 140px;">
                        <input id="Combobox_CaseAnalysisType" class="easyui-combobox" style="width: 130px;" />
                    </td>
                    <th style="width: 90px;">关键字</th>
                    <td>
                        <input id="TextBox_Keyword" class="easyui-textbox" style="width: 120px;" />
                    </td>
                </tr>
                <tr>
                    <th style="height: 30px;">优先级</th>
                    <td>
                        <input id="numberspinner_CaseAnalysisLevel" class="easyui-numberspinner" style="width: 130px;" data-options="min:1, max:99, editable:false" />
                    </td>
                    <th>案例性质</th>
                    <td>
                        <input type="radio" name="SelectRadio_CaseAnalysisNature" id="Radio_CaseAnalysisNatureOn" value="good" />经验
                        <input type="radio" name="SelectRadio_CaseAnalysisNature" id="Radio_CaseAnalysisNatureOff" value="bad" checked="checked" />教训
                    </td>
                    <th>分析时间</th>
                    <td>
                        <input id="Datebox_CaseAnalysisTime" class="easyui-datebox" data-options="validType:'md[\'2012-10\']', required:true" style="width: 120px" />
                    </td>
                </tr>
                <tr>
                    <th>参加人</th>
                    <td colspan="5">
                        <input id="Textbox_CaseAnalysisParticipants" class="easyui-textbox" data-options="multiline:true" style="width: 100%; height: 30px;"/></td>
                </tr>
                <tr>
                    <th>内容</th>
                    <td colspan="5" style ="width:510px;height: 300px;">
                        <script id="editor" type="text/plain" style="width:100%;height:100%;"></script>
                        
                    </td>
                </tr>

            </table>
        </fieldset>
    </div>
    <div id="buttons_AddCaseAnalysis">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveCaseAnalysisFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddCaseAnalysis').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <!------------------------操作指导详细信息--------------------------->
    <div id="dlg_ViewTextDetail" class="easyui-dialog" data-options="iconCls:'icon-search',resizable:false,modal:true">
        <table class="table" style="width:100%;">
            <tr>
                <th id="TextTitle" colspan="4" style ="height:20px; font-size:14pt; font-weight:bold; text-align:center; vertical-align:middle;">
                </th>
            </tr>
            <tr>
                <th style ="width:90px; height:20px;text-align:center; vertical-align:middle;">参会人</th>
                <td id ="CaseAnalysisParticipants" style ="width:260px;text-align:left; vertical-align:middle;">
                </td>
                <th style ="width:90px;text-align:center; vertical-align:middle;">分析时间</th>
                <td id ="CaseAnalysisTime" style ="width:100px;text-align:center; vertical-align:middle;">
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
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="javascript:SaveCaseAnalysisFun();">保存</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="javascript:$('#dlg_AddCaseAnalysis').dialog('close');">取消</a>
                </td>
            </tr>
        </table>
    </div>
    <form id="form_CaseAnalysis" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
