var AddPostOperationGuideFlag;               //标记当前操作是添加还是删除.1表示添加;2表示修改
var PostOperationKnowledgeId;
var SelectedOrganizationId = "";
$(document).ready(function () {
    InitializingDefaultData()
    InitializingDialog();
    LoadPostOperationGuideData('first');
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
function LoadPostOperationGuideData(myLoadType) {
    var m_CreateYear = $('#numberspinner_ProposedYearF').numberspinner('getValue');
    var m_Keyword = $('#TextBox_KeywordF').textbox('getText');
    $.ajax({
        type: "POST",
        url: "PostOperationGuide.aspx/GetPostOperationGuideInfo",
        data: "{myCreateYear:'" + m_CreateYear + "',myKeyword:'" + m_Keyword + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializePostOperationGuideGrid("PostOperationGuide", m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_PostOperationGuide').datagrid('loadData', m_MsgData);
            }
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializePostOperationGuideGrid(myGridId, myData) {
    $('#grid_' + myGridId).datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'PostOperationKnowledgeId',
        columns: [[{
            width: 200,
            title: '名称',
            field: 'PostOperationKnowledgeName'
        }, {
            width: 120,
            title: '岗位',
            field: 'PostName'
        }, {
            width: 120,
            title: '提交单位',
            field: 'OrganizationName'
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
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="ModifyPostOperationGuideFun(\'' + row.PostOperationKnowledgeId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeletePostOperationGuideFun(\'' + row.PostOperationKnowledgeId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/map/magnifier.png" title="查看" onclick="ViewePostOperationGuideTextFun(\'' + row.PostOperationKnowledgeId + '\',\'' + row.PostOperationKnowledgeName + '\',\'' + row.Propounder + '\',\'' + row.ProposedTime + '\');"/>';
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
    $('#dlg_AddPostOperationGuide').dialog({
        title: '岗位操作指导',
        left: 60,
        top: 50,
        width: 800,
        height: 450,
        closed: true,
        cache: false,
        modal: false,
        buttons: "#buttons_AddPostOperationGuide"
    });
    $('#dlg_ViewTextDetail').dialog({
        title: '岗位操作指导',
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
function QueryPostOperationGuideFun() {
    LoadPostOperationGuideData('last');
}
function ViewePostOperationGuideTextFun(myPostOperationKnowledgeId, myTitle, myPropounder, myProposedTime) {
    $.ajax({
        type: "POST",
        url: "PostOperationGuide.aspx/GetPostOperationGuideTextById",
        data: "{myPostOperationKnowledgeId:'" + myPostOperationKnowledgeId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg != undefined && m_Msg != null) {
                $('#TextTitle').text(myTitle);
                $('#Propounder').text(myPropounder);
                $('#ProposedTime').text(myProposedTime);
                $('#TextDetail').html(m_Msg);
                InitializatingViewFiles(myPostOperationKnowledgeId);
                $('#dlg_ViewTextDetail').dialog('open');
            }
        }
    });
}
////////////////////////////////添加操作指南////////////////////////////////
function AddPostOperationGuideFun() {
    $('#PostOperationKnowledgeName').textbox('setText', '');
    $('#TextBox_PostName').textbox('setText', '');
    $('#TextBox_Propounder').textbox('setText', '');
    $('#TextBox_Keyword').textbox('setText', '');
    UE.getEditor('editor').setContent('', false);
    $('#Textbox_Remarks').textbox('setText', '');
    AddPostOperationGuideFlag = 1;           //1表示添加;2表示修改
    GetServerGuid();
    $('#dlg_AddPostOperationGuide').dialog('open');
}
function ModifyPostOperationGuideFun(myPostOperationKnowledgeId) {

    $.ajax({
        type: "POST",
        url: "PostOperationGuide.aspx/GetPostOperationGuideInfoById",
        data: "{myPostOperationKnowledgeId:'" + myPostOperationKnowledgeId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows'] && m_MsgData['rows'].length > 0) {
                var m_Row = m_MsgData['rows'][0];
                PostOperationKnowledgeId = myPostOperationKnowledgeId;
                $('#PostOperationKnowledgeName').textbox('setText', m_Row.PostOperationKnowledgeName);
                $('#TextBox_PostName').textbox('setText', m_Row.PostName);
                $('#TextBox_Propounder').textbox('setText', m_Row.Propounder);
                $('#TextBox_Keyword').textbox('setText', m_Row.Keyword);
                
                $('#Textbox_Remarks').textbox('setText', m_Row.Remarks);
                var m_ProposedTime = m_Row.ProposedTime.replace(/\//g, "-");
                $('#Datebox_ProposedTime').datebox('setValue', m_ProposedTime);

                SelectedOrganizationId = m_Row.OrganizationId;

                AddPostOperationGuideFlag = 2;           //1表示添加;2表示修改

                InitializatingUploadFiles(myPostOperationKnowledgeId);
                $('#dlg_AddPostOperationGuide').dialog('open');
            }
        }
    });
    /////////////////////////只回调操作文本/////////////////////////
    $.ajax({
        type: "POST",
        url: "PostOperationGuide.aspx/GetPostOperationGuideTextById",
        data: "{myPostOperationKnowledgeId:'" + myPostOperationKnowledgeId + "'}",
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
function DeletePostOperationGuideFun(myPostOperationKnowledgeId) {
    parent.$.messager.confirm('询问', '您确定要删除该操作指导?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "PostOperationGuide.aspx/DeleteOperationGuideById",
                data: "{myPostOperationKnowledgeId:'" + myPostOperationKnowledgeId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        QueryPostOperationGuideFun();
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
function SavePostOperationGuideFun() {
    var m_PostOperationKnowledgeName = $('#PostOperationKnowledgeName').textbox('getText');
    var m_PostName = $('#TextBox_PostName').textbox('getText');
    var m_Propounder = $('#TextBox_Propounder').textbox('getText');
    var m_Keyword = $('#TextBox_Keyword').textbox('getText');
    var m_KnowledgeText = UE.getEditor('editor').getContent();
    var m_Remarks = $('#Textbox_Remarks').textbox('getText');
    var m_ProposedTime = $('#Datebox_ProposedTime').datebox('getValue');
    if (AddPostOperationGuideFlag == 1) {
        $.ajax({
            type: "POST",
            url: "PostOperationGuide.aspx/AddOperationGuide",
            data: "{myPostOperationKnowledgeId:'" + PostOperationKnowledgeId + "',myPostOperationKnowledgeName:'" + m_PostOperationKnowledgeName + "',myKeyword:'" + m_Keyword + "',myPostName:'" + m_PostName
                + "',myPostOperationKnowledgeText:'" + m_KnowledgeText + "',myPropounder:'" + m_Propounder + "',myProposedTime:'" + m_ProposedTime + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_AddResult = msg.d;
                if (m_AddResult == 1) {
                    alert("添加成功!");
                    $('#dlg_AddPostOperationGuide').dialog('close');
                    QueryPostOperationGuideFun();
                    ajaxFileUpload();               //上传文件
                }
                else if (m_AddResult == 0) {
                    alert("该操作指导已存在!");
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
    else if (AddPostOperationGuideFlag == 2) {
        $.ajax({
            type: "POST",
            url: "PostOperationGuide.aspx/ModifyOperationGuide",
            data: "{myPostOperationKnowledgeId:'" + PostOperationKnowledgeId + "',myPostOperationKnowledgeName:'" + m_PostOperationKnowledgeName + "',myKeyword:'" + m_Keyword + "',myOrganizationId:'" + SelectedOrganizationId + "',myPostName:'" + m_PostName
                + "',myPostOperationKnowledgeText:'" + m_KnowledgeText + "',myPropounder:'" + m_Propounder + "',myProposedTime:'" + m_ProposedTime + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_ModifyResult = msg.d;
                if (m_ModifyResult == 1) {
                    alert("修改成功!");
                    $('#dlg_AddPostOperationGuide').dialog('close');
                    QueryPostOperationGuideFun();
                    ajaxFileUpload();               //上传文件
                }
                else if (m_ModifyResult == 0) {
                    alert("该操作指导已存在!");
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

function GetServerGuid() {
    $.ajax({
        type: "POST",
        url: "PostOperationGuide.aspx/GetServerGuid",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                PostOperationKnowledgeId = m_Msg;

                InitializatingUploadFiles(PostOperationKnowledgeId);
            }
        }
    });
}
function InitializatingUploadFiles(myFileGroup) {
    if (typeof (SubScriptLoad) == "function") {
        var m_UserId = $('#HiddenField_UserId').val();
        var m_FolderPath = $('#HiddenField_FolderPath').val();
        var m_FileClassify = "PostOperationGuide";
        var m_FileGroup = myFileGroup;
        SubScriptLoad(m_FileGroup, m_FileClassify, m_UserId, m_FolderPath, "PostOperationGuide.aspx");
    }
}
function InitializatingViewFiles(myFileGroup) {
    if (typeof (SubViewScriptLoad) == "function") {
        var m_FileClassify = "PostOperationGuide";
        var m_FileGroup = myFileGroup;
        SubViewScriptLoad(m_FileGroup, m_FileClassify, "PostOperationGuide.aspx");
    }
}
