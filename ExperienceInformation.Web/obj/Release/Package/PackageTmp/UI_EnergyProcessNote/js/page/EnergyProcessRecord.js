var AddEnergyProcessRecordFlag;               //标记当前操作是添加还是删除.1表示添加;2表示修改
var RecordItemId;
$(document).ready(function () {
    InitializingDefaultData()
    InitializingDialog();
    LoadEnergyProcessRecordData('first');
    //SetYearValue();
    //LoadEnergyConsumptionData('first');
});
/////////////////////////////初始化默认数据//////////////////////////
function InitializingDefaultData() {
    var m_Date = new Date();
    var m_StartDate = m_Date.getFullYear() + "-" + (m_Date.getMonth() + 1) + "-" + (m_Date.getDate() - 10);
    $('#Datebox_StartTimeF').datebox('setValue', m_StartDate);
    $('#Datebox_EndTimeF').datebox('setValue', m_Date.toString());
    $('#Datebox_RecordTime').datebox('setValue', m_Date.toString());

}
//////////////////////列出所有岗位操作指导列表/////////////////////
function LoadEnergyProcessRecordData(myLoadType) {
    var m_StartTime = $('#Datebox_StartTimeF').datebox('getValue');
    var m_EndTime = $('#Datebox_EndTimeF').datebox('getValue');
    var m_RecordName = $('#TextBox_RecordNameF').textbox('getText');
    var m_DepartmentName = $('#TextBox_DepartmentNameF').textbox('getText');
    var m_RecordType = "";
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    $.ajax({
        type: "POST",
        url: "EnergyProcessRecord.aspx/GetEnergyProcessRecordInfo",
        data: "{myStartTime:'" + m_StartTime + "',myEndTime:'" + m_EndTime + "',myRecordName:'" + m_RecordName + "',myDepartmentName:'" + m_DepartmentName + "',myRecordType:'" + m_RecordType + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (myLoadType == 'first') {
                InitializeEnergyProcessRecordGrid("EnergyProcessRecord", m_MsgData);
            }
            else if (myLoadType == 'last') {
                $('#grid_EnergyProcessRecord').datagrid('loadData', m_MsgData);
            }
        },
        beforeSend: function (XMLHttpRequest) {
            win;
        }
    });
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeEnergyProcessRecordGrid(myGridId, myData) {
    $('#grid_' + myGridId).datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'RecordItemId',
        columns: [[{
            width: 200,
            title: '名称',
            field: 'RecordName'
        }, {
            width: 120,
            title: '组织机构',
            field: 'OrganizationName',
            hidden: true
        }, {
            width: 100,
            title: '类别',
            field: 'RecordType',
            hidden: true
        }, {
            width: 120,
            title: '记录人',
            field: 'Recorder',
            hidden: true
        }, {
            width: 120,
            title: '部门',
            field: 'DepartmentName'
        }, {
            width: 100,
            title: '记录时间',
            field: 'RecordTime'
        }, {
            width: 120,
            title: '创建人',
            field: 'CreateName'
        }, {
            width: 120,
            title: '创建时间',
            field: 'CreateTime'
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
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="ModifyEnergyProcessRecordFun(\'' + row.RecordItemId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteEnergyProcessRecordFun(\'' + row.RecordItemId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/map/magnifier.png" title="查看" onclick="VieweEnergyProcessRecordTextFun(\'' + row.RecordItemId + '\',\'' + row.RecordName + '\',\'' + row.DepartmentName + '\',\'' + row.Recorder + '\',\'' + row.RecordTime + '\');"/>';
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
    $('#dlg_AddEnergyProcessRecord').dialog({
        title: '运行过程记录',
        left: 60,
        top: 50,
        width: 800,
        height: 450,
        closed: true,
        cache: false,
        modal: false,
        buttons: "#buttons_AddEnergyProcessRecord"
    });
    $('#dlg_ViewTextDetail').dialog({
        title: '运行过程记录',
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
function QueryEnergyProcessRecordFun() {
    LoadEnergyProcessRecordData('last');
}
function VieweEnergyProcessRecordTextFun(myRecordItemId, myTitle, myDepartmentName, myRecorder, myRecordTime) {
    $.ajax({
        type: "POST",
        url: "EnergyProcessRecord.aspx/GetEnergyProcessRecordTextById",
        data: "{myRecordItemId:'" + myRecordItemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg != undefined && m_Msg != null) {
                $('#TextTitle').text(myTitle);
                $('#DepartmentName').text(myDepartmentName);
                $('#Recorder').text(myRecorder);
                $('#RecordTime').text(myRecordTime);
                $('#TextDetail').html(m_Msg);
                $('#dlg_ViewTextDetail').dialog('open');

                InitializatingViewFiles(myRecordItemId);
            }
        }
    });
}
////////////////////////////////添加设备节能挖潜建议////////////////////////////////
function AddEnergyProcessRecordFun() {
    $('#Textbox_RecordName').textbox('setText', '');
    $('#Textbox_DepartmentName').textbox('setText', '');
    UE.getEditor('editor').setContent('', false);
    $('#Textbox_Remarks').textbox('setText', '');
    AddEnergyProcessRecordFlag = 1;           //1表示添加;2表示修改
    //InitializatingUploadFiles("adfadfadsf");
    $('#dlg_AddEnergyProcessRecord').dialog('open');

    GetServerGuid();             //获得Guid为了AddRecord

}
function GetServerGuid() {
    $.ajax({
        type: "POST",
        url: "EnergyProcessRecord.aspx/GetServerGuid",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                RecordItemId = m_Msg;

                InitializatingUploadFiles(RecordItemId);
            }
        }
    });
}
function ModifyEnergyProcessRecordFun(myRecordItemId) {

    $.ajax({
        type: "POST",
        url: "EnergyProcessRecord.aspx/GetEnergyProcessRecordInfoById",
        data: "{myRecordItemId:'" + myRecordItemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows'] && m_MsgData['rows'].length > 0) {
                var m_Row = m_MsgData['rows'][0];
                RecordItemId = myRecordItemId;

                $('#Textbox_RecordName').textbox('setText', m_Row.RecordName);
                $('#Textbox_DepartmentName').textbox('setText', m_Row.DepartmentName);
                UE.getEditor('editor').setContent('', false);
                $('#Textbox_Remarks').textbox('setText', m_Row.Remarks);

                var m_RecordTime = m_Row.RecordTime.replace(/\//g, "-");
                $('#Datebox_RecordTime').datebox('setValue', m_RecordTime);
                AddEnergyProcessRecordFlag = 2;           //1表示添加;2表示修改

                InitializatingUploadFiles(myRecordItemId);
                $('#dlg_AddEnergyProcessRecord').dialog('open');

            }
        }
    });
    /////////////////////////只回调操作文本/////////////////////////
    $.ajax({
        type: "POST",
        url: "EnergyProcessRecord.aspx/GetEnergyProcessRecordTextById",
        data: "{myRecordItemId:'" + myRecordItemId + "'}",
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
function DeleteEnergyProcessRecordFun(myRecordItemId) {
    parent.$.messager.confirm('询问', '您确定要删除该运行过程记录?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "EnergyProcessRecord.aspx/DeleteEnergyProcessRecordById",
                data: "{myRecordItemId:'" + myRecordItemId + "', myFileClassify:'EnergyProcessRecord'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        QueryEnergyProcessRecordFun();
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该运行过程记录已被删除!");
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
function SaveEnergyProcessRecordFun() {
    var m_RecordName = $('#Textbox_RecordName').textbox('getText');
    var m_DepartmentName = $('#Textbox_DepartmentName').textbox('getText');
    var m_RecordTime = $('#Datebox_RecordTime').datebox('getValue');
    var m_RecordText = UE.getEditor('editor').getContent();
    var m_Remarks = $('#Textbox_Remarks').textbox('getText');
    if (m_RecordName != "") {
        if (AddEnergyProcessRecordFlag == 1) {
            $.ajax({
                type: "POST",
                url: "EnergyProcessRecord.aspx/AddEnergyProcessRecord",
                data: "{myRecordItemId:'" + RecordItemId + "',myRecordName:'" + m_RecordName + "',myRecordTime:'" + m_RecordTime + "',myDepartmentName:'" + m_DepartmentName + "',myRecordText:'" + m_RecordText
                    + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_AddResult = msg.d;
                    if (m_AddResult == 1) {
                        alert("添加成功!");
                        $('#dlg_AddEnergyProcessRecord').dialog('close');
                        QueryEnergyProcessRecordFun();
                        ajaxFileUpload();               //上传文件
                    }
                    else if (m_AddResult == 0) {
                        alert("该运行过程记录已存在!");
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
        else if (AddEnergyProcessRecordFlag == 2) {
            $.ajax({
                type: "POST",
                url: "EnergyProcessRecord.aspx/ModifyEnergyProcessRecord",
                data: "{myRecordItemId:'" + RecordItemId + "',myRecordName:'" + m_RecordName + "',myDepartmentName:'" + m_DepartmentName + "',myRecordTime:'" + m_RecordTime
                     + "',myRecordText:'" + m_RecordText + "',myRemarks:'" + m_Remarks + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_ModifyResult = msg.d;
                    if (m_ModifyResult == 1) {
                        alert("修改成功!");
                        $('#dlg_AddEnergyProcessRecord').dialog('close');
                        QueryEnergyProcessRecordFun();

                        ajaxFileUpload();               //上传文件
                    }
                    else if (m_ModifyResult == 0) {
                        alert("该运行过程记录已存在!");
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
    else {
        alert("请输入名称!");
    }

}

function CloseRecordEdit() {            //关闭时清空文件列表
    $('#dlg_AddEnergyProcessRecord').dialog('close');
}

function InitializatingUploadFiles(myFileGroup) {
    if (typeof (SubScriptLoad) == "function") {
        var m_UserId = $('#HiddenField_UserId').val();
        var m_FolderPath = $('#HiddenField_FolderPath').val();
        var m_FileClassify = "EnergyProcessRecord";
        var m_FileGroup = myFileGroup;
        SubScriptLoad(m_FileGroup, m_FileClassify, m_UserId, m_FolderPath, "EnergyProcessRecord.aspx");
    }
}
function InitializatingViewFiles(myFileGroup) {
    if (typeof (SubViewScriptLoad) == "function") {
        var m_FileClassify = "EnergyProcessRecord";
        var m_FileGroup = myFileGroup;
        SubViewScriptLoad(m_FileGroup, m_FileClassify, "EnergyProcessRecord.aspx");
    }
}