var AddEnergySavingSuggestionsFlag;               //标记当前操作是添加还是删除.1表示添加;2表示修改
var SuggestionsId;
$(document).ready(function () {
    InitializingDefaultData()
    InitializingDialog();
    LoadEnergySavingSuggestionsData('first');
    //SetYearValue();
    //LoadEnergyConsumptionData('first');
});
/////////////////////////////初始化默认数据//////////////////////////
function InitializingDefaultData() {
    var m_Date = new Date();
    $('#numberspinner_ProposedYearF').numberspinner('setValue', m_Date.getFullYear().toString());
    $('#Datebox_ProposedTime').datebox('setValue', m_Date.toString());
}
//////////////////////列出所有岗位操作指导列表/////////////////////
function LoadEnergySavingSuggestionsData(myLoadType) {
    var m_CreateYear = $('#numberspinner_ProposedYearF').numberspinner('getValue');
    var m_Keyword = $('#TextBox_KeywordF').textbox('getText');
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    $.ajax({
        type: "POST",
        url: "OperationEnergySavingSuggestions.aspx/GetEnergySavingSuggestionsInfo",
        data: "{myCreateYear:'" + m_CreateYear + "',myKeyword:'" + m_Keyword + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeEnergySavingSuggestionsGrid("EnergySavingSuggestions", m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_EnergySavingSuggestions').datagrid('loadData', m_MsgData);
            }
        },
        beforeSend: function (XMLHttpRequest) {
            win;
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeEnergySavingSuggestionsGrid(myGridId, myData) {
    $('#grid_' + myGridId).datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'SuggestionsId',
        columns: [[{
            width: 200,
            title: '名称',
            field: 'SuggestionsName'
        }, {
            width: 120,
            title: '岗位',
            field: 'PostName'
        }, {
            width: 100,
            title: '提交人',
            field: 'Propounder'
        }, {
            width: 120,
            title: '提交时间',
            field: 'ProposedTime'
        }, {
            width: 100,
            title: '汇总人',
            field: 'CreateName'
        }, {
            width: 120,
            title: '汇总时间',
            field: 'CreateTime'
        }, {
            width: 120,
            title: '关键字',
            field: 'Keyword'
        }
        , {
            width: 150,
            title: '备注',
            field: 'Remarks'
        }, {
            width: 70,
            title: '操作',
            field: 'Op',
            formatter: function (value, row, index) {
                var str = '';
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="ModifyEnergySavingSuggestionsFun(\'' + row.SuggestionsId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteEnergySavingSuggestionsFun(\'' + row.SuggestionsId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/map/magnifier.png" title="查看" onclick="VieweEnergySavingSuggestionsTextFun(\'' + row.SuggestionsId + '\',\'' + row.SuggestionsName + '\',\'' + row.Propounder + '\',\'' + row.ProposedTime + '\');"/>';
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
    $('#dlg_AddEnergySavingSuggestions').dialog({
        title: '操作节能挖潜',
        left: 60,
        top: 50,
        width: 800,
        height: 450,
        closed: true,
        cache: false,
        modal: false,
        buttons: "#buttons_AddEnergySavingSuggestions"
    });
    $('#dlg_ViewTextDetail').dialog({
        title: '操作节能挖潜',
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
function QueryEnergySavingSuggestionsFun() {
    LoadEnergySavingSuggestionsData('last');
}
function VieweEnergySavingSuggestionsTextFun(mySuggestionsId, myTitle, myPropounder, myProposedTime) {
    $.ajax({
        type: "POST",
        url: "OperationEnergySavingSuggestions.aspx/GetEnergySavingSuggestionsTextById",
        data: "{mySuggestionsId:'" + mySuggestionsId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                $('#TextTitle').text(myTitle);
                $('#Propounder').text(myPropounder);
                $('#ProposedTime').text(myProposedTime);
                $('#TextDetail').html(m_Msg);
                $('#dlg_ViewTextDetail').dialog('open');
            }
        }
    });
}
////////////////////////////////添加操作节能挖潜建议////////////////////////////////
function AddEnergySavingSuggestionsFun() {
    $('#SuggestionsName').textbox('setText', '');
    $('#TextBox_PostName').textbox('setText', '');
    $('#TextBox_Propounder').textbox('setText', '');
    $('#TextBox_Keyword').textbox('setText', '');
    UE.getEditor('editor').setContent('', false);
    $('#Textbox_Remarks').textbox('setText', '');
    AddEnergySavingSuggestionsFlag = 1;           //1表示添加;2表示修改
    $('#dlg_AddEnergySavingSuggestions').dialog('open');
}
function ModifyEnergySavingSuggestionsFun(mySuggestionsId) {

    $.ajax({
        type: "POST",
        url: "OperationEnergySavingSuggestions.aspx/GetEnergySavingSuggestionsInfoById",
        data: "{mySuggestionsId:'" + mySuggestionsId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows'] && m_MsgData['rows'].length > 0) {
                var m_Row = m_MsgData['rows'][0];
                SuggestionsId = mySuggestionsId;
                $('#SuggestionsName').textbox('setText', m_Row.SuggestionsName);
                $('#TextBox_PostName').textbox('setText', m_Row.PostName);
                $('#TextBox_Propounder').textbox('setText', m_Row.Propounder);
                $('#TextBox_Keyword').textbox('setText', m_Row.Keyword);

                $('#Textbox_Remarks').textbox('setText', m_Row.Remarks);
                var m_ProposedTime = m_Row.ProposedTime.replace(/\//g, "-");
                $('#Datebox_ProposedTime').datebox('setValue', m_ProposedTime);
                AddEnergySavingSuggestionsFlag = 2;           //1表示添加;2表示修改
                $('#dlg_AddEnergySavingSuggestions').dialog('open');
            }
        }
    });
    /////////////////////////只回调操作文本/////////////////////////
    $.ajax({
        type: "POST",
        url: "OperationEnergySavingSuggestions.aspx/GetEnergySavingSuggestionsTextById",
        data: "{mySuggestionsId:'" + mySuggestionsId + "'}",
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
function DeleteEnergySavingSuggestionsFun(mySuggestionsId) {
    parent.$.messager.confirm('询问', '您确定要删除该操作节能挖潜建议?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "OperationEnergySavingSuggestions.aspx/DeleteEnergySavingSuggestionsById",
                data: "{mySuggestionsId:'" + mySuggestionsId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        QueryEnergySavingSuggestionsFun();
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该操作节能挖潜建议已被删除!");
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
function SaveEnergySavingSuggestionsFun() {
    var m_SuggestionsName = $('#SuggestionsName').textbox('getText');
    var m_PostName = $('#TextBox_PostName').textbox('getText');
    var m_Propounder = $('#TextBox_Propounder').textbox('getText');
    var m_Keyword = $('#TextBox_Keyword').textbox('getText');
    var m_KnowledgeText = UE.getEditor('editor').getContent();
    var m_Remarks = $('#Textbox_Remarks').textbox('getText');
    var m_ProposedTime = $('#Datebox_ProposedTime').datebox('getValue');
    if (AddEnergySavingSuggestionsFlag == 1) {
        $.ajax({
            type: "POST",
            url: "OperationEnergySavingSuggestions.aspx/AddEnergySavingSuggestions",
            data: "{mySuggestionsName:'" + m_SuggestionsName + "',myKeyword:'" + m_Keyword + "',myPostName:'" + m_PostName
                + "',mySuggestionsText:'" + m_KnowledgeText + "',myPropounder:'" + m_Propounder + "',myProposedTime:'" + m_ProposedTime + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_AddResult = msg.d;
                if (m_AddResult == 1) {
                    alert("添加成功!");
                    $('#dlg_AddEnergySavingSuggestions').dialog('close');
                    QueryEnergySavingSuggestionsFun();
                }
                else if (m_AddResult == 0) {
                    alert("该操作节能挖潜建议已存在!");
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
    else if (AddEnergySavingSuggestionsFlag == 2) {
        $.ajax({
            type: "POST",
            url: "OperationEnergySavingSuggestions.aspx/ModifyEnergySavingSuggestions",
            data: "{mySuggestionsId:'" + SuggestionsId + "',mySuggestionsName:'" + m_SuggestionsName + "',myKeyword:'" + m_Keyword + "',myPostName:'" + m_PostName
                + "',mySuggestionsText:'" + m_KnowledgeText + "',myPropounder:'" + m_Propounder + "',myProposedTime:'" + m_ProposedTime + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_ModifyResult = msg.d;
                if (m_ModifyResult == 1) {
                    alert("修改成功!");
                    $('#dlg_AddEnergySavingSuggestions').dialog('close');
                    QueryEnergySavingSuggestionsFun();
                }
                else if (m_ModifyResult == 0) {
                    alert("该操作节能挖潜已存在!");
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
