var AddCaseAnalysisFlag;               //标记当前操作是添加还是删除.1表示添加;2表示修改
var CaseAnalysisId;
var CaseAnalysisNatureGood = "good";
var CaseAnalysisNatureBad = "bad";
$(document).ready(function () {
    InitializingDefaultData()
    InitializingDialog();
    LoadCaseAnalysisTypeData('first');
    LoadCaseAnalysisData('first');
});
/////////////////////////////初始化默认数据//////////////////////////
function InitializingDefaultData() {
    var m_Date = new Date();
    $('#numberspinner_ProposedYearF').numberspinner('setValue', m_Date.getFullYear().toString());
    $('#Datebox_ProposedTime').datebox('setValue', m_Date.toString());
}
//////////////////////列出所有岗位操作指导列表/////////////////////
function LoadCaseAnalysisData(myLoadType) {
    var m_CreateYear = $('#numberspinner_ProposedYearF').numberspinner('getValue');
    var m_Keyword = $('#TextBox_KeywordF').textbox('getText');
    var m_CaseAnalysisType = $('#Combobox_CaseAnalysisTypeF').combobox('getValue');
    var m_CaseAnalysisNature = $("input[name='SelectRadio_CaseAnalysisNatureF']:checked").val();
    $.ajax({
        type: "POST",
        url: "CaseAnalysis.aspx/GetCaseAnalysisInfo",
        data: "{myCreateYear:'" + m_CreateYear + "',myKeyword:'" + m_Keyword + "',myCaseAnalysisType:'" + m_CaseAnalysisType + "',myCaseAnalysisNature:'" + m_CaseAnalysisNature + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeCaseAnalysisGrid("CaseAnalysis", m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_CaseAnalysis').datagrid('loadData', m_MsgData);
            }
        }
    });
}
///读取分析类型数据
function LoadCaseAnalysisTypeData(myLoadType) {
    $.ajax({
        type: "POST",
        url: "CaseAnalysis.aspx/GetCaseAnalysisType",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeCaseAnalysisTypeCombox(m_MsgData['rows']);
            }
            else if (myLoadType == 'last') {
                $('#Combobox_CaseAnalysisTypeF').combobox('loadData', m_MsgData['rows']);
                $('#Combobox_CaseAnalysisType').combobox('loadData', m_MsgData['rows']);
            }
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
///初始化生产线类型combox
function InitializeCaseAnalysisTypeCombox(myData) {
    var m_CaseAnalysisTypeAndAll = new Array();
    for (var i = 0; i < myData.length; i++) {
        m_CaseAnalysisTypeAndAll.push(myData[i]);
    }
    m_CaseAnalysisTypeAndAll.unshift({ "TypeId": "", "TypeName": "全部" });
    $('#Combobox_CaseAnalysisTypeF').combobox({
        data: m_CaseAnalysisTypeAndAll,
        dataType: "json",
        valueField: 'TypeId',
        textField: 'TypeName',
        panelHeight: 'auto',
        required: false,
        editable: false,
        onSelect: function (myRecord) {
            //LoadInitializeProductionLineData(myRecord.ProductionLineId);
        }
    });
    $('#Combobox_CaseAnalysisType').combobox({
        data: myData,
        dataType: "json",
        valueField: 'TypeId',
        textField: 'TypeName',
        panelHeight: 'auto',
        required: false,
        editable: false,
        onSelect: function (myRecord) {
            //LoadInitializeProductionLineData(myRecord.ProductionLineId);
        }
    });
    $('#Combobox_CaseAnalysisTypeF').combobox('setValue', m_CaseAnalysisTypeAndAll[0]["TypeId"]);
    $('#Combobox_CaseAnalysisType').combobox('setValue', myData[0]["TypeId"]);
}
///////////////////////////////初始化grid///////////////////////////////
function InitializeCaseAnalysisGrid(myGridId, myData) {
    $('#grid_' + myGridId).datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'CaseAnalysisId',
        columns: [[{
            width: 200,
            title: '名称',
            field: 'CaseAnalysisName'
        }, {
            width: 120,
            title: '关键字',
            field: 'Keyword'
        }, {
            width: 100,
            title: '分析类型',
            field: 'CaseAnalysisTypeName'
        }, {
            width: 120,
            title: '优先级',
            field: 'CaseAnalysisLevel'
        }, {
            width: 150,
            title: '参加人',
            field: 'CaseAnalysisParticipants'
        }, {
            width: 120,
            title: '讨论时间',
            field: 'CaseAnalysisTime'
        }, {
            width: 100,
            title: '汇总人',
            field: 'CreateName'
        }, {
            width: 120,
            title: '汇总时间',
            field: 'CreateTime'
        }, {
            width: 70,
            title: '操作',
            field: 'Op',
            formatter: function (value, row, index) {
                var str = '';
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="ModifyCaseAnalysisFun(\'' + row.CaseAnalysisId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteCaseAnalysisFun(\'' + row.CaseAnalysisId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/map/magnifier.png" title="查看" onclick="VieweCaseAnalysisTextFun(\'' + row.CaseAnalysisId + '\',\'' + row.CaseAnalysisName + '\',\'' + row.CaseAnalysisParticipants + '\',\'' + row.CaseAnalysisTime + '\');"/>';
                return str;
            }
        }]],
        toolbar: '#toolbar_' + myGridId
    });
}
//////////////////////////初始化dialog/////////////////////////////
function InitializingDialog() {
    window.console = window.console || (function () {
        var c = {}; c.log = c.warn = c.debug = c.info = c.error = c.time = c.dir = c.profile
        = c.clear = c.exception = c.trace = c.assert = function () { };
        return c;
    })();
    UE.getEditor('editor');
    $('#dlg_AddCaseAnalysis').dialog({
        title: '案例分析',
        left: 60,
        top: 50,
        width: 800,
        height: 450,
        closed: true,
        cache: false,
        modal: false,
        buttons: "#buttons_AddCaseAnalysis"
    });
    $('#dlg_ViewTextDetail').dialog({
        title: '案例分析',
        left: 60,
        top: 50,
        width: 680,
        height: 430,
        closed: true,
        cache: false,
        modal: true,
        buttons: "#dlg_ViewTextDetail"
    });

}
function QueryCaseAnalysisFun() {
    LoadCaseAnalysisData('last');
}
function VieweCaseAnalysisTextFun(myCaseAnalysisId, myTitle, myCaseAnalysisParticipants, myCaseAnalysisTime) {
    $.ajax({
        type: "POST",
        url: "CaseAnalysis.aspx/GetCaseAnalysisTextById",
        data: "{myCaseAnalysisId:'" + myCaseAnalysisId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                $('#TextTitle').text(myTitle);
                $('#CaseAnalysisParticipants').text(myCaseAnalysisParticipants);
                $('#CaseAnalysisTime').text(myCaseAnalysisTime);
                $('#TextDetail').html(m_Msg);
                $('#dlg_ViewTextDetail').dialog('open');
            }
        }
    });
}
////////////////////////////////添加操作指南////////////////////////////////
function AddCaseAnalysisFun() {
    $('#Textbox_CaseAnalysisName').textbox('setText', '');
    $('#TextBox_Keyword').textbox('setText', '');
    $('#numberspinner_CaseAnalysisLevel').numberspinner('setValue', 1);
    $('#Radio_CaseAnalysisNatureOn').attr('checked', true);
    
    $('#Datebox_CaseAnalysisTime').textbox('setText', '');
    $('#Textbox_CaseAnalysisParticipants').textbox('setText', '');
    UE.getEditor('editor').setContent('', false);

    AddCaseAnalysisFlag = 1;           //1表示添加;2表示修改
    $('#dlg_AddCaseAnalysis').dialog('open');
}
function ModifyCaseAnalysisFun(myCaseAnalysisId) {

    $.ajax({
        type: "POST",
        url: "CaseAnalysis.aspx/GetCaseAnalysisInfoById",
        data: "{myCaseAnalysisId:'" + myCaseAnalysisId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows'] && m_MsgData['rows'].length > 0) {
                var m_Row = m_MsgData['rows'][0];
                CaseAnalysisId = myCaseAnalysisId;

                $('#Textbox_CaseAnalysisName').textbox('setText', m_Row.CaseAnalysisName);
                $('#TextBox_Keyword').textbox('setText', m_Row.Keyword);
                $('#CaseAnalysisType').combobox('setValue', m_Row.CaseAnalysisType);
                $('#numberspinner_CaseAnalysisLevel').numberspinner('setValue', m_Row.CaseAnalysisLevel);

                if (m_Row.CaseAnalysisNature == CaseAnalysisNatureGood) {
                    $('#Radio_CaseAnalysisNatureOn').attr('checked', true);
                }
                else {
                    $('#Radio_CaseAnalysisNatureOff').attr('checked', true);
                }
                var m_CaseAnalysisTime = m_Row.CaseAnalysisTime.replace(/\//g, "-");
                $('#Datebox_CaseAnalysisTime').datebox('setValue', m_CaseAnalysisTime);
                $('#Textbox_CaseAnalysisParticipants').textbox('setText', m_Row.CaseAnalysisParticipants);
                UE.getEditor('editor').setContent('', false);

                AddCaseAnalysisFlag = 2;           //1表示添加;2表示修改
                $('#dlg_AddCaseAnalysis').dialog('open');
            }
        }
    });
    /////////////////////////只回调操作文本/////////////////////////
    $.ajax({
        type: "POST",
        url: "CaseAnalysis.aspx/GetCaseAnalysisTextById",
        data: "{myCaseAnalysisId:'" + myCaseAnalysisId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                UE.getEditor('editor').setContent(m_Msg, false);
            }
        }
    });
}
///////////////////////////////删除操作/////////////////////////
function DeleteCaseAnalysisFun(myCaseAnalysisId) {
    parent.$.messager.confirm('询问', '您确定要删除该案例分析?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "CaseAnalysis.aspx/DeleteCaseAnalysisById",
                data: "{myCaseAnalysisId:'" + myCaseAnalysisId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        QueryCaseAnalysisFun();
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该操作指导已被删除!");
                    }
                    else {
                        alert(m_Msg);
                    }
                }
            });
        }
    });
}
//////////////////////////保存编辑值////////////////////////////
function SaveCaseAnalysisFun() {
    var m_CaseAnalysisName = $('#Textbox_CaseAnalysisName').textbox('getText');
    var m_Keyword = $('#TextBox_Keyword').textbox('getText');
    var m_CaseAnalysisType = $('#Combobox_CaseAnalysisType').combobox('getValue');
    var m_CaseAnalysisLevel = $('#numberspinner_CaseAnalysisLevel').textbox('getText');
    var m_CaseAnalysisNature = $("input[name='SelectRadio_CaseAnalysisNature']:checked").val();
    var m_CaseAnalysisText = UE.getEditor('editor').getContent();
    var m_CaseAnalysisParticipants = $('#Textbox_CaseAnalysisParticipants').textbox('getText');
    var m_CaseAnalysisTime = $('#Datebox_CaseAnalysisTime').datebox('getValue');
    if (AddCaseAnalysisFlag == 1) {
        $.ajax({
            type: "POST",
            url: "CaseAnalysis.aspx/AddCaseAnalysis",
            data: "{myCaseAnalysisName:'" + m_CaseAnalysisName + "',myKeyword:'" + m_Keyword + "',myCaseAnalysisType:'" + m_CaseAnalysisType + "',myCaseAnalysisLevel:'" + m_CaseAnalysisLevel
                + "',myCaseAnalysisNature:'" + m_CaseAnalysisNature + "',myCaseAnalysisText:'" + m_CaseAnalysisText + "',myCaseAnalysisParticipants:'" + m_CaseAnalysisParticipants + "',myCaseAnalysisTime:'" + m_CaseAnalysisTime + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_AddResult = msg.d;
                if (m_AddResult == 1) {
                    alert("添加成功!");
                    $('#dlg_AddCaseAnalysis').dialog('close');
                    QueryCaseAnalysisFun();
                }
                else if (m_AddResult == 0) {
                    alert("该案例分析已存在!");
                }
                else if (m_AddResult == -1) {
                    alert("数据库连接错误!");
                }
                else {
                    alert(m_AddResult);
                }

            }
        });
    }
    else if (AddCaseAnalysisFlag == 2) {
        $.ajax({
            type: "POST",
            url: "CaseAnalysis.aspx/ModifyCaseAnalysis",
            data: "{myCaseAnalysisId:'" + CaseAnalysisId + "',myCaseAnalysisName:'" + m_CaseAnalysisName + "',myKeyword:'" + m_Keyword + "',myCaseAnalysisType:'" + m_CaseAnalysisType + "',myCaseAnalysisLevel:'" + m_CaseAnalysisLevel
                + "',myCaseAnalysisNature:'" + m_CaseAnalysisNature + "',myCaseAnalysisText:'" + m_CaseAnalysisText + "',myCaseAnalysisParticipants:'" + m_CaseAnalysisParticipants + "',myCaseAnalysisTime:'" + m_CaseAnalysisTime + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_ModifyResult = msg.d;
                if (m_ModifyResult == 1) {
                    alert("修改成功!");
                    $('#dlg_AddCaseAnalysis').dialog('close');
                    QueryCaseAnalysisFun();
                }
                else if (m_ModifyResult == 0) {
                    alert("该案例分析已存在!");
                }
                else if (m_ModifyResult == -1) {
                    alert("数据库连接错误!");
                }
                else {
                    alert(m_ModifyResult);
                }

            }
        });
    }
}
