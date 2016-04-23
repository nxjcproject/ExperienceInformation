var AddDowntimeTreatmentFlag;               //标记当前操作是添加还是删除.1表示添加;2表示修改
var DowntimeTreatmentItemId;
var DowntimeReasonIdF;
var DowntimeReasonId;
$(document).ready(function () {
    InitializingDefaultData()
    InitializingDialog();
    InitializeDowntimeTreatmentGrid("DowntimeTreatment", { 'rows': [], 'total': 0 });
});
/////////////////////////////初始化默认数据//////////////////////////
function InitializingDefaultData() {
    $.ajax({
        type: "POST",
        url: "DowntimeTreatment.aspx/GetDowntimeReasonInfo",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            InitializeDowntimeReasonComboxTree(m_MsgData);
        }
    });
}
//////////////////////////////初始化停机原因下拉菜单////////////////////////////
function InitializeDowntimeReasonComboxTree(myData) {
    $('#Combobox_DowntimeReasonF').combotree({
        data: myData,
        dataType: "json",
        valueField: 'id',
        textField: 'text',
        required: false,
        panelHeight: 'auto',
        editable: false,
        onSelect: function (myRecord) {
            DowntimeReasonIdF = myRecord.ReasonItemId;
        }
    });
    $('#Combobox_DowntimeReason').combotree({
        data: myData,
        dataType: "json",
        valueField: 'id',
        textField: 'text',
        required: false,
        panelHeight: 'auto',
        editable: false,
        onSelect: function (myRecord) {
            DowntimeReasonId = myRecord.ReasonItemId;
        }
    });
}

//////////////////////列出所有岗位操作指导列表/////////////////////
function LoadDowntimeTreatmentData(myLoadType) {
    var m_ReasonItemId = $('#Combobox_DowntimeReasonF').combotree('getValue');
    if (m_ReasonItemId != "" && m_ReasonItemId != undefined) {
        $.ajax({
            type: "POST",
            url: "DowntimeTreatment.aspx/GetDowntimeTreatmentInfo",
            data: "{myReasonItemId:'" + m_ReasonItemId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_MsgData = jQuery.parseJSON(msg.d);
                if (myLoadType == 'first') {
                    InitializeDowntimeTreatmentGrid("DowntimeTreatment", m_MsgData);
                }
                else if (myLoadType == 'last') {
                    $('#grid_DowntimeTreatment').datagrid('loadData', m_MsgData);
                }
            }
        });
    }
    else {
        alert("请选择停机原因!");
    }
}
//////////////////////////////////初始化基础数据//////////////////////////////////////////
function InitializeDowntimeTreatmentGrid(myGridId, myData) {
    $('#grid_' + myGridId).datagrid({
        title: '',
        data: myData,
        dataType: "json",
        striped: true,
        //loadMsg: '',   //设置本身的提示消息为空 则就不会提示了的。这个设置很关键的
        rownumbers: true,
        singleSelect: true,
        idField: 'DowntimeTreatmentItemId',
        columns: [[{
            width: 200,
            title: '名称',
            field: 'DowntimeTreatmentName'
        }, {
            width: 300,
            title: '停机原因',
            field: 'ReasonText'
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
                str = '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_edit.png" title="编辑" onclick="ModifyDowntimeTreatmentFun(\'' + row.DowntimeTreatmentItemId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/notes/note_delete.png" title="删除" onclick="DeleteDowntimeTreatmentFun(\'' + row.DowntimeTreatmentItemId + '\');"/>';
                str = str + '<img class="iconImg" src = "/lib/extlib/themes/images/ext_icons/map/magnifier.png" title="查看" onclick="VieweDowntimeTreatmentTextFun(\'' + row.DowntimeTreatmentItemId + '\',\'' + row.DowntimeTreatmentName + '\',\'' + row.CreateName + '\',\'' + row.CreateTime + '\');"/>';
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
    UE.getEditor('editor_Description');
    $('#dlg_AddDowntimeTreatment').dialog({
        title: '故障停机处理方法',
        left: 60,
        top: 50,
        width: 800,
        height: 450,
        closed: true,
        cache: false,
        modal: false,
        buttons: "#buttons_AddDowntimeTreatment"
    });
    $('#dlg_ViewTextDetail').dialog({
        title: '故障停机处理方法',
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
function QueryDowntimeTreatmentFun() {
    LoadDowntimeTreatmentData('last');
}
function VieweDowntimeTreatmentTextFun(myDowntimeTreatmentItemId, myTitle, myCreator, myCreateTime) {
    $.ajax({
        type: "POST",
        url: "DowntimeTreatment.aspx/GetDowntimePhenomenonTreatmentTextById",
        data: "{myDowntimeTreatmentItemId:'" + myDowntimeTreatmentItemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                $('#TextTitle').text(myTitle);
                $('#Creator').text(myCreator);
                $('#CreateTime').text(myCreateTime);
                $('#TextDetail').html(m_Msg);
                $('#dlg_ViewTextDetail').dialog('open');
            }
        }
    });
}
////////////////////////////////添加设备节能挖潜建议////////////////////////////////
function AddDowntimeTreatmentFun() {
    $('#Textbox_DowntimeTreatmentName').textbox('setText', '');
    UE.getEditor('editor').setContent('', false);
    UE.getEditor('editor_Description').setContent('', false);
    $('#Textbox_Remarks').textbox('setText', '');
    AddDowntimeTreatmentFlag = 1;           //1表示添加;2表示修改
    $('#dlg_AddDowntimeTreatment').dialog('open');
}
function ModifyDowntimeTreatmentFun(myDowntimeTreatmentItemId) {

    $.ajax({
        type: "POST",
        url: "DowntimeTreatment.aspx/GetDowntimeTreatmentInfoById",
        data: "{myDowntimeTreatmentItemId:'" + myDowntimeTreatmentItemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows'] && m_MsgData['rows'].length > 0) {
                var m_Row = m_MsgData['rows'][0];
                DowntimeTreatmentItemId = myDowntimeTreatmentItemId;

                $('#Textbox_DowntimeTreatmentName').textbox('setText', m_Row.DowntimeTreatmentName);
                UE.getEditor('editor_Description').setContent('', false);
                UE.getEditor('editor').setContent('', false);
                $('#Combobox_DowntimeReason').combotree('setValue', m_Row.ReasonItemId);
                $('#Textbox_Remarks').textbox('setText', m_Row.Remarks);
                AddDowntimeTreatmentFlag = 2;           //1表示添加;2表示修改

                $('#dlg_AddDowntimeTreatment').dialog('open');
            }
        }
    });
    /////////////////////////只回调操作文本/////////////////////////
    $.ajax({
        type: "POST",
        url: "DowntimeTreatment.aspx/GetDowntimePhenomenonTextById",
        data: "{myDowntimeTreatmentItemId:'" + myDowntimeTreatmentItemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_Msg = msg.d;
            if (m_Msg) {
                UE.getEditor('editor_Description').setContent(m_Msg, false);
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "DowntimeTreatment.aspx/GetDowntimeTreatmentTextById",
        data: "{myDowntimeTreatmentItemId:'" + myDowntimeTreatmentItemId + "'}",
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
function DeleteDowntimeTreatmentFun(myDowntimeTreatmentItemId) {
    parent.$.messager.confirm('询问', '您确定要删除该停机处理记录?', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "DowntimeTreatment.aspx/DeleteDowntimeTreatmentById",
                data: "{myDowntimeTreatmentItemId:'" + myDowntimeTreatmentItemId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var m_Msg = msg.d;
                    if (m_Msg == "1") {
                        QueryDowntimeTreatmentFun();
                        alert("删除成功!");
                    }
                    else if (m_Msg == "-1") {
                        alert("数据库错误!");
                    }
                    else if (m_Msg == "0") {
                        alert("该停机处理已被删除!");
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
function SaveDowntimeTreatmentFun() {
    var m_DowntimeTreatmentName = $('#Textbox_DowntimeTreatmentName').textbox('getText');
    var m_ReasonItemId = $('#Combobox_DowntimeReason').combotree('getValue');
    var m_DescriptionText = UE.getEditor('editor_Description').getContent();
    var m_TreatmentText = UE.getEditor('editor').getContent();
    var m_Remarks = $('#Textbox_Remarks').textbox('getText');
    $('#Combobox_DowntimeReasonF').combotree('setValue', m_ReasonItemId);
    if (AddDowntimeTreatmentFlag == 1) {
        $.ajax({
            type: "POST",
            url: "DowntimeTreatment.aspx/AddDowntimeTreatment",
            data: "{myDowntimeTreatmentName:'" + m_DowntimeTreatmentName + "',myReasonItemId:'" + m_ReasonItemId + "',myPhenomenon:'" + m_DescriptionText
                + "',myTreatment:'" + m_TreatmentText + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_AddResult = msg.d;
                if (m_AddResult == 1) {
                    alert("添加成功!");
                    $('#dlg_AddDowntimeTreatment').dialog('close');
                    QueryDowntimeTreatmentFun();
                }
                else if (m_AddResult == 0) {
                    alert("该停机处理已存在!");
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
    else if (AddDowntimeTreatmentFlag == 2) {
        $.ajax({
            type: "POST",
            url: "DowntimeTreatment.aspx/ModifyDowntimeTreatment",
            data: "{myDowntimeTreatmentItemId:'" + DowntimeTreatmentItemId + "',myDowntimeTreatmentName:'" + m_DowntimeTreatmentName + "',myReasonItemId:'" + m_ReasonItemId
                 + "',myPhenomenon:'" + m_DescriptionText + "',myTreatment:'" + m_TreatmentText + "',myRemarks:'" + m_Remarks + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_ModifyResult = msg.d;
                if (m_ModifyResult == 1) {
                    alert("修改成功!");
                    $('#dlg_AddDowntimeTreatment').dialog('close');
                    QueryDowntimeTreatmentFun();
                }
                else if (m_ModifyResult == 0) {
                    alert("该停机处理已存在!");
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
